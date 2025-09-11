namespace Rubns.Application.Interface.Scrapper.Queries
{
    public interface IGetTitleOutputPort : IPresenter<Dictionary<string, string>>
    {
        Task Success(Dictionary<string, string> titles);
    }
}
