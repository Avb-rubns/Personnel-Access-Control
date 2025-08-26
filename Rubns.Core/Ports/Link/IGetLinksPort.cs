using Personnel.Client.Shared.DTOs.Link;

namespace Rubns.Core.Ports.Link
{
    public interface IGetLinksPort
    {
        Task<List<LinkDTO>> GetPortsAsync(int? page, int? pagesize, string? filter);
    }
}
