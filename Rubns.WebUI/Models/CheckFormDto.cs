namespace Rubns.WebUI.Models
{
    public class CheckFormDto
    {
        public Guid TicketId { get; set; }
        public string Name { get; set; } = null!;
        public string RegistrationCode { get; set; } = null!;
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
