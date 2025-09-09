namespace Rubns.WebAPI.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class RegisterController : ControllerBase
    {
        private readonly IPostUserPort _postUserPort;

        public RegisterController(IPostUserPort postUserPort)
        {
            _postUserPort = postUserPort;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUserAsync(RegisterUserDTO registerUser)
        {
            await _postUserPort.RegisterUserAsync(registerUser);
            return Created();

        }
    }
}
