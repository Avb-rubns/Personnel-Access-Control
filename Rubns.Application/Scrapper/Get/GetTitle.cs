using Rubns.Core.Ports.Link;

namespace Rubns.Application.Scrapper.Get
{
    internal class GetTitle(IProxyServer proxyServer,
        IWebPageFetcher playwrighFetcher,
        ILogger logger,
        IHtmlParser regexParserHtml,
        IUtils utils,
        ILinkRepositoryEFC qRRepositoryEFC)
        : ITitlePort<Dictionary<string, string>>
    {
        readonly IProxyServer _proxyServer = proxyServer;
        readonly IWebPageFetcher _playwrighFetcher = playwrighFetcher;
        readonly ILogger _logger = logger;
        readonly IUtils _utils = utils;
        readonly ILinkRepositoryEFC _qRRepositoryEF = qRRepositoryEFC;
        readonly IHtmlParser _regexParserHtml = regexParserHtml;

        public async Task<Dictionary<string, string>> GetPortPort(string url)
        {
            Dictionary<string, string> result = new();
            try
            {

                var head = await _playwrighFetcher.GetHeadHtmlAsync(url);
                var title = _regexParserHtml.ExtractTitle(head);
                if (string.IsNullOrEmpty(title))
                {
                    head = await _playwrighFetcher.GetHeadHtmlAsync(url);
                    if (string.IsNullOrEmpty(head))
                        return result;

                    title = _regexParserHtml.ExtractTitle(head);
                    if (string.IsNullOrEmpty(title))
                        return result;

                }

                result["title"] = title;
                string slug = _utils.GenerateSlug(title);
                var exists = await _qRRepositoryEF.FindSlugAsync(slug);
                if (string.IsNullOrEmpty(exists))
                {
                    result["slug"] = slug;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error GetPortPort:{error}", ex.Message);
            }

            return result;
        }
    }
}
