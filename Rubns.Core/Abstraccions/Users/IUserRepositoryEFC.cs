namespace Rubns.Core.Abstraccions.Users
{
    public interface IUserRepositoryEFC
    {
        Task<int> RegisterAsync(User registerUser, string password);
        Task<User> FindUserByPhoneOrEmailAsync(string email, string phone);
        Task<List<User>> FindUsersAsync(string search, int? page, int? pageSize);
        Task<int> TotalUsersAsync();
        Task<User> FindUserforIDAsync(int ID);
        Task<User> FindUserforEmailAsync(string email);
        Task<int> UpdateUserforIDAsync(User user);
        Task<List<User>> GetAllUsersforPageAsync(int? page, int? pageSize);
    }
}
