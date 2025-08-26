namespace Rubns.Application.Link.Get
{
    internal class LinksUseCase(ILinkRepositoryEFC qRRepositoryEFC
        , ILogger logger)
        : IGetLinksPort
    {

        private readonly ILinkRepositoryEFC _qRRepositoryEFC = qRRepositoryEFC;
        private readonly ILogger _logger = logger;

        public async Task<List<LinkDTO>> GetPortsAsync(int? page, int? pagesize, string? filter)
        {
            List<LinkDTO> qrs = new List<LinkDTO>();

            try
            {
                qrs = await _qRRepositoryEFC.GetAllLinksForPageAsync(page, pagesize, filter);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error GetPortsAsync:{error}", ex.Message);

            }
            return qrs;
        }
    }
}
