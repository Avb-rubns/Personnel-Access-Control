namespace Rubns.Core.Ports.Auth
{
    public interface IResetPasswordEFC
    {
        Task<int> AddResetPasswordAsync(ResetPasswordDTO reset);
        Task<ResetPasswordDTO> FindResetPasswordAsync(string token);
        Task<bool> FindUserIDResetPasswordAsync(int userID);
        Task<int> DeleteResetPasswordAsync(ResetPasswordDTO reset);
    }
}
