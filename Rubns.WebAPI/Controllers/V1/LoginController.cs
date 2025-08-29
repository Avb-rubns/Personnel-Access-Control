namespace Rubns.WebAPI.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class LoginController : ControllerBase
    {
        private readonly ILogInPort _logInPort;
        private readonly ILogInOutPort _logOutPort;
        public LoginController(ILogInPort postLogIn, ILogInOutPort logInOutPort)
        {
            _logInPort = postLogIn;
            _logOutPort = logInOutPort;
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LoginRequestDTO loginRequest)
        {
            try
            {
                await _logInPort.LogIn(loginRequest);

                var result = ((IPresenter<AuthResponseDTO>)_logOutPort).Content;

                if (result is not null)
                {
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
            catch
            {

            }
            return Unauthorized(new { message = "Credenciales incorrectas o usuario no registrado." });
        }

    }
}
