using Personnel.Client.Shared.POCO.Abstractions;
using System.Net;

namespace Personnel.Client.Shared.POCO.ResponseAPI
{
    public class Response : IApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string? Message { get; set; }
    }
}
