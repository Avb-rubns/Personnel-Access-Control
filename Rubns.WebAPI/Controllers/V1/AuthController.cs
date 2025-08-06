namespace Rubns.WebAPI.Controllers.V1
{

    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class AuthController : ControllerBase
    {

        private readonly IRefreshJWTPort<RefreshTokenResponseDTO> RefreshJWT;
        private readonly IUserInformationPort UserInformationPort;
        private readonly IForgotPassword ForgotPasswordPort;
        private readonly IResetPasswordPort ResetPasswordPort;
        private readonly IResetPasswordValidatePort ResetPasswordValidatePort;
        private ILogOutPort LogOutPort { get; }

        public AuthController(IRefreshJWTPort<RefreshTokenResponseDTO> refreshJWTPort
            , IUserInformationPort userInformationPort
            , ILogOutPort logOut
            , IForgotPassword forgotPassword
            , IResetPasswordPort resetPasswordPort
            , IResetPasswordValidatePort resetPasswordValidatePort)
        {
            RefreshJWT = refreshJWTPort;
            UserInformationPort = userInformationPort;
            LogOutPort = logOut;
            ForgotPasswordPort = forgotPassword;
            ResetPasswordPort = resetPasswordPort;
            ResetPasswordValidatePort = resetPasswordValidatePort;
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

            if (Request.Cookies.TryGetValue("accessToken", out var cookieToken))
            {
                var user = UserInformationPort.UserInfo(cookieToken);

                if (user is { Status: true })
                {
                    return Ok(user);
                }

            }

            return Unauthorized(new { message = "Token inválido o expirado" });
        }

        [HttpDelete("logout")]
        public async Task<IActionResult> LogOut()
        {
            if (Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            {
                if (await LogOutPort.LogOut(refreshToken))
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
                    Response.Cookies.Delete("refreshToken", refreshTokenCookie);
                    return Ok(new { message = "Sesión cerrada." });
                }
            }

            return BadRequest();
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO request)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            await ForgotPasswordPort.GeneratePasswordResetTokenAsync(request, baseUrl);

            return Ok();
        }
        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDTO request)
        {
            var result = await ResetPasswordPort.ResetPasswordAsync(request);
            if (result)
                return Ok();

            return BadRequest();
        }
        [HttpGet("reset-password/validate")]
        public async Task<IActionResult> ValidateTokenResetPassword(string token)
        {

            var result = await ResetPasswordValidatePort.ValidateTokenPasswordAsync(token);
            if (result)
                return Ok();

            return BadRequest();
        }
    }
}
