namespace Rubns.Core.Services
{
    public interface IIPGeolocation
    {
        Task<T> GetIpInfo<T>(string clientIP, string ipAddress);
    }
}
