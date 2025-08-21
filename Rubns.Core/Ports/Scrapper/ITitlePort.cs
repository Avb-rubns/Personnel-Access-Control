namespace Rubns.Core.Ports.Scrapper
{
    public interface ITitlePort<R>
    {
        Task<R> GetPortPort(string url);
    }
}
