namespace Rubns.Core.Abstraccions.Links
{
    public interface ILinkRepositoryDapper
    {
        Task<List<LinkWithClicks>> GetLinksAsync(int page, int rows, string filter);
        Task<int> CountLinks(string filter);
    }
}
