namespace Rubns.Core.Ports.User
{
    public interface IUserRepository
    {
        Task<int> RegisterAsync(RegisterUserDTO registerUser, string password);
        Task<UserDTO> FindUserAsync(RegisterUserDTO registerUser);
    }
}
