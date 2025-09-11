namespace Rubns.Application.UseCases.Auth.Commands
{
    internal sealed class ForgotPasswordUseCase(IResetPasswordRepositoryEFC resetPasswordEFC
        , ILogger logger
        , IUserRepositoryEFC userRepositoryEFC
        , IEncryptionService encryptionService
        , ITemplateRepositoryDapper templateRepositoryDapper
        , IProxyServer proxyServer
        , IConfiguration configuration) : IForgotPasswordUseCase
    {
        private readonly IResetPasswordRepositoryEFC _resetPasswordRepository = resetPasswordEFC;
        private readonly IUserRepositoryEFC _userRepositoryEFC = userRepositoryEFC;
        private readonly ILogger _logger = logger;
        private readonly IEncryptionService _encryptionService = encryptionService;
        private readonly ITemplateRepositoryDapper _templateRepositoryDapper = templateRepositoryDapper;
        private readonly IProxyServer _proxyServer = proxyServer;
        private readonly IConfiguration _configuration = configuration;

        public async Task ExecuteAsyn(ForgotPasswordDTO request, string host)
        {
            try
            {
                var user = await _userRepositoryEFC.FindUserforEmailAsync(request.Email);
                if (user is { UserID: <= 0 })
                {
                    _logger.Information("El correo no existe:{email}", request.Email);
                }

                var isReset = await _resetPasswordRepository.FindByUserIdAsync(user.UserID);
                if (isReset)
                {
                    _logger.Information("El correo ya tiene un token de reincio valido existe:{email}", request.Email);
                }

                string token = _encryptionService.GenerateTokenForgotPass(request.Email);

                ResetPassword resetPassword = new()
                {
                    UserId = user.UserID,
                    Token = token
                };


                string link = $"{host}/reset-password/{token}";
                var add = await _resetPasswordRepository.AddAsync(resetPassword);

                if (add > 0)
                {
                    var mail = await _templateRepositoryDapper.GetMailForgotPasswordAsync(user.Name, link);

                    RequestMailDTO requestMail = new()
                    {
                        To = request.Email,
                        From = _configuration.GetSection("Maileroo")["Email"],
                        Html = mail,
                        Subject = "Solicitaste una nueva contraseña",

                    };
                    var IsSendMail = await _proxyServer.PostAsFormDataAsync<HttpResponseMessage, RequestMailDTO>(
                                            "maileroo",
                                            "send",
                                            requestMail);
                    switch (IsSendMail.StatusCode)
                    {

                        default:
                            var content = await IsSendMail.Content.ReadFromJsonAsync<ResponseMailDTO>();
                            _logger.Error("Error Send MailForgotPassword:{error}", content.Message);
                            break;

                    }
                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error GeneratePasswordResetTokenAsync: {error}", ex.Message);
                throw;
            }
        }
    }
}
