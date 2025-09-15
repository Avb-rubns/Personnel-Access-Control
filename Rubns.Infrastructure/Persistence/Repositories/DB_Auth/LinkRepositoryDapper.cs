namespace Rubns.Infrastructure.Persistence.Repositories.DB_Auth
{
    internal sealed class LinkRepositoryDapper(IConfiguration configuration) : ILinkRepositoryDapper

    {
        private readonly IConfiguration _configuration = configuration;

        public async Task<int> CountLinks(string filter)
        {
            try
            {
                int total = 0;

                await using var connection = new SqlConnection(_configuration.GetConnectionString("dbAuth"));
                await connection.OpenAsync();

                var proc = "p_CountLinks";

                total = await connection.ExecuteScalarAsync<int>(proc, new { filter }, commandType: CommandType.StoredProcedure);

                await connection.CloseAsync();

                return total;

            }
            catch { throw; }

        }
    }
}
