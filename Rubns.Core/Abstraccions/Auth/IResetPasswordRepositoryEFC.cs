namespace Rubns.Core.Abstraccions.Auth
{
    public interface IResetPasswordRepositoryEFC
    {
        Task<int> AddAsync(ResetPassword reset);
        Task<ResetPassword> FindbyTokenAsync(string token);
        Task<bool> FindByUserIdAsync(int userID);
        Task<int> DeleteAsync(ResetPassword reset);
    }
}
