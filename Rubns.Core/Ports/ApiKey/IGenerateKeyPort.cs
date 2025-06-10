using Rubns.Core.DTOs.ApiKey;

namespace Rubns.Core.Ports.ApiKey
{
    public interface IGenerateKeyPort<T>
    {
        T GenerateKey(RegisterDTO register, HttpRequest? request);
    }
}
