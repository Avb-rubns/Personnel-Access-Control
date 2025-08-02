namespace Rubns.Core.Services
{
    public interface IEncryptionService
    {
        string GenerateApiKey(RegisterDTO register);

        string GeneratePassTemp(RegisterUserDTO register);
        string GenerateNewPass(string newPass);
        bool ValidatePass(string pass, string passUser);
        string GenerateTokenForgotPass(string email);

    }
}
