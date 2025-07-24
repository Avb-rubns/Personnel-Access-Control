namespace Rubns.Core.Ports.Rol
{
    public interface IRolRepositoryEFC
    {
        Task<List<RolDTO>> GetRolsAsync();
    }
}
