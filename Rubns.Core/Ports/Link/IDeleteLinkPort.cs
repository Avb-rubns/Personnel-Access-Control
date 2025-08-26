namespace Rubns.Core.Ports.Link
{
    public interface IDeleteLinkPort
    {
        Task<int> DeleteLinkPortAsync(int id);
    }
}
