namespace Rubns.WebAPI.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [RoleAndStatusAuth("Administrator,root")]
    public class QRController : ControllerBase
    {
        IQRGeneratePort<MemoryStream> QRGeneratePort { get; }
        IQRCreatePort<QRDTO> QRCreatePort { get; }
        public QRController(IQRGeneratePort<MemoryStream> qrGeneratePort
            , IQRCreatePort<QRDTO> qRCreatePort)
        {
            QRGeneratePort = qrGeneratePort;
            QRCreatePort = qRCreatePort;
        }

        [HttpPost("generate")]
        public async Task<IActionResult> GenerateCode(GenerateQrDTO generateQr)
        {
            var result = await QRGeneratePort.GenerateQRCodeAsync(generateQr);
            return File(result, "image/png", $"{generateQr.Content}.png");
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateCodeAsync(QRCreateDTO generateQr)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            generateQr.Url = baseUrl;
            var result = await QRCreatePort.CreateQRAsync(generateQr);
            if (result is { ID: > 0 })
            {
                return Ok(result);
            }
            return BadRequest();
        }
    }
}
