namespace Rubns.Core.Abstraccions.Sessions
{
    public interface ISessionUserRepositoryDapper
    {
        Task<int> DeleteSessionforTokenAsync(string token);
        Task<int> DeleteSessionforUserIdAsync(int userID);
    }
}
