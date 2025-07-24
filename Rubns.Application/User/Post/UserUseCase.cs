using Personnel.Client.Shared.DTOs.Maileroo;
using System.Net;
using System.Net.Http.Json;

namespace Rubns.Application.User.Post
{
    internal sealed class UserUseCase : IPostUserPort
    {
        IUserRepositoryEFC UserRepository { get; }
        IEncryptionService EncryptionService { get; }
        ILogger Logger { get; }
        ITemplateRepositoryDapper TemplateRepositoryDapper { get; }
        IProxyServer ProxyServer { get; }
        IConfiguration Configuration { get; }

        public UserUseCase(IEncryptionService encryptionService,
            IUserRepositoryEFC userRepository,
            ILogger logger,
            ITemplateRepositoryDapper templateRepositoryDapper,
            IProxyServer proxyServer,
            IConfiguration configuration)
        {
            UserRepository = userRepository;
            EncryptionService = encryptionService;
            Logger = logger;
            TemplateRepositoryDapper = templateRepositoryDapper;
            ProxyServer = proxyServer;
            Configuration = configuration;
        }

        public async Task<int> RegisterUserAsync(RegisterUserDTO registerUser)
        {
            int result = 400;
            try
            {

                var UserFinded = await UserRepository.FindUserAsync(registerUser);
                if (UserFinded is { UserID: <= 0 })
                {
                    string passTemp = EncryptionService.GeneratePassTemp(registerUser);
                    int create = await UserRepository.RegisterAsync(registerUser, passTemp);

                    if (create > 0)
                    {
                        var mail = await TemplateRepositoryDapper.GetMailRegistedAsync(registerUser);
                        RequestMailDTO requestMail = new()
                        {
                            To = registerUser.Email,
                            From = Configuration.GetSection("Maileroo")["Email"],
                            Html = mail,
                            Subject = "Registro de usuario",

                        };
                        var IsSendMail = await ProxyServer.PostAsFormDataAsync<HttpResponseMessage, RequestMailDTO>(
                                                "maileroo",
                                                "send",
                                                requestMail);
                        switch (IsSendMail.StatusCode)
                        {
                            case HttpStatusCode.OK:
                                return 201;
                            default:
                                var content = await IsSendMail.Content.ReadFromJsonAsync<ResponseMailDTO>();
                                Logger.Error("Error Send MailRegister:{error}", content.Message);
                                return 201;

                        }


                    }


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
