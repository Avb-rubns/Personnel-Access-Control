namespace Rubns.Application.Interface.Links.Queries
{
    public interface IGetLinkBySlugOutputPort : IPresenter<LinkDetailDTO>
    {
        Task Success(Link link);
    }
}
