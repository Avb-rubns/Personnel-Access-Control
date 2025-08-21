namespace Rubns.Core.Services
{
    public interface IWebPageFetcher
    {
        Task<string> GetFullHtmlAsync(string url);
        Task<string> GetHeadHtmlAsync(string url);
        Task<string> GetBodyHtmlAsync(string url);

    }
}
