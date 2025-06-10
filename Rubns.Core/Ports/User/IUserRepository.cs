namespace Rubns.Core.Ports.User
{
    public interface IUserRepository
    {
        Task<int> RegisterAsync(RegisterUserDTO registerUser, string password);
    }
}
