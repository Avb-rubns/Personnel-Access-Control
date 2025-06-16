using Microsoft.AspNetCore.Mvc;
using Rubns.WebUI.Models;

namespace Rubns.WebUI.Controllers
{
    [Route("check")]
    public class TicketController : Controller
    {
        [HttpGet]
        public IActionResult Check()
        {
            var model = new CheckFormViewModel
            {
                TicketId = Guid.NewGuid(),
                EventName = "Prueba",
                EventDate = DateTime.UtcNow.AddHours(-5)
            };
            return View("CheckForm", model);
        }
        [HttpPost]
        public IActionResult CheckAsync([FromForm] CheckFormDto dto)
        {
            var localization = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Desconocida";
            //var ticket = await _db.Tickets.FindAsync(ticketId);
            //if (ticket == null) return View("Error", new ErrorViewModel { Message = "Ticket no válido.", TicketId = ticketId });
            //if (ticket.CheckedAt != null) return View("Error", new ErrorViewModel { Message = "Ticket ya validado.", TicketId = ticketId });

            //if (ticket.RegistrationCode != dto.RegistrationCode)
            //{
            //    ModelState.AddModelError("RegistrationCode", "Código incorrecto");
            //    return View("CheckForm", new CheckFormViewModel
            //    {
            //        TicketId = ticket.Id,
            //        EventName = ticket.Event.Name,
            //        EventDate = ticket.Event.Date,
            //        Name = dto.Name
            //    });
            //}

            //// Marcar como usado
            //ticket.CheckedAt = DateTime.UtcNow;
            //ticket.CheckedBy = dto.Name;
            //ticket.CheckedLatitude = dto.Latitude;
            //ticket.CheckedLongitude = dto.Longitude;
            //await _db.SaveChangesAsync();


            return View("Success", new SuccessViewModel
            {
                //TicketId = ticket.Id,
                //CheckedAt = ticket.CheckedAt.Value
            });
        }
    }
}
