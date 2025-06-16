namespace Rubns.WebAPI.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class QRController : ControllerBase
    {
        IQRGeneratePort<MemoryStream> QRGeneratePort { get; }

        public QRController(IQRGeneratePort<MemoryStream> qrGeneratePort)
        {
            QRGeneratePort = qrGeneratePort;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateCode(GenerateQrDTO generateQr)
        {
            var result = await QRGeneratePort.GenerateQRCodeAsync(generateQr);
            return File(result, "image/png", $"{generateQr.Content}.png");
        }
    }
}
