namespace Rubns.Application.Auth.Post
{
    internal sealed class ResetPasswordUseCase(IResetPasswordEFC resetPasswordEFC
        , ILogger logger
        , IUserRepositoryEFC userRepositoryEFC
        , IEncryptionService encryptionService)

        : IResetPasswordPort
    {
        private readonly IResetPasswordEFC _resetPasswordEFC = resetPasswordEFC;
        private readonly IUserRepositoryEFC _userRepositoryEFC = userRepositoryEFC;
        private readonly IEncryptionService _encryptionService = encryptionService;
        private readonly ILogger _logger = logger;

        public async Task<bool> ResetPasswordAsync(ResetPasswordRequestDTO request)
        {
            try
            {
                var user = await _userRepositoryEFC.FindUserforEmailAsync(request.Email);
                if (user is { UserID: < 0 })
                {
                    return false;
                }
                var resetPassword = await _resetPasswordEFC.FindResetPasswordAsync(request.ResetCode);
                if (resetPassword is { ResetPasswordID: < 0 })
                {
                    return false;
                }
                if (resetPassword.Registed.AddMinutes(15) < DateTimeOffset.UtcNow.AddHours(-6))
                {

                    return false;
                }

                var newPassword = _encryptionService.GenerateNewPass(request.NewPassword);

                user.Password = newPassword;

                var updatePass = await _userRepositoryEFC.UpdateUserforIDAsync(user);
                await _resetPasswordEFC.DeleteResetPasswordAsync(resetPassword);

                return updatePass > 0;


            }
            catch (Exception e)
            {
                _logger.Error(e, "Error ResetPasswordAsync: {error}", e.Message);
            }
            return false;
        }
    }
}
