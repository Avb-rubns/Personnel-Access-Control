using Personnel.Client.Shared.DTOs.Link;

namespace Rubns.Core.Ports.Link
{
    public interface ICreateLinkPort<T>
    {
        Task<T> CreateQRAsync(LinkCreateDTO createDTO);
    }
}
