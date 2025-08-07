namespace Rubns.Infrastructure.Persistence.Repositories.DB_Auth
{
    internal class CheckUserRepositoryDapper(IConfiguration configuration) : ICheckUserRepositoryDapper
    {
        private readonly IConfiguration _configuration = configuration;

        public async Task<List<CheckUserTodayDTO>> CheckUserTodayAsync()
        {
            List<CheckUserTodayDTO> result = new();

            try
            {
                await using var connection = new SqlConnection(_configuration.GetConnectionString("dbAuth"));
                await connection.OpenAsync();

                var proc = "p_CheckUserToday";

                var data = await connection.QueryAsync<CheckUserTodayDTO>
                    (proc,
                    commandType: CommandType.StoredProcedure);

                return data.Count() > 0 ? data.ToList() : result;

            }
            catch
            {
                throw;
            }
        }
    }
}
