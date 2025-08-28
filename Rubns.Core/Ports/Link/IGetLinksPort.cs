namespace Rubns.Core.Ports.Link
{
    public interface IGetLinksPort
    {
        Task<TableLinkDTO> GetPortsAsync(int page, int pagesize, string filter);
    }
}
