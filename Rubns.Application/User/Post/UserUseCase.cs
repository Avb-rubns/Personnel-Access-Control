
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

        public async Task<int> RegitserUserAsync(RegisterUserDTO registerUser)
        {
            int result = 400;
            try
            {

                var UserFinded = await UserRepository.FindUserAsync(registerUser);
                if (UserFinded is { UserID: <= 0 })
                {
                    string passTemp = EncryptionService.GeneratePassTemp(registerUser);
                    int create = await UserRepository.RegisterAsync(registerUser, passTemp);
                    result = create > 0 ? 201 : 500;

                }


            }
            catch (Exception e)
            {
                Logger.Error(e, "RegitserUserAsync an error occurred: {ErrorMessage}", e.Message);
                result = 500;
            }

            return result;
        }
    }
}
