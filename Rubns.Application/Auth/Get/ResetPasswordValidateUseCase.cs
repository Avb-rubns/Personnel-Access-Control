
namespace Rubns.Application.Auth.Get
{
    internal sealed class ResetPasswordValidateUseCase(IResetPasswordEFC resetPasswordEFC
        , ILogger logger)
        : IResetPasswordValidatePort
    {
        private readonly IResetPasswordEFC _resetPasswordEFC = resetPasswordEFC;
        private readonly ILogger _logger = logger;

        public async Task<bool> ValidateTokenPasswordAsync(string token)
        {
            try
            {

                var resetPassword = await _resetPasswordEFC.FindResetPasswordAsync(token);
                return resetPassword is
                { ResetPasswordID: > 0 }
                && resetPassword.Registed.AddMinutes(15) < DateTimeOffset.UtcNow.AddHours(-6);

            }
            catch (Exception e)
            {
                _logger.Error(e, "Error ValidateTokenPasswordAsync:{error}", e.Message);
            }

            return false;
        }
    }
}
