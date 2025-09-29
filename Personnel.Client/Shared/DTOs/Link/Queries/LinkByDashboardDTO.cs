namespace Personnel.Client.Shared.DTOs.Link.Queries
{
    public class LinkByDashboardDTO
    {
        public DateTimeOffset ClickAt { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public string? Country { get; set; }
        public string? Region { get; set; }
        public string? City { get; set; }
        public string? DeviceType { get; set; }
        public string? Os { get; set; }
        public string? Browser { get; set; }
    }
}
