namespace Rubns.Core.Abstraccions.Sessions
{
    public interface ISessionUserRepositoryDapper
    {
        Task<int> DeleteByTokenAsync(string token);
        Task<int> DeleteByUserIdAsync(int userID);
    }
}
