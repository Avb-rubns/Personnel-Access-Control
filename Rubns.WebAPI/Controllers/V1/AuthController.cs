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
            try
            {
                if (Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
                {
                    await _refreshJWT.RefreshJWTAsync(refreshToken);
                    var RefreshJWT = ((IPresenter<RefreshTokenResponseDTO>)_refreshJWTOutPort).Content;
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
            }
            catch (InvalidOperationException)
            {
                Response.Cookies.Delete("accessToken");
                var refreshTokenCookieDelete = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Path = "/api/v1/auth"
                };
                Response.Cookies.Delete("refreshToken", refreshTokenCookieDelete);
                return Unauthorized(new { message = "Token inválido o expirado" });

            }
            catch
            {
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
            return StatusCode(500, new { message = "Token inválido o expirado" });

        }

        [HttpGet("me")]
        public async Task<IActionResult> Me()
        {
            try
            {
                if (Request.Cookies.TryGetValue("accessToken", out var cookieToken))
                {
                    await _userInformationInPort.UserInfo(cookieToken);
                    var user = ((IPresenter<UserInfoDTO>)_userInformationOutPort).Content;
                    return Ok(user);

                }

            }
            catch { }

            return Unauthorized(new { message = "Token inválido o expirado" });
        }

        [HttpDelete("logout")]
        public async Task<IActionResult> LogOut()
        {
            try
            {
                if (Request.Cookies.TryGetValue("refreshToken", out var refreshToken))
                {
                    await _logOutPort.LogOut(refreshToken);
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
            catch (ArgumentNullException)
            {
                return BadRequest();
            }
            catch (ArithmeticException)
            {
                return Unauthorized();
            }
            return StatusCode(500, new { Error = "Ocurrió un error inesperado." });

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
            try
            {
                await _resetPasswordPort.ResetPasswordAsync(request);
                return Ok();

            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch
            {
                return StatusCode(500, new { Error = "Ocurrió un error inesperado." });
            }
        }
        [HttpGet("reset-password/validate")]
        public async Task<IActionResult> ValidateTokenResetPassword(string token)
        {
            try
            {
                await _resetPasswordValidatePort.ValidateTokenPasswordAsync(token);
                return Ok();

            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch
            {
                return StatusCode(500, new { Error = "Ocurrió un error inesperado." });
            }
        }
    }
}
