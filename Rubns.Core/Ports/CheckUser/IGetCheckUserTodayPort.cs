namespace Rubns.Core.Ports.CheckUser
{
    public interface IGetCheckUserTodayPort
    {
        Task<List<CheckUserTodayDTO>> CheckTodayAsync();
    }
}
