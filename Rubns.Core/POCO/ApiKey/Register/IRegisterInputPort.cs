namespace Rubns.Core.POCO.ApiKey.Register
{
    public interface IRegisterInputPort
    {
        Task RegisterAppAsync(RegisterDTO registerDTO, HttpRequest? httpRequest);
    }
}
