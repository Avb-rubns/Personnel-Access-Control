namespace Rubns.Infrastructure.Persistence.Repositories.DB_Auth
{
    internal class UserRepositoryDapper : IUserRepositoryDapper
    {
        IConfiguration Configuration { get; }

        public UserRepositoryDapper(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public async Task<UserWithRolInfo> GetUserByEmailAsync(string email)
        {
            UserWithRolInfo result = new();
            try
            {
                await using var connection = new SqlConnection(Configuration.GetConnectionString("dbAuth"));
                await connection.OpenAsync();


                var proc = "p_UserByEmail";

                var user = await connection.QuerySingleOrDefaultAsync<UserWithRolInfoSpResultDb>(proc, new { email }, commandType: CommandType.StoredProcedure);
                await connection.CloseAsync();

                if (user is not null)
                {
                    result.UserId = user.UserId;
                    result.UserName = user.UserName;
                    result.Email = user.Email;
                    result.Password = user.Password;
                    result.RolName = user.Value;
                    result.LevelPermission = user.LevelPermission;
                    result.Status = user.Status;
                    result.Phone = user.Phone;
                    result.Registed = user.Registed;
                    result.RolId = user.RolId;
                }

            }
            catch
            {
                throw;
            }

            return result;
        }

        public async Task<UserWithRolInfo> GetUserByIDAsync(int id)
        {
            UserWithRolInfo result = new();
            try
            {
                await using var connection = new SqlConnection(Configuration.GetConnectionString("dbAuth"));
                await connection.OpenAsync();

                var proc = "p_UserByID";

                var user = await connection.QuerySingleOrDefaultAsync<UserWithRolInfoSpResultDb>(proc, new { id }, commandType: CommandType.StoredProcedure);
                await connection.CloseAsync();

                if (user is not null)
                {
                    result.UserId = user.UserId;
                    result.UserName = user.UserName;
                    result.Email = user.Email;
                    result.Password = user.Password;
                    result.RolName = user.Value;
                    result.LevelPermission = user.LevelPermission;
                    result.Status = user.Status;
                    result.Registed = user.Registed;
                    result.RolId = user.RolId;

                }

            }
            catch
            {
                throw;
            }

            return result;
        }

        public async Task<UserWithRolInfo> GetUserByPhoneAsync(string number)
        {
            UserWithRolInfo result = new();
            try
            {
                await using var connection = new SqlConnection(Configuration.GetConnectionString("dbAuth"));
                await connection.OpenAsync();

                var proc = "p_UserByPhone";

                var user = await connection.QuerySingleOrDefaultAsync<UserWithRolInfoSpResultDb>(proc, new { Phone = number }, commandType: CommandType.StoredProcedure);
                await connection.CloseAsync();

                if (user is not null)
                {
                    result.UserId = user.UserId;
                    result.UserName = user.UserName;
                    result.Email = user.Email;
                    result.Password = user.Password;
                    result.RolName = user.Value;
                    result.LevelPermission = user.LevelPermission;
                    result.Status = user.Status;
                    result.Phone = user.Phone;
                    result.RolId = user.RolId;
                    result.Registed = user.Registed;
                }

            }
            catch
            {
                throw;
            }

            return result;
        }

        public async Task<int> UpdateUserPasswordforUserIDAsync(int useID, string newPassword)
        {
            int result = 0;
            try
            {
                await using var connection = new SqlConnection(Configuration.GetConnectionString("dbAuth"));
                await connection.OpenAsync();

                var proc = "p_UpdateUserPasswordforUserID";

                result = await connection.ExecuteAsync(proc, new { UserID = useID, NewPassword = newPassword }, commandType: CommandType.StoredProcedure);
                await connection.CloseAsync();

            }
            catch
            {
                throw;
            }

            return result;
        }
    }
}
