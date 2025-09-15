namespace Rubns.Application.Interface.Links.Queries
{
    public interface IGetLinksOutputPort : IPresenter<LinksDTO>
    {
        Task Success(List<LinkWithCountClick> links, int total, int currentPage, int pageSize, bool hasNextPage, bool hasPreviousPage);
    }
}
