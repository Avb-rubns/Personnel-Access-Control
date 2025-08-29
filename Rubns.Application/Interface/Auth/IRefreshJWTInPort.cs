namespace Rubns.Application.Interface.Auth
{
    public interface IRefreshJWTInPort
    {
        Task RefreshJWTAsync(string refresh);
    }
}
