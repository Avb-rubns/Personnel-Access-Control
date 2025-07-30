using Personnel.Client.Shared.POCO.Abstractions;

namespace Personnel.Client.Shared.Abstractions
{
    public interface IProxy
    {
        public Task<R> DeleteAsync<R, S>(string url, S PostData, string userName) where R : IApiResponse;
        public Task<R> DeleteAsync<R>(string url, string userName) where R : IApiResponse;
        public Task<R> PostAsync<R, S>(string url, S postData, string userName = null) where R : IApiResponse;
        public Task<R> PostAsync<R>(string url, string userName = null) where R : IApiResponse;
        public Task<R> PatchAsync<R, S>(string url, S PathData) where R : IApiResponse;
        public Task<R> GetAsync<R>(string url, string userName = null) where R : IApiResponse;
        public Task<R> PutAsync<R, S>(string url, S postData, string userName = null);
        public Task<R> PostFileAsync<R, S>(string url, S PostFile, string userName = null);


    }
}
