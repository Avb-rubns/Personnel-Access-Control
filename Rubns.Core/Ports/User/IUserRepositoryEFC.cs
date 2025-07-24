namespace Rubns.Core.Ports.User
{
    public interface IUserRepositoryEFC
    {
        Task<int> RegisterAsync(RegisterUserDTO registerUser, string password);
        Task<UserDTO> FindUserAsync(RegisterUserDTO registerUser);
        Task<List<UserRegistedDTO>> GetAllUsersforPageAsync(int? page, int? pageSize);
    }
}
