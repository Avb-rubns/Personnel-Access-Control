namespace Rubns.Application.CheckUser.Get
{
    internal class CheckTodayUseCase(ICheckUserRepositoryDapper checkUserRepositoryDapper
        , ILogger logger)
        : IGetCheckUserTodayPort
    {
        private readonly ICheckUserRepositoryDapper _checkUserRepositoryDapper = checkUserRepositoryDapper;
        private readonly ILogger _logger = logger;
        public async Task<List<CheckUserTodayDTO>> CheckTodayAsync()
        {
            List<CheckUserTodayDTO> result = new List<CheckUserTodayDTO>();

            try
            {
                result = await _checkUserRepositoryDapper.CheckUserTodayAsync();

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error CheckTodayUseCase: {error}", ex.Message);
            }

            return result;


        }
    }
}
