namespace Rubns.Core.Ports.Scrapper
{
    public interface IMetaProxyPort
    {
        Task<Dictionary<string, string>> GetMetaAsync(string url);
    }
}
