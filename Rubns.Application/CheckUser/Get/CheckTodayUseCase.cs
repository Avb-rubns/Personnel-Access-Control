using Rubns.Core.Abstraccions.Checker;

namespace Rubns.Application.CheckUser.Get
{
    internal class CheckTodayUseCase(ICheckUserRepositoryDapper checkUserRepositoryDapper
        , IGetCheckUserTodayOutPort getChetUserTodayOutPort
        , ILogger logger)
        : IGetCheckUserTodayInPort
    {
        private readonly ICheckUserRepositoryDapper _checkUserRepositoryDapper = checkUserRepositoryDapper;
        private readonly ILogger _logger = logger;
        private readonly IGetCheckUserTodayOutPort _checkUserTodayOutPort = getChetUserTodayOutPort;
        public async Task CheckTodayAsync()
        {

            try
            {
                var data = await _checkUserRepositoryDapper.CheckUserTodayAsync();
                await _checkUserTodayOutPort.Handler(data);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error CheckTodayUseCase: {error}", ex.Message);
                throw;
            }
        }
    }
}
