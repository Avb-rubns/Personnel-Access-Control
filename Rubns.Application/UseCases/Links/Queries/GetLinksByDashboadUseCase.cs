namespace Rubns.Application.UseCases.Links.Queries
{
    internal class GetLinksByDashboadUseCase : IGetLinksByDashboadUseCase
    {
        private readonly ILinkRepositoryEFC _linkRepositoryEFC;
        private readonly ILogger _logger;
        private readonly IGetLinksByDashboarOutputPort _outputPort;

        public GetLinksByDashboadUseCase(ILinkRepositoryEFC linkRepositoryEFC,
            ILogger logger,
            IGetLinksByDashboarOutputPort outputPort)
        {
            _linkRepositoryEFC = linkRepositoryEFC;
            _logger = logger;
            _outputPort = outputPort;
        }

        public async Task ExecuteAsync(int page, int pageSize)
        {
            try
            {
                if (page <= 0)
                {
                    throw new ArgumentException("La pagina no esta en el rango permitido.");
                }
                if (pageSize <= 0)
                {
                    throw new ArgumentException("El numero de registros no puede ser cero.");
                }
                int total = 0;
                var links = await _linkRepositoryEFC.GetLinksTodayByPaginationAsync(page, pageSize);
                bool hasNextPage = page * pageSize < total;
                bool hasPreviousPage = page > 1;

                total = await _linkRepositoryEFC.CountLinksTodayByPaginationAsync();

                await _outputPort.Success(links, total, page, pageSize, hasNextPage, hasPreviousPage);

            }
            catch (Exception e)
            {
                _logger.Error(e, "Error in GetLinksByDashboadUseCase:{error}", e.Message);
                throw;

            }
        }
    }
}
