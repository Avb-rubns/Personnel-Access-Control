namespace Rubns.Application.Interface.Links.Queries
{
    public interface IGetLinksOutputPort : IPresenter<TableLinkDTO>
    {
        Task Success(List<LinkWithClicks> links, int total, int currentPage, int pageSize, bool hasNextPage, bool hasPreviousPage);
    }
}
