using Personnel.Client.Shared.DTOs.Maileroo;
using System.Net;
using System.Net.Http.Json;

namespace Rubns.Application.Auth.Post
{
    internal sealed class ForgotPasswordAsync(IResetPasswordEFC resetPasswordEFC
        , ILogger logger
        , IUserRepositoryEFC userRepositoryEFC
        , IEncryptionService encryptionService
        , ITemplateRepositoryDapper templateRepositoryDapper
        , IProxyServer proxyServer
        , IConfiguration configuration) : IForgotPassword
    {
        private readonly IResetPasswordEFC _resetPasswordRepository = resetPasswordEFC;
        private readonly IUserRepositoryEFC _userRepositoryEFC = userRepositoryEFC;
        private readonly ILogger _logger = logger;
        private readonly IEncryptionService _encryptionService = encryptionService;
        private readonly ITemplateRepositoryDapper _templateRepositoryDapper = templateRepositoryDapper;
        private readonly IProxyServer _proxyServer = proxyServer;
        private readonly IConfiguration _configuration = configuration;

        public async Task<bool> GeneratePasswordResetTokenAsync(ForgotPasswordDTO request, string host)
        {
            try
            {
                var user = await _userRepositoryEFC.FindUserforEmailAsync(request.Email);
                if (user is { UserID: <= 0 })
                {
                    _logger.Information("El correo no existe:{email}", request.Email);
                    return false;
                }

                var isReset = await _resetPasswordRepository.FindUserIDResetPasswordAsync(user.UserID);
                if (isReset)
                {
                    return false;
                }

                string token = _encryptionService.GenerateTokenForgotPass(request.Email);

                ResetPasswordDTO resetPassword = new()
                {
                    UserId = user.UserID,
                    Token = token
                };


                string link = $"{host}/reset-password/{token}";
                var add = await _resetPasswordRepository.AddResetPasswordAsync(resetPassword);

                if (add > 0)
                {
                    var mail = await _templateRepositoryDapper.GetMailForgotPasswordAsync(user.UserName, link);

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
                        case HttpStatusCode.OK:
                            return true;
                        default:
                            var content = await IsSendMail.Content.ReadFromJsonAsync<ResponseMailDTO>();
                            _logger.Error("Error Send MailForgotPassword:{error}", content.Message);
                            return true;

                    }
                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error GeneratePasswordResetTokenAsync: {error}", ex.Message);
            }
            return false;
        }
    }
}
