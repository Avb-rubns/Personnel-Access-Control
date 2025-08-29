namespace Rubns.Application.Interface.Auth
{
    public interface ILogOutPort
    {
        Task LogOut(string token);
    }
}
