using Rubns.Core.Abstraccions.Sessions;

namespace Rubns.Application.UseCases.Auth.Post
{
    internal sealed class LogOutUseCase : ILogOutPort
    {
        private readonly ISessionUserRepositoryDapper _sessionUserRepository;
        private readonly ILogger _logger;

        public LogOutUseCase(ISessionUserRepositoryDapper sessionUserRepository
            , ILogger logger)
        {
            _sessionUserRepository = sessionUserRepository;
            _logger = logger;
        }

        public async Task LogOutAsync(string token)
        {
            try
            {
                await _sessionUserRepository.DeleteSessionforTokenAsync(token);
            }
            catch (Exception e)
            {
                _logger.Error(e, "Error LogOut:{error}", e.Message);
                throw;
            }

        }
    }
}
