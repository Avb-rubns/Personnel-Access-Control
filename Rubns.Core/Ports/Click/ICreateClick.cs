namespace Rubns.Core.Ports.Click
{
    public interface ICreateClick
    {
        Task<string> CreateClick(HttpRequest request, string slug);
    }
}
