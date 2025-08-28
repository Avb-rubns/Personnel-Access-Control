namespace Rubns.WebAPI.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [RoleAndStatusAuth("Administrador,root")]
    public class LinkController : ControllerBase
    {
        private readonly IGenerateQRPort<MemoryStream> _generateQRPort;
        private readonly ICreateLinkPort<LinkDTO> _createQRPort;
        private readonly IGetLinksPort _getLinksPort;
        private readonly IUpdateQREditorPort _updateQREditorPort;
        private readonly IDeleteLinkPort _deleteLinkPort;
        private readonly ISearchSlugPort _searchSlugPort;

        public LinkController(IGenerateQRPort<MemoryStream> qrGeneratePort
            , ICreateLinkPort<LinkDTO> qrCreatePort
            , IGetLinksPort qRsGetPort
            , IUpdateQREditorPort updateQREditorPort
            , IDeleteLinkPort deleteLinkPort
            , ISearchSlugPort searchSlugPort)
        {
            _generateQRPort = qrGeneratePort;
            _createQRPort = qrCreatePort;
            _getLinksPort = qRsGetPort;
            _updateQREditorPort = updateQREditorPort;
            _deleteLinkPort = deleteLinkPort;
            _searchSlugPort = searchSlugPort;
        }

        [HttpGet("links")]
        public async Task<IActionResult> GetQrsAsync(int? page = 0, int? pageSize = 10, string? filter = "")
        {
            var data = await _getLinksPort.GetPortsAsync(page.Value, pageSize.Value, filter);
            switch (data.Total)
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
            var result = await _generateQRPort.GenerateQRCodeAsync(generateQr);
            return File(result, "image/png", $"{generateQr.Content}.png");
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateCodeAsync(LinkCreateDTO generateQr)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            generateQr.Url = baseUrl;
            var result = await _createQRPort.CreateQRAsync(generateQr);
            if (result is { ID: > 0 })
            {
                return Ok(result);
            }
            return BadRequest();
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateQRAsync(int id, [FromBody] JsonPatchDocument<QRDTO> patchDocument)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = await _updateQREditorPort.UpdateQREditorPortAsync(id, patchDocument);
            switch (result)
            {
                case >= 1: return Ok();
                case 0: return NoContent();
                default: return BadRequest();
            }

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLinkAsync(int id)
        {
            var result = await _deleteLinkPort.DeleteLinkPortAsync(id);
            switch (result)
            {
                case >= 1: return Ok();
                case 0: return NoContent();
                default: return BadRequest();
            }
        }
        [HttpGet("check")]
        public async Task<IActionResult> CheckSlugAsync(string slug)
        {
            var result = await _searchSlugPort.SearchSlugAsync(slug, true);

            if (string.IsNullOrEmpty(result))
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
