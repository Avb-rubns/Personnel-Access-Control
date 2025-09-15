namespace Rubns.Application.UseCases.Links.Commands
{
    internal class UpdateQRUseCase(IQRRepository qrRepositoryEFC,
        ILogger logger,
        IUserContextService userContext
        )
        : IUpdateQRUseCase
    {
        private readonly IQRRepository _qrRepositoryEFC = qrRepositoryEFC;
        private readonly ILogger _logger = logger;
        private readonly IUserContextService _userContextService = userContext;

        public async Task ExecuteAsync(int id, JsonPatchDocument<QRDTO> qr)
        {
            try
            {
                var qrUpdate = await _qrRepositoryEFC.FindById(id);
                if (qrUpdate is { Id: <= 0 })
                {
                    _logger.Information($"Se intento actualizar:{id} por usuario:{_userContextService.UserId}");
                    throw new NotFoundException("No se encotro el QR del enlace", "No se encontro informacion");
                }

                QRDTO qrEdit = new();

                qr.ApplyTo(qrEdit);

                int.TryParse(_userContextService.UserId, out int userId);

                QR update = new()
                {
                    Id = id,
                    DotScale = qrEdit.DotScale,
                    ColorDark = qrEdit.ColorDark,
                    ColorLight = qrEdit.ColorLight,
                    QuietZone = qrEdit.QuietZone,
                    LastUserID = userId,
                };
                await _qrRepositoryEFC.UpdateAsync(update);

            }
            catch (Exception e)
            {
                _logger.Error(e, "Error in UpdateQREditorPortAsync:{error}", e.Message);
                throw;
            }
        }
    }
}
