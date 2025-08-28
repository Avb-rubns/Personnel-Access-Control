namespace Rubns.Core.Ports.Link
{
    public interface IGetLinkPort
    {
        Task<LinkDTO> GetLinkPortAsync();
    }
}
