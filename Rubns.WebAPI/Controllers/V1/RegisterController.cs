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
            if (ModelState.IsValid)
            {
                var result = await PostUserPort.RegisterUserAsync(registerUser);

                return result switch
                {
                    (int)System.Net.HttpStatusCode.Created => Created(),
                    (int)System.Net.HttpStatusCode.BadRequest => BadRequest(new { message = "Datos incorrectos o usuario ya registrado." }),
                    _ => StatusCode((int)result, new { message = "Error en el servicio." })
                };

            }

            return BadRequest(ModelState);
        }
    }
}
