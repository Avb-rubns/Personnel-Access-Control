namespace Rubns.Core.Ports.QR
{
    public interface IQRsGetPort
    {
        Task<List<QRDTO>> GetPortsAsync(int? page, int? pagesize, string? filter);
    }
}
