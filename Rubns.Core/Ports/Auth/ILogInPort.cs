namespace Rubns.Core.Ports.Auth
{
    public interface ILogInPort<T>
    {

        Task<T> LogIn(LoginRequestDTO login);
    }
}
