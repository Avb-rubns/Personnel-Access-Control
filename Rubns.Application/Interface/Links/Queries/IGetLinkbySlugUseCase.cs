namespace Rubns.Application.Interface.Links.Queries
{
    public interface IGetLinkbySlugUseCase
    {
        Task ExecuteAsync(HttpRequest request, string slug);
    }
}
