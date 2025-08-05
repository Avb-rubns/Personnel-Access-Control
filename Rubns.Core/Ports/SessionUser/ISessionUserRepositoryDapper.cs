namespace Rubns.Core.Ports.SessionUser
{
    public interface ISessionUserRepositoryDapper
    {
        Task<int> DeleteSessionforTokenAsync(string token);
        Task<int> DeleteSessionforUserIdAsync(int userID);
    }
}
