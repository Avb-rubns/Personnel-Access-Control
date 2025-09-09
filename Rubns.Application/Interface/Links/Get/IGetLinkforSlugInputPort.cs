namespace Rubns.Application.Interface.Links.Get
{
    public interface IGetLinkforSlugInputPort
    {
        Task SearchLinkforSlug(HttpRequest request, string slug);
    }
}
