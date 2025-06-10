using Rubns.Core.POCO.Abstractions;
using System.Net;

namespace Rubns.Core.POCO.ResponseAPI
{
    public class Response: IApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string? Message { get; set; }
    }
}
