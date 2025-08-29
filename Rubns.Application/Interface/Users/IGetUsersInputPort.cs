namespace Rubns.Application.Interface.Users
{
    public interface IGetUsersInputPort
    {
        Task GetAllUsersforPageAsync(string search, int? page, int? pageSize);
    }
}
