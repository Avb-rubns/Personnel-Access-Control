using Personnel.Client.Shared.DTOs.Ticket;

namespace Rubns.WebAPI.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class TicketController : ControllerBase
    {

        [HttpPost("check")]
        public IActionResult CheckTicket([FromBody] CheckDTO checkTicket)
        {
            // Aquí iría la lógica para verificar el ticket
            // Por ejemplo, consultar una base de datos o un servicio externo
            // Simulación de verificación exitosa
            var isValid = true; // Esto debería ser el resultado de la verificación real
            if (isValid)
            {
                return Ok(new { message = "Registrada la entrada" });
            }
            else
            {
                return BadRequest(new { message = "No se registrada la entrada" });
            }
        }
    }
}
