namespace Rubns.Application.Interface.Links.Queries
{
    public interface IGetURLDestinationBySlugUseCase
    {
        Task ExecuteAsync(HttpRequest request, string slug);
    }
}
