namespace Rubns.Core.Ports.ApiKey.Register
{
    public interface IRegisterRepository
    {
        Task<int> RegisterApplicationAsync(TokenRegisterDTO token);
    }
}
