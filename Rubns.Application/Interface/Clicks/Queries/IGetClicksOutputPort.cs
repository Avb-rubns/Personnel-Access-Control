namespace Rubns.Application.Interface.Clicks.Queries
{
    public interface IGetClicksOutputPort : IPresenter<ClicksDashboardDto>
    {
        Task Success(ClicksDashboard clicks);
    }
}
