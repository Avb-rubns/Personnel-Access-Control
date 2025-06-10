namespace Rubns.Core.Ports.Login
{
    public interface ILogInRepository
    {

        Task<UserDTO> GetUserByEmailAsync(string email);
        Task<UserDTO> GetUserByIDAsync(int ID);
    }
}
