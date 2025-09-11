namespace Rubns.Core.Abstraccions.Links
{
    public interface ILinkRepositoryEFC
    {
        Task<Link> AddAsync(Link link);
        Task<string> FindBySlugAsync(string slug);
        Task<List<Link>> GetAllLinksAsync(int? page, int? pageSize, string filter);
        Task<int> CountLinksAsync();
        Task<Link> GetLinkByIdAsync(int id);
        Task<Link> GetLinkBySlugAsync(string slug);
        Task<int> UpdateQRAsync(int id, int userId, QR qr);
        Task<int> DeleteAsync(int id);
    }
}
