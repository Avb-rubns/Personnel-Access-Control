
namespace Personnel.Client.Shared.Abstractions
{
    public interface IProxyServer
    {
        public Task<R> GetAsync<R>(string clientName, string url) where R : IApiResponse;
        public Task<R> GetStringAsync<R>(string clientName, string url);
        public Task<R> PostAsJsonAsync<R, S>(string clientName, string url, S postData);
        public Task<R> PostAsFormDataAsync<R, S>(string clientName, string url, S postData);
        public Task<R> PostAsync<R>(string clientName, string url);
        public Task<R> PutAsync<R, S>(string clientName, string url, S postData);
        public Task<R> DeleteAsync<R, S>(string clientName, string url, S PostData);
        public Task<R> DeleteAsync<R>(string clientName, string url);
    }
}
