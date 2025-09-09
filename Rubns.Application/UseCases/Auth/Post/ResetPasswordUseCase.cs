namespace Rubns.Application.UseCases.Auth.Post
{
    internal sealed class ResetPasswordUseCase(IResetPasswordEFC resetPasswordEFC
        , ILogger logger
        , IUserRepositoryEFC userRepositoryEFC
        , IEncryptionService encryptionService
        , ISessionUserRepositoryDapper sessionUserRepositoryDapper
        , IUserRepositoryDapper userRepositoryDapper)

        : IResetPasswordPort
    {
        private readonly IResetPasswordEFC _resetPasswordEFC = resetPasswordEFC;
        private readonly IUserRepositoryEFC _userRepositoryEFC = userRepositoryEFC;
        private readonly IEncryptionService _encryptionService = encryptionService;
        private readonly ILogger _logger = logger;
        private readonly ISessionUserRepositoryDapper _sessionUserRepositoryDapper = sessionUserRepositoryDapper;
        private readonly IUserRepositoryDapper _userRepositoryDapper = userRepositoryDapper;

        public async Task ResetPasswordAsync(ResetPasswordRequestDTO request)
        {
            try
            {

                var resetPassword = await _resetPasswordEFC.FindResetPasswordAsync(request.ResetCode);
                if (resetPassword is { ResetPasswordID: < 0 })
                {
                    //No tiene un token en para reinicio de contraseña.
                    throw new TokenInvalidException("El refresh token proporcionado no es válido o ya expiró.");
                }

                var referenceOffset = resetPassword.Registed.Offset;
                var nowWithSameOffset = DateTimeOffset.UtcNow.ToOffset(referenceOffset);

                if (resetPassword.Registed.AddMinutes(16) < nowWithSameOffset)
                {
                    await _resetPasswordEFC.DeleteResetPasswordAsync(resetPassword);
                    throw new TokenInvalidException("El refresh token proporcionado no es válido o ya expiró.");
                }

                var user = await _userRepositoryEFC.FindUserforIDAsync(resetPassword.UserId);
                if (user is { UserID: < 0 })
                {
                    //No existe el usuario.
                    throw new ArgumentException("Los datos proporcionados no son correctos.");
                }

                if (!request.Password.Equals(request.NewPassword))
                {
                    throw new ArgumentException("Las contraseñas no son iguales.");
                }

                var newPassword = _encryptionService.GenerateNewPass(request.NewPassword);

                user.Password = newPassword;

                var updatePass = await _userRepositoryDapper.UpdateUserPasswordforUserIDAsync(user.UserID, newPassword);
                await _resetPasswordEFC.DeleteResetPasswordAsync(resetPassword);
                if (updatePass > 0)
                {
                    var result = await _sessionUserRepositoryDapper.DeleteSessionforUserIdAsync(user.UserID);

                    if (result <= 0)
                    {
                        _logger.Error("No se cerro las sesion para:{0}", user.Email);

                    }
                }
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error ResetPasswordAsync: {error}", e.Message);
                throw;
            }
        }
    }
}
