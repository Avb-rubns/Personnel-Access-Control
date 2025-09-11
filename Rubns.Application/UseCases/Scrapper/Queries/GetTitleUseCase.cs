namespace Rubns.Application.UseCases.Scrapper.Get
{
    internal class GetTitleUseCase(
        IWebPageFetcher playwrighFetcher,
        ILogger logger,
        IHtmlParser regexParserHtml,
        IUtils utils,
        IGetTitleOutputPort titleOutputPort,
        ILinkRepositoryEFC qRRepositoryEFC)
        : IGetTitleUseCase
    {
        private readonly IWebPageFetcher _playwrighFetcher = playwrighFetcher;
        private readonly ILogger _logger = logger;
        private readonly IUtils _utils = utils;
        private readonly ILinkRepositoryEFC _qRRepositoryEF = qRRepositoryEFC;
        private readonly IHtmlParser _regexParserHtml = regexParserHtml;
        private readonly IGetTitleOutputPort _titleOutputPort = titleOutputPort;

        public async Task ExecuteAsync(string url)
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
                var exists = await _qRRepositoryEF.FindBySlugAsync(slug);
                if (string.IsNullOrEmpty(exists))
                {
                    result["slug"] = slug;
                }

                await _titleOutputPort.Success(result);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error GetPortPort:{error}", ex.Message);
                throw;
            }

        }
    }
}
