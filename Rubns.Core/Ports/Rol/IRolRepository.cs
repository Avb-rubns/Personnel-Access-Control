namespace Rubns.Core.Ports.Rol
{
    public interface IRolRepository
    {
        Task<List<RolDTO>> GetRolsAsync();
    }
}
