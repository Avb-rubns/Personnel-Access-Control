namespace Rubns.Application.UseCases.Links.Queries
{
    internal sealed class GetURLDestinationBySlugUseCase(ILinkRepositoryEFC linkRepositoryEFC,
        ILogger logger,
        IGetURLDestinationBySlugOutputPort linkforSlugOutport,
        ICreateClickUseCase createClick)
        : IGetURLDestinationBySlugUseCase
    {

        private readonly ILinkRepositoryEFC _linkRepositoryEFC = linkRepositoryEFC;
        private readonly ILogger _logger = logger;
        private readonly IGetURLDestinationBySlugOutputPort _linkforSlugOutport = linkforSlugOutport;
        private readonly ICreateClickUseCase _createClickPort = createClick;
        public async Task ExecuteAsync(HttpRequest request, string slug)
        {
            try
            {
                var link = await _linkRepositoryEFC.GetLinkBySlugAsync(slug);
                if (link is { ID: <= 0 })
                {
                    _logger.Warning($"El slug:{slug} no se encontro.");
                    throw new Exception();
                }
                if (!link.Status)
                {
                    throw new ResourceInactiveException($"El slug:{slug} se encuentra desactivado.");
                }

                await _createClickPort.ExecuteAsync(request, link.ID);
                await _linkforSlugOutport.Success(link.Content);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error SearchLinkforSlug", e.Message);

            }

        }
    }
}
