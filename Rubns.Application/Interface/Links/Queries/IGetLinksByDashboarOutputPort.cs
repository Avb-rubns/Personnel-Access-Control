namespace Rubns.Application.Interface.Links.Queries
{
    public interface IGetLinksByDashboarOutputPort : IPresenter<LinksTodayDTO>
    {
        Task Success(List<LinkByDashboard> links, int total, int currentPage, int pageSize, bool hasNextPage, bool hasPreviousPage);
    }
}
