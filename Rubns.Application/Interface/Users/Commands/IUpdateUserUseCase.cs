namespace Rubns.Application.Interface.Users.Commands
{
    public interface IUpdateUserUseCase
    {
        Task Executeasync(string userID, JsonPatchDocument<UserRegistedDTO> patchDocument);
    }
}
