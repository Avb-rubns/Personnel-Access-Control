namespace Rubns.Application.UseCases.Links.Get
{
    internal class LinksUseCase(ILinkRepositoryDapper linkRepositoryDapper
        , ILinkRepositoryEFC linkRepositoryEFC
        , ILogger logger
        , IGetLinksOutputPort getLinksOurPort)
        : IGetLinksPort
    {

        private readonly ILinkRepositoryDapper _linkRepositoryDapper = linkRepositoryDapper;
        private readonly ILogger _logger = logger;
        private readonly ILinkRepositoryEFC _linkRepositoryEFC = linkRepositoryEFC;
        private readonly IGetLinksOutputPort _getLinksOurPort = getLinksOurPort;

        public async Task GetPortsAsync(int page, int pagesize, string filter)
        {
            try
            {
                int total = 0;
                var links = await _linkRepositoryDapper.GetLinksAsync(page, pagesize, filter);
                total = await _linkRepositoryEFC.CountLinksAsync();
                await _getLinksOurPort.Handler(links, total);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error GetPortsAsync:{error}", ex.Message);
                throw;
            }
        }
    }
}
