namespace Rubns.WebAPI.Controllers.V1
{

    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class AuthController : ControllerBase
    {

        private readonly IRefreshJWTPort<RefreshTokenResponseDTO> RefreshJWT;

        public AuthController(IRefreshJWTPort<RefreshTokenResponseDTO> refreshJWTPort)
        {
            RefreshJWT = refreshJWTPort;
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
                return Ok(update);
            }
            return Unauthorized(new { message = "Token inválido o expirado" });
        }
    }
}
