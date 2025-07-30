namespace Rubns.Core.Ports.User
{
    public interface IUserRepositoryEFC
    {
        Task<int> RegisterAsync(RegisterUserDTO registerUser, string password);
        Task<UserDTO> FindUserAsync(RegisterUserDTO registerUser);
        Task<TableUserDTO> FindUserAsync(string search, int? page, int? pageSize);
        Task<int> TotalUsersAsync();
        Task<UserDTO> FindUserforIDAsync(int ID);
        Task<int> UpdateUserforIDAsync(UserDTO user);
        Task<List<UserRegistedDTO>> GetAllUsersforPageAsync(int? page, int? pageSize);
    }
}
