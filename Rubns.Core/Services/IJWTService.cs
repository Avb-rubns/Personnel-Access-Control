namespace Rubns.Core.Services
{
    public interface IJWTService
    {
        public UserInfoDTO JWTtoUserInfo(string jwt);
    }
}
