namespace Rubns.WebAPI.Presenters.Rols
{
    internal class RolsPresenter : IGetRolsOutputPort
    {
        public List<RolDTO> Result { get; private set; }

        public Task Success(List<Rol> rols)
        {
            List<RolDTO> result = new();

            result = rols.Select(rol => new RolDTO()
            {
                RolID = rol.RolID,
                Name = rol.Name,
                Value = rol.Value,
                LevelPermission = rol.LevelPermission,
                Status = rol.Status

            }).ToList();

            Result = result;

            return Task.CompletedTask;
        }
    }
}
