namespace Personnel.Client.Shared.DTOs.Clik
{
    public class ClicksDashboardDto
    {
        public int TotalClicks { get; set; }
        public string Id { get; set; }
        public string Slug { get; set; }
        public string URL { get; set; }

        public Dictionary<string, int> ByDate { get; set; } = new();
        public Dictionary<string, int> ByDevice { get; set; } = new();
        public Dictionary<string, int> ByRegion { get; set; } = new();
        public Dictionary<string, int> ByCountry { get; set; } = new();
        public Dictionary<string, int> ByBrowser { get; set; } = new();
        public Dictionary<string, int> ByOS { get; set; } = new();
        public Dictionary<string, int> ByCity { get; set; } = new();
    }
}
