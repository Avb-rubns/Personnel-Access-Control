namespace Rubns.Core.Abstraccions.Links
{
    public interface IQRRepository
    {
        Task<QR> FindById(int id);
        Task<int> UpdateAsync(QR qr);
    }
}
