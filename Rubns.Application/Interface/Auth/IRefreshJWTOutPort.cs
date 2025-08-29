namespace Rubns.Application.Interface.Auth
{
    public interface IRefreshJWTOutPort : IPresenter<RefreshTokenResponseDTO>
    {
        Task Handler(JWT jtw, string refreshToken, long expiration);
    }
}
