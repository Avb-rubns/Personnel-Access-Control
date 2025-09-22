
namespace Rubns.Infrastructure.Persistence.Repositories.DB_Auth
{
    internal class ClickRepository(AuthDbContextEFC authDbContextEFC)
        : IClickRepositoryEFC
    {
        private readonly AuthDbContextEFC _contextEFC = authDbContextEFC;

        public async Task<List<Click>> GetClicksAsync(int id, DateTime starDate, DateTime endDate)
        {
            List<Click> clicks = new List<Click>();


            var data = await _contextEFC.Clicks
                .AsNoTracking()
                .Where(clic => (clic.LinkId == id && clic.ClickedAt >= starDate && clic.ClickedAt <= endDate))
                .ToListAsync();

            if (data.Count() > 0)
            {
                clicks = data.Select(
                    clic => new Click
                    {
                        ID = clic.ID,
                        LinkId = clic.LinkId,
                        ClickedAt = clic.ClickedAt,
                        Country = clic.Country,
                        Region = clic.Region,
                        City = clic.City,
                        DeviceType = clic.DeviceType,
                        OS = clic.OS,
                        Browser = clic.Browser,
                        IpAddress = clic.IpAddress,

                    }).ToList();
            }
            return clicks;
        }

        public async Task<int> InsertAsync(Click click)
        {
            ClickDb ClicNew = new()
            {
                LinkId = click.LinkId,
                Country = click.Country,
                Region = click.Region,
                City = click.City,
                DeviceType = click.DeviceType,
                OS = click.OS,
                Browser = click.Browser,
                IpAddress = click.IpAddress
            };

            await _contextEFC.Clicks.AddAsync(ClicNew);

            return await _contextEFC.SaveChangesAsync();
        }
    }
}
