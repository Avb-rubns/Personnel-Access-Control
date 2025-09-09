namespace Rubns.Application.Interface.Links.Get
{
    public interface IGetLinksOutputPort : IPresenter<TableLinkDTO>
    {
        Task Handler(List<LinkWithClicks> links, int total, int currentPage, int pageSize, bool hasNextPage, bool hasPreviousPage);
    }
}
