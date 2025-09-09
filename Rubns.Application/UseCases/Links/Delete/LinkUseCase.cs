namespace Rubns.Application.UseCases.Links.Delete
{
    internal class LinkUseCase(ILinkRepositoryEFC linkRepositoryEFC
        , ILogger logger)
        : IDeleteLinkPort
    {
        private readonly ILinkRepositoryEFC _linkRepositoryEFC = linkRepositoryEFC;
        private readonly ILogger _logger = logger;
        public async Task DeleteLinkPortAsync(int id)
        {

            try
            {
                var link = await _linkRepositoryEFC.GetLinkForIdAsync(id);
                if (link is { ID: <= 0 })
                {
                    throw new NotFoundException("El enlace no existe.", "Enlace no encontrado");
                }

                await _linkRepositoryEFC.DeleteLinkAsync(id);

            }
            catch (Exception e)
            {
                _logger.Error(e, "Error in LinkUseCase:{error}", e.Message);
                throw;
            }

        }
    }
}
