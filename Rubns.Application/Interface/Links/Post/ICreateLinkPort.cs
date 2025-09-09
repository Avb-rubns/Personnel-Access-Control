namespace Rubns.Application.Interface.Links.Post
{
    public interface ICreateLinkPort
    {
        Task CreateQRAsync(LinkCreateDTO createDTO);
    }
}
