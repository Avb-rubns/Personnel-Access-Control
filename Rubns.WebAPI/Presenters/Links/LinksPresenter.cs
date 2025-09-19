namespace Rubns.WebAPI.Presenters.Links
{
    internal class LinksPresenter : IGetLinksOutputPort
    {
        public LinksDTO Result { get; private set; }

        public Task Success(List<LinkWithCountClick> links, int total, int currentPage, int pageSize, bool hasNextPage, bool hasPreviousPage)
        {
            LinksDTO table = new();
            QRDTO qr = new();

            table.Links = links.Select(link => new LinkDTO
            {
                ID = link.FriendlyId,
                Name = link.Name,
                Slug = link.Slug,
                Content = link.Content,
                Url = link.Url,
                QR = new QRDTO()
                {
                    Id = link.QR.FriendlyId,
                    LinkId = link.QR.FriendlyLinkId,
                    DotScale = link.QR.DotScale,
                    ColorDark = link.QR.ColorDark,
                    ColorLight = link.QR.ColorLight,
                    QuietZone = link.QR.QuietZone,
                    LastUserID = link.QR.FriendLastUserID,
                    LastModificated = link.QR.LastModificated
                },
                Status = link.Status,
                Clicks = link.Clicks,
                Registered = link.Registered,

            }).ToList();

            PaginationDTO pagination = new()
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
