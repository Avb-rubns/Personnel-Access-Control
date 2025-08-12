namespace Rubns.Core.Ports.QR
{
    public interface IQRRepositoryEFC
    {
        Task<QRDTO> AddAsync(QRCreateDTO qR);
    }
}
