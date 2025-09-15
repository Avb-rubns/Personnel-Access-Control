namespace Rubns.Application.Interface.Links.Queries
{
    public interface IGetLinkBySlugOutputPort : IPresenter<LinkDTO>
    {
        Task Success(Link link);
    }
}
