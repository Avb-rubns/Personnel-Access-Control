namespace Rubns.Core.Ports.Link
{
    public interface ILinkRepositoryEFC
    {
        Task<LinkDTO> AddAsync(LinkCreateDTO qR);
        Task<string> FindSlugAsync(string slug);
        Task<List<LinkDTO>> GetAllLinksForPageAsync(int? page, int? pageSize, string filter);
        Task<int> CountLinksAsync();
        Task<LinkDTO> GetLinkForIdAsync(int id);
        Task<LinkDTO> GetLinkForSlugAsync(string slug);
        Task<int> UpdateQRAsync(int id, int userId, QRDTO qr);
        Task<int> DeleteLinkAsync(int id);
    }
}
