namespace Rubns.Infrastructure.Persistence.Repositories.DB_Auth
{
    internal class ClickRepository(AuthDbContextEFC authDbContextEFC)
        : IClickRepositoryEFC
    {
        private readonly AuthDbContextEFC _contextEFC = authDbContextEFC;

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
