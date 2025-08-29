using Rubns.Application.Interface.Users;

namespace Rubns.Application.UseCases.Users.Get
{
    internal class GetUsersUseCase(IUserRepositoryEFC repositoryEFC
        , IGetUsersOutPort getUsersOutPort
        , ILogger logger)
        : IGetUsersInputPort
    {
        private readonly IUserRepositoryEFC _repositoryEFC = repositoryEFC;
        private readonly ILogger _logger = logger;
        private readonly IGetUsersOutPort _getUsersOutPort = getUsersOutPort;

        public async Task GetAllUsersforPageAsync(string search, int? page, int? pageSize)
        {
            List<User> Users = new();
            int Total = 0;
            try
            {
                if (!string.IsNullOrEmpty(search))
                {
                    Users = await _repositoryEFC.FindUsersAsync(search, page, pageSize);
                }
                else
                {
                    Users = await _repositoryEFC.GetAllUsersforPageAsync(page, pageSize);
                }
                Total = await _repositoryEFC.TotalUsersAsync();

                await _getUsersOutPort.Handler(Users, Total);


            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error GetAllUsersforPageAsync:{error}", ex.Message);
            }
        }
    }
}
