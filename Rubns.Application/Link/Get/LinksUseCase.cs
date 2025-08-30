namespace Rubns.Application.Link.Get
{
    internal class LinksUseCase(ILinkRepositoryDapper linkRepositoryDapper
        , ILinkRepositoryEFC linkRepositoryEFC
        , ILogger logger
        , IGetLinksOurPort getLinksOurPort)
        : IGetLinksPort
    {

        private readonly ILinkRepositoryDapper _linkRepositoryDapper = linkRepositoryDapper;
        private readonly ILogger _logger = logger;
        private readonly ILinkRepositoryEFC _linkRepositoryEFC = linkRepositoryEFC;
        private readonly IGetLinksOurPort _getLinksOurPort = getLinksOurPort;

        public async Task GetPortsAsync(int page, int pagesize, string filter)
        {
            try
            {
                int total = 0;
                var links = await _linkRepositoryDapper.GetLinksAsync(page, pagesize, filter);
                if (links.Count >= pagesize)
                {
                    total = await _linkRepositoryEFC.CountLinksAsync();
                }
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
