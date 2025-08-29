namespace Rubns.Core.Abstraccions.Users
{
    public interface IUserRepositoryDapper
    {

        Task<UserWithRolInfo> GetUserByEmailAsync(string email);
        Task<UserWithRolInfo> GetUserByIDAsync(int ID);
        Task<UserWithRolInfo> GetUserByPhoneAsync(string number);
        Task<int> UpdateUserPasswordforUserIDAsync(int useID, string newPassword);
    }
}
