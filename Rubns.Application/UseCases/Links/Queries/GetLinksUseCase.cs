namespace Rubns.Application.UseCases.Links.Queries
{
    internal class GetLinksUseCase(ILinkRepositoryDapper linkRepositoryDapper
        , ILinkRepositoryEFC linkRepositoryEFC
        , ILogger logger
        , IGetLinksOutputPort getLinksOurPort)
        : IGetLinksUseCase
    {

        private readonly ILinkRepositoryDapper _linkRepositoryDapper = linkRepositoryDapper;
        private readonly ILogger _logger = logger;
        private readonly ILinkRepositoryEFC _linkRepositoryEFC = linkRepositoryEFC;
        private readonly IGetLinksOutputPort _getLinksOurPort = getLinksOurPort;

        public async Task ExecuteAsync(int page, int pagesize, string filter)
        {
            try
            {
                if (page <= 0)
                {
                    throw new ArgumentException("La pagina no esta en el rango permitido.");
                }
                if (pagesize <= 0)
                {
                    throw new ArgumentException("El numero de registros no puede ser cero.");
                }

                int total = 0;
                var links = await _linkRepositoryEFC.GetLinkWithClickByPaginationAsync(page, pagesize, filter);
                if (filter.Equals("all"))
                {
                    total = await _linkRepositoryEFC.CountLinksAsync();
                }
                else
                {
                    total = await _linkRepositoryDapper.CountLinks(filter);
                }
                bool hasNextPage = page * pagesize < total;
                bool hasPreviousPage = page > 1;
                await _getLinksOurPort.Success(links, total, page, pagesize, hasNextPage, hasPreviousPage);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error GetPortsAsync:{error}", ex.Message);
                throw;
            }
        }
    }
}
