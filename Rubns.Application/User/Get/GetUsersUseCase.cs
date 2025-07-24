namespace Rubns.Application.User.Get
{
    internal class GetUsersUseCase(IUserRepositoryEFC repositoryEFC, ILogger logger) : IGetUsersPort
    {
        private readonly IUserRepositoryEFC _repositoryEFC = repositoryEFC;
        private readonly ILogger _logger = logger;

        public async Task<List<UserRegistedDTO>> GetAllUsersforPageAsync(int? page, int? pageSize)
        {
            List<UserRegistedDTO> users = new();

            try
            {
                users = await _repositoryEFC.GetAllUsersforPageAsync(page, pageSize);

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error GetAllUsersforPageAsync:{error}", ex.Message);
            }

            return users;

        }
    }
}
