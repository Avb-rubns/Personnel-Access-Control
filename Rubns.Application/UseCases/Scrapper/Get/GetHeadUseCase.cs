namespace Rubns.Application.UseCases.Scrapper.Get
{
    internal class GetHeadUseCase(
        IHeadOutputPort headOutputPort,
        ILogger logger,
        IWebPageFetcher PlaywrighFetcher,
        IHtmlParser RegexParserHtml)
        : IHeadInputPort
    {
        private readonly ILogger _logger = logger;
        private readonly IHtmlParser _regexParserHtml = RegexParserHtml;
        private readonly IWebPageFetcher _playwrightService = PlaywrighFetcher;
        private readonly IHeadOutputPort _headOutputPort = headOutputPort;



        public async Task GetHeadAsync(string url)
        {
            List<Dictionary<string, string>> result = new();

            try
            {
                if (string.IsNullOrEmpty(url))
                {
                    throw new ArgumentException("url");
                }

                var data = await _playwrightService.GetHeadHtmlAsync(url);
                if (string.IsNullOrEmpty(data))
                {
                    throw new NotFoundException("El scrapper no puedo recuperar informacion", "No se recupero informacion.");
                }

                result = _regexParserHtml.ParseHead(data);

                await _headOutputPort.Handler(result);

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error GetHeadAsync:{e} ", ex.Message);
                throw;
            }
        }
    }
}
