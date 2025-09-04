namespace Rubns.Core.Abstraccions.Links
{
    public interface ILinkRepositoryEFC
    {
        Task<Link> AddAsync(Link link);
        Task<string> FindSlugAsync(string slug);
        Task<List<Link>> GetAllLinksForPageAsync(int? page, int? pageSize, string filter);
        Task<int> CountLinksAsync();
        Task<Link> GetLinkForIdAsync(int id);
        Task<Link> GetLinkForSlugAsync(string slug);
        Task<int> UpdateQRAsync(int id, int userId, QR qr);
        Task<int> DeleteLinkAsync(int id);
    }
}
