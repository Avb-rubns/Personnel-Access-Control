namespace Rubns.Application.Interface.Users.Queries
{
    public interface IGetUsersUseCase
    {
        Task ExecuteAsync(string search, int? page, int? pageSize);
    }
}
