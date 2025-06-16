namespace Rubns.WebUI.Models
{
    public class CheckFormViewModel
    {
        public Guid TicketId { get; set; }
        public string EventName { get; set; } = null!;
        public DateTime EventDate { get; set; }
        public string? Name { get; set; }
        public string? RegistrationCode { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
