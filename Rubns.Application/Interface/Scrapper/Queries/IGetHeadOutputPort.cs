namespace Rubns.Application.Interface.Scrapper.Queries
{
    public interface IGetHeadOutputPort : IPresenter<List<Dictionary<string, string>>>
    {
        Task Success(List<Dictionary<string, string>> head);
    }
}
