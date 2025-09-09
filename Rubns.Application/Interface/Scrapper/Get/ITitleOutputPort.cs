namespace Rubns.Application.Interface.Scrapper.Get
{
    public interface ITitleOutputPort : IPresenter<Dictionary<string, string>>
    {
        Task Handler(Dictionary<string, string> titles);
    }
}
