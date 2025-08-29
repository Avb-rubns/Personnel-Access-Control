namespace Rubns.Core.Services
{
    public interface ILogInService
    {
        JWT CreateJWT(UserWithRolInfo user);
        string CreateClaims(UserWithRolInfo user, string salt);
        string CreateRefreshToken();
    }
}
