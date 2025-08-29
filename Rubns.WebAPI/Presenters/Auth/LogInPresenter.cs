namespace Rubns.WebAPI.Presenters.Auth
{
    public class LogInPresenter : ILogInOutPort, IPresenter<AuthResponseDTO>
    {
        public AuthResponseDTO Content { get; set; }

        public Task Handler(JWT jwt, string refreshToken, long expiration)
        {
            AuthResponseDTO authResponseDTO = new AuthResponseDTO()
            {
                AccessToken = jwt,
                RefreshToken = refreshToken,
                Expiration = expiration
            };
            Content = authResponseDTO;
            return Task.CompletedTask;
        }
    }
}
