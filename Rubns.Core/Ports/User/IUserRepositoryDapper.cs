namespace Rubns.Core.Ports.User
{
    public interface IUserRepositoryDapper
    {

        Task<UserDTO> GetUserByEmailAsync(string email);
        Task<UserDTO> GetUserByIDAsync(int ID);
        Task<UserDTO> GetUserByPhoneAsync(string number);
    }
}
