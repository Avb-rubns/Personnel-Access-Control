namespace Rubns.Application.Interface.Links.Delete
{
    public interface IDeleteLinkPort
    {
        Task DeleteLinkAsync(int id);
    }
}
