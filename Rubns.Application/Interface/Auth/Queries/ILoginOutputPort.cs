namespace Rubns.Application.Interface.Auth.Queries
{
    public interface ILoginOutputPort
    {
        Task Success(JWT jwt, string refreshToken, long Expiration);
    }
}
