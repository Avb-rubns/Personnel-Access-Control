using Personnel.Client.Shared.POCO.Abstractions;
using Personnel.Client.Shared.POCO.ResponseAPI;
using System.Net;

namespace Rubns.Infrastructure.Services
{
    public class Proxy(IHttpClientFactory factory, IUtils utils) : IProxyServer
    {
        private readonly IHttpClientFactory _factory = factory;
        private readonly IUtils _utils = utils;

        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };


        public Task<R> DeleteAsync<R, S>(string clientName, string url, S PostData)
        {
            throw new NotImplementedException();
        }

        public Task<R> DeleteAsync<R>(string clientName, string url)
        {
            throw new NotImplementedException();
        }

        public async Task<R> GetAsync<R>(string clientName, string url) where R : IApiResponse
        {

            using var client = _factory.CreateClient(clientName);
            var httpResponse = await client.GetAsync(url);
            return await HandleResponseAsync<R>(httpResponse);

        }

        public async Task<R> GetStringAsync<R>(string clientName, string url)
        {
            using var client = _factory.CreateClient(clientName);
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (compatible; Bot/1.0)");

            var html = await client.GetStringAsync(url);

            return (R)(object)html;
        }

        public async Task<R> PostAsFormDataAsync<R, S>(string clientName, string url, S postData)
        {
            using var client = _factory.CreateClient(clientName);
            MultipartFormDataContent form = new MultipartFormDataContent();

            form = _utils.ToMultipartFormDataContent(postData);
            var response = await client.PostAsync(url, form);

            return (R)(object)response;
        }

        public async Task<R> PostAsJsonAsync<R, S>(string clientName, string url, S postData)
        {

            throw new NotImplementedException();
        }

        public Task<R> PostAsync<R>(string clientName, string url)
        {
            throw new NotImplementedException();
        }

        public Task<R> PutAsync<R, S>(string clientName, string url, S postData)
        {
            throw new NotImplementedException();
        }

        private async Task<R> HandleResponseAsync<R>(HttpResponseMessage httpResponse) where R : IApiResponse
        {
            var status = httpResponse.StatusCode;
            var content = await httpResponse.Content.ReadAsStringAsync();
            Type returnType = typeof(R);

            switch (status)
            {
                case HttpStatusCode.OK:
                case HttpStatusCode.Created:
                case HttpStatusCode.Accepted:

                    if (string.IsNullOrWhiteSpace(content))
                    {
                        return CreateEmptyResponseInstance<R>(status);
                    }
                    return await ProcessContentAsync<R>(content, status, returnType);

                case HttpStatusCode.NoContent:
                case HttpStatusCode.ResetContent:
                    return CreateEmptyResponseInstance<R>(status);


                case HttpStatusCode.BadRequest:
                case HttpStatusCode.Unauthorized:
                case HttpStatusCode.Forbidden:
                case HttpStatusCode.NotFound:
                case HttpStatusCode.Conflict:
                    if (!string.IsNullOrWhiteSpace(content))
                    {

                        try
                        {
                            return await ProcessContentAsync<R>(content, status, returnType);
                        }
                        catch
                        {

                            return CreateErrorResponseInstance<R>(status, content);
                        }
                    }
                    return CreateErrorResponseInstance<R>(status, $"HTTP {(int)status} {status}");

                // Otros -> lanzar excepción para que el llamador decida
                default:
                    var err = string.IsNullOrWhiteSpace(content) ? $"HTTP {(int)status} {status}" : content;
                    throw new HttpRequestException($"Respuesta no manejada del servidor: {(int)status} {status}. Contenido: {err}");
            }
        }
        private async Task<R> ProcessContentAsync<R>(string content, HttpStatusCode status, Type returnType) where R : IApiResponse
        {
            // Si el consumidor espera ResponseData<T>
            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(ResponseData<>))
            {
                var dataType = returnType.GetGenericArguments()[0];
                var wrapperType = typeof(ResponseData<>).MakeGenericType(dataType);

                var wrapper = await DeserializeWrapperAsync(content, wrapperType, dataType);
                if (wrapper != null)
                {
                    // set status si aplica
                    wrapperType.GetProperty(nameof(Response.StatusCode))?
                        .SetValue(wrapper, status);
                    return (R)wrapper;
                }

                // Si no fue posible deserializar como wrapper, intentar deserializar solo el objeto T (puede ser array)
                try
                {
                    var rawData = JsonSerializer.Deserialize(content, dataType, _jsonOptions);
                    var instance = CreateWrapperInstance(wrapperType, rawData, status, "Operación exitosa");
                    return (R)instance;
                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Error deserializando el contenido plano a tipo {dataType}: {ex.Message}\nContenido: {content}");
                }
            }

            // Si el consumidor espera Response
            if (returnType == typeof(Response))
            {
                try
                {
                    var simple = JsonSerializer.Deserialize<Response>(content, _jsonOptions) ?? new Response();
                    simple.StatusCode = status;
                    simple.Message ??= "Operación completada";
                    return (R)(object)simple;
                }
                catch
                {
                    var simple = new Response { StatusCode = status, Message = "Operación completada" };
                    return (R)(object)simple;
                }
            }

            throw new InvalidOperationException($"El tipo de retorno {returnType} no está soportado.");
        }

        private async Task<object?> DeserializeWrapperAsync(string content, Type wrapperType, Type dataType)
        {
            // Intentar detectar si el JSON trae la estructura { "data": ... } (wrapper)
            try
            {
                using var doc = JsonDocument.Parse(content);
                var root = doc.RootElement;

                if (root.ValueKind == JsonValueKind.Object && root.TryGetProperty("data", out _))
                {
                    // Deserialize al tipo wrapper
                    var wrapper = JsonSerializer.Deserialize(content, wrapperType, _jsonOptions);
                    return wrapper;
                }

                // No es un objeto con la propiedad 'data', no devolvemos wrapper
                return null;
            }
            catch (JsonException)
            {
                // contenido no es JSON válido -> null para que el caller intente otras estrategias
                return null;
            }
        }

        private object CreateWrapperInstance(Type wrapperType, object? data, HttpStatusCode status, string message)
        {
            var instance = Activator.CreateInstance(wrapperType) ?? throw new InvalidOperationException("No se pudo crear instancia del wrapper.");
            var dataProp = wrapperType.GetProperty(nameof(ResponseData<object>.Data));
            var statusProp = wrapperType.GetProperty(nameof(Response.StatusCode));
            var messageProp = wrapperType.GetProperty(nameof(Response.Message));

            // Asignar Data (si el tipo es compatible)
            dataProp?.SetValue(instance, data);

            // Asignar Status y Message
            statusProp?.SetValue(instance, status);
            messageProp?.SetValue(instance, message);

            return instance;
        }

        private R CreateEmptyResponseInstance<R>(HttpStatusCode status) where R : IApiResponse
        {
            Type returnType = typeof(R);

            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(ResponseData<>))
            {
                var dataType = returnType.GetGenericArguments()[0];
                var wrapperType = typeof(ResponseData<>).MakeGenericType(dataType);
                var instance = CreateWrapperInstance(wrapperType, null, status, "Operación completada");
                return (R)instance;
            }

            if (returnType == typeof(Response))
            {
                var simple = new Response { StatusCode = status, Message = "Operación completada" };
                return (R)(object)simple;
            }

            throw new InvalidOperationException($"El tipo de retorno {returnType} no está soportado.");
        }

        private R CreateErrorResponseInstance<R>(HttpStatusCode status, string message) where R : IApiResponse
        {
            Type returnType = typeof(R);

            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(ResponseData<>))
            {
                var dataType = returnType.GetGenericArguments()[0];
                var wrapperType = typeof(ResponseData<>).MakeGenericType(dataType);
                var instance = CreateWrapperInstance(wrapperType, null, status, message);
                return (R)instance;
            }

            if (returnType == typeof(Response))
            {
                var simple = new Response { StatusCode = status, Message = message };
                return (R)(object)simple;
            }

            throw new InvalidOperationException($"El tipo de retorno {returnType} no está soportado.");
        }

    }
}
