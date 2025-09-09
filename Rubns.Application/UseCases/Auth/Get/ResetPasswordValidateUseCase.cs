namespace Rubns.Application.UseCases.Auth.Get
{
    internal sealed class ResetPasswordValidateUseCase(IResetPasswordEFC resetPasswordEFC
        , ILogger logger)
        : IResetPasswordValidatePort
    {
        private readonly IResetPasswordEFC _resetPasswordEFC = resetPasswordEFC;
        private readonly ILogger _logger = logger;

        public async Task ValidateTokenPasswordAsync(string token)
        {
            try
            {

                var resetPassword = await _resetPasswordEFC.FindResetPasswordAsync(token);
                var referenceOffset = resetPassword.Registed.Offset;
                var nowWithSameOffset = DateTimeOffset.UtcNow.ToOffset(referenceOffset);

                if (resetPassword is
                    { ResetPasswordID: > 0 }
                && resetPassword.Registed.AddMinutes(15) < nowWithSameOffset)
                {
                    await _resetPasswordEFC.DeleteResetPasswordAsync(resetPassword);
                    throw new TokenInvalidException("El refresh token proporcionado no es válido o ya expiró.");
                }


            }
            catch (Exception e)
            {
                _logger.Error(e, "Error ValidateTokenPasswordAsync:{error}", e.Message);
                throw;
            }
        }
    }
}
