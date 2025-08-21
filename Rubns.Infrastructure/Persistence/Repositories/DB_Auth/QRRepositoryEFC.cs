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

        public async Task<string> FindSlugAsync(string slug)
        {
            string result = string.Empty;

            var isExists = await _context.QRs
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Slug == slug);

            if (isExists is { QRID: > 0 })
            {
                result = isExists.Content;
            }

            return result;
        }

        public async Task<List<QRDTO>> GetAllQrsForPageAsync(int? page, int? pagesize, string? filter)
        {
            List<QRDTO> qrs = new List<QRDTO>();
            IEnumerable<QR> data;
            int pageSize = ((pagesize.HasValue && pagesize.Value > 0) ? pagesize.Value : 25);
            int pageNumber = ((page.HasValue && page.Value > 0) ? page.Value : 1);

            if (!string.IsNullOrEmpty(filter))
            {
                bool status = filter.Equals("true") ? true : false;
                data = await _context.QRs.Where(s => s.Status == status)
                                .OrderBy(id => id.QRID)
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .AsNoTracking()
                                .ToListAsync();
            }
            else
            {

                data = await _context.QRs.OrderBy(id => id.QRID)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .AsNoTracking()
                    .ToListAsync();
            }



            if (data.Count() > 0)
            {
                qrs = data.Select(s => new QRDTO
                {
                    ID = s.QRID,
                    Name = s.Name,
                    Slug = s.Slug,
                    Content = s.Content,
                    Url = s.Url,
                    DotScale = s.DotScale,
                    ColorDark = s.ColorDark,
                    ColorLight = s.ColorLight,
                    QuietZone = s.QuietZone,
                    Status = s.Status,
                    Registered = s.Registered,
                    LastModificated = s.LastModificated,

                }).ToList();
            }


            return qrs;
        }
    }
}
