
namespace Rubns.Application.Register.Post
{
    internal class RegisterAppUseCase : IRegisterInputPort
    {
        IEncryptionService EncryptionService { get; }
        ILogger Logger { get; }
        public RegisterAppUseCase(IEncryptionService encryptionService, ILogger logger)
        {
            EncryptionService = encryptionService;
            Logger = logger;
        }

        public Task RegisterAppAsync(RegisterDTO registerDTO, HttpRequest? request)
        {
            try
            {
                var apikey = EncryptionService.GenerateApiKey(registerDTO);

            }
            catch (Exception e)
            {
                Logger.Error(e, "An error occurred: {ErrorMessage}", e.Message);

            }
            throw new NotImplementedException();
        }
    }
}
