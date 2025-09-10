namespace Rubns.Core.POCO.ApiKey.Register
{
    public interface IRegisterRepository
    {
        Task<int> RegisterApplicationAsync(TokenRegisterDTO token);
    }
}
