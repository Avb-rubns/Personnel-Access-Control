namespace Rubns.Application.Link.Post
{
    internal class LinkUseCase(ILinkRepositoryEFC repositoryEFC,
        ILogger logger,
        ISlugHelper slugHelper,
        IUserContextService userContextService)
        : ICreateLinkPort<LinkDTO>

    {
        private readonly ILinkRepositoryEFC _repositoryEFC = repositoryEFC;
        private readonly ILogger _logger = logger;
        private readonly ISlugHelper _slugHelper = slugHelper;
        private readonly IUserContextService _userContextService = userContextService;

        public async Task<LinkDTO> CreateQRAsync(LinkCreateDTO createDTO)
        {
            LinkDTO link = new();
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
                    return link;
                }

                createDTO.Url = $"{createDTO.Url}/qr/{createDTO.Slug}";

                link = await _repositoryEFC.AddAsync(createDTO);
                link.UserRegistered = _userContextService.Name;
                link.UserLastModificated = _userContextService.Name;


            }
            catch (Exception e)
            {
                _logger.Error(e, "Error CreateQRAsync: {error}", e.Message);
            }

            return link;
        }
    }
}
