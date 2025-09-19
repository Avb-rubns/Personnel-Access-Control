namespace Rubns.Application.UseCases.Links.Commands
{
    internal class DeleteLinkUseCase(ILinkRepositoryEFC linkRepositoryEFC
        , ISqidService sqidService
        , ILogger logger)
        : IDeleteLinkUseCase
    {
        private readonly ILinkRepositoryEFC _linkRepositoryEFC = linkRepositoryEFC;
        private readonly ILogger _logger = logger;
        private readonly ISqidService _sqidService = sqidService;

        public async Task ExecuteAsync(string id)
        {
            try
            {
                var IdDecode = _sqidService.Decode(id);

                var link = await _linkRepositoryEFC.GetLinkByIdAsync(IdDecode);
                if (link is { ID: <= 0 })
                {
                    throw new NotFoundException("El enlace no existe.", "Enlace no encontrado");
                }

                await _linkRepositoryEFC.DeleteAsync(IdDecode);

            }
            catch (Exception e)
            {
                _logger.Error(e, "Error in LinkUseCase:{error}", e.Message);
                throw;
            }

        }
    }
}
