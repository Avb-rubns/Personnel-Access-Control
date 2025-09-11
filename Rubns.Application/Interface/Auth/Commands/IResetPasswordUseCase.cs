namespace Rubns.Application.Interface.Auth.Commands
{
    public interface IResetPasswordUseCase
    {
        Task ExecuteAsync(ResetPasswordRequestDTO request);
    }
}
