namespace Rubns.Application.Interface.Checker.Queries
{
    public interface IGetCheckUserTodayOutputPort
    : IPresenter<List<CheckUserTodayDTO>>
    {
        Task Success(List<UserCheck> checks);
    }

}
