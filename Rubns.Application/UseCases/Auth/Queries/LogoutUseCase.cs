namespace Rubns.Application.UseCases.Auth.Queries
{
    internal sealed class LogOutUseCase : ILogoutUseCase
    {
        private readonly ISessionUserRepositoryDapper _sessionUserRepository;
        private readonly ILogger _logger;

        public LogOutUseCase(ISessionUserRepositoryDapper sessionUserRepository
            , ILogger logger)
        {
            _sessionUserRepository = sessionUserRepository;
            _logger = logger;
        }

        public async Task ExecuteAsync(string token)
        {
            try
            {
                await _sessionUserRepository.DeleteByTokenAsync(token);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error LogOut:{error}", e.Message);
                throw;
            }

        }
    }
}
