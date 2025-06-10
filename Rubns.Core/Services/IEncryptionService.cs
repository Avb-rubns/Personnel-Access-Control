using Rubns.Core.DTOs.ApiKey;

namespace Rubns.Core.Services
{
    public interface IEncryptionService
    {
        string GenerateApiKey(RegisterDTO register);

        string GeneratePassTemp(RegisterUserDTO register);
        bool ValidatePass(string pass, string passUser);

    }
}
