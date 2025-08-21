namespace Rubns.Core.Ports.Scrapper
{
    public interface IHeadPort<R>
    {
        Task<R> GetHeadAsync(string url);
    }
}
