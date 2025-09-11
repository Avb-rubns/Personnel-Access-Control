namespace Rubns.Application.UseCases.Auth.Queries
{
    internal sealed class ResetPasswordValidateUseCase(IResetPasswordRepositoryEFC resetPasswordEFC
        , ILogger logger)
        : IValidateTokenUseCase
    {
        private readonly IResetPasswordRepositoryEFC _resetPasswordEFC = resetPasswordEFC;
        private readonly ILogger _logger = logger;

        public async Task ExecuteAsync(string token)
        {
            try
            {

                var resetPassword = await _resetPasswordEFC.FindbyTokenAsync(token);
                var referenceOffset = resetPassword.Registed.Offset;
                var nowWithSameOffset = DateTimeOffset.UtcNow.ToOffset(referenceOffset);

                if (resetPassword is
                    { ResetPasswordID: > 0 }
                && resetPassword.Registed.AddMinutes(15) < nowWithSameOffset)
                {
                    await _resetPasswordEFC.DeleteAsync(resetPassword);
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
