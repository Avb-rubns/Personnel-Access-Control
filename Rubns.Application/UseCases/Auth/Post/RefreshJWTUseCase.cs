namespace Rubns.Application.UseCases.Auth.Post
{
    internal sealed class RefreshJWTUseCase : IRefreshJWTInPort
    {
        private readonly ISessionUserRepositoryEFC _sessionUserRepository;
        private readonly IUserRepositoryDapper _logInRepository;
        private readonly ILogInService _logInService;
        private readonly ILogger _logger;
        private readonly IRefreshJWTOutPort _refreshJWTOutPort;

        public RefreshJWTUseCase(ISessionUserRepositoryEFC sessionUserRepository,
            IUserRepositoryDapper logInRepository,
            ILogInService logInService,
            ILogger logger,
            IRefreshJWTOutPort refreshJWTOutPort)
        {
            _sessionUserRepository = sessionUserRepository;
            _logInRepository = logInRepository;
            _logInService = logInService;
            _logger = logger;
            _refreshJWTOutPort = refreshJWTOutPort;
        }
        public async Task RefreshJWTAsync(string refreshRequest)
        {
            var response = new RefreshTokenResponseDTO();
            try
            {
                var session = await _sessionUserRepository.FindAsyn(refreshRequest);
                var nzTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");
                DateTime nzDateTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, TimeZoneInfo.Utc, nzTimeZone);
                if (session?.UserID <= 0 || session.Expiration <= nzDateTime)
                {
                    throw new TokenInvalidException("El refresh token proporcionado no es válido o ya expiró.");
                }

                var user = await _logInRepository.GetUserByIDAsync(session.UserID);
                if (user?.UserId <= 0)
                {
                    //Usuario no existe
                    _logger.Information("El usuario:{0} no existe.", session.ID);
                    throw new ArgumentException("Los datos proporcionados no son correctos.");
                }

                var newJwt = _logInService.CreateJWT(user);

                long expiration = new DateTimeOffset(session.Expiration, nzTimeZone.GetUtcOffset(session.Expiration)).ToUnixTimeSeconds();

                await _refreshJWTOutPort.Handler(newJwt, refreshRequest, expiration);

            }
            catch (Exception e)
            {
                _logger.Error(e, "An error ocurred:{error}", e.Message);
                throw;
            }

        }

    }
}
