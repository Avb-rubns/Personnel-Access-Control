namespace Rubns.Infrastructure.Persistence.Repositories.DB_Auth
{
    internal class TicketRepository : ITicketRepository
    {
        private readonly IConfiguration Configuration;
        public TicketRepository(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public async Task<int> InsertAsync(CheckDTO check, int userID, string p)
        {
            try
            {
                using var connection = new SqlConnection(Configuration.GetConnectionString("dbAuth"));
                await connection.OpenAsync();
                var parameters = new
                {
                    UserID = userID,
                    Latitude = check.Latitude,
                    Longitude = check.Longitude,
                    Ip = check.IP?.ToString()
                };

                int result = await connection.ExecuteScalarAsync<int>(
                    p,
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                return result;

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
