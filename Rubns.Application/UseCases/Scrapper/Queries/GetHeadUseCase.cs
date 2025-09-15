namespace Rubns.Application.UseCases.Scrapper.Queries
{
    internal class GetHeadUseCase(
        IGetHeadOutputPort headOutputPort,
        ILogger logger,
        IWebPageFetcher PlaywrighFetcher,
        IHtmlParser RegexParserHtml)
        : IGetHeadUseCase
    {
        private readonly ILogger _logger = logger;
        private readonly IHtmlParser _regexParserHtml = RegexParserHtml;
        private readonly IWebPageFetcher _playwrightService = PlaywrighFetcher;
        private readonly IGetHeadOutputPort _headOutputPort = headOutputPort;



        public async Task ExecuteAsync(string url)
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

                await _headOutputPort.Success(result);

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error GetHeadAsync:{e} ", ex.Message);
                throw;
            }
        }
    }
}
