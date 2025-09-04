namespace Rubns.Application.Scrapper.Get
{
    internal class GetMeta(IProxyServer proxyServer,
        ILogger logger,
        IHtmlParser webScrappynService,
        IWebPageFetcher PlaywrighFetcher,
        ILinkRepositoryEFC qRRepositoryEFC,
        IUtils utilitis)
        : IMetaProxyPort
    {

        readonly IProxyServer _proxyServer = proxyServer;
        private readonly ILogger _logger = logger;
        private readonly IHtmlParser _webScrappynService = webScrappynService;
        private readonly IWebPageFetcher _playwrighFetcher = PlaywrighFetcher;
        readonly IUtils _utils = utilitis;
        readonly ILinkRepositoryEFC _qRRepositoryEFC = qRRepositoryEFC;

        public async Task<Dictionary<string, string>> GetMetaAsync(string url)
        {
            Dictionary<string, string> metas = new();
            try
            {
                var data = await _proxyServer.GetStringAsync<string>("clean", url);
                metas = _webScrappynService.GetMeta(data);

                if (!metas.TryGetValue("title", out var title) || string.IsNullOrEmpty(title))
                {
                    data = await _playwrighFetcher.GetHeadHtmlAsync(url);
                    if (string.IsNullOrEmpty(data))
                        return metas;

                    metas = _webScrappynService.GetMeta(data);
                    if (!metas.TryGetValue("title", out title) || string.IsNullOrEmpty(title))
                        return metas;
                }

                // Generar slug si no existe
                var slug = _utils.GenerateSlug(title);
                var exists = await _qRRepositoryEFC.FindSlugAsync(slug);
                if (string.IsNullOrEmpty(exists))
                {
                    metas.Add("slug", slug);
                }

                return metas;


            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error GetMetaAsync:{e} ", ex.Message);
            }
            return metas;
        }
    }
}
