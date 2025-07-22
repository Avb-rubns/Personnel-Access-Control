namespace Rubns.Infrastructure.Persistence.Repositories.DB_Auth
{
    internal sealed class RolRepository(AuthDbContextEFC contextEFC) : IRolRepository
    {
        AuthDbContextEFC Context { get; } = contextEFC;

        public async Task<List<RolDTO>> GetRolsAsync()
        {
            List<RolDTO> rols = new();

            var data = await Context.Rols
                .AsNoTracking()
                .ToListAsync();

            if (data.Count() > 0)
            {
                rols = data.Select(
                    r => new RolDTO()
                    {
                        RolID = r.RolID,
                        Name = r.Name,
                        Value = r.Value,
                        LevelPermission = r.LevelPermission,
                        Status = r.Status,
                    }).ToList();
            }

            return rols;
        }
    }
}
