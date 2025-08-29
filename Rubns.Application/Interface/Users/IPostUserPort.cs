namespace Rubns.Application.Interface.Users
{
    public interface IPostUserPort
    {
        Task<int> RegisterUserAsync(RegisterUserDTO registerUser);
    }
}
