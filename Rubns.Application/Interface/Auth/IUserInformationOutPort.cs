namespace Rubns.Application.Interface.Auth
{
    public interface IUserInformationOutPort
    {
        Task Handler(UserClaim userClaim);
    }
}
