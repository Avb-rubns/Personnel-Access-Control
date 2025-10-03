namespace Rubns.Application.UseCases.Rols.Queries
{
    internal class GetRolsUseCase(IRolRepositoryEFC rolRepository,
        IGetRolsOutputPort getRolsOutPort,
        ILogger logger) : IGetRolsUseCase
    {
        private readonly IRolRepositoryEFC _rolRepository = rolRepository;
        private readonly ILogger _logger = logger;
        private readonly IGetRolsOutputPort _getRolsOutPort = getRolsOutPort;

        public async Task ExecuteAsync()
        {
            try
            {

                var data = await _rolRepository.GetAllAsync();
                await _getRolsOutPort.Success(data);

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetRolsAsync:{error}", ex.Message);
                throw;
            }
        }
    }
}
