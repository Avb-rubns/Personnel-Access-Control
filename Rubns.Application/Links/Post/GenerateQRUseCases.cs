
using Personnel.Client.Shared.DTOs.Link;
using Rubns.Core.POCO.Link;

namespace Rubns.Application.Links.Post
{
    internal class GenerateQRUseCases(IQRServices qRServices
        , ILogger logger)
        : IGenerateQRPort<MemoryStream>
    {
        private readonly IQRServices _qRServices = qRServices;
        private readonly ILogger _logger = logger;

        public async Task<MemoryStream> GenerateQRCodeAsync(GenerateQrDTO qrDTO)
        {
            MemoryStream result = new MemoryStream();
            try
            {
                result = await _qRServices.GenerateQRCodeAsync(qrDTO);

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error GenerateQRCodeAsync :{error}", ex.Message);
            }
            return result;
        }
    }
}
