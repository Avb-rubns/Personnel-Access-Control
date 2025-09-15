namespace Rubns.Application.UseCases.Users.Queries
{
    internal class GetUsersUseCase(IUserRepositoryEFC repositoryEFC
        , IGetUsersOutputPort getUsersOutPort
        , ILogger logger)
        : IGetUsersUseCase
    {
        private readonly IUserRepositoryEFC _repositoryEFC = repositoryEFC;
        private readonly ILogger _logger = logger;
        private readonly IGetUsersOutputPort _getUsersOutPort = getUsersOutPort;

        public async Task ExecuteAsync(string search, int? page, int? pageSize)
        {
            List<User> Users = new();
            int Total = 0;
            try
            {
                if (page <= 0)
                {
                    throw new ArgumentException("La pagina no puede ser cero o menor.");
                }
                if (pageSize <= 0)
                {
                    throw new ArgumentException("Los registros no puede ser cero o menor.");
                }

                if (!string.IsNullOrEmpty(search))
                {
                    Users = await _repositoryEFC.FindUsersAsync(search, page, pageSize);
                }
                else
                {
                    Users = await _repositoryEFC.GetAllUsersforPageAsync(page, pageSize);
                }
                Total = await _repositoryEFC.TotalUsersAsync();

                await _getUsersOutPort.Success(Users, Total);


            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error GetAllUsersforPageAsync:{error}", ex.Message);
                throw;
            }
        }
    }
}
