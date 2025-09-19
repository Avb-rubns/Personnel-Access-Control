namespace Rubns.Infrastructure.Persistence.Repositories.DB_Auth
{
    internal class QRRepositoryEFC(AuthDbContextEFC contextEFC)
        : IQRRepository
    {

        private readonly AuthDbContextEFC _authDbContextEFC = contextEFC;

        public async Task<QR> FindById(int id)
        {
            QR qr = new();

            QRDb findQR = new QRDb() { Id = id };

            var data = await _authDbContextEFC.QRs.SingleOrDefaultAsync(i => i.Id == id);

            if (data is { Id: > 0 })
            {
                qr.Id = data.Id;
                qr.LinkId = data.LinkId;
                qr.DotScale = data.DotScale;
                qr.ColorLight = data.ColorLight;
                qr.ColorDark = data.ColorDark;
                qr.QuietZone = data.QuietZone;
                qr.LastModificated = data.LastModificated;
                qr.LastUserID = data.LastUserID;
            }


            return qr;

        }

        public async Task<int> UpdateAsync(QR qr)
        {
            QRDb qrUpdate = new()
            {
                Id = qr.Id,
                DotScale = qr.DotScale,
                ColorDark = qr.ColorDark,
                ColorLight = qr.ColorLight,
                QuietZone = qr.QuietZone,
                LastUserID = qr.LastUserID,
            };

            _authDbContextEFC.Entry(qrUpdate).Property(u => u.DotScale).IsModified = qr.DotScale is not double.NaN;
            _authDbContextEFC.Entry(qrUpdate).Property(u => u.ColorDark).IsModified = !string.IsNullOrEmpty(qr.ColorDark);
            _authDbContextEFC.Entry(qrUpdate).Property(u => u.ColorLight).IsModified = !string.IsNullOrEmpty(qr.ColorLight); ;
            _authDbContextEFC.Entry(qrUpdate).Property(u => u.QuietZone).IsModified = true;
            _authDbContextEFC.Entry(qrUpdate).Property(u => u.LastUserID).IsModified = true;
            return await _authDbContextEFC.SaveChangesAsync();
        }
    }
}
