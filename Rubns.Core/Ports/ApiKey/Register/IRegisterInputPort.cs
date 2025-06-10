using Rubns.Core.DTOs.ApiKey;

namespace Rubns.Core.Ports.ApiKey.Register
{
    public interface IRegisterInputPort
    {
        Task RegisterAppAsync(RegisterDTO registerDTO, HttpRequest? httpRequest);
    }
}
