namespace Rubns.WebAPI.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class RolController(IGetRolsPort getRolsPort, IGetRolsOutPort getRolsOutPort) : ControllerBase
    {
        private readonly IGetRolsPort _getRolsPort = getRolsPort;
        private readonly IGetRolsOutPort _getRolsOutPort = getRolsOutPort;
        [HttpGet]
        public async Task<IActionResult> GetRolsAsync()
        {

            await _getRolsPort.GetRolsAsync();
            var data = _getRolsOutPort.Content;
            return data.Count() > 0 ? Ok(data) : NoContent();

        }
    }
}
