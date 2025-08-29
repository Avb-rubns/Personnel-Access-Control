namespace Rubns.Core.Abstraccions.Auth
{
    public interface IResetPasswordEFC
    {
        Task<int> AddResetPasswordAsync(ResetPassword reset);
        Task<ResetPassword> FindResetPasswordAsync(string token);
        Task<bool> FindUserIDResetPasswordAsync(int userID);
        Task<int> DeleteResetPasswordAsync(ResetPassword reset);
    }
}
