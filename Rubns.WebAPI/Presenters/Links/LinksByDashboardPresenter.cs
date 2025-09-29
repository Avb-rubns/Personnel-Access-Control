namespace Rubns.WebAPI.Presenters.Links
{
    internal class LinksByDashboardPresenter : IGetLinksByDashboarOutputPort
    {
        public LinksTodayDTO Result { get; set; }

        public Task Success(List<LinkByDashboard> links, int total, int currentPage, int pageSize, bool hasNextPage, bool hasPreviousPage)
        {
            LinksTodayDTO LinksToday = new LinksTodayDTO();

            LinksToday.LinkByDashboard = links.Select(link => new LinkByDashboardDTO
            {

                ClickAt = link.ClickAt,
                Name = link.Name,
                Slug = link.Slug,
                Country = link.Country,
                Region = link.Region,
                City = link.City,
                DeviceType = link.DeviceType,
                Os = link.Os,
                Browser = link.Browser,

            }).ToList();

            PaginationDTO Pagination = new()
            {
                Total = total,
                CurrentPage = currentPage,
                PageSize = pageSize,
                HasNextPage = hasNextPage,
                HasPreviousPage = hasPreviousPage
            };

            LinksToday.Pagination = Pagination;

            Result = LinksToday;

            return Task.CompletedTask;
        }
    }
}
