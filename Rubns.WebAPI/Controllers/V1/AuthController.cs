using Microsoft.AspNetCore.Http;

namespace Rubns.WebAPI.Controllers.V1
{

    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class AuthController : ControllerBase
    {

        private readonly IRefreshJWTPort<RefreshTokenResponseDTO> RefreshJWT;
        private readonly IUserInformationPort UserInformationPort;
        ILogOutPort LogOutPort { get; }

        public AuthController(IRefreshJWTPort<RefreshTokenResponseDTO> refreshJWTPort
            , IUserInformationPort userInformationPort
            , ILogOutPort logOut)
        {
            RefreshJWT = refreshJWTPort;
            UserInformationPort = userInformationPort;
            LogOutPort = logOut;
        }


        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            if (Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            {
                var jwt = await RefreshJWT.RefreshJWTAsync(refreshToken);
                if (jwt is { Token: not null })
                {
                    var accessTokenCookie = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(jwt.Token.ExpiresIn)),
                        Path = "/"
                    };

                    Response.Cookies.Append("accessToken", jwt.Token.AccessToken, accessTokenCookie);

                    var refreshTokenCookie = new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.FromUnixTimeSeconds(jwt.Expiration),
                        Path = "/api/v1/auth"
                    };
                    Response.Cookies.Append("refreshToken", jwt.RefreshToken, refreshTokenCookie);

                    return Ok(new { message = "Tokens renovados" });
                }

                Response.Cookies.Delete("accessToken");
                var refreshTokenCookieDelete = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Path = "/api/v1/auth"
                };
                Response.Cookies.Delete("refreshToken", refreshTokenCookieDelete);

            }
            return Unauthorized(new { message = "Token inválido o expirado" });
        }

        [HttpGet("me")]
        public IActionResult Me()
        {
            Request.Cookies.TryGetValue("accessToken", out var cookieToken);
            var user = UserInformationPort.UserInfo(cookieToken);

            if (user is { Status: true })
            {
                return Ok(user);
            }

            return Unauthorized(new { message = "Token inválido o expirado" });
        }

        [HttpDelete("logout")]
        public async Task<IActionResult> LogOut()
        {
            if (Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            {
                var accessTokenCookie = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Path = "/"
                };
                Response.Cookies.Delete("accessToken", accessTokenCookie);

                var refreshTokenCookie = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Path = "/api/v1/auth"
                };

                if (await LogOutPort.LogOut(refreshToken))
                {
                    Response.Cookies.Delete("refreshToken", refreshTokenCookie);
                    return Ok(new { message = "Sesión cerrada." });
                }
            }

            return BadRequest();
        }
    }
}
