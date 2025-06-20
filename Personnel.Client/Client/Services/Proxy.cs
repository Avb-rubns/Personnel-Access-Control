using System.Net.Http.Json;
using System.Text.Json;

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

        public async Task<ResponseData<T>> GetAsync<T>(string url, string userName = null)
        {
            if (!string.IsNullOrEmpty(userName))
            {
                if (!Client.DefaultRequestHeaders.Contains("User"))
                    Client.DefaultRequestHeaders.Add("User", userName);
            }

            var response = await Client.GetAsync(url);

            var result = new ResponseData<T>
            {
                StatusCode = response.StatusCode,
                Message = response.IsSuccessStatusCode ? "Operación exitosa" : "Error en la operación"
            };

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrWhiteSpace(content))
                {
                    try
                    {
                        var deserialized = JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });

                        result.Data = deserialized;
                    }
                    catch (JsonException)
                    {
                        result.Message = "No se pudo deserializar la respuesta.";
                    }
                }
                else
                {
                    result.Message = "Respuesta sin contenido.";
                }
            }

            return result;
        }

        public async Task<R> PostAsync<R, S>(string url, S postData, string userName = null)
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
                        return (R)(object)new Response
                        {
                            StatusCode = response.StatusCode,

                        };
                    }
                }
                catch (JsonException)
                {
                    throw new InvalidOperationException("No se pudo deserializar la respuesta del servidor.");
                }
            }

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
