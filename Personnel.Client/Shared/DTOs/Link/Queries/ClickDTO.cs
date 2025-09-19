namespace Personnel.Client.Shared.DTOs.Link.Queries
{
    public class ClickDTO
    {
        public string ID { get; set; }
        public string LinkId { get; set; }
        public DateTimeOffset ClickedAt { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string DeviceType { get; set; }
        public string OS { get; set; }
        public string Browser { get; set; }
        public string? IpAddress { get; set; }

    }
}
