using Rubns.Core.DTOs.LogOut;

namespace Rubns.Core.Ports.SessionUser
{
    public interface ISessionUserRepository
    {
        Task<SessionUserDTO> FindAsyn(string token);
        Task<int> AddSessionAsync(int userId, string token);
        Task<int> DeleteSessionAsync(string token);
        Task<int> UpdateSessionUserAsync(SessionUserDTO session);
    }
}
