namespace Personnel.Client.Shared.DTOs.Ticket
{
    public class CheckDTO
    {
        public string Email { get; set; } = null!;
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
