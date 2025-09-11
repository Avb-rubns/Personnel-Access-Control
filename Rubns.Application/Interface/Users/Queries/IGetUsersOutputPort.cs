namespace Rubns.Application.Interface.Users.Queries
{
    public interface IGetUsersOutputPort : IPresenter<TableUserDTO>
    {
        Task Success(List<User> users, int total);
    }
}
