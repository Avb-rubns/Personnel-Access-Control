namespace Rubns.WebAPI.Presenters.Links
{
    internal class LinksPresenter : IGetLinksOutputPort
    {
        public TableLinkDTO Content { get; private set; }

        public Task Handler(List<LinkWithClicks> links, int total, int currentPage, int pageSize, bool hasNextPage, bool hasPreviousPage)
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

            table.Total = total;
            Content = table;
            table.CurrentPage = currentPage;
            table.PageSize = pageSize;
            table.HasPreviousPage = hasPreviousPage;
            table.HasNextPage = hasNextPage;
            return Task.CompletedTask;
        }

    }
}
