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


        [HttpGet("validate")]
        public IActionResult ValidateToken()
        {
            return Ok(new { message = "Token válido" });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestDTO refresh)
        {
            var update = await RefreshJWT.RefreshJWTAsync(refresh);

            if (update.Token is not null)
            {
                return Ok(new { message = "Tokens renovados" });
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
            var refreshToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");


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
                Path = "/api/v1/auth/refresh"
            };
            Response.Cookies.Delete("refreshToken", refreshTokenCookie);

            if (await LogOutPort.LogOut(refreshToken))
            {
                return Ok(new { message = "Sesión cerrada." });
            }
            return BadRequest();
        }
    }
}
