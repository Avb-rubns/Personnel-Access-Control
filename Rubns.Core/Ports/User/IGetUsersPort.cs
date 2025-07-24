namespace Rubns.Core.Ports.User
{
    public interface IGetUsersPort
    {
        Task<List<UserRegistedDTO>> GetAllUsersforPageAsync(int? page, int? pageSize);
    }
}
