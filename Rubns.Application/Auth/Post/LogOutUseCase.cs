namespace Rubns.Application.Auth.Post
{
    internal class LogOutUseCase : ILogOutPort
    {
        private readonly ISessionUserRepositoryDapper SessionUserRepository;
        private readonly ILogger Logger;

        public LogOutUseCase(ISessionUserRepositoryDapper sessionUserRepository
            , ILogger logger)
        {
            SessionUserRepository = sessionUserRepository;
            Logger = logger;
        }

        public async Task<bool> LogOut(string token)
        {
            bool result = false;
            try
            {
                return await SessionUserRepository.DeleteSessionforTokenAsync(token) > 0;
            }
            catch (Exception e)
            {
                Logger.Error(e, "Error LogOut");
            }

            return result;
        }
    }
}
