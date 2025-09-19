namespace Rubns.Application.UseCases.Links.Commands
{
    internal class UpdateLinkUseCase(ILogger logger,
        ILinkRepositoryEFC linkRepositoryEFC,
        ISqidService sqidService,
        IUserContextService userContext,
        ISlugHelper slugHelper)
        : IUpdateLinkUseCase
    {
        private readonly ILogger _logger = logger;
        private readonly ILinkRepositoryEFC _linkRepositoryEFC = linkRepositoryEFC;
        private readonly ISqidService _sqidService = sqidService;
        private readonly IUserContextService _userContextService = userContext;
        private readonly ISlugHelper _slugHelper = slugHelper;

        public async Task ExecuteAsync(string id, JsonPatchDocument<LinkUpdateDTO> link)
        {
            try
            {
                var IdDecode = _sqidService.Decode(id);
                var UserId = _sqidService.Decode(_userContextService.UserId);
                var LinkUpdate = await _linkRepositoryEFC.GetLinkByIdAsync(IdDecode);
                if (LinkUpdate is { ID: <= 0 })
                {
                    _logger.Information($"Se intento actualizar el link:{IdDecode} por usuario:{UserId}");
                    throw new NotFoundException("No se encotro el enlace", "No se encontro informacion");
                }

                LinkUpdateDTO LinEdit = new();

                link.ApplyTo(LinEdit);

                Link Update = new()
                {
                    ID = IdDecode,
                    Name = LinkUpdate.Name != LinEdit.Name ? LinEdit.Name : string.Empty,
                    Content = LinkUpdate.Content != LinEdit.Content ? LinEdit.Content : string.Empty,
                };
                if (!string.IsNullOrWhiteSpace(LinEdit.Slug)
                    &&
                    !string.Equals(LinkUpdate.Slug, LinEdit.Slug, StringComparison.OrdinalIgnoreCase))
                {
                    Update.Slug = await _slugHelper.GenerateUniqueSlugByNameOrSlugAsync(LinEdit.Slug.ToLower());
                }
                else
                {
                    Update.Slug = LinkUpdate.Slug; // mantener el actual si no cambió
                }

                // reconstruir URL con el nuevo slug
                var baseUrl = LinkUpdate.Url.Substring(0, LinkUpdate.Url.LastIndexOf(LinkUpdate.Slug));
                Update.Url = $"{baseUrl}{Update.Slug}";

                await _linkRepositoryEFC.UpdateAsync(Update);

            }
            catch (Exception e)
            {
                _logger.Error(e, "Error in UpdateLinkUseCase:{error}", e.Message);
                throw;
            }
        }
    }
}
