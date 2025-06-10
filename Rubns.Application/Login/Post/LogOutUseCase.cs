namespace Rubns.Application.Login.Post
{
    internal class LogOutUseCase : ILogOutPort
    {
        private readonly ISessionUserRepository SessionUserRepository;
        private readonly ILogger Logger;

        public LogOutUseCase(ISessionUserRepository sessionUserRepository
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
                return await SessionUserRepository.DeleteSessionAsync(token) > 0;
            }
            catch (Exception e)
            {
                Logger.Error(e, "Error LogOut");
            }

            return result;
        }
    }
}
