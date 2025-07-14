
namespace Rubns.Application.Auth.Post
{
    internal class RefreshJWTUseCase : IRefreshJWTPort<RefreshTokenResponseDTO>
    {
        private readonly ISessionUserRepository SessionUserRepository;
        private readonly ILogInRepository LogInRepository;
        private readonly ILogInService LogInService;
        private readonly IConfiguration Configuration;

        public RefreshJWTUseCase(ISessionUserRepository sessionUserRepository,
            ILogInRepository logInRepository,
            ILogInService logInService,
            IConfiguration configuration)
        {
            SessionUserRepository = sessionUserRepository;
            LogInRepository = logInRepository;
            LogInService = logInService;
            Configuration = configuration;
        }
        public async Task<RefreshTokenResponseDTO> RefreshJWTAsync(string refreshRequest)
        {

            var response = new RefreshTokenResponseDTO();

            var session = await SessionUserRepository.FindAsyn(refreshRequest);
            var nzTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time (Mexico)");
            DateTime nzDateTime = TimeZoneInfo.ConvertTime(new DateTime(), TimeZoneInfo.Utc, nzTimeZone);
            if (session?.UserID <= 0 || session.Expiration <= nzDateTime)
            {
                return response;
            }

            var user = await LogInRepository.GetUserByIDAsync(session.UserID);
            if (user?.UserID <= 0)
            {
                return response;
            }

            var newJwt = LogInService.CreateJWT(user);
            var newRefreshToken = LogInService.CreateRefreshToken();
            if (newJwt is null || string.IsNullOrEmpty(newRefreshToken))
            {
                return response;
            }

            if (!double.TryParse(Configuration["DaysRefresh"], out double daysRefresh))
            {
                daysRefresh = 7;
            }

            session.Token = newRefreshToken;
            session.Expiration = DateTime.UtcNow.AddDays(daysRefresh);
            var updateResult = await SessionUserRepository.UpdateSessionUserAsync(session);
            if (updateResult <= 0)
            {
                return response;
            }

            response.RefreshToken = newRefreshToken;
            response.Token = newJwt;
            response.Expiration = DateTimeOffset.UtcNow.AddDays(daysRefresh).ToUnixTimeSeconds();

            return response;
        }

    }
}
