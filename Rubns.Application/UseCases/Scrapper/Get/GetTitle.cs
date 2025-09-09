namespace Rubns.Application.UseCases.Scrapper.Get
{
    internal class GetTitle(
        IWebPageFetcher playwrighFetcher,
        ILogger logger,
        IHtmlParser regexParserHtml,
        IUtils utils,
        ITitleOutputPort titleOutputPort,
        ILinkRepositoryEFC qRRepositoryEFC)
        : ITitleInputPort
    {
        private readonly IWebPageFetcher _playwrighFetcher = playwrighFetcher;
        private readonly ILogger _logger = logger;
        private readonly IUtils _utils = utils;
        private readonly ILinkRepositoryEFC _qRRepositoryEF = qRRepositoryEFC;
        private readonly IHtmlParser _regexParserHtml = regexParserHtml;
        private readonly ITitleOutputPort _titleOutputPort = titleOutputPort;

        public async Task GetPortPort(string url)
        {
            Dictionary<string, string> result = new();
            try
            {
                if (string.IsNullOrEmpty(url))
                {
                    throw new ArgumentException("url");
                }

                var head = await _playwrighFetcher.GetHeadHtmlAsync(url);
                head = await _playwrighFetcher.GetHeadHtmlAsync(url);
                if (string.IsNullOrEmpty(head))
                {
                    throw new NotFoundException("El scrapper no puedo recuperar informacion", "No se recupero informacion.");
                }
                var title = _regexParserHtml.ExtractTitle(head);
                if (string.IsNullOrEmpty(title))
                {
                    throw new NotFoundException("El scrapper no puedo recuperar informacion", "No se recupero informacion.");
                }

                result["title"] = title;
                string slug = _utils.GenerateSlug(title);
                var exists = await _qRRepositoryEF.FindSlugAsync(slug);
                if (string.IsNullOrEmpty(exists))
                {
                    result["slug"] = slug;
                }

                await _titleOutputPort.Handler(result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error GetPortPort:{error}", ex.Message);
                throw;
            }

        }
    }
}
