namespace Rubns.Application.UseCases.Links.Post
{
    internal class LinkUseCase(ILinkRepositoryEFC repositoryEFC,
        ILogger logger,
        ISlugHelper slugHelper,
        LinkBuilder linkBuilder)
        : ICreateLinkPort

    {
        private readonly ILinkRepositoryEFC _repositoryEFC = repositoryEFC;
        private readonly LinkBuilder _linkBuilder = linkBuilder;
        private readonly ILogger _logger = logger;
        private readonly ISlugHelper _slugHelper = slugHelper;

        public async Task CreateQRAsync(LinkCreateDTO createDTO)
        {
            try
            {
                if (!string.IsNullOrEmpty(createDTO.Slug))
                {
                    createDTO.Slug = await _slugHelper.GenerateUniqueSlugAsync(createDTO.Slug);
                }
                else
                {
                    createDTO.Slug = await _slugHelper.GenerateUniqueSlugAsync(createDTO.Name);
                }

                var isExistSlug = await _repositoryEFC.FindSlugAsync(createDTO.Slug);
                if (!string.IsNullOrEmpty(isExistSlug))
                {
                    throw new Exception();
                }

                createDTO.Url = $"{createDTO.Url}/qr/{createDTO.Slug}";

                Link create = _linkBuilder.WithName(createDTO.Name)
                               .WithSlug(createDTO.Slug)
                               .WithContent(createDTO.Content)
                               .WithURL(createDTO.Url)
                               .WithStatus(createDTO.Status)
                               .WithUserId(createDTO.UserIDRegistered)
                               .Build();

                await _repositoryEFC.AddAsync(create);

            }
            catch (Exception e)
            {
                _logger.Error(e, "Error CreateQRAsync: {error}", e.Message);
            }
        }
    }
}
