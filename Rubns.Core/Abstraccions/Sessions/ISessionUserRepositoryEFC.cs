namespace Rubns.Core.Abstraccions.Sessions
{
    public interface ISessionUserRepositoryEFC
    {
        Task<Session> FindByTokenAsyn(string token);
        Task<int> AddAsync(int userId, string token);
        Task<int> UpdateAsync(Session session);

    }
}
