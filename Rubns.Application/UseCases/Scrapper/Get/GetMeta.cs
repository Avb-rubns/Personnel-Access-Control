namespace Rubns.Application.UseCases.Scrapper.Get
{
    internal sealed class GetMeta(IProxyServer proxyServer,
        ILogger logger,
        IHtmlParser webScrappynService,
        IWebPageFetcher PlaywrighFetcher,
        ILinkRepositoryEFC qRRepositoryEFC,
        IMetasOutputPort metasOutputPort,
        IUtils utilitis)
        : IMetaProxyInputPort
    {

        private readonly IProxyServer _proxyServer = proxyServer;
        private readonly ILogger _logger = logger;
        private readonly IHtmlParser _webScrappynService = webScrappynService;
        private readonly IWebPageFetcher _playwrighFetcher = PlaywrighFetcher;
        private readonly IUtils _utils = utilitis;
        private readonly ILinkRepositoryEFC _qRRepositoryEFC = qRRepositoryEFC;
        private readonly IMetasOutputPort _metasOutputPort = metasOutputPort;

        public async Task GetMetaAsync(string url)
        {
            Dictionary<string, string> metas = new();
            try
            {
                if (string.IsNullOrEmpty(url))
                {
                    throw new ArgumentException("url");
                }

                var data = await _proxyServer.GetStringAsync<string>("clean", url);
                metas = _webScrappynService.GetMeta(data);

                if (!metas.TryGetValue("title", out var title) || string.IsNullOrEmpty(title))
                {
                    data = await _playwrighFetcher.GetHeadHtmlAsync(url);
                    if (string.IsNullOrEmpty(data))
                    {
                        throw new NotFoundException("El scrapper no puedo recuperar informacion", "No se recupero informacion.");
                    }

                    metas = _webScrappynService.GetMeta(data);
                    if (!metas.TryGetValue("title", out title) || string.IsNullOrEmpty(title))
                    {
                        throw new NotFoundException("El scrapper no puedo recuperar informacion", "No se recupero informacion.");
                    }

                }

                // Generar slug si no existe
                var slug = _utils.GenerateSlug(title);
                var exists = await _qRRepositoryEFC.FindSlugAsync(slug);
                if (string.IsNullOrEmpty(exists))
                {
                    metas.Add("slug", slug);
                }

                await _metasOutputPort.Handeler(metas);

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error GetMetaAsync:{e} ", ex.Message);
                throw;
            }
        }
    }
}
