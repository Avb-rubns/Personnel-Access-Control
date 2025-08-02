namespace Rubns.Core.Ports.Auth
{
    public interface IRefreshJWTPort<T>
    {
        Task<T> RefreshJWTAsync(string refresh);
    }
}
