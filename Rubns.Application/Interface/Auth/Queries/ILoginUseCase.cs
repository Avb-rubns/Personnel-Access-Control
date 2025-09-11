namespace Rubns.Application.Interface.Auth.Queries
{
    public interface ILoginUseCase
    {
        Task ExecuteAsync(LoginRequestDTO login);
    }
}
