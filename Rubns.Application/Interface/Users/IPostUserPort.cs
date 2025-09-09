namespace Rubns.Application.Interface.Users
{
    public interface IPostUserPort
    {
        Task RegisterUserAsync(RegisterUserDTO registerUser);
    }
}
