namespace Rubns.Application.Interface.Scrapper.Get
{
    public interface IHeadOutputPort : IPresenter<List<Dictionary<string, string>>>
    {
        Task Handler(List<Dictionary<string, string>> head);
    }
}
