namespace Rubns.Application.UseCases.Links.Queries
{
    internal class GetLinkBySlugUseCase : IGetLinkBySlugUseCase
    {
        private readonly ILogger _logger;
        private readonly ILinkRepositoryEFC _linkRepositoryEFC;
        private readonly IGetLinkBySlugOutputPort _outputPort;

        public GetLinkBySlugUseCase(ILogger logger, ILinkRepositoryEFC linkRepositoryEFC,
            IGetLinkBySlugOutputPort outputPort)
        {
            _linkRepositoryEFC = linkRepositoryEFC;
            _logger = logger;
            _outputPort = outputPort;
        }

        public async Task ExecuteAsync(string slug)
        {
            try
            {
                if (string.IsNullOrEmpty(slug))
                {
                    throw new ArgumentException("Datos invalidos");
                }

                var result = await _linkRepositoryEFC.GetLinkBySlugAsync(slug);
                if (result is { ID: <= 0 })
                {
                    throw new NotFoundException("No se encontro el enlace.", "Sin informacion");
                }
                await _outputPort.Success(result);

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetLinkBySlugUseCase:{error}", ex.Message);
                throw;
            }
        }
    }
}
