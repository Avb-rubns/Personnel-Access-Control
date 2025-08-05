namespace Rubns.Core.Ports.User
{
    public interface IUserRepositoryDapper
    {

        Task<UserDTO> GetUserByEmailAsync(string email);
        Task<UserDTO> GetUserByIDAsync(int ID);
        Task<UserDTO> GetUserByPhoneAsync(string number);
        Task<int> UpdateUserPasswordforUserIDAsync(int useID, string newPassword);
    }
}
