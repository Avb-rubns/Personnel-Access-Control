namespace Rubns.WebAPI.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class RegisterController : ControllerBase
    {
        private readonly ICreateUserUseCase _createUserUseCase;

        public RegisterController(ICreateUserUseCase postUserPort)
        {
            _createUserUseCase = postUserPort;
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUserAsync(RegisterUserDTO registerUser)
        {
            await _createUserUseCase.ExecuteAsync(registerUser);
            return Created();

        }
    }
}
