namespace Rubns.Core.Ports.Auth
{
    public interface IForgotPassword
    {
        Task<bool> GeneratePasswordResetTokenAsync(ForgotPasswordDTO request, string host);
    }
}
