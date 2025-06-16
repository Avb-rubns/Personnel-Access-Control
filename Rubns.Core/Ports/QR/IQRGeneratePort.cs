using Rubns.Core.DTOs.QR;

namespace Rubns.Core.Ports.QR
{
    public interface IQRGeneratePort<T>
    {
        Task<T> GenerateQRCodeAsync(GenerateQrDTO qrDTO);
    }
}
