namespace Rubns.Application.Interface.Auth
{
    public interface IForgotPassword
    {
        Task GeneratePasswordResetTokenAsync(ForgotPasswordDTO request, string host);
    }
}
