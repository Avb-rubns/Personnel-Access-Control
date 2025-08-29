namespace Rubns.Application.Interface.Auth
{
    public interface ILogInPort
    {

        Task LogIn(LoginRequestDTO login);
    }
}
