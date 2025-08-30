namespace Rubns.Core.Ports.Link
{
    public interface IGetLinksPort
    {
        Task GetPortsAsync(int page, int pagesize, string filter);
    }
}
