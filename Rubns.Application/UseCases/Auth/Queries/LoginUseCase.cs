namespace Rubns.Application.UseCases.Auth.Queries
{
    internal sealed class LogInUseCase : ILoginUseCase
    {
        private readonly IEncryptionService _encryptionService;
        private readonly IUserRepositoryDapper _logInRepository;
        private readonly ILogInService _logInService;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly ISessionUserRepositoryEFC _sessionUserRepository;
        private readonly ILoginOutputPort _logInOutPort;
        public LogInUseCase(IEncryptionService encryptionService,
            IUserRepositoryDapper logInRepository,
            ILogInService logInService,
            ILogger logger,
            ISessionUserRepositoryEFC sessionUserRepository,
            IConfiguration configuration,
            ILoginOutputPort logInOutPort)
        {
            _encryptionService = encryptionService;
            _logInRepository = logInRepository;
            _logInService = logInService;
            _logger = logger;
            _sessionUserRepository = sessionUserRepository;
            _configuration = configuration;
            _logInOutPort = logInOutPort;
        }

        public async Task ExecuteAsync(LoginRequestDTO login)
        {
            JWT JWT = new();
            string RefreshToken = string.Empty;
            try
            {
                var user = await _logInRepository.GetUserByEmailAsync(login.Email);

                if (user is { Status: false })
                {
                    _logger.Information("Usuario no esta activo");
                    throw new LoginException("Credenciales incorrectas o usuario no registrado.", "Datos incorrectos");
                }

                if (user.Email is null)
                {
                    _logger.Information($"Usuario no existe:{login.Email}");
                    throw new LoginException("Credenciales incorrectas o usuario no registrado.", "Datos incorrectos");
                }
                if (!_encryptionService.ValidatePass(login.Password, user.Password))
                {
                    throw new LoginException("Credenciales incorrectas o usuario no registrado.", "Datos incorrectos");
                }

                JWT = _logInService.CreateJWT(user);
                RefreshToken = _logInService.CreateRefreshToken();
                if (JWT is null && string.IsNullOrEmpty(RefreshToken))
                {
                    _logger.Information($"No se genero la sesion para {login.Email}.");
                    throw new Exception();
                }
                var saveSessionUser = await _sessionUserRepository.AddAsync(user.UserId, RefreshToken);

                if (saveSessionUser <= 0)
                {
                    _logger.Information($"No se genero la sesion para {login.Email}.");
                    throw new Exception();
                }

                var Expiration = DateTimeOffset.UtcNow.AddDays(Convert.ToInt64(_configuration["DaysRefresh"])).ToUnixTimeSeconds();
                await _logInOutPort.Success(JWT, RefreshToken, Expiration);


            }
            catch (Exception e)
            {
                _logger.Error(e, "LogIn an error occurred: {ErrorMessage}", e.Message);
                throw;
            }

        }
    }
}
