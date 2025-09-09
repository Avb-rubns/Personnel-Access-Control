using Rubns.Application.Interface.Links.Patch;

namespace Rubns.Application.UseCases.Links.Patch
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

        public async Task UpdateQREditorPortAsync(int id, JsonPatchDocument<QRDTO> qr)
        {
            try
            {
                var link = await _linkRepositoryEFC.GetLinkForIdAsync(id);
                if (link is { ID: <= 0 })
                {
                    _logger.Information($"Se intento actualizar:{id} por usuario:{_userContextService.UserId}");
                    throw new Exception();
                }

                QRDTO qrEdit = new();

                qr.ApplyTo(qrEdit);
                QR update = new()
                {
                    DotScale = qrEdit.DotScale,
                    ColorDark = qrEdit.ColorDark,
                    ColorLight = qrEdit.ColorLight,
                    QuietZone = qrEdit.QuietZone,
                };
                int.TryParse(_userContextService.UserId, out int userId);
                await _linkRepositoryEFC.UpdateQRAsync(id, userId, update);

            }
            catch (Exception e)
            {
                _logger.Error(e, "Error in UpdateQREditorPortAsync:{error}", e.Message);
            }
        }
    }
}
