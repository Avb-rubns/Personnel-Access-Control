namespace Rubns.Application.Auth.Post
{
    internal class RefreshJWTUseCase : IRefreshJWTPort<RefreshTokenResponseDTO>
    {
        private readonly ISessionUserRepository SessionUserRepository;
        private readonly ILogInRepository LogInRepository;
        private readonly ILogInService LogInService;
        private readonly ILogger Logger;

        public RefreshJWTUseCase(ISessionUserRepository sessionUserRepository,
            ILogInRepository logInRepository,
            ILogInService logInService,
            ILogger logger)
        {
            SessionUserRepository = sessionUserRepository;
            LogInRepository = logInRepository;
            LogInService = logInService;
            Logger = logger;
        }
        public async Task<RefreshTokenResponseDTO> RefreshJWTAsync(string refreshRequest)
        {
            var response = new RefreshTokenResponseDTO();
            try
            {
                var session = await SessionUserRepository.FindAsyn(refreshRequest);
                var nzTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");
                DateTime nzDateTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.Utc, nzTimeZone);
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


                response.RefreshToken = refreshRequest;
                response.Token = newJwt;
                response.Expiration = new DateTimeOffset(session.Expiration, nzTimeZone.GetUtcOffset(session.Expiration)).ToUnixTimeSeconds();

            }
            catch (Exception e)
            {
                Logger.Error(e, "An error ocurred:{error}", e.Message);
            }


            return response;
        }

    }
}
