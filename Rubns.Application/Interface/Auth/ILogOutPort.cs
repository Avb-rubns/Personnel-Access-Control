namespace Rubns.Application.Interface.Auth
{
    public interface ILogOutPort
    {
        Task LogOutAsync(string token);
    }
}
