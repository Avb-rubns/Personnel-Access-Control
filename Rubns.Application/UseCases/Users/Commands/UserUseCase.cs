namespace Rubns.Application.UseCases.Users.Commands
{
    internal sealed class UserUseCase : ICreateUserUseCase
    {
        private readonly IUserRepositoryEFC _userRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly ILogger _logger;
        private readonly ITemplateRepositoryDapper _templateRepositoryDapper;
        private readonly IProxyServer _proxyServer;
        private readonly IConfiguration _configuration;

        public UserUseCase(IEncryptionService encryptionService,
            IUserRepositoryEFC userRepository,
            ILogger logger,
            ITemplateRepositoryDapper templateRepositoryDapper,
            IProxyServer proxyServer,
            IConfiguration configuration)
        {
            _userRepository = userRepository;
            _encryptionService = encryptionService;
            _logger = logger;
            _templateRepositoryDapper = templateRepositoryDapper;
            _proxyServer = proxyServer;
            _configuration = configuration;
        }

        public async Task ExecuteAsync(RegisterUserDTO registerUser)
        {
            try
            {

                var UserFinded = await _userRepository.FindUserByPhoneOrEmailAsync(registerUser.Email, registerUser.Phone);
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

                string passTemp = _encryptionService.GeneratePassTemp(registerUser);
                int create = await _userRepository.RegisterAsync(userRegisted, passTemp);

                var mail = await _templateRepositoryDapper.GetMailRegistedAsync(registerUser);
                RequestMailDTO requestMail = new()
                {
                    To = registerUser.Email,
                    From = _configuration.GetSection("Maileroo")["Email"],
                    Html = mail,
                    Subject = "Registro de usuario",

                };
                var IsSendMail = await _proxyServer.PostAsFormDataAsync<HttpResponseMessage, RequestMailDTO>(
                                        "maileroo",
                                        "send",
                                        requestMail);
                switch (IsSendMail.StatusCode)
                {
                    default:
                        var content = await IsSendMail.Content.ReadFromJsonAsync<ResponseMailDTO>();
                        _logger.Error("Error Send MailRegister:{error}", content.Message);
                        break;

                }


            }
            catch (Exception e)
            {
                _logger.Error(e, "RegitserUserAsync an error occurred: {ErrorMessage}", e.Message);
                throw;
            }

        }
    }
}
