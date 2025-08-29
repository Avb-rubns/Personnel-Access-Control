namespace Rubns.WebAPI.Presenters.Auth
{
    internal sealed class RefreshJWTPresenter : IRefreshJWTOutPort, IPresenter<RefreshTokenResponseDTO>
    {
        public RefreshTokenResponseDTO Content { get; set; }

        public Task Handler(JWT jtw, string refreshToken, long expiration)
        {
            RefreshTokenResponseDTO response = new()
            {
                RefreshToken = refreshToken,
                Expiration = expiration,
                Token = jtw
            };

            Content = response;

            return Task.CompletedTask;
        }
    }
}
