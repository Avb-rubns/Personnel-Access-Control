namespace Rubns.Application.Interface.Links.Post
{
    public interface ICreateLinkPort
    {
        Task CreateLinkAsync(LinkCreateDTO createDTO);
    }
}
