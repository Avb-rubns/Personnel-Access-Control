namespace Rubns.WebAPI.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class DashboardController
        : ControllerBase
    {
        private readonly IGetCheckUserTodayUseCase _getCheckUserTodayUseCase;
        private readonly IGetCheckUserTodayOutputPort _getCheckUserTodayOutputPort;

        private readonly IGetLinksByDashboadUseCase _getLinksByDashboadUseCase;
        private readonly IGetLinksByDashboarOutputPort _getLinksByDashboarOutputPort;


        public DashboardController(IGetLinksByDashboadUseCase getLinksByDashboadUseCase,
            IGetCheckUserTodayUseCase getCheckUserTodayPort,
            IGetCheckUserTodayOutputPort getCheckUserTodayOutPort,
            IGetLinksByDashboarOutputPort getLinksByDashboarOutputPort)
        {
            _getLinksByDashboadUseCase = getLinksByDashboadUseCase;
            _getCheckUserTodayOutputPort = getCheckUserTodayOutPort;
            _getCheckUserTodayUseCase = getCheckUserTodayPort;
            _getLinksByDashboarOutputPort = getLinksByDashboarOutputPort;
        }

        [HttpGet("checks")]
        public async Task<IActionResult> GetCheckUsersAsyn()
        {
            await _getCheckUserTodayUseCase.ExecuteAsync();
            var data = _getCheckUserTodayOutputPort.Result;
            return data?.Count > 0 ? Ok(data) : NoContent();
        }

        [HttpGet("links")]
        public async Task<IActionResult> GetLinksToday([FromQuery] int page, [FromQuery] int pageSize)
        {
            await _getLinksByDashboadUseCase.ExecuteAsync(page, pageSize);
            var data = _getLinksByDashboarOutputPort.Result;
            return data.LinkByDashboard.Count > 0 ? Ok(data) : NoContent();
        }

    }
}
