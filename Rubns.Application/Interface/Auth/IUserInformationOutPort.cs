namespace Rubns.Application.Interface.Auth
{
    public interface IUserInformationOutPort : IPresenter<UserInfoDTO>
    {
        Task Handler(UserClaim userClaim);
    }
}
