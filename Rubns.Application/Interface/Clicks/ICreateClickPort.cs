namespace Rubns.Application.Interface.Clicks
{
    public interface ICreateClickPort
    {
        Task CreateAsync(HttpRequest request, int idLink);
    }
}
