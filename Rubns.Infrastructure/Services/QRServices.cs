using Personnel.Client.Shared.DTOs.Link.Commands;

namespace Rubns.Infrastructure.Services
{
    internal class QRServices : IQRServices
    {
        public Task<MemoryStream> GenerateQRCodeAsync(GenerateQrDTO qrDTO)
        {
            using var qrGenerator = new QRCodeGenerator();
            using QRCodeData qrData = qrGenerator.CreateQrCode(qrDTO.Content, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new BitmapByteQRCode(qrData);
            byte[] bitmap = qrCode.GetGraphic(20);
            var ms = new MemoryStream(bitmap);
            ms.Position = 0;
            return Task.FromResult(ms);
        }
    }
}
