namespace Rubns.Core.Ports.Auth
{
    public interface IResetPasswordValidatePort
    {
        Task<bool> ValidateTokenPasswordAsync(string token);
    }
}
