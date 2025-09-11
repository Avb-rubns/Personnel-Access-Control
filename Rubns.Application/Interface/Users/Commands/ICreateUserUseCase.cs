namespace Rubns.Application.Interface.Users.Commands
{
    public interface ICreateUserUseCase
    {
        Task ExecuteAsync(RegisterUserDTO registerUser);
    }
}
