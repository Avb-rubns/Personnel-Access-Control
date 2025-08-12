namespace Rubns.Infrastructure.Persistence.Repositories.DB_Auth
{
    internal class QRRepositoryEFC(AuthDbContextEFC contextEFC)
        : IQRRepositoryEFC
    {

        private readonly AuthDbContextEFC _context = contextEFC;
        public async Task<QRDTO> AddAsync(QRCreateDTO qR)
        {
            QRDTO QRResult = new QRDTO();
            QR qr = new QR()
            {
                Name = qR.Name,
                Slug = qR.Slug,
                Content = qR.Content,
                Url = qR.Url,
                UserID = qR.UserIDRegistered,
                LastUserID = qR.UserIDRegistered,
                Status = qR.Status,
            };

            await _context.QRs.AddAsync(qr);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                QRResult.ID = qr.QRID;
                QRResult.Name = qR.Name;
                QRResult.Slug = qR.Slug;
                QRResult.Content = qR.Content;
                QRResult.Url = qR.Url;
                QRResult.Status = qR.Status;
                QRResult.Registered = qr.Registered;
                QRResult.LastModificated = qr.Registered;

            }


            return QRResult;
        }
    }
}
