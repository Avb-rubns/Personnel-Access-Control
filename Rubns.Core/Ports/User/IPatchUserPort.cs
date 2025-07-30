namespace Rubns.Core.Ports.User
{
    public interface IPatchUserPort
    {
        Task<int> PatchUserAsync(int userID, JsonPatchDocument<UserRegistedDTO> patchDocument);
    }
}
