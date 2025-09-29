namespace Personnel.Client.Shared.DTOs.Link.Queries
{
    public class LinksTodayDTO
    {
        public List<LinkByDashboardDTO> LinkByDashboard { get; set; }
        public PaginationDTO Pagination { get; set; }
    }
}
