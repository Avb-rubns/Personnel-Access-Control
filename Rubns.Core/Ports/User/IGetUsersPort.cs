namespace Rubns.Core.Ports.User
{
    public interface IGetUsersPort
    {
        Task<TableUserDTO> GetAllUsersforPageAsync(string search, int? page, int? pageSize);
    }
}
