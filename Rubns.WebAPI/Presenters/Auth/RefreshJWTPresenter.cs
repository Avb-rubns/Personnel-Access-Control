using Rubns.Application.Interface.Auth.Queries;

namespace Rubns.WebAPI.Presenters.Auth
{
    internal sealed class RefreshJWTPresenter : IRefreshJWTOutputPort
    {
        public RefreshTokenResponseDTO Result { get; set; }

        public Task Success(JWT jtw, string refreshToken, long expiration)
        {
            RefreshTokenResponseDTO response = new()
            {
                RefreshToken = refreshToken,
                Expiration = expiration,
                Token = jtw
            };

            Result = response;

            return Task.CompletedTask;
        }
    }
}
