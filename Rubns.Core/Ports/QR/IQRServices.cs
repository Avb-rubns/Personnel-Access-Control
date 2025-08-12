namespace Rubns.Core.Ports.QR
{
    public interface IQRServices
    {
        Task<MemoryStream> GenerateQRCodeAsync(GenerateQrDTO qrDTO);
    }
}
