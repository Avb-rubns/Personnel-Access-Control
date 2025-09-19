namespace Rubns.WebAPI.Presenters.Links
{
    internal class LinkPresenter : IGetLinkBySlugOutputPort
    {
        public LinkDetailDTO Result { get; private set; }

        public Task Success(Link link)
        {
            LinkDetailDTO result = new LinkDetailDTO()
            {
                ID = link.FriendlyId,
                Name = link.Name,
                Slug = link.Slug,
                Content = link.Content,
                Url = link.Url,
                Clicks = link.Clicks.Select(clic => new ClickDTO
                {
                    ID = clic.FriendlyId,
                    LinkId = clic.FriendlyLinkId,
                    ClickedAt = clic.ClickedAt,
                    Country = clic.Country,
                    Region = clic.Region,
                    City = clic.City,
                    DeviceType = clic.DeviceType,
                    OS = clic.OS,
                    Browser = clic.Browser,
                    IpAddress = clic.IpAddress,
                }).ToList(),
                QR = new QRDTO
                {
                    Id = link.QR.FriendlyId,
                    LinkId = link.QR.FriendlyLinkId,
                    DotScale = link.QR.DotScale,
                    ColorDark = link.QR.ColorDark,
                    ColorLight = link.QR.ColorLight,
                    QuietZone = link.QR.QuietZone,
                    LastModificated = link.QR.LastModificated,
                    LastUserID = link.QR.FriendLastUserID,
                    LastUser = link.QR.LastUser
                },
                Status = link.Status,
                Registered = link.Registered,
                UserRegistered = link.UserRegistered,
                UserLastModificated = link.UserLastModificated,
                LastModificated = link.LastModificated,
            };

            Result = result;

            return Task.CompletedTask;
        }
    }
}
