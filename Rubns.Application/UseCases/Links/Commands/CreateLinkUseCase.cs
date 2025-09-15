using Personnel.Client.Shared.DTOs.Link.Commands;

namespace Rubns.Application.UseCases.Links.Commands
{
    internal class CreateLinkUseCase(ILinkRepositoryEFC repositoryEFC,
        ILogger logger,
        ISlugHelper slugHelper,
        LinkBuilder linkBuilder)
        : ICreateLinkUseCase

    {
        private readonly ILinkRepositoryEFC _repositoryEFC = repositoryEFC;
        private readonly LinkBuilder _linkBuilder = linkBuilder;
        private readonly ILogger _logger = logger;
        private readonly ISlugHelper _slugHelper = slugHelper;

        public async Task ExecuteAsync(LinkCreateDTO createDTO)
        {
            try
            {
                if (!string.IsNullOrEmpty(createDTO.Slug))
                {
                    createDTO.Slug = await _slugHelper.GenerateUniqueSlugByNameOrSlugAsync(createDTO.Slug);
                }
                else
                {
                    createDTO.Slug = await _slugHelper.GenerateUniqueSlugByNameOrSlugAsync(createDTO.Name);
                }

                var isExistSlug = await _repositoryEFC.FindBySlugAsync(createDTO.Slug);
                if (!string.IsNullOrEmpty(isExistSlug))
                {
                    throw new ResourceExistException("El nombre descriptivo ya existe.", "Enlace ya existenete");
                }

                createDTO.Url = $"{createDTO.Url}/qr/{createDTO.Slug}";

                Link create = _linkBuilder.WithName(createDTO.Name)
                               .WithSlug(createDTO.Slug)
                               .WithContent(createDTO.Content)
                               .WithURL(createDTO.Url)
                               .WithStatus(createDTO.Status)
                               .WithUserIdRegisted(createDTO.UserIDRegistered)
                               .WithUserLastIdModificated(createDTO.UserIDRegistered)
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
