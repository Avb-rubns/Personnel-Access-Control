namespace Rubns.Application.Auth.Post
{
    internal class LogInUseCase : ILogInPort<AuthResponseDTO>
    {
        IEncryptionService EncryptionService { get; }
        IUserRepositoryDapper LogInRepository { get; }
        ILogInService LogInService { get; }
        ILogger Logger { get; }
        private readonly IConfiguration Configuration;
        ISessionUserRepositoryEFC SessionUserRepository { get; }
        public LogInUseCase(IEncryptionService encryptionService,
            IUserRepositoryDapper logInRepository,
            ILogInService logInService,
            ILogger logger,
            ISessionUserRepositoryEFC sessionUserRepository,
            IConfiguration configuration)
        {
            EncryptionService = encryptionService;
            LogInRepository = logInRepository;
            LogInService = logInService;
            Logger = logger;
            SessionUserRepository = sessionUserRepository;
            Configuration = configuration;
        }

        public async Task<AuthResponseDTO> LogIn(LoginRequestDTO login)
        {
            AuthResponseDTO auth = new();
            JWT jwt = new();
            string refreshToken = string.Empty;
            try
            {
                var user = await LogInRepository.GetUserByEmailAsync(login.Email);
                if (user.Email is null)
                {
                    return null;
                }
                if (!EncryptionService.ValidatePass(login.Password, user.Password))
                {
                    return null;
                }

                jwt = LogInService.CreateJWT(user);
                refreshToken = LogInService.CreateRefreshToken();
                if (jwt is not null && !string.IsNullOrEmpty(refreshToken))
                {

                    var saveSessionUser = await SessionUserRepository.AddSessionAsync(user.UserID, refreshToken);

                    if (saveSessionUser > 0)
                    {
                        auth.RefreshToken = refreshToken;
                        auth.AccessToken = jwt;
                        auth.Expiration = DateTimeOffset.UtcNow.AddDays(Convert.ToInt64(Configuration["DaysRefresh"])).ToUnixTimeSeconds();
                    }
                }

            }
            catch (Exception e)
            {
                Logger.Error(e, "LogIn an error occurred: {ErrorMessage}", e.Message);
            }

            return auth;
        }
    }
}
