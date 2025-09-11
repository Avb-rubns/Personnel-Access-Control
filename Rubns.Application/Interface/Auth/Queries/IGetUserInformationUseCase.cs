namespace Rubns.Application.Interface.Auth.Queries
{
    public interface IGetUserInformationUseCase
    {
        Task ExecuteAsync(string JWT);
    }
}
