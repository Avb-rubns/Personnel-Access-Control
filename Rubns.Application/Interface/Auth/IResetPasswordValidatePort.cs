namespace Rubns.Application.Interface.Auth
{
    public interface IResetPasswordValidatePort
    {
        Task ValidateTokenPasswordAsync(string token);
    }
}
