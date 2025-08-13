namespace Rubns.Application.QR.Post
{
    internal class CreateQRUseCase(IQRRepositoryEFC repositoryEFC,
        ILogger logger,
        IUtils utils,
        IUserContextService userContextService)
        : IQRCreatePort<QRDTO>

    {
        private readonly IQRRepositoryEFC _repositoryEFC = repositoryEFC;
        private readonly ILogger _logger = logger;
        private readonly IUtils _utils = utils;
        private readonly IUserContextService _userContextService = userContextService;

        public async Task<QRDTO> CreateQRAsync(QRCreateDTO createDTO)
        {
            QRDTO qr = new();
            try
            {
                if (!string.IsNullOrEmpty(createDTO.Slug))
                {
                    createDTO.Slug = _utils.CreateSlug(createDTO.Slug);
                }
                else
                {
                    createDTO.Slug = _utils.CreateSlug(createDTO.Name);
                }

                var isExistSlug = await _repositoryEFC.FindSlugAsync(createDTO.Slug);
                if (!string.IsNullOrEmpty(isExistSlug))
                {
                    return qr;
                }

                createDTO.Url = $"{createDTO.Url}/qr/{createDTO.Slug}";

                qr = await _repositoryEFC.AddAsync(createDTO);
                qr.UserRegistered = _userContextService.Name;
                qr.UserLastModificated = _userContextService.Name;


            }
            catch (Exception e)
            {
                _logger.Error(e, "Error CreateQRAsync: {error}", e.Message);
            }

            return qr;
        }
    }
}
