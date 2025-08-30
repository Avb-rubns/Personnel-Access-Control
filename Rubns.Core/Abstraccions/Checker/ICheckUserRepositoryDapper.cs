namespace Rubns.Core.Abstraccions.Checker
{
    public interface ICheckUserRepositoryDapper
    {
        Task<List<UserCheck>> CheckUserTodayAsync();
    }
}
