namespace Personnel.Client.Shared.DTOs.Link.Queries
{
    public class PaginationDTO
    {
        public int Total { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
    }
}
