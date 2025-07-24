namespace Rubns.Infrastructure.Services
{
    public class Proxy(IHttpClientFactory factory, IUtils utils) : IProxyServer
    {
        private readonly IHttpClientFactory _factory = factory;
        private readonly IUtils _utils = utils;
        public Task<R> DeleteAsync<R, S>(string clientName, string url, S PostData)
        {
            throw new NotImplementedException();
        }

        public Task<R> DeleteAsync<R>(string clientName, string url)
        {
            throw new NotImplementedException();
        }

        public Task<R> GetAsync<R>(string clientName, string url)
        {
            throw new NotImplementedException();
        }

        public async Task<R> PostAsFormDataAsync<R, S>(string clientName, string url, S postData)
        {
            var client = _factory.CreateClient(clientName);
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
    }
}
