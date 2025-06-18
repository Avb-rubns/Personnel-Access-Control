namespace Rubns.Core.Ports.Login
{
    public interface IRefreshJWTPort<T>
    {
        Task<T> RefreshJWTAsync(RefreshTokenRequestDTO refresh);
    }
}
