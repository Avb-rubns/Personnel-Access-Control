namespace Rubns.Application.Interface.Scrapper.Queries
{
    public interface IGetMetasUseCase
    {
        Task ExecuteAsync(string url);
    }
}
