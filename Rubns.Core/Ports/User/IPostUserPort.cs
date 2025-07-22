namespace Rubns.Core.Ports.User
{
    public interface IPostUserPort
    {
        Task<int> RegitserUserAsync(RegisterUserDTO registerUser);
    }
}
