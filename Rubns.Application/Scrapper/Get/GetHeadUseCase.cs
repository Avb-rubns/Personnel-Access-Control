namespace Rubns.Application.Scrapper.Get
{
    internal class GetHeadUseCase(IProxyServer proxyServer,
        ILogger logger,
        IWebPageFetcher PlaywrighFetcher,
        IHtmlParser RegexParserHtml,
        ILinkRepositoryEFC qRRepositoryEFC,
        IUtils utilitis)
        : IHeadPort<List<Dictionary<string, string>>>
    {
        readonly IProxyServer _proxyServer = proxyServer;
        private readonly ILogger _logger = logger;
        private readonly IHtmlParser _regexParserHtml = RegexParserHtml;
        private readonly IWebPageFetcher _playwrightService = PlaywrighFetcher;
        readonly IUtils _utils = utilitis;
        readonly ILinkRepositoryEFC _qRRepositoryEFC = qRRepositoryEFC;



        public async Task<List<Dictionary<string, string>>> GetHeadAsync(string url)
        {
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

            try
            {
                var data = await _playwrightService.GetHeadHtmlAsync(url);
                if (string.IsNullOrEmpty(data))
                {
                    return result;
                }

                result = _regexParserHtml.ParseHead(data);

            }
            catch (Exception ex) { }
            return result;
        }
    }
}
