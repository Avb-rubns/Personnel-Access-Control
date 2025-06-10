namespace Rubns.Core.Ports.Login
{
    public interface ILogInPort<T>
    {

        Task<T> LogIn(LoginRequestDTO login);
    }
}
