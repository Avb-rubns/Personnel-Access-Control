namespace Rubns.WebAPI.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class RolsController(IGetRolsPort getRolsPort) : ControllerBase
    {
        private IGetRolsPort GetRolsPort { get; } = getRolsPort;

        [HttpGet]
        public async Task<IActionResult> GetRolsAsync()
        {

            var data = await GetRolsPort.GetRolsAsync();
            return Ok(data);

        }
    }
}
