namespace Rubns.WebAPI.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class DashboardController(IGetCheckUserTodayPort getCheckUserTodayPort)
        : ControllerBase
    {
        private readonly IGetCheckUserTodayPort checkUserTodayPort = getCheckUserTodayPort;


        [HttpGet]
        public async Task<IActionResult> GetCheckUsersAsyn()
        {
            var data = await checkUserTodayPort.CheckTodayAsync();
            if (data.Count() > 0)
            {
                return Ok(data);

            }
            else
            {
                return NoContent();
            }

        }

    }
}
