namespace Rubns.Core.Abstraccions.Clicks
{
    public interface IClickRepositoryEFC
    {
        Task<int> InsertAsync(Click click);
    }
}
