namespace Rubns.Core.Ports.CheckUser
{
    public interface ICheckUserRepositoryDapper
    {
        Task<List<UserCheck>> CheckUserTodayAsync();
    }
}
