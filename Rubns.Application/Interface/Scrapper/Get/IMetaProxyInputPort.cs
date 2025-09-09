namespace Rubns.Application.Interface.Scrapper.Get
{
    public interface IMetaProxyInputPort
    {
        Task GetMetaAsync(string url);
    }
}
