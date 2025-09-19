namespace Rubns.Application.Interface.Links.Queries
{
    public interface IGetLinksUseCase
    {
        Task ExecuteAsync(int page, int pagesize, string filter, string? search);
    }
}
