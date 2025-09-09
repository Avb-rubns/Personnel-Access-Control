namespace Rubns.Application.Interface.Links.Get
{
    public interface IGetLinksPort
    {
        Task GetPortsAsync(int page, int pagesize, string filter);
    }
}
