namespace Rubns.WebAPI.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class AuthController : ControllerBase
    {

        private readonly ILogoutUseCase _logoutUseCase;
        private readonly IForgotPasswordUseCase _forgotPasswordUseCase;
        private readonly IResetPasswordUseCase _resetPasswordUseCase;
        private readonly IValidateTokenUseCase _validateTokenUseCase;
        private readonly IRefreshJWTUseCase _refreshJWTUseCase;
        private readonly IRefreshJWTOutputPort _refreshJWTOutPort;
        private readonly IGetUserInformationUseCase _getUserInformationUseCase;
        private readonly IGetUserInformationOutputPort _gGetUserInformationOutputPort;

        public AuthController(IRefreshJWTUseCase refreshJWTPort
            , IGetUserInformationUseCase userInformationPort
            , ILogoutUseCase logOut
            , IForgotPasswordUseCase forgotPassword
            , IResetPasswordUseCase resetPasswordPort
            , IValidateTokenUseCase resetPasswordValidatePort
            , IRefreshJWTOutputPort refreshJWTOutPort
            , IGetUserInformationOutputPort userInformationOutPort)
        {
            _logoutUseCase = logOut;
            _forgotPasswordUseCase = forgotPassword;
            _resetPasswordUseCase = resetPasswordPort;
            _refreshJWTUseCase = refreshJWTPort;
            _validateTokenUseCase = resetPasswordValidatePort;
            _refreshJWTOutPort = refreshJWTOutPort;
            _getUserInformationUseCase = userInformationPort;
            _gGetUserInformationOutputPort = userInformationOutPort;
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

            await _refreshJWTUseCase.ExecuteAsync(refreshToken);
            var RefreshJWT = _refreshJWTOutPort.Result;
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
            await _getUserInformationUseCase.ExecuteAsync(cookieToken);
            var user = _gGetUserInformationOutputPort.Result;
            return Ok(user);

        }
        [HttpDelete("logout")]
        public async Task<IActionResult> LogOut()
        {
            Request.Cookies.TryGetValue("refreshToken", out var refreshToken);
            await _logoutUseCase.ExecuteAsync(refreshToken);
            DeleteAuthCookies();
            return Ok(new { message = "Sesión cerrada." });

        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO request)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            await _forgotPasswordUseCase.ExecuteAsyn(request, baseUrl);
            return Ok();
        }
        [HttpPut("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDTO request)
        {
            await _resetPasswordUseCase.ExecuteAsync(request);
            return Ok();
        }
        [HttpGet("reset-password/validate")]
        public async Task<IActionResult> ValidateTokenResetPassword(string token)
        {
            await _validateTokenUseCase.ExecuteAsync(token);
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
