namespace Rubns.Application.UseCases.Rols.Get
{
    internal class RolsUseCase(IRolRepositoryEFC rolRepository,
        IGetRolsOutPort getRolsOutPort,
        ILogger logger) : IGetRolsPort
    {
        private readonly IRolRepositoryEFC _rolRepository = rolRepository;
        private readonly ILogger _logger = logger;
        private readonly IGetRolsOutPort _getRolsOutPort = getRolsOutPort;

        public async Task GetRolsAsync()
        {
            try
            {
                var data = await _rolRepository.GetRolsAsync();
                await _getRolsOutPort.Handler(data);

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in GetRolsAsync:{error}", ex.Message);
                throw;
            }
        }
    }
}
