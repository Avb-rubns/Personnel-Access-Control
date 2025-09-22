namespace Rubns.Application.Interface.Clicks.Queries
{
    public interface IGetClicksUseCase
    {
        Task ExecuteAsync(string id, DateTime? startDate, DateTime? endDate);
    }
}
