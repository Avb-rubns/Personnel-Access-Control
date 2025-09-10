namespace Rubns.Core.Abstraccions.Sessions
{
    public interface ISessionUserRepositoryEFC
    {
        Task<Session> FindAsyn(string token);
        Task<int> AddSessionAsync(int userId, string token);
        Task<int> UpdateSessionUserAsync(Session session);

    }
}
