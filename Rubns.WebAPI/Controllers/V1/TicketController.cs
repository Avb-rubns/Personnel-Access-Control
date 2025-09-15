using Rubns.Application.Interface.Tickets.Commands;

namespace Rubns.WebAPI.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class TicketController : ControllerBase
    {

        private readonly ICreateCheckUseCase _createCheckUseCase;

        public TicketController(ICreateCheckUseCase checkInUseCase)
        {
            _createCheckUseCase = checkInUseCase;
        }

        [HttpPost("check-in")]
        public async Task<IActionResult> CheckInTicket([FromBody] CheckDTO checkTicket)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            checkTicket.IP = ip;
            await _createCheckUseCase.ExecuteAsync(checkTicket, "in");
            return Created();
        }
        [HttpPost("check-out")]
        public async Task<IActionResult> CheckOutTicket([FromBody] CheckDTO checkTicket)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            checkTicket.IP = ip;
            await _createCheckUseCase.ExecuteAsync(checkTicket, "out");
            return Created();
        }
    }
}
