using System.Net;

namespace Personnel.Client.Shared.POCO.Abstractions
{
    public interface IApiResponse
    {
        HttpStatusCode StatusCode { get; set; }
        string? Message { get; set; }
    }
}
