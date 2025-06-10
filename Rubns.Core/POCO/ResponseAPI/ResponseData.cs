using Rubns.Core.POCO.Abstractions;
using System.Net;

namespace Rubns.Core.POCO.ResponseAPI
{
    public class ResponseData<T> : IApiResponse
    {
        public T? Data { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string? Message { get; set; }
    }
}
