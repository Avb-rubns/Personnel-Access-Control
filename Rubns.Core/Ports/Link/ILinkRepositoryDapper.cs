namespace Rubns.Core.Ports.Link
{
    public interface ILinkRepositoryDapper
    {

        Task<List<LinkTableDTO>> GetLinksAsync(int page, int rows, string filter);
    }
}
