namespace Rubns.WebAPI.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class TicketController : ControllerBase
    {

        ICheckInPort<Response> CheckInUseCase { get; }

        public TicketController(ICheckInPort<Response> checkInUseCase)
        {
            CheckInUseCase = checkInUseCase;
        }


        [HttpPost("check-in")]
        public async Task<IActionResult> CheckTicket([FromBody] CheckInDTO checkTicket)
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            checkTicket.IP = ip;
            var checkInResult = await CheckInUseCase.CheckIn(checkTicket);

            return checkInResult.StatusCode switch
            {
                System.Net.HttpStatusCode.Created => CreatedAtAction(nameof(CheckTicket), new { user = checkInResult.Message }, checkInResult),
                System.Net.HttpStatusCode.NotFound => NotFound(checkInResult),
                System.Net.HttpStatusCode.BadRequest => BadRequest(checkInResult),
                _ => StatusCode((int)checkInResult.StatusCode, checkInResult)
            };
        }
    }
}
