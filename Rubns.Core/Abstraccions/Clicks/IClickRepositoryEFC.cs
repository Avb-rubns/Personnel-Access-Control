namespace Rubns.Core.Abstraccions.Clicks
{
    public interface IClickRepositoryEFC
    {
        Task<int> InsertAsync(Click click);
        Task<List<Click>> GetClicksByLinkIdAsync(int id, DateTime starDate, DateTime endDate);
    }
}
