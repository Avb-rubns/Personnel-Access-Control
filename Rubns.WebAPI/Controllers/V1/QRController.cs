namespace Rubns.WebAPI.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [RoleAndStatusAuth("Administrator,root")]
    public class QRController : ControllerBase
    {
        private IQRGeneratePort<MemoryStream> QRGeneratePort { get; }
        private IQRCreatePort<QRDTO> QRCreatePort { get; }
        private IQRsGetPort QRsGetPort { get; }

        public QRController(IQRGeneratePort<MemoryStream> qrGeneratePort
            , IQRCreatePort<QRDTO> qRCreatePort
            , IQRsGetPort qRsGetPort)
        {
            QRGeneratePort = qrGeneratePort;
            QRCreatePort = qRCreatePort;
            QRsGetPort = qRsGetPort;
        }

        [HttpGet("qrs")]
        public async Task<IActionResult> GetQrsAsync(int? page = 0, int? pageSize = 10, string? filter = "")
        {
            var data = await QRsGetPort.GetPortsAsync(page, pageSize, filter);
            switch (data.Count)
            {
                case > 0:
                    return Ok(data);
                case 0:
                    return NoContent();
            }
            return BadRequest();
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
