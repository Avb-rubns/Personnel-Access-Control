namespace Rubns.Application.QR.Get
{
    internal class QRListUseCase(IQRRepositoryEFC qRRepositoryEFC
        , ILogger logger)
        : IQRsGetPort
    {

        private readonly IQRRepositoryEFC _qRRepositoryEFC = qRRepositoryEFC;
        private readonly ILogger _logger = logger;

        public async Task<List<QRDTO>> GetPortsAsync(int? page, int? pagesize, string? filter)
        {
            List<QRDTO> qrs = new List<QRDTO>();

            try
            {
                qrs = await _qRRepositoryEFC.GetAllQrsForPageAsync(page, pagesize, filter);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error GetPortsAsync:{error}", ex.Message);

            }
            return qrs;
        }
    }
}
