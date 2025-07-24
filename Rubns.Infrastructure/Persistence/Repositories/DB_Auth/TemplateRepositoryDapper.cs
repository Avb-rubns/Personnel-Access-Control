namespace Rubns.Infrastructure.Persistence.Repositories.DB_Auth
{
    internal class TemplateRepositoryDapper(IConfiguration configuration) : ITemplateRepositoryDapper
    {
        IConfiguration Configuration = configuration;

        public async Task<string> GetMailRegistedAsync(RegisterUserDTO registerUser)
        {
            string mailRegisted = string.Empty;
            try
            {
                await using var connection = new SqlConnection(Configuration.GetConnectionString("dbAuth"));
                await connection.OpenAsync();


                var proc = "p_CreateMailRegister";

                var mail = await connection.QuerySingleOrDefaultAsync<string>(
                    proc,
                    new
                    {
                        Name = registerUser.UserName,
                        Mail = registerUser.Email,
                        Password = registerUser.Email.Split("@")[0]
                    },
                    commandType: CommandType.StoredProcedure);
                await connection.CloseAsync();

                if (!string.IsNullOrEmpty(mail))
                {
                    mailRegisted = mail;
                }


            }
            catch
            {
                throw;
            }
            return mailRegisted;
        }
    }
}
