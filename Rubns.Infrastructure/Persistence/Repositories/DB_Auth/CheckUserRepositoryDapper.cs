using Rubns.Core.Entities.Checker;

namespace Rubns.Infrastructure.Persistence.Repositories.DB_Auth
{
    internal class CheckUserRepositoryDapper(IConfiguration configuration) : ICheckUserRepositoryDapper
    {
        private readonly IConfiguration _configuration = configuration;

        public async Task<List<UserCheck>> CheckUserTodayAsync()
        {
            List<UserCheck> result = new();

            try
            {
                await using var connection = new SqlConnection(_configuration.GetConnectionString("dbAuth"));
                await connection.OpenAsync();

                var proc = "p_CheckUserToday";

                var data = await connection.QueryAsync<CheckUserSpResult>
                    (proc,
                    commandType: CommandType.StoredProcedure);
                if (data.Count() > 0)
                {
                    result = data.Select(c => new UserCheck
                    {
                        Name = c.Name,
                        Rol = c.Rol,
                        AccessIn = c.AccessIn,
                        DistanceCheckIn = c.DistanceCheckIn,
                        HourCheckIn = c.HourCheckIn,
                        AccessOut = c.AccessOut,
                        DistanceCheckOut = c.DistanceCheckOut,
                        HourCheckOut = c.HourCheckOut
                    }).ToList();
                }

                return result;

            }
            catch
            {
                throw;
            }
        }
    }
}
