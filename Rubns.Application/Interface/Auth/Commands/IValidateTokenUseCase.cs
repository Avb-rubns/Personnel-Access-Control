namespace Rubns.Application.Interface.Auth.Commands
{
    public interface IValidateTokenUseCase
    {
        Task ExecuteAsync(string token);
    }
}
