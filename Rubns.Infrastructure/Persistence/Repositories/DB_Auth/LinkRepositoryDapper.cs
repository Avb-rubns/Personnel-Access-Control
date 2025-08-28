namespace Rubns.Infrastructure.Persistence.Repositories.DB_Auth
{
    internal sealed class LinkRepositoryDapper(IConfiguration configuration) : ILinkRepositoryDapper

    {
        private readonly IConfiguration _configuration = configuration;

        public async Task<List<LinkTableDTO>> GetLinksAsync(int page, int rows, string filter)
        {
            try
            {
                List<LinkTableDTO> linkTableDTOs = new List<LinkTableDTO>();

                await using var connection = new SqlConnection(_configuration.GetConnectionString("dbAuth"));
                await connection.OpenAsync();

                var proc = "p_GetLinks";


                var data = await connection.QueryAsync<LinkTableDTO>(proc, new { page, rows, filter }, commandType: CommandType.StoredProcedure);
                await connection.CloseAsync();

                if (data.Count() > 0)
                {
                    linkTableDTOs = data.ToList();
                }

                return linkTableDTOs;

            }
            catch (Exception) { throw; }
        }
    }
}
