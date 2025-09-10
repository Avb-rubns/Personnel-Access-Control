namespace Rubns.Application.Interface.Links.Get
{
    public interface IGetLinkforSlugInputPort
    {
        Task SearchLinkbySlug(HttpRequest request, string slug);
    }
}
