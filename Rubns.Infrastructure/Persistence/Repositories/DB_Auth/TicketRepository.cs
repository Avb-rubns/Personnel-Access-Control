namespace Rubns.Infrastructure.Persistence.Repositories.DB_Auth
{
    internal class TicketRepository : ITicketRepository
    {
        private readonly AuthDbContextEFC AuthDbContextEFC;

        public TicketRepository(AuthDbContextEFC authDbContext)
        {
            AuthDbContextEFC = authDbContext;
        }
        public async Task<int> InsertCheckAsync(CheckDTO check, int userID)
        {
            try
            {
                using var connection = new SqlConnection(AuthDbContextEFC.Database.GetConnectionString());
                await connection.OpenAsync();
                var parameters = new { UserID = userID, Latitude = check.Latitude, Longitude = check.Longitude, Ip = check.IP?.ToString() };

                int result = await connection.ExecuteScalarAsync<int>(
                    "p_InsertCheckPersonal",
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
