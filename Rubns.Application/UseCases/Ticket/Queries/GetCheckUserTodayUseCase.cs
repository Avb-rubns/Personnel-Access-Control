using Rubns.Application.Interface.Checker.Queries;

namespace Rubns.Application.UseCases.Ticket.Queries
{
    internal class GetCheckUserTodayUseCase(ICheckUserRepositoryDapper checkUserRepositoryDapper
        , IGetCheckUserTodayOutputPort getChetUserTodayOutPort
        , ILogger logger)
        : IGetCheckUserTodayUseCase
    {
        private readonly ICheckUserRepositoryDapper _checkUserRepositoryDapper = checkUserRepositoryDapper;
        private readonly ILogger _logger = logger;
        private readonly IGetCheckUserTodayOutputPort _checkUserTodayOutPort = getChetUserTodayOutPort;
        public async Task ExecuteAsync()
        {

            try
            {
                var data = await _checkUserRepositoryDapper.GetAllAsync();
                await _checkUserTodayOutPort.Success(data);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error CheckTodayUseCase: {error}", ex.Message);
                throw;
            }
        }
    }
}
