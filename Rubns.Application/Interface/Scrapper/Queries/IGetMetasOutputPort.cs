namespace Rubns.Application.Interface.Scrapper.Queries
{
    public interface IGetMetasOutputPort : IPresenter<Dictionary<string, string>>
    {
        Task Success(Dictionary<string, string> metas);
    }
}
