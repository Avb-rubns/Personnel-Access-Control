namespace Rubns.Infrastructure.Persistence.Repositories.DB_Auth
{
    internal sealed class SessionUserRepositoryDapper(IConfiguration configuration) : ISessionUserRepositoryDapper
    {
        IConfiguration Configuration = configuration;
        public async Task<int> DeleteSessionforTokenAsync(string token)
        {
            try
            {
                await using var connection = new SqlConnection(Configuration.GetConnectionString("dbAuth"));
                await connection.OpenAsync();

                var result = await connection.ExecuteAsync("p_DeleteSessionUser",
                    new { token },
                    commandType: CommandType.StoredProcedure);


                return result;

            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<int> DeleteSessionforUserIdAsync(int userID)
        {
            try
            {
                await using var connection = new SqlConnection(Configuration.GetConnectionString("dbAuth"));
                await connection.OpenAsync();

                var proc = "p_DeleteSessionUserforUserID";

                var result = await connection.ExecuteAsync(proc, new { userID }, commandType: CommandType.StoredProcedure);

                return result;

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
