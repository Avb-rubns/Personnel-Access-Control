namespace Rubns.Application.Interface.Auth
{
    public interface IResetPasswordPort
    {
        Task ResetPasswordAsync(ResetPasswordRequestDTO request);
    }
}
