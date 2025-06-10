namespace Rubns.Core.Services
{
    public interface ILogInService
    {
        JWT CreateJWT(UserDTO user);
        string CreateClaims(UserDTO user, string salt);
        string CreateRefreshToken();
    }
}
