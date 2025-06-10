using Rubns.Core.DTOs.LogOut;

namespace Rubns.Infrastructure.Persistence.Repositories.DB_Auth
{
    internal class SessionUserRepository : ISessionUserRepository
    {
        private readonly AuthDbContextEFC AuthDbContextEFC;
        private readonly IConfiguration Configuration;
        public SessionUserRepository(AuthDbContextEFC authDbContext, IConfiguration configuration)
        {
            AuthDbContextEFC = authDbContext;
            Configuration = configuration;
        }
        public async Task<int> AddSessionAsync(int userId, string token)
        {
            SessionUser sessionUser = new()
            {
                Token = token,
                UserID = userId,
                Expiration = DateTime.UtcNow.AddDays(Convert.ToDouble(Configuration["DaysRefresh"]))
            };

            await AuthDbContextEFC.AddAsync(sessionUser);
            return await AuthDbContextEFC.SaveChangesAsync();
        }

        public async Task<int> DeleteSessionAsync(string token)
        {
            try
            {
                using var connection = new SqlConnection(AuthDbContextEFC.Database.GetConnectionString());
                await connection.OpenAsync();
                var parameters = new { Token = token };

                int result = await connection.ExecuteScalarAsync<int>(
                    "p_DeleteSessionUser",
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

        public async Task<SessionUserDTO> FindAsyn(string token)
        {
            SessionUserDTO session = new();

            var sessionUser = await AuthDbContextEFC.SessionUser.SingleOrDefaultAsync(s => s.Token == token);

            if (sessionUser is { UserID: > 0 })
            {
                session.ID = sessionUser.ID;
                session.UserID = sessionUser.UserID;
                session.Token = sessionUser.Token;
                session.Expiration = sessionUser.Expiration;
            }


            return session;
        }

        public async Task<int> UpdateSessionUserAsync(SessionUserDTO session)
        {
            SessionUser sessionUser = new()
            {
                ID = session.ID,
                Token = session.Token,
                Expiration = session.Expiration
            };

            AuthDbContextEFC.SessionUser.Update(sessionUser);
            return await AuthDbContextEFC.SaveChangesAsync();
        }
    }
}
