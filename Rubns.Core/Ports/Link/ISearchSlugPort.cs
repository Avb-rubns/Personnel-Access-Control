namespace Rubns.Core.Ports.Link
{
    public interface ISearchSlugPort
    {
        Task<string> SearchSlugAsync(string query, bool? check = false);
    }
}
