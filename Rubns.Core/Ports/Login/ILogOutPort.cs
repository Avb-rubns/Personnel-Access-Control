namespace Rubns.Core.Ports.Login
{
    public interface ILogOutPort
    {
        Task<bool> LogOut(string token);
    }
}
