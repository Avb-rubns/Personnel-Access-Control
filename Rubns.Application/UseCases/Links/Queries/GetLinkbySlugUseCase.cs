namespace Rubns.Application.UseCases.Links.Queries
{
    internal class GetLinkBySlugUseCase : IGetLinkBySlugUseCase
    {
        private readonly ILogger _logger;
        private readonly ILinkRepositoryEFC _linkRepositoryEFC;
        private readonly IGetLinkBySlugOutputPort _outputPort;
        private readonly ISqidService _sqidService;

        public GetLinkBySlugUseCase(ILogger logger, ILinkRepositoryEFC linkRepositoryEFC,
            IGetLinkBySlugOutputPort outputPort, ISqidService sqidService)
        {
            _linkRepositoryEFC = linkRepositoryEFC;
            _logger = logger;
            _outputPort = outputPort;
            _sqidService = sqidService;
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
                result.FriendlyId = _sqidService.Encode(result.ID);
                result.QR.FriendlyLinkId = result.FriendlyId;
                result.QR.FriendlyId = _sqidService.Encode(result.QR.Id);
                result.QR.FriendLastUserID = _sqidService.Encode(result.QR.LastUserID);
                result.Clicks.ForEach(s =>
                {
                    s.FriendlyId = _sqidService.Encode(s.ID);
                    s.FriendlyLinkId = result.FriendlyId;
                });
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
