namespace Rubns.Application.Interface.Auth.Commands
{
    public interface IForgotPasswordUseCase
    {
        Task ExecuteAsyn(ForgotPasswordDTO request, string host);
    }
}
