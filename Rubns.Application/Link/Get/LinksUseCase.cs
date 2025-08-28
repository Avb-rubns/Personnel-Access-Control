namespace Rubns.Application.Link.Get
{
    internal class LinksUseCase(ILinkRepositoryDapper linkRepositoryDapper
        , ILinkRepositoryEFC linkRepositoryEFC
        , ILogger logger)
        : IGetLinksPort
    {

        private readonly ILinkRepositoryDapper _linkRepositoryDapper = linkRepositoryDapper;
        private readonly ILogger _logger = logger;
        private readonly ILinkRepositoryEFC _linkRepositoryEFC = linkRepositoryEFC;

        public async Task<TableLinkDTO> GetPortsAsync(int page, int pagesize, string filter)
        {
            TableLinkDTO table = new TableLinkDTO();
            List<LinkTableDTO> links = new List<LinkTableDTO>();
            int total = 0;

            try
            {

                links = await _linkRepositoryDapper.GetLinksAsync(page, pagesize, filter);
                if (links.Count() >= pagesize)
                {
                    total = await _linkRepositoryEFC.CountLinksAsync();
                }

                table.Links = links;
                table.Total = links.Count() >= pagesize ? total : links.Count();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error GetPortsAsync:{error}", ex.Message);

            }
            return table;
        }
    }
}
