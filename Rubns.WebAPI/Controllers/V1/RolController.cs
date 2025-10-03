namespace Rubns.WebAPI.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class RolController(IGetRolsUseCase getRolsPort, IGetRolsOutputPort getRolsOutPort) : ControllerBase
    {
        private readonly IGetRolsUseCase _getRolsUseCase = getRolsPort;
        private readonly IGetRolsOutputPort _getRolsOutputPort = getRolsOutPort;
        [HttpGet("rols")]
        public async Task<IActionResult> GetRolsAsync()
        {

            await _getRolsUseCase.ExecuteAsync();
            var data = _getRolsOutputPort.Result;
            return data.Count() > 0 ? Ok(data) : NoContent();

        }
    }
}
