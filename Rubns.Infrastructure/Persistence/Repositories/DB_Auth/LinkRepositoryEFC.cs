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
                link.Url = data.Url;
                link.Status = data.Status;
                link.Registered = data.Registered;
            }


            return link;
        }
        public async Task<Link> GetLinkBySlugAsync(string slug)
        {
            Link link = new();

            var data = await _context.Links.AsNoTracking()
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
            }


            return link;
        }

        public async Task<List<LinkWithCountClick>> GetLinkWithClickByPaginationAsync(int page, int pageSize, string filter)
        {
            List<LinkWithCountClick> links = new();
            IEnumerable<LinkWithClick> data;

            if (filter.Equals("all"))
            {
                data = await _context.Links
                            .AsNoTracking()
                            .Include(link => link.QR)
                            .Select(link => new LinkWithClick
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
                            .Take(pageSize)
                            .ToListAsync();

            }
            else
            {
                bool status = filter.Equals("true") ? true : false;
                data = await _context.Links
                        .AsNoTracking()
                        .Include(i => i.QR)
                        .Where(i => i.Status == status)
                        .Select(link => new LinkWithClick
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
                            .Take(pageSize)
                            .ToListAsync();

            }

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
    }
}
