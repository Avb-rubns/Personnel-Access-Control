namespace Rubns.Application.Interface.Checker
{
    public interface IGetCheckUserTodayOutPort
    : IPresenter<List<CheckUserTodayDTO>>
    {
        Task Handler(List<UserCheck> checks);
    }

}
