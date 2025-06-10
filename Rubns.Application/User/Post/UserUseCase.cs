
namespace Rubns.Application.User.Post
{
    internal sealed class UserUseCase : IPostUserPort
    {
        IUserRepository UserRepository { get; }
        IEncryptionService EncryptionService { get; }
        ILogger Logger { get; }
        public UserUseCase(IEncryptionService encryptionService,
            IUserRepository userRepository,
            ILogger logger)
        {
            UserRepository = userRepository;
            EncryptionService = encryptionService;
            Logger = logger;
        }

        public async Task<bool> RegitserUserAsync(RegisterUserDTO registerUser)
        {
            bool result = false;
            try
            {
                string passTemp = EncryptionService.GeneratePassTemp(registerUser);
                int create = await UserRepository.RegisterAsync(registerUser, passTemp);
                result = create > 0 ? true : false;

            }
            catch (Exception e)
            {
                Logger.Error(e, "LogIn an error occurred: {ErrorMessage}", e.Message);
            }

            return result;
        }
    }
}
