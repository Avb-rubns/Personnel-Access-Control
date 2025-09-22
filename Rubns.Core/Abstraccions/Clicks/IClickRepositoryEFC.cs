namespace Rubns.Core.Abstraccions.Clicks
{
    public interface IClickRepositoryEFC
    {
        Task<int> InsertAsync(Click click);
        Task<List<Click>> GetClicksAsync(int id, DateTime starDate, DateTime endDate);
    }
}
