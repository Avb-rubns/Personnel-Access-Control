namespace Rubns.Core.Ports.User
{
    public interface IPostUserPort
    {
        Task<bool> RegitserUserAsync(RegisterUserDTO registerUser);
    }
}
