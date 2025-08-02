namespace Rubns.Core.Ports.Auth
{
    public interface ILogOutPort
    {
        Task<bool> LogOut(string token);
    }
}
