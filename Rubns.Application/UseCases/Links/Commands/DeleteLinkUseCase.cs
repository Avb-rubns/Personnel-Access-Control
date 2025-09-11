namespace Rubns.Application.UseCases.Links.Commands
{
    internal class DeleteLinkUseCase(ILinkRepositoryEFC linkRepositoryEFC
        , ILogger logger)
        : IDeleteLinkUseCase
    {
        private readonly ILinkRepositoryEFC _linkRepositoryEFC = linkRepositoryEFC;
        private readonly ILogger _logger = logger;
        public async Task ExecuteAsync(int id)
        {

            try
            {
                var link = await _linkRepositoryEFC.GetLinkByIdAsync(id);
                if (link is { ID: <= 0 })
                {
                    throw new NotFoundException("El enlace no existe.", "Enlace no encontrado");
                }

                await _linkRepositoryEFC.DeleteAsync(id);

            }
            catch (Exception e)
            {
                _logger.Error(e, "Error in LinkUseCase:{error}", e.Message);
                throw;
            }

        }
    }
}
