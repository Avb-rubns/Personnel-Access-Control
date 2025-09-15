using Personnel.Client.Shared.DTOs.Link.Commands;

namespace Rubns.Core.POCO.Link
{
    public interface IGenerateQRPort<T>
    {
        Task<T> GenerateQRCodeAsync(GenerateQrDTO qrDTO);
    }
}
