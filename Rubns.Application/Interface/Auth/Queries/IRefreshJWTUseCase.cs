namespace Rubns.Application.Interface.Auth.Queries
{
    public interface IRefreshJWTUseCase
    {
        Task ExecuteAsync(string refresh);
    }
}
