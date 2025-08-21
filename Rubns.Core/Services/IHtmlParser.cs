namespace Rubns.Core.Services
{
    public interface IHtmlParser
    {
        public Dictionary<string, string> GetMeta(string html);
        public List<Dictionary<string, string>> ParseHead(string html);
        string ExtractTitle(string html);
    }
}
