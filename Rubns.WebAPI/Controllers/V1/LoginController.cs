
using Microsoft.AspNetCore.Http;

namespace Rubns.WebAPI.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class LoginController : ControllerBase
    {
        ILogInPort<AuthResponseDTO> PostLogIn { get; }
        public LoginController(ILogInPort<AuthResponseDTO> postLogIn)
        {
            PostLogIn = postLogIn;
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LoginRequestDTO loginRequest)
        {

            var result = await PostLogIn.LogIn(loginRequest);

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


            if (result is not null)
                return Ok(new { message = "Login exitoso" });

            return Unauthorized(new { message = "Credenciales incorrectas o usuario no registrado." });
        }

    }
}
