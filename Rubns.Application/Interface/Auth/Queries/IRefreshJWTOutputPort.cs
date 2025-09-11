namespace Rubns.Application.Interface.Auth.Queries
{
    public interface IRefreshJWTOutputPort : IPresenter<RefreshTokenResponseDTO>
    {
        Task Success(JWT jtw, string refreshToken, long expiration);
    }
}
