namespace Rubns.Application.UseCases.Auth.Post
{
    internal sealed class LogInUseCase : ILogInPort
    {
        private readonly IEncryptionService _encryptionService;
        private readonly IUserRepositoryDapper _logInRepository;
        private readonly ILogInService _logInService;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly ISessionUserRepositoryEFC _sessionUserRepository;
        private readonly ILogInOutPort _logInOutPort;
        public LogInUseCase(IEncryptionService encryptionService,
            IUserRepositoryDapper logInRepository,
            ILogInService logInService,
            ILogger logger,
            ISessionUserRepositoryEFC sessionUserRepository,
            IConfiguration configuration,
            ILogInOutPort logInOutPort)
        {
            _encryptionService = encryptionService;
            _logInRepository = logInRepository;
            _logInService = logInService;
            _logger = logger;
            _sessionUserRepository = sessionUserRepository;
            _configuration = configuration;
            _logInOutPort = logInOutPort;
        }

        public async Task LogIn(LoginRequestDTO login)
        {
            JWT JWT = new();
            string RefreshToken = string.Empty;
            try
            {
                var user = await _logInRepository.GetUserByEmailAsync(login.Email);

                if (user is { Status: false })
                {
                    throw new Exception("Usuario no esta activo");
                }

                if (user.Email is null)
                {
                    throw new Exception("Usuario no existe");
                }
                if (!_encryptionService.ValidatePass(login.Password, user.Password))
                {
                    throw new Exception("Usuario no existe");
                }

                JWT = _logInService.CreateJWT(user);
                RefreshToken = _logInService.CreateRefreshToken();
                if (JWT is null && string.IsNullOrEmpty(RefreshToken))
                {
                    throw new Exception("No se genero la sesion.");
                }
                var saveSessionUser = await _sessionUserRepository.AddSessionAsync(user.UserId, RefreshToken);

                if (saveSessionUser <= 0)
                {
                    throw new Exception("No se genero la sesion.");
                }

                var Expiration = DateTimeOffset.UtcNow.AddDays(Convert.ToInt64(_configuration["DaysRefresh"])).ToUnixTimeSeconds();
                await _logInOutPort.Handler(JWT, RefreshToken, Expiration);


            }
            catch (Exception e)
            {
                _logger.Error(e, "LogIn an error occurred: {ErrorMessage}", e.Message);
            }

        }
    }
}
