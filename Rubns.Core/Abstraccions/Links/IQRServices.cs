using Personnel.Client.Shared.DTOs.Link.Commands;

namespace Rubns.Core.Abstraccions.Links
{
    public interface IQRServices
    {
        Task<MemoryStream> GenerateQRCodeAsync(GenerateQrDTO qrDTO);
    }
}
