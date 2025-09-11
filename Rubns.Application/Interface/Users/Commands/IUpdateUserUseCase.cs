namespace Rubns.Application.Interface.Users.Commands
{
    public interface IUpdateUserUseCase
    {
        Task Executeasync(int userID, JsonPatchDocument<UserRegistedDTO> patchDocument);
    }
}
