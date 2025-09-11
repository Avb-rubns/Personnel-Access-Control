using Rubns.Application.Interface.Auth.Queries;

namespace Rubns.WebAPI.Presenters.Auth
{
    public class LogInPresenter : ILoginOutputPort, IPresenter<AuthResponseDTO>
    {
        public AuthResponseDTO Result { get; set; }

        public Task Success(JWT jwt, string refreshToken, long expiration)
        {
            AuthResponseDTO authResponseDTO = new AuthResponseDTO()
            {
                AccessToken = jwt,
                RefreshToken = refreshToken,
                Expiration = expiration
            };
            Result = authResponseDTO;
            return Task.CompletedTask;
        }
    }
}
