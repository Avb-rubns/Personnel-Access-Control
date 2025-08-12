namespace Rubns.Core.Ports.QR
{
    public interface IQRCreatePort<T>
    {
        Task<T> CreateQRAsync(QRCreateDTO createDTO);
    }
}
