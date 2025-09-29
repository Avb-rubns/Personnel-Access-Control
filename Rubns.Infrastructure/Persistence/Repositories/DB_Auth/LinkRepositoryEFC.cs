namespace Rubns.Infrastructure.Persistence.Repositories.DB_Auth
{
    internal class LinkRepositoryEFC(AuthDbContextEFC contextEFC)
        : ILinkRepositoryEFC
    {

        private readonly AuthDbContextEFC _context = contextEFC;
        public async Task<Link> AddAsync(Link link)
        {
            LinkDb newLink = new LinkDb()
            {
                Name = link.Name,
                Slug = link.Slug,
                Content = link.Content,
                Url = link.Url,
                Status = link.Status,
                UserID = link.UserIdRegistered,
                LastUserID = link.UserLastIdModificated
            };
            QRDb qr = new()
            {
                LastUserID = link.UserLastIdModificated
            };
            newLink.QR = qr;
            await _context.Links.AddAsync(newLink);
            var result = await _context.SaveChangesAsync();

            link.ID = newLink.ID;
            return link;
        }

        public async Task<int> CountLinksAsync()
        {
            try
            {
                int total = 0;

                total = await _context.Links.CountAsync();

                return total;
            }
            catch
            { throw; }
        }

        public async Task<int> CountLinksTodayByPaginationAsync()
        {
            int total = 0;
            total = await _context.Links
                .AsNoTracking()
                .SelectMany(link => link.Clicks
                .Where(click => click.ClickedAt >= DateTime.Today)
                .Select(click => new LinkByDashboard
                {
                    Slug = link.Slug,
                    Name = link.Name,
                    ClickAt = click.ClickedAt,
                    Country = click.Country,
                    Region = click.Region,
                    DeviceType = click.DeviceType,
                    City = click.City,
                    Os = click.OS,
                    Browser = click.Browser,

                }))
                .CountAsync();
            return total;
        }

        public async Task<int> DeleteAsync(int id)
        {
            LinkDb remove = new()
            {
                ID = id,
            };
            _context.Links.Remove(remove);
            return await _context.SaveChangesAsync();
        }
        public async Task<string> FindBySlugAsync(string slug)
        {
            string result = string.Empty;

            var isExists = await _context.Links
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Slug == slug);

            if (isExists is { ID: > 0 })
            {
                result = isExists.Content;
            }

            return result;
        }

        public async Task<Link> GetLinkByIdAsync(int id)
        {
            Link link = new();

            var data = await _context.Links.Where(s => s.ID == id)
                .AsNoTracking()
                .SingleOrDefaultAsync();

            if (data is { ID: > 0 })
            {
                link.ID = id;
                link.Name = data.Name;
                link.Slug = data.Slug;
                link.Content = data.Content;
                link.Url = data.Url;
                link.Status = data.Status;
                link.UserIdRegistered = data.UserID;
                link.UserLastIdModificated = data.LastUserID;
                link.Registered = data.Registered;
            }


            return link;
        }
        public async Task<Link> GetLinkBySlugAsync(string slug)
        {
            Link link = new();

            var data = await _context.Links
                .AsNoTracking()
                .Include(qr => qr.QR).ThenInclude(userLast => userLast.User)
                .Include(click => click.Clicks)
                .Include(user => user.User)
                .Include(lastUser => lastUser.LastUser)
                .SingleOrDefaultAsync(i => i.Slug == slug);

            if (data is { ID: > 0 })
            {
                link.ID = data.ID;
                link.Name = data.Name;
                link.Slug = data.Slug;
                link.Content = data.Content;
                link.Url = data.Url;
                link.Status = data.Status;
                link.Registered = data.Registered;
                link.LastModificated = data.LastModificated;
                link.UserIdRegistered = data.UserID;
                link.UserLastIdModificated = data.LastUserID;
                link.UserRegistered = data.User.Name;
                link.UserLastModificated = data.LastUser.Name;
                link.QR = new QR()
                {
                    Id = data.QR.Id,
                    LinkId = data.QR.LinkId,
                    DotScale = data.QR.DotScale,
                    ColorDark = data.QR.ColorDark,
                    ColorLight = data.QR.ColorLight,
                    QuietZone = data.QR.QuietZone,
                    LastModificated = data.QR.LastModificated,
                    LastUserID = data.QR.LastUserID,
                    LastUser = data.QR.User.Name,
                };
                link.Clicks = data.Clicks.Select(clic => new Click()
                {
                    ID = clic.ID,
                    LinkId = clic.LinkId,
                    ClickedAt = clic.ClickedAt,
                    Country = clic.Country,
                    Region = clic.Region,
                    City = clic.City,
                    DeviceType = clic.DeviceType,
                    OS = clic.OS,
                    Browser = clic.Browser,
                    IpAddress = clic.IpAddress

                }).ToList();
            }


            return link;
        }

        public async Task<List<LinkByDashboard>> GetLinksTodayByPaginationAsync(int page, int pageSize)
        {
            List<LinkByDashboard> Links = new();

            Links = await _context.Links
                .AsNoTracking()
                .SelectMany(link => link.Clicks
                    .Where(click => click.ClickedAt >= DateTime.Today)
                    .Select(click => new LinkByDashboard
                    {
                        Slug = link.Slug,
                        Name = link.Name,
                        ClickAt = click.ClickedAt,
                        Country = click.Country,
                        Region = click.Region,
                        DeviceType = click.DeviceType,
                        City = click.City,
                        Os = click.OS,
                        Browser = click.Browser,

                    }))
                .OrderByDescending(link => link.ClickAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Links;
        }

        public async Task<List<LinkWithCountClick>> GetLinkWithClickByPaginationAsync(int page, int pageSize,
            string filter, string? search)
        {
            List<LinkWithCountClick> links = new();


            var query = _context.Links.AsNoTracking();

            if (!string.IsNullOrEmpty(search))
                query = query.Where(link =>
                    EF.Functions.Like(link.Name, $"%{search}%") ||
                    EF.Functions.Like(link.Slug, $"%{search}%"));

            if (filter != "all")
            {
                bool status = filter == "true";
                query = query.Where(link => link.Status == status);
            }
            var projectedQuery = query
                        .Select(link => new LinkWithClickDb
                        {
                            ID = link.ID,
                            Name = link.Name,
                            Slug = link.Slug,
                            Content = link.Content,
                            Url = link.Url,
                            Status = link.Status,
                            Registered = link.Registered,
                            UserID = link.UserID,
                            LastModificated = link.LastModificated,
                            LastUserID = link.LastUserID,
                            QR = link.QR,
                            Clicks = link.Clicks.Count(),
                        })
                        .OrderBy(link => link.ID)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize);


            var data = await projectedQuery.ToListAsync();
            if (data.Count() > 0)
            {
                links = data.Select(link => new LinkWithCountClick()
                {
                    ID = link.ID,
                    Name = link.Name,
                    Slug = link.Slug,
                    Content = link.Content,
                    Url = link.Url,
                    Status = link.Status,
                    Registered = link.Registered,
                    UserID = link.UserID,
                    LastUserID = link.LastUserID,
                    LastModificated = link.LastModificated,
                    QR = new QR()
                    {
                        Id = link.QR.Id,
                        LinkId = link.QR.LinkId,
                        DotScale = link.QR.DotScale,
                        ColorDark = link.QR.ColorDark,
                        ColorLight = link.QR.ColorLight,
                        QuietZone = link.QR.QuietZone,
                        LastModificated = link.QR.LastModificated,
                        LastUserID = link.QR.LastUserID,
                    },
                    Clicks = link.Clicks
                }).ToList();
            }

            return links;
        }

        public Task<int> UpdateAsync(Link link)
        {
            LinkDb LinkUpdate = new()
            {
                ID = link.ID,
                Name = link.Name,
                Slug = link.Slug,
                Content = link.Content,
                LastUserID = link.UserLastIdModificated
            };

            _context.Entry(LinkUpdate).Property(x => x.Name).IsModified = !string.IsNullOrEmpty(link.Name);
            _context.Entry(LinkUpdate).Property(x => x.Slug).IsModified = !string.IsNullOrEmpty(link.Slug);
            _context.Entry(LinkUpdate).Property(x => x.Content).IsModified = !string.IsNullOrEmpty(link.Content);
            _context.Entry(LinkUpdate).Property(x => x.LastUserID).IsModified = true;

            return _context.SaveChangesAsync();


        }
    }
}
