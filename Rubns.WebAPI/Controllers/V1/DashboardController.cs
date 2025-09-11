namespace Rubns.WebAPI.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class DashboardController(IGetCheckUserTodayUseCase getCheckUserTodayPort,
        IGetCheckUserTodayOutputPort getCheckUserTodayOutPort)
        : ControllerBase
    {
        private readonly IGetCheckUserTodayUseCase _getCheckUserTodayUseCase = getCheckUserTodayPort;
        private readonly IGetCheckUserTodayOutputPort _getCheckUserTodayOutputPort = getCheckUserTodayOutPort;

        [HttpGet]
        public async Task<IActionResult> GetCheckUsersAsyn()
        {
            await _getCheckUserTodayUseCase.ExecuteAsync();
            var data = _getCheckUserTodayOutputPort.Result;
            return data?.Count > 0 ? Ok(data) : NoContent();
        }

    }
}
