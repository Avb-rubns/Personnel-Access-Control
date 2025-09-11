namespace Rubns.Core.Abstraccions.Rols
{
    public interface IRolRepositoryEFC
    {
        Task<List<Rol>> GetAllAsync();
    }
}
