namespace Rubns.WebAPI.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class TicketController : ControllerBase
    {

        ICheckPort<Response> CheckInUseCase { get; }

        public TicketController(ICheckPort<Response> checkInUseCase)
        {
            CheckInUseCase = checkInUseCase;
        }


        [HttpPost("check-in")]
        public async Task<IActionResult> CheckInTicket([FromBody] CheckDTO checkTicket)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            checkTicket.IP = ip;
            var checkInResult = await CheckInUseCase.CheckAsync(checkTicket, "in");

            return checkInResult.StatusCode switch
            {
                System.Net.HttpStatusCode.Created => CreatedAtAction(nameof(CheckInTicket), new { user = checkInResult.Message }, checkInResult),
                System.Net.HttpStatusCode.NotFound => NotFound(checkInResult),
                System.Net.HttpStatusCode.BadRequest => BadRequest(checkInResult),
                _ => StatusCode((int)checkInResult.StatusCode, checkInResult)
            };
        }
        [HttpPost("check-out")]
        public async Task<IActionResult> CheckOutTicket([FromBody] CheckDTO checkTicket)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            checkTicket.IP = ip;
            var checkInResult = await CheckInUseCase.CheckAsync(checkTicket, "out");

            return checkInResult.StatusCode switch
            {
                System.Net.HttpStatusCode.Created => CreatedAtAction(nameof(CheckOutTicket), new { user = checkInResult.Message }, checkInResult),
                System.Net.HttpStatusCode.NotFound => NotFound(checkInResult),
                System.Net.HttpStatusCode.BadRequest => BadRequest(checkInResult),
                _ => StatusCode((int)checkInResult.StatusCode, checkInResult)
            };
        }
    }
}
