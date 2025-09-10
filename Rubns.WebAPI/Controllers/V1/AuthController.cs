namespace Rubns.WebAPI.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class AuthController : ControllerBase
    {

        private readonly ILogOutPort _logOutPort;
        private readonly IForgotPassword _forgotPasswordPort;
        private readonly IResetPasswordPort _resetPasswordPort;
        private readonly IResetPasswordValidatePort _resetPasswordValidatePort;
        private readonly IRefreshJWTInPort _refreshJWT;
        private readonly IRefreshJWTOutPort _refreshJWTOutPort;
        private readonly IUserInformationInPort _userInformationInPort;
        private readonly IUserInformationOutPort _userInformationOutPort;

        public AuthController(IRefreshJWTInPort refreshJWTPort
            , IUserInformationInPort userInformationPort
            , ILogOutPort logOut
            , IForgotPassword forgotPassword
            , IResetPasswordPort resetPasswordPort
            , IResetPasswordValidatePort resetPasswordValidatePort
            , IRefreshJWTOutPort refreshJWTOutPort
            , IUserInformationOutPort userInformationOutPort)
        {
            _logOutPort = logOut;
            _forgotPasswordPort = forgotPassword;
            _resetPasswordPort = resetPasswordPort;
            _refreshJWT = refreshJWTPort;
            _resetPasswordValidatePort = resetPasswordValidatePort;
            _refreshJWTOutPort = refreshJWTOutPort;
            _userInformationInPort = userInformationPort;
            _userInformationOutPort = userInformationOutPort;
        }


        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            if (!Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
            {
                return Unauthorized(new ProblemDetails
                {
                    Title = "Refresh Token requerido",
                    Detail = "No se encontró el refresh token en la cookie.",
                    Status = StatusCodes.Status400BadRequest,
                    Type = "https://httpstatuses.com/400"
                });
            }

            await _refreshJWT.RefreshJWTAsync(refreshToken);
            var RefreshJWT = _refreshJWTOutPort.Content;
            var accessTokenCookie = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(RefreshJWT.Token.ExpiresIn)),
                Path = "/"
            };

            Response.Cookies.Append("accessToken", RefreshJWT.Token.AccessToken, accessTokenCookie);

            var refreshTokenCookie = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.FromUnixTimeSeconds(RefreshJWT.Expiration),
                Path = "/api/v1/auth"
            };
            Response.Cookies.Append("refreshToken", RefreshJWT.RefreshToken, refreshTokenCookie);

            return Ok(new { message = "Tokens renovados" });

        }

        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            Request.Cookies.TryGetValue("accessToken", out var cookieToken);
            await _userInformationInPort.UserInfo(cookieToken);
            var user = _userInformationOutPort.Content;
            return Ok(user);

        }
        [HttpDelete("logout")]
        public async Task<IActionResult> LogOut()
        {
            Request.Cookies.TryGetValue("refreshToken", out var refreshToken);
            await _logOutPort.LogOutAsync(refreshToken);
            DeleteAuthCookies();
            return Ok(new { message = "Sesión cerrada." });

        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO request)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            await _forgotPasswordPort.GeneratePasswordResetTokenAsync(request, baseUrl);
            return Ok();
        }
        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDTO request)
        {
            await _resetPasswordPort.ResetPasswordAsync(request);
            return Ok();
        }
        [HttpGet("reset-password/validate")]
        public async Task<IActionResult> ValidateTokenResetPassword(string token)
        {
            await _resetPasswordValidatePort.ValidateTokenPasswordAsync(token);
            return Ok();
        }
        private void DeleteAuthCookies()
        {
            var refreshTokenCookieDelete = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = "/api/v1/auth"
            };
            Response.Cookies.Delete("refreshToken", refreshTokenCookieDelete);

            var accessTokenCookieDelete = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = "/"
            };
            Response.Cookies.Delete("accessToken", accessTokenCookieDelete);
        }
    }
}
