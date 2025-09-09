using Rubns.Application.Interface.Users;

namespace Rubns.Application.UseCases.Users.Post
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

        public async Task RegisterUserAsync(RegisterUserDTO registerUser)
        {
            try
            {

                var UserFinded = await UserRepository.FindUserByPhoneOrEmailAsync(registerUser.Email, registerUser.Phone);
                if (UserFinded is { UserID: >= 0 })
                {
                    throw new ResourceExistException($"El usuario:{registerUser.Email} ya esta registrado.", "El elemento ya existe");
                }


                User userRegisted = new User
                {
                    Name = registerUser.UserName,
                    Email = registerUser.Email,
                    LastName = registerUser.LastName,
                    Phone = registerUser.Phone,
                    RolID = registerUser.RolID,
                    Status = registerUser.Status,

                };

                string passTemp = EncryptionService.GeneratePassTemp(registerUser);
                int create = await UserRepository.RegisterAsync(userRegisted, passTemp);

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
                    default:
                        var content = await IsSendMail.Content.ReadFromJsonAsync<ResponseMailDTO>();
                        Logger.Error("Error Send MailRegister:{error}", content.Message);
                        break;

                }


            }
            catch (Exception e)
            {
                Logger.Error(e, "RegitserUserAsync an error occurred: {ErrorMessage}", e.Message);
                throw;
            }

        }
    }
}
