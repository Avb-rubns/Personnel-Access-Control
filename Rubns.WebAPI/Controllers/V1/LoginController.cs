namespace Rubns.WebAPI.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class LoginController : ControllerBase
    {
        private readonly ILoginUseCase _loginUseCase;
        private readonly ILoginOutputPort _loginOutputPort;
        public LoginController(ILoginUseCase postLogIn, ILoginOutputPort logInOutPort)
        {
            _loginUseCase = postLogIn;
            _loginOutputPort = logInOutPort;
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LoginRequestDTO loginRequest)
        {
            await _loginUseCase.ExecuteAsync(loginRequest);

            var result = ((IPresenter<AuthResponseDTO>)_loginOutputPort).Result;
            var accessTokenCookie = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(result.AccessToken.ExpiresIn)),
                Path = "/"
            };
            Response.Cookies.Append("accessToken", result.AccessToken.AccessToken, accessTokenCookie);

            var refreshTokenCookie = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.FromUnixTimeSeconds(result.Expiration),
                Path = "/api/v1/auth"
            };
            Response.Cookies.Append("refreshToken", result.RefreshToken, refreshTokenCookie);
            return Ok(new { message = "Login exitoso" });

        }

    }
}
