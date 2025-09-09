namespace Rubns.Core.Ports.Link
{
    public interface IGenerateQRPort<T>
    {
        Task<T> GenerateQRCodeAsync(GenerateQrDTO qrDTO);
    }
}
