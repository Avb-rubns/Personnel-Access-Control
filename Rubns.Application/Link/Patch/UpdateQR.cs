namespace Rubns.Application.Link.Patch
{
    internal class UpdateQR(ILinkRepositoryEFC linkRepositoryEFC,
        ILogger logger,
        IUserContextService userContext
        )
        : IUpdateQREditorPort
    {
        private readonly ILinkRepositoryEFC _linkRepositoryEFC = linkRepositoryEFC;
        private readonly ILogger _logger = logger;
        private readonly IUserContextService _userContextService = userContext;

        public async Task<int> UpdateQREditorPortAsync(int id, JsonPatchDocument<QRDTO> qr)
        {
            int result = -1;

            try
            {
                var link = await _linkRepositoryEFC.GetLinkForIdAsync(id);
                if (link is { ID: <= 0 })
                {
                    return 0;
                }

                QRDTO qrEdit = new();

                qr.ApplyTo(qrEdit);

                Int32.TryParse(_userContextService.UserId, out int userId);
                var update = await _linkRepositoryEFC.UpdateQRAsync(id, userId, qrEdit);

                return update;


            }
            catch (Exception e)
            {
                _logger.Error(e, "Error in UpdateQREditorPortAsync:{error}", e.Message);
            }

            return result;
        }
    }
}
