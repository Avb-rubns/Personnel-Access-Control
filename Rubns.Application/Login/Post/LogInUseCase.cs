
using Microsoft.Extensions.Configuration;

namespace Rubns.Application.Login.Post
{
    internal class LogInUseCase : ILogInPort<AuthResponseDTO>
    {
        IEncryptionService EncryptionService { get; }
        ILogInRepository LogInRepository { get; }
        ILogInService LogInService { get; }
        ILogger Logger { get; }
        private readonly IConfiguration Configuration;
        ISessionUserRepository SessionUserRepository { get; }
        public LogInUseCase(IEncryptionService encryptionService,
            ILogInRepository logInRepository,
            ILogInService logInService,
            ILogger logger,
            ISessionUserRepository sessionUserRepository,
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
                        auth.Expiration = DateTimeOffset.UtcNow.AddMinutes(Convert.ToInt64(Configuration["JWT:Expiration"])).ToUnixTimeSeconds();

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
