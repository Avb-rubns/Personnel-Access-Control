namespace Rubns.Infrastructure.Persistence.Repositories.DB_Auth
{
    internal sealed class LinkRepositoryDapper(IConfiguration configuration) : ILinkRepositoryDapper

    {
        private readonly IConfiguration _configuration = configuration;

        public async Task<List<LinkWithClicks>> GetLinksAsync(int page, int rows, string filter)
        {
            try
            {
                List<LinkWithClicks> links = new();

                await using var connection = new SqlConnection(_configuration.GetConnectionString("dbAuth"));
                await connection.OpenAsync();

                var proc = "p_GetLinks";


                var data = await connection.QueryAsync<LinksSpResult>(proc, new { page, rows, filter }, commandType: CommandType.StoredProcedure);
                await connection.CloseAsync();

                if (data.Any())
                {
                    links = data.Select(l => new LinkWithClicks
                    {
                        ID = l.ID,
                        Name = l.Name,
                        Slug = l.Slug,
                        Content = l.Content,
                        Url = l.Url,
                        DotScale = l.DotScale,
                        ColorDark = l.ColorDark,
                        ColorLight = l.ColorLight,
                        QuietZone = l.QuietZone,
                        Status = l.Status,
                        Clicks = l.Clicks,

                    }).ToList();

                }

                return links;

            }
            catch (Exception) { throw; }
        }
    }
}
