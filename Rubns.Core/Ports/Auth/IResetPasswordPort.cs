namespace Rubns.Core.Ports.Auth
{
    public interface IResetPasswordPort
    {
        Task<bool> ResetPasswordAsync(ResetPasswordRequestDTO request);
    }
}
