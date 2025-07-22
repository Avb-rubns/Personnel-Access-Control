namespace Rubns.Application.Rol.Get
{
    internal class RolsUseCase(IRolRepository rolRepository,
        ILogger logger) : IGetRolsPort
    {
        private readonly IRolRepository RolRepository = rolRepository;
        private readonly ILogger Logger = logger;

        public async Task<List<RolDTO>> GetRolsAsync()
        {
            List<RolDTO> rols = new List<RolDTO>();
            try
            {
                var data = await RolRepository.GetRolsAsync();
                if (data.Count > 0)
                {
                    return data;
                }

            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error in GetRolsAsync:{error}", ex.Message);
            }


            return rols;
        }
    }
}
