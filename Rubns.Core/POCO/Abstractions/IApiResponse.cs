using System.Net;

namespace Rubns.Core.POCO.Abstractions
{
    public interface IApiResponse
    {
        HttpStatusCode StatusCode { get; set; }
        string? Message { get; set; }
    }
}
