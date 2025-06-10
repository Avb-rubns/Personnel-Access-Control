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
        public async Task<IActionResult> Register(RegisterUserDTO registerUser)
        {
            if (ModelState.IsValid)
            {
                var result = await PostUserPort.RegitserUserAsync(registerUser);
                if (result)
                {
                    return Created();
                }
                return BadRequest();

            }

            return BadRequest(ModelState);
        }
    }
}
