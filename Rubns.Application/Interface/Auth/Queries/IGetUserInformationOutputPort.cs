namespace Rubns.Application.Interface.Auth.Queries
{
    public interface IGetUserInformationOutputPort : IPresenter<UserInfoDTO>
    {
        Task ExecuteAsync(UserClaim userClaim);
    }
}
