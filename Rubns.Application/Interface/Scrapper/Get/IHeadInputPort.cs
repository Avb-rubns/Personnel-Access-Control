namespace Rubns.Application.Interface.Scrapper.Get
{
    public interface IHeadInputPort
    {
        Task GetHeadAsync(string url);
    }
}
