namespace Rubns.Application.User.Get
{
    internal class GetUsersUseCase(IUserRepositoryEFC repositoryEFC, ILogger logger) : IGetUsersPort
    {
        private readonly IUserRepositoryEFC _repositoryEFC = repositoryEFC;
        private readonly ILogger _logger = logger;

        public async Task<TableUserDTO> GetAllUsersforPageAsync(string search, int? page, int? pageSize)
        {
            TableUserDTO tableUser = new();
            List<UserRegistedDTO> users = new();

            try
            {
                if (!string.IsNullOrEmpty(search))
                {
                    tableUser = await _repositoryEFC.FindUserAsync(search, page, pageSize);
                }
                else
                {
                    users = await _repositoryEFC.GetAllUsersforPageAsync(page, pageSize);
                    tableUser.RegisteredUsers = users;
                    tableUser.Total = await _repositoryEFC.TotalUsersAsync();
                }



            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error GetAllUsersforPageAsync:{error}", ex.Message);
            }

            return tableUser;

        }
    }
}
