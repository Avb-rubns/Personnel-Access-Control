namespace Rubns.WebAPI.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ClickController : ControllerBase
    {

        private readonly IGetClicksUseCase _getClicksUseCase;
        private readonly IGetClicksOutputPort _getClicksOutputPort;


        public ClickController(IGetClicksUseCase getClicksUseCase, IGetClicksOutputPort getClicksOutputPort)
        {
            _getClicksUseCase = getClicksUseCase;
            _getClicksOutputPort = getClicksOutputPort;
        }

        [HttpGet("{slug}")]
        public async Task<IActionResult> ClicksAsyn(string slug, DateTime? starDate, DateTime? endDate)
        {
            await _getClicksUseCase.ExecuteAsync(slug, starDate, endDate);
            var data = _getClicksOutputPort.Result;
            return Ok(data);

        }
    }
}
