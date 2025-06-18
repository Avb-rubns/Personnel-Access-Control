using Personnel.Client.Shared.POCO.Abstractions;
using System.Net;

namespace Personnel.Client.Shared.POCO.ResponseAPI
{
    public class ResponseData<T> : IApiResponse
    {
        public T? Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string? Message { get; set; }
    }
}
