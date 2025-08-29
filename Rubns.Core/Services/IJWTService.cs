namespace Rubns.Core.Services
{
    public interface IJWTService
    {
        public UserClaim JWTtoUserInfo(string jwt);
    }
}
