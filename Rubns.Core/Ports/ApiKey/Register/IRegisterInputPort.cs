namespace Rubns.Core.Ports.ApiKey.Register
{
    public interface IRegisterInputPort
    {
        Task RegisterAppAsync(RegisterDTO registerDTO, HttpRequest? httpRequest);
    }
}
