namespace Rubns.Application.Interface.Links.Queries
{
    public interface IGetLinksByDashboadUseCase
    {
        Task ExecuteAsync(int page, int pageSize);
    }
}
