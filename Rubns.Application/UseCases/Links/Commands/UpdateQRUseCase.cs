namespace Rubns.Application.UseCases.Links.Commands
{
    internal class UpdateQRUseCase(IQRRepository qrRepositoryEFC,
        ILogger logger,
        IUserContextService userContext,
        ISqidService sqidService)
        : IUpdateQRUseCase
    {
        private readonly IQRRepository _qrRepositoryEFC = qrRepositoryEFC;
        private readonly ILogger _logger = logger;
        private readonly IUserContextService _userContextService = userContext;
        private readonly ISqidService _sqidService = sqidService;

        public async Task ExecuteAsync(string id, JsonPatchDocument<QRUpdateDTO> qr)
        {
            try
            {
                var idDecode = _sqidService.Decode(id);
                var userId = _sqidService.Decode(_userContextService.UserId);
                var qrUpdate = await _qrRepositoryEFC.FindById(idDecode);
                if (qrUpdate is { Id: <= 0 })
                {
                    _logger.Information($"Se intento actualizar:{idDecode} por usuario:{userId}");
                    throw new NotFoundException("No se encotro el QR del enlace", "No se encontro informacion");
                }

                QRUpdateDTO qrEdit = new();

                qr.ApplyTo(qrEdit);

                QR update = new()
                {
                    Id = idDecode,
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
