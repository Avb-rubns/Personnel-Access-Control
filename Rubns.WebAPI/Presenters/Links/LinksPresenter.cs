namespace Rubns.WebAPI.Presenters.Links
{
    internal class LinksPresenter : IGetLinksOutputPort
    {
        public TableLinkDTO Result { get; private set; }

        public Task Success(List<LinkWithClicks> links, int total, int currentPage, int pageSize, bool hasNextPage, bool hasPreviousPage)
        {
            TableLinkDTO table = new();

            table.Links = links.Select(link => new LinkTableDTO
            {
                ID = link.ID,
                Name = link.Name,
                Slug = link.Slug,
                Content = link.Content,
                Url = link.Url,
                DotScale = link.DotScale,
                ColorDark = link.ColorDark,
                ColorLight = link.ColorLight,
                QuietZone = link.QuietZone,
                Status = link.Status,
                Clicks = link.Clicks,
            }).ToList();

            PaginationDTO pagination = new PaginationDTO()
            {
                Total = total,
                CurrentPage = currentPage,
                PageSize = pageSize,
                HasPreviousPage = hasPreviousPage,
                HasNextPage = hasNextPage,
            };
            table.Pagination = pagination;
            Result = table;
            return Task.CompletedTask;
        }

    }
}
