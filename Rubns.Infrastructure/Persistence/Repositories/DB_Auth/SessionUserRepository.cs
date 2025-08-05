namespace Rubns.Infrastructure.Persistence.Repositories.DB_Auth
{
    internal class SessionUserRepository : ISessionUserRepositoryEFC
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
