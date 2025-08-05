namespace Rubns.Core.Ports.SessionUser
{
    public interface ISessionUserRepositoryEFC
    {
        Task<SessionUserDTO> FindAsyn(string token);
        Task<int> AddSessionAsync(int userId, string token);
        Task<int> UpdateSessionUserAsync(SessionUserDTO session);
    }
}
