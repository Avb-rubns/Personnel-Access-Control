namespace Rubns.Application.Interface.Scrapper.Queries
{
    public interface IGetHeadUseCase
    {
        Task ExecuteAsync(string url);
    }
}
