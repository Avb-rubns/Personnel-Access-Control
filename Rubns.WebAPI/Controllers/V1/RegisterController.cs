namespace Rubns.WebAPI.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class RegisterController : ControllerBase
    {
        IPostUserPort PostUserPort { get; }

        public RegisterController(IPostUserPort postUserPort)
        {
            PostUserPort = postUserPort;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUserAsync(RegisterUserDTO registerUser)
        {
            await PostUserPort.RegisterUserAsync(registerUser);
            return Created();

        }
    }
}
