namespace Rubns.Core.Ports.Auth
{
    public interface IUserInformationPort
    {
        public UserInfoDTO UserInfo(string JWT);
    }
}
