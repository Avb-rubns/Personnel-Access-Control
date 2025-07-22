namespace Rubns.Core.Ports.Rol
{
    public interface IGetRolsPort
    {

        Task<List<RolDTO>> GetRolsAsync();
    }
}
