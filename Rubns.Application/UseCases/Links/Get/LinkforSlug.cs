namespace Rubns.Application.UseCases.Links.Get
{
    internal sealed class LinkforSlug(ILinkRepositoryEFC linkRepositoryEFC,
        ILogger logger,
        IGetLinkforSlugOutputport linkforSlugOutport,
        ICreateClickPort createClick)
        : IGetLinkforSlugInputPort
    {

        private readonly ILinkRepositoryEFC _linkRepositoryEFC = linkRepositoryEFC;
        private readonly ILogger _logger = logger;
        private readonly IGetLinkforSlugOutputport _linkforSlugOutport = linkforSlugOutport;
        private readonly ICreateClickPort _createClickPort = createClick;
        public async Task SearchLinkforSlug(HttpRequest request, string slug)
        {
            try
            {
                var link = await _linkRepositoryEFC.GetLinkForSlugAsync(slug);
                if (link is { ID: <= 0 })
                {
                    _logger.Warning($"El slug:{slug} no se encontro.");
                    throw new Exception();
                }
                if (!link.Status)
                {
                    throw new ResourceInactiveException($"El slug:{slug} se encuentra desactivado.");
                }

                await _createClickPort.CreateAsync(request, link.ID);
                await _linkforSlugOutport.Handler(link.Content);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error SearchLinkforSlug", e.Message);

            }

        }
    }
}
