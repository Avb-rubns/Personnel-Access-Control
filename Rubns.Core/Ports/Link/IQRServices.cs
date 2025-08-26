using Personnel.Client.Shared.DTOs.Link;

namespace Rubns.Core.Ports.Link
{
    public interface IQRServices
    {
        Task<MemoryStream> GenerateQRCodeAsync(GenerateQrDTO qrDTO);
    }
}
