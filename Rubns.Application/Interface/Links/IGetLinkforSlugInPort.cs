namespace Rubns.Application.Interface.Links
{
    public interface IGetLinkforSlugInPort
    {
        Task SearchLinkforSlug(HttpRequest request, string slug);
    }
}
