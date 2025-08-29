namespace Rubns.Application.Interface.Auth
{
    public interface IRefreshJWTOutPort
    {
        Task Handler(JWT jtw, string refreshToken, long expiration);
    }
}
