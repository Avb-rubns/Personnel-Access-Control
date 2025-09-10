namespace Rubns.Application.Interface.Links.Get
{
    public interface IGetLinksPort
    {
        Task GetLinksAsync(int page, int pagesize, string filter);
    }
}
