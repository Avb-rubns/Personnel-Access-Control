namespace Rubns.Core.Ports.QR
{
    public interface ISearchSlugPort
    {
        Task<string> SearchSlugAsync(string query);
    }
}
