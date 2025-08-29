namespace Rubns.Application.Interface.Users
{
    public interface IPatchUserPort
    {
        Task<int> PatchUserAsync(int userID, JsonPatchDocument<UserRegistedDTO> patchDocument);
    }
}
