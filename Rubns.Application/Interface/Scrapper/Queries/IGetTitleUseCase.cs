namespace Rubns.Application.Interface.Scrapper.Queries
{
    public interface IGetTitleUseCase
    {
        Task ExecuteAsync(string url);
    }
}
