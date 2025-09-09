namespace Rubns.Infrastructure.Persistence.Repositories.DB_Auth
{
    internal class LinkRepositoryEFC(AuthDbContextEFC contextEFC)
        : ILinkRepositoryEFC
    {

        private readonly AuthDbContextEFC _context = contextEFC;
        public async Task<Link> AddAsync(Link link)
        {
            LinkDB newLink = new LinkDB()
            {
                Name = link.Name,
                Slug = link.Slug,
                Content = link.Content,
                Url = link.Url,
                Status = link.Status,
                UserID = link.UserIdRegistered,
                LastUserID = link.UserLastIdModificated
            };

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

        public async Task<int> DeleteLinkAsync(int id)
        {
            LinkDB remove = new()
            {
                ID = id,
            };
            _context.Links.Remove(remove);
            return await _context.SaveChangesAsync();
        }
        public async Task<string> FindSlugAsync(string slug)
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
        public async Task<List<Link>> GetAllLinksForPageAsync(int? page, int? pagesize, string? filter)
        {
            List<Link> links = new();
            IEnumerable<LinkDB> data;
            int pageSize = ((pagesize.HasValue && pagesize.Value > 0) ? pagesize.Value : 25);
            int pageNumber = ((page.HasValue && page.Value > 0) ? page.Value : 1);

            if (!string.IsNullOrEmpty(filter))
            {
                bool status = filter.Equals("true") ? true : false;
                data = await _context.Links.Where(s => s.Status == status)
                                .OrderBy(id => id.ID)
                                .Skip((pageNumber - 1) * pageSize)
                                .Take(pageSize)
                                .AsNoTracking()
                                .ToListAsync();
            }
            else
            {

                data = await _context.Links.OrderBy(id => id.ID)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .AsNoTracking()
                    .ToListAsync();
            }



            if (data.Count() > 0)
            {
                links = data.Select(s => new Link
                {
                    ID = s.ID,
                    Name = s.Name,
                    Slug = s.Slug,
                    Content = s.Content,
                    Url = s.Url,
                    DotScale = s.DotScale,
                    ColorDark = s.ColorDark,
                    ColorLight = s.ColorLight,
                    QuietZone = s.QuietZone,
                    Status = s.Status,
                    Registered = s.Registered,
                    LastModificated = s.LastModificated,

                }).ToList();
            }


            return links;
        }
        public async Task<Link> GetLinkForIdAsync(int id)
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
                link.DotScale = data.DotScale;
                link.ColorDark = data.ColorDark;
                link.ColorLight = data.ColorLight;
                link.QuietZone = data.QuietZone;
                link.Status = data.Status;
                link.Registered = data.Registered;
            }


            return link;
        }

        public async Task<Link> GetLinkForSlugAsync(string slug)
        {
            Link linkDTO = new();

            var data = await _context.Links.AsNoTracking()
                            .SingleOrDefaultAsync(i => i.Slug == slug);

            if (data is { ID: > 0 })
            {
                linkDTO.ID = data.ID;
                linkDTO.Name = data.Name;
                linkDTO.Slug = data.Slug;
                linkDTO.Content = data.Content;
                linkDTO.Url = data.Url;
                linkDTO.DotScale = data.DotScale;
                linkDTO.ColorDark = data.ColorDark;
                linkDTO.ColorLight = data.ColorLight;
                linkDTO.QuietZone = data.QuietZone;
                linkDTO.Status = data.Status;
                linkDTO.Registered = data.Registered;
                linkDTO.LastModificated = data.LastModificated;
                linkDTO.UserIdRegistered = data.UserID;
                linkDTO.UserLastIdModificated = data.LastUserID;
            }


            return linkDTO;
        }

        public async Task<int> UpdateQRAsync(int id, int userID, QR qr)
        {
            LinkDB link = new()
            {
                ID = id,
                DotScale = qr.DotScale,
                ColorDark = qr.ColorDark,
                ColorLight = qr.ColorLight,
                QuietZone = qr.QuietZone,
                LastUserID = userID,
            };

            _context.Entry(link).Property(u => u.DotScale).IsModified = true;
            _context.Entry(link).Property(u => u.ColorDark).IsModified = true;
            _context.Entry(link).Property(u => u.ColorLight).IsModified = true;
            _context.Entry(link).Property(u => u.QuietZone).IsModified = true;
            _context.Entry(link).Property(u => u.LastUserID).IsModified = true;

            return await _context.SaveChangesAsync();
        }
    }
}
