namespace Rubns.WebAPI.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class TicketController : ControllerBase
    {

        private readonly ICheckPort _checkInUseCase;

        public TicketController(ICheckPort checkInUseCase)
        {
            _checkInUseCase = checkInUseCase;
        }

        [HttpPost("check-in")]
        public async Task<IActionResult> CheckInTicket([FromBody] CheckDTO checkTicket)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            checkTicket.IP = ip;
            await _checkInUseCase.CheckAsync(checkTicket, "in");
            return Created();
        }
        [HttpPost("check-out")]
        public async Task<IActionResult> CheckOutTicket([FromBody] CheckDTO checkTicket)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            checkTicket.IP = ip;
            await _checkInUseCase.CheckAsync(checkTicket, "out");
            return Created();
        }
    }
}
