namespace Personnel.Client.Client.Services
{
    public class Proxy : IProxy
    {
        private readonly HttpClient Client;

        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };
        public Proxy(HttpClient client)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<R> DeleteAsync<R>(string url) where R : IApiResponse
        {
            using var httpResponse = await Client.DeleteAsync(url);
            return await HandleResponseAsync<R>(httpResponse);

        }

        public async Task<R> GetAsync<R>(string url) where R : IApiResponse
        {

            using var httpResponse = await Client.GetAsync(url);
            return await HandleResponseAsync<R>(httpResponse);
        }

        public async Task<R> PatchAsync<R, S>(string url, S pathData) where R : IApiResponse
        {

            // Construir JsonPatchDocument-like body a partir de S (tu enfoque original)
            List<object> patchs = new();
            var props = typeof(S).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props)
            {
                patchs.Add(new
                {
                    op = "replace",
                    path = $"/{prop.Name}",
                    value = prop.GetValue(pathData)?.ToString() ?? string.Empty
                });
            }

            var json = JsonSerializer.Serialize(patchs, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            using var content = new StringContent(json, Encoding.UTF8, "application/json-patch+json");

            using var httpResponse = await Client.PatchAsync(url, content);
            return await HandleResponseAsync<R>(httpResponse);
        }

        public async Task<R> PostAsync<R, S>(string url, S postData) where R : IApiResponse
        {

            using var httpResponse = await Client.PostAsJsonAsync(url, postData);
            return await HandleResponseAsync<R>(httpResponse);
        }

        public async Task<R> PostAsync<R>(string url) where R : IApiResponse
        {
            using var httpResponse = await Client.PostAsync(url, null);
            return await HandleResponseAsync<R>(httpResponse);
        }

        public Task<R> PostFileAsync<R, S>(string url, S PostFile)
        {
            throw new NotImplementedException();
        }

        public async Task<R> PutAsync<R, S>(string url, S postData) where R : IApiResponse
        {

            using var httpResponse = await Client.PutAsJsonAsync(url, postData);
            return await HandleResponseAsync<R>(httpResponse);
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
