namespace Rubns.WebAPI.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class DashboardController(IGetCheckUserTodayInPort getCheckUserTodayPort,
        IGetCheckUserTodayOutPort getCheckUserTodayOutPort)
        : ControllerBase
    {
        private readonly IGetCheckUserTodayInPort _checkUserTodayPort = getCheckUserTodayPort;
        private readonly IGetCheckUserTodayOutPort _checkUserTodayOutPort = getCheckUserTodayOutPort;

        [HttpGet]
        public async Task<IActionResult> GetCheckUsersAsyn()
        {
            try
            {
                await _checkUserTodayPort.CheckTodayAsync();
                var data = _checkUserTodayOutPort.Content;
                return data?.Count > 0 ? Ok(data) : NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Error interno",
                    Detail = "Ocurrió un error inesperado. Intente nuevamente más tarde.",
                    Status = StatusCodes.Status500InternalServerError,
                    Type = "https://httpstatuses.com/500"
                });

            }
        }

    }
}
