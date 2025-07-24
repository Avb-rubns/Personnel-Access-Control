namespace Rubns.Core.Ports.User
{
    public interface IPostUserPort
    {
        Task<int> RegisterUserAsync(RegisterUserDTO registerUser);
    }
}
