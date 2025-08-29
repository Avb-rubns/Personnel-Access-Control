namespace Rubns.Application.Interface.Auth
{
    public interface IUserInformationInPort
    {
        Task UserInfo(string JWT);
    }
}
