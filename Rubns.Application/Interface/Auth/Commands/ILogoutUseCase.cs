namespace Rubns.Application.Interface.Auth.Commands
{
    public interface ILogoutUseCase
    {
        Task ExecuteAsync(string token);
    }
}
