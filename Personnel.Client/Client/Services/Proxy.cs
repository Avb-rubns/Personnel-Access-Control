namespace Personnel.Client.Client.Services
{
    public class Proxy : IProxy
    {
        HttpClient Client;
        public Proxy(HttpClient client)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<R> DeleteAsync<R, S>(string url, S postData, string userName) where R : IApiResponse
        {
            if (!string.IsNullOrEmpty(userName))
            {
                if (!Client.DefaultRequestHeaders.Contains("User"))
                    Client.DefaultRequestHeaders.Add("User", userName);
            }

            var httpResponse = await Client.PostAsJsonAsync(url, postData);

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
                try
                {
                    if (doc.RootElement.ValueKind == JsonValueKind.Object && doc.RootElement.TryGetProperty("data", out _))
                    {
                        var deserialized = JsonSerializer.Deserialize(content, wrapperType, options);
                        if (deserialized is R rWrapped)
                        {
                            rWrapped.StatusCode = status;
                            return rWrapped;
                        }
                    }

                    var rawData = JsonSerializer.Deserialize(doc.RootElement.GetRawText(), dataType, options);

                    var instance = Activator.CreateInstance(wrapperType)!;

                    wrapperType.GetProperty(nameof(ResponseData<object>.Data))?
                               .SetValue(instance, rawData);

                    wrapperType.GetProperty(nameof(ResponseData<object>.StatusCode))?
                               .SetValue(instance, status);

                    wrapperType.GetProperty(nameof(ResponseData<object>.Message))?
                               .SetValue(instance, "Operación exitosa");
                    return (R)instance;

                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Error deserializando el contenido plano a tipo {dataType}: {ex.Message}\nContenido: {content}");
                }

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
                try
                {
                    if (doc.RootElement.ValueKind == JsonValueKind.Object && doc.RootElement.TryGetProperty("data", out _))
                    {
                        var deserialized = JsonSerializer.Deserialize(content, wrapperType, options);
                        if (deserialized is R rWrapped)
                        {
                            rWrapped.StatusCode = status;
                            return rWrapped;
                        }
                    }

                    var rawData = JsonSerializer.Deserialize(doc.RootElement.GetRawText(), dataType, options);

                    var instance = Activator.CreateInstance(wrapperType)!;

                    wrapperType.GetProperty(nameof(ResponseData<object>.Data))?
                               .SetValue(instance, rawData);

                    wrapperType.GetProperty(nameof(ResponseData<object>.StatusCode))?
                               .SetValue(instance, status);

                    wrapperType.GetProperty(nameof(ResponseData<object>.Message))?
                               .SetValue(instance, "Operación exitosa");
                    return (R)instance;

                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Error deserializando el contenido plano a tipo {dataType}: {ex.Message}\nContenido: {content}");
                }

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
                try
                {
                    if (doc.RootElement.ValueKind == JsonValueKind.Object && doc.RootElement.TryGetProperty("data", out _))
                    {
                        var deserialized = JsonSerializer.Deserialize(content, wrapperType, options);
                        if (deserialized is R rWrapped)
                        {
                            rWrapped.StatusCode = status;
                            return rWrapped;
                        }
                    }

                    var rawData = JsonSerializer.Deserialize(doc.RootElement.GetRawText(), dataType, options);

                    var instance = Activator.CreateInstance(wrapperType)!;

                    wrapperType.GetProperty(nameof(ResponseData<object>.Data))?
                               .SetValue(instance, rawData);

                    wrapperType.GetProperty(nameof(ResponseData<object>.StatusCode))?
                               .SetValue(instance, status);

                    wrapperType.GetProperty(nameof(ResponseData<object>.Message))?
                               .SetValue(instance, "Operación exitosa");
                    return (R)instance;

                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Error deserializando el contenido plano a tipo {dataType}: {ex.Message}\nContenido: {content}");
                }

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
                try
                {
                    if (doc.RootElement.ValueKind == JsonValueKind.Object && doc.RootElement.TryGetProperty("data", out _))
                    {
                        var deserialized = JsonSerializer.Deserialize(content, wrapperType, options);
                        if (deserialized is R rWrapped)
                        {
                            rWrapped.StatusCode = status;
                            return rWrapped;
                        }
                    }

                    var rawData = JsonSerializer.Deserialize(doc.RootElement.GetRawText(), dataType, options);

                    var instance = Activator.CreateInstance(wrapperType)!;

                    wrapperType.GetProperty(nameof(ResponseData<object>.Data))?
                               .SetValue(instance, rawData);

                    wrapperType.GetProperty(nameof(ResponseData<object>.StatusCode))?
                               .SetValue(instance, status);

                    wrapperType.GetProperty(nameof(ResponseData<object>.Message))?
                               .SetValue(instance, "Operación exitosa");
                    return (R)instance;

                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Error deserializando el contenido plano a tipo {dataType}: {ex.Message}\nContenido: {content}");
                }

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
                try
                {
                    if (doc.RootElement.ValueKind == JsonValueKind.Object && doc.RootElement.TryGetProperty("data", out _))
                    {
                        var deserialized = JsonSerializer.Deserialize(content, wrapperType, options);
                        if (deserialized is R rWrapped)
                        {
                            rWrapped.StatusCode = status;
                            return rWrapped;
                        }
                    }

                    var rawData = JsonSerializer.Deserialize(doc.RootElement.GetRawText(), dataType, options);

                    var instance = Activator.CreateInstance(wrapperType)!;

                    wrapperType.GetProperty(nameof(ResponseData<object>.Data))?
                               .SetValue(instance, rawData);

                    wrapperType.GetProperty(nameof(ResponseData<object>.StatusCode))?
                               .SetValue(instance, status);

                    wrapperType.GetProperty(nameof(ResponseData<object>.Message))?
                               .SetValue(instance, "Operación exitosa");
                    return (R)instance;

                }
                catch (Exception ex)
                {
                    throw new InvalidOperationException($"Error deserializando el contenido plano a tipo {dataType}: {ex.Message}\nContenido: {content}");
                }

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
