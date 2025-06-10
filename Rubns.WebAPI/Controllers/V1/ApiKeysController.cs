using Rubns.Core.DTOs.ApiKey;
using Rubns.Core.Ports.ApiKey.Register;

namespace Rubns.WebAPI.Controllers.V1.Auth
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ApiKeysController : ControllerBase
    {
        IRegisterInputPort RegisterInputPort { get; }
        IGenerateKeyPort<ResponseData<string>> GenerateKeyPort { get; }

        public ApiKeysController(IRegisterInputPort registerInputPort
            , IGenerateKeyPort<ResponseData<string>> generateKeyPort)
        {
            RegisterInputPort = registerInputPort;
            GenerateKeyPort = generateKeyPort;
        }

        [HttpGet("check")]
        public IActionResult Get() => Ok("Online");

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAppAsync(RegisterDTO register)
        {
            await RegisterInputPort.RegisterAppAsync(register, Request);
            return Ok();
        }
        [HttpPost("generate")]
        public IActionResult GenerateKey(RegisterDTO register)
        {
            var result = GenerateKeyPort.GenerateKey(register, Request);
            if (result.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
