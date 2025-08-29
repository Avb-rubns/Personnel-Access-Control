namespace Rubns.Application.Interface.Auth
{
    public interface ILogInOutPort
    {
        Task Handler(JWT jwt, string refreshToken, long Expiration);
    }
}
