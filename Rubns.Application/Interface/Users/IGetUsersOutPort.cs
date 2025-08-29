namespace Rubns.Application.Interface.Users
{
    public interface IGetUsersOutPort
    {
        Task Handler(List<User> users, int total);
    }
}
