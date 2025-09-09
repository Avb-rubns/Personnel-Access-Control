namespace Rubns.Application.Interface.Scrapper.Get
{
    public interface IMetasOutputPort : IPresenter<Dictionary<string, string>>
    {
        Task Handeler(Dictionary<string, string> metas);
    }
}
