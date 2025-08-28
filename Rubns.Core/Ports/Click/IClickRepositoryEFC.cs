namespace Rubns.Core.Ports.Click
{
    public interface IClickRepositoryEFC
    {
        Task<int> InsertAsync(ClickDTO click);
    }
}
