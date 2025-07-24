namespace Rubns.Infrastructure.Persistence.Repositories.DB_Auth
{
    internal class UserRepositoryDapper : IUserRepositoryDapper
    {
        IConfiguration Configuration { get; }

        public UserRepositoryDapper(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public async Task<UserDTO> GetUserByEmailAsync(string email)
        {
            UserDTO result = new();
            try
            {
                await using var connection = new SqlConnection(Configuration.GetConnectionString("dbAuth"));
                await connection.OpenAsync();


                var proc = "p_UserByEmail";

                var user = await connection.QuerySingleOrDefaultAsync<UserDTO>(proc, new { email }, commandType: CommandType.StoredProcedure);
                await connection.CloseAsync();

                if (user is not null)
                {
                    result = user;
                }

            }
            catch
            {
                throw;
            }

            return result;
        }

        public async Task<UserDTO> GetUserByIDAsync(int id)
        {
            UserDTO result = new();
            try
            {
                await using var connection = new SqlConnection(Configuration.GetConnectionString("dbAuth"));
                await connection.OpenAsync();

                var proc = "p_UserByID";

                var user = await connection.QuerySingleOrDefaultAsync<UserDTO>(proc, new { id }, commandType: CommandType.StoredProcedure);
                await connection.CloseAsync();

                if (user is not null)
                {
                    result = user;
                }

            }
            catch
            {
                throw;
            }

            return result;
        }

        public async Task<UserDTO> GetUserByPhoneAsync(string number)
        {
            UserDTO result = new();
            try
            {
                await using var connection = new SqlConnection(Configuration.GetConnectionString("dbAuth"));
                await connection.OpenAsync();

                var proc = "p_UserByPhone";

                var user = await connection.QuerySingleOrDefaultAsync<UserDTO>(proc, new { Phone = number }, commandType: CommandType.StoredProcedure);
                await connection.CloseAsync();

                if (user is not null)
                {
                    result = user;
                }

            }
            catch
            {
                throw;
            }

            return result;
        }
    }
}
