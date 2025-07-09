namespace Personnel.Client.Client.Services
{
    public class Proxy : IProxy
    {
        HttpClient Client;

        public Proxy(HttpClient client)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<R> DeleteAsync<R, S>(string url, S postData, string userName)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                if (!Client.DefaultRequestHeaders.Contains("User"))
                    Client.DefaultRequestHeaders.Add("User", userName);
            }

            var response = await Client.PostAsJsonAsync(url, postData);

            var content = await response.Content.ReadAsStringAsync();
            bool hasContent = !string.IsNullOrWhiteSpace(content);

            if (hasContent)
            {
                try
                {
                    var result = JsonSerializer.Deserialize<R>(content, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (result != null)
                    {
                        return result;
                    }
                }
                catch (JsonException)
                {
                    // Manejar si la deserialización falla
                    throw new InvalidOperationException("No se pudo deserializar la respuesta del servidor.");
                }
            }

            // Si no hay contenido, construye un ApiResponseDTO genérico si aplica
            if (typeof(R) == typeof(Response))
            {
                return (R)(object)new Response
                {
                    StatusCode = response.StatusCode,
                    Message = "Operación completada sin respuesta de contenido"
                };
            }

            throw new InvalidOperationException("No se pudo obtener una respuesta válida.");

        }

        public async Task<R> DeleteAsync<R>(string url, string userName) where R : IApiResponse
        {
            if (!string.IsNullOrEmpty(userName) &&
                !Client.DefaultRequestHeaders.Contains("User"))
            {
                Client.DefaultRequestHeaders.Add("User", userName);
            }

            using var httpResponse = await Client.DeleteAsync(url);
            var content = await httpResponse.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var status = httpResponse.StatusCode;

            Type returnType = typeof(R);

            // Manejar ResponseData<T>
            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(ResponseData<>))
            {
                var dataType = returnType.GetGenericArguments()[0];
                var wrapperType = typeof(ResponseData<>).MakeGenericType(dataType);

                using var doc = JsonDocument.Parse(content);
                // Si viene el envoltorio completo
                if (doc.RootElement.TryGetProperty("data", out _))
                {
                    var wrapper = JsonSerializer.Deserialize(content, wrapperType, options);
                    if (wrapper is R rWrapped)
                    {
                        rWrapped.StatusCode = status;
                        return rWrapped;
                    }
                }

                // Si viene solo el objeto T, lo deserializamos y envolvemos
                var data = JsonSerializer.Deserialize(doc.RootElement.GetRawText(), dataType, options);
                var instance = (IApiResponse)Activator.CreateInstance(wrapperType)!;
                wrapperType.GetProperty(nameof(ResponseData<object>.Data))?.SetValue(instance, data);
                wrapperType.GetProperty(nameof(ResponseData<object>.StatusCode))?.SetValue(instance, status);
                wrapperType.GetProperty(nameof(ResponseData<object>.Message))?.SetValue(instance, "Operación exitosa");
                return (R)instance;
            }

            // Manejar Response simple
            if (returnType == typeof(Response))
            {
                Response simple;
                try
                {
                    simple = JsonSerializer.Deserialize<Response>(content, options) ?? new Response();
                }
                catch
                {
                    simple = new Response();
                }
                simple.StatusCode = status;
                simple.Message ??= "Operación completada";
                return (R)(object)simple;
            }

            throw new InvalidOperationException($"El tipo de retorno {returnType} no está soportado.");
        }

        public async Task<R> GetAsync<R>(string url, string userName = null) where R : IApiResponse
        {
            if (!string.IsNullOrEmpty(userName))
            {
                if (!Client.DefaultRequestHeaders.Contains("User"))
                    Client.DefaultRequestHeaders.Add("User", userName);
            }

            using var httpResponse = await Client.GetAsync(url);
            var content = await httpResponse.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var status = httpResponse.StatusCode;

            Type returnType = typeof(R);

            // Manejar ResponseData<T>
            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(ResponseData<>))
            {
                var dataType = returnType.GetGenericArguments()[0];
                var wrapperType = typeof(ResponseData<>).MakeGenericType(dataType);

                using var doc = JsonDocument.Parse(content);
                // Si viene el envoltorio completo
                if (doc.RootElement.TryGetProperty("data", out _))
                {
                    var wrapper = JsonSerializer.Deserialize(content, wrapperType, options);
                    if (wrapper is R rWrapped)
                    {
                        rWrapped.StatusCode = status;
                        return rWrapped;
                    }
                }

                // Si viene solo el objeto T, lo deserializamos y envolvemos
                var data = JsonSerializer.Deserialize(doc.RootElement.GetRawText(), dataType, options);
                var instance = (IApiResponse)Activator.CreateInstance(wrapperType)!;
                wrapperType.GetProperty(nameof(ResponseData<object>.Data))?.SetValue(instance, data);
                wrapperType.GetProperty(nameof(ResponseData<object>.StatusCode))?.SetValue(instance, status);
                wrapperType.GetProperty(nameof(ResponseData<object>.Message))?.SetValue(instance, "Operación exitosa");
                return (R)instance;
            }

            // Manejar Response simple
            if (returnType == typeof(Response))
            {
                Response simple;
                try
                {
                    simple = JsonSerializer.Deserialize<Response>(content, options) ?? new Response();
                }
                catch
                {
                    simple = new Response();
                }
                simple.StatusCode = status;
                simple.Message ??= "Operación completada";
                return (R)(object)simple;
            }

            throw new InvalidOperationException($"El tipo de retorno {returnType} no está soportado.");
        }

        public async Task<R> PostAsync<R, S>(string url, S postData, string userName = null) where R : IApiResponse
        {
            if (!string.IsNullOrEmpty(userName) &&
                !Client.DefaultRequestHeaders.Contains("User"))
            {
                Client.DefaultRequestHeaders.Add("User", userName);
            }

            using var httpResponse = await Client.PostAsJsonAsync(url, postData);
            var content = await httpResponse.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var status = httpResponse.StatusCode;

            Type returnType = typeof(R);

            // Manejar ResponseData<T>
            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(ResponseData<>))
            {
                var dataType = returnType.GetGenericArguments()[0];
                var wrapperType = typeof(ResponseData<>).MakeGenericType(dataType);

                using var doc = JsonDocument.Parse(content);
                // Si viene el envoltorio completo
                if (doc.RootElement.TryGetProperty("data", out _))
                {
                    var wrapper = JsonSerializer.Deserialize(content, wrapperType, options);
                    if (wrapper is R rWrapped)
                    {
                        rWrapped.StatusCode = status;
                        return rWrapped;
                    }
                }

                // Si viene solo el objeto T, lo deserializamos y envolvemos
                var data = JsonSerializer.Deserialize(doc.RootElement.GetRawText(), dataType, options);
                var instance = (IApiResponse)Activator.CreateInstance(wrapperType)!;
                wrapperType.GetProperty(nameof(ResponseData<object>.Data))?.SetValue(instance, data);
                wrapperType.GetProperty(nameof(ResponseData<object>.StatusCode))?.SetValue(instance, status);
                wrapperType.GetProperty(nameof(ResponseData<object>.Message))?.SetValue(instance, "Operación exitosa");
                return (R)instance;
            }

            // Manejar Response simple
            if (returnType == typeof(Response))
            {
                Response simple;
                try
                {
                    simple = JsonSerializer.Deserialize<Response>(content, options) ?? new Response();
                }
                catch
                {
                    simple = new Response();
                }
                simple.StatusCode = status;
                simple.Message ??= "Operación completada";
                return (R)(object)simple;
            }

            throw new InvalidOperationException($"El tipo de retorno {returnType} no está soportado.");
        }

        public async Task<R> PostAsync<R>(string url, string userName = null) where R : IApiResponse
        {
            if (!string.IsNullOrEmpty(userName) &&
                !Client.DefaultRequestHeaders.Contains("User"))
            {
                Client.DefaultRequestHeaders.Add("User", userName);
            }

            using var httpResponse = await Client.PostAsync(url, null);
            var content = await httpResponse.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var status = httpResponse.StatusCode;

            Type returnType = typeof(R);

            // Manejar ResponseData<T>
            if (returnType.IsGenericType && returnType.GetGenericTypeDefinition() == typeof(ResponseData<>))
            {
                var dataType = returnType.GetGenericArguments()[0];
                var wrapperType = typeof(ResponseData<>).MakeGenericType(dataType);

                using var doc = JsonDocument.Parse(content);
                // Si viene el envoltorio completo
                if (doc.RootElement.TryGetProperty("data", out _))
                {
                    var wrapper = JsonSerializer.Deserialize(content, wrapperType, options);
                    if (wrapper is R rWrapped)
                    {
                        rWrapped.StatusCode = status;
                        return rWrapped;
                    }
                }

                // Si viene solo el objeto T, lo deserializamos y envolvemos
                var data = JsonSerializer.Deserialize(doc.RootElement.GetRawText(), dataType, options);
                var instance = (IApiResponse)Activator.CreateInstance(wrapperType)!;
                wrapperType.GetProperty(nameof(ResponseData<object>.Data))?.SetValue(instance, data);
                wrapperType.GetProperty(nameof(ResponseData<object>.StatusCode))?.SetValue(instance, status);
                wrapperType.GetProperty(nameof(ResponseData<object>.Message))?.SetValue(instance, "Operación exitosa");
                return (R)instance;
            }

            // Manejar Response simple
            if (returnType == typeof(Response))
            {
                Response simple;
                try
                {
                    simple = JsonSerializer.Deserialize<Response>(content, options) ?? new Response();
                }
                catch
                {
                    simple = new Response();
                }
                simple.StatusCode = status;
                simple.Message ??= "Operación completada";
                return (R)(object)simple;
            }

            throw new InvalidOperationException($"El tipo de retorno {returnType} no está soportado.");
        }

        public Task<R> PostFileAsync<R, S>(string url, S PostFile, string userName = null)
        {
            throw new NotImplementedException();
        }

        public Task<R> PutAsync<R, S>(string url, S postData, string userName = null)
        {
            throw new NotImplementedException();
        }
    }
}
