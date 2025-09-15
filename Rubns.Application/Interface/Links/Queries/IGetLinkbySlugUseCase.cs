namespace Rubns.Application.Interface.Links.Queries
{
    public interface IGetLinkBySlugUseCase
    {
        Task ExecuteAsync(string slug);
    }
}
