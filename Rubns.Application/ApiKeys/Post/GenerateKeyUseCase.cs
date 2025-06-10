
namespace Rubns.Application.Register.Post
{
    internal class GenerateKeyUseCase : IGenerateKeyPort<ResponseData<string>>
    {
        IEncryptionService EncryptionService { get; }
        ILogger Logger { get; }

        public GenerateKeyUseCase(IEncryptionService encryptionService
            , ILogger logger)
        {
            EncryptionService = encryptionService;
            Logger = logger;
        }

        public ResponseData<string> GenerateKey(RegisterDTO register, HttpRequest? request)
        {
            ResponseData<string> response = new();
            try
            {


                var token = EncryptionService.GenerateApiKey(register);
                if (!string.IsNullOrEmpty(token))
                {
                    response.Data = token;
                    response.StatusCode = System.Net.HttpStatusCode.Created;
                    return response;

                }

            }
            catch (Exception e)
            {
                Logger.Error(e, "An error occurred: {ErrorMessage}", e.Message);
            }
            response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            response.Message = "Por el momento no se puede generar el apikey.";
            return response;
        }
    }
}
