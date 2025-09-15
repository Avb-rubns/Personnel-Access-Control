namespace Rubns.Core.Abstraccions.Links
{
    public interface ILinkRepositoryEFC
    {
        Task<Link> AddAsync(Link link);
        Task<string> FindBySlugAsync(string slug);
        Task<int> CountLinksAsync();
        Task<List<LinkWithCountClick>> GetLinkWithClickByPaginationAsync(int page, int pageSize, string status);
        Task<Link> GetLinkByIdAsync(int id);
        Task<Link> GetLinkBySlugAsync(string slug);
        Task<int> DeleteAsync(int id);
    }
}
