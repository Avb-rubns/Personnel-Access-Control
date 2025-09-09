namespace Rubns.WebAPI.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [RoleAndStatusAuth("Administrador,root")]
    public class LinkController : ControllerBase
    {
        private readonly IGetLinksPort _getLinksPort;
        private readonly IGetLinksOutputPort _getLinksOurPort;
        private readonly IGenerateQRPort<MemoryStream> _generateQRPort;
        private readonly ICreateLinkPort _createQRPort;
        private readonly IUpdateQREditorPort _updateQREditorPort;
        private readonly IDeleteLinkPort _deleteLinkPort;
        private readonly ISearchSlugPort _searchSlugPort;

        public LinkController(IGenerateQRPort<MemoryStream> qrGeneratePort
            , ICreateLinkPort qrCreatePort
            , IGetLinksPort qRsGetPort
            , IUpdateQREditorPort updateQREditorPort
            , IDeleteLinkPort deleteLinkPort
            , ISearchSlugPort searchSlugPort
            , IGetLinksOutputPort getLinksOurPort)
        {
            _generateQRPort = qrGeneratePort;
            _createQRPort = qrCreatePort;
            _getLinksPort = qRsGetPort;
            _updateQREditorPort = updateQREditorPort;
            _deleteLinkPort = deleteLinkPort;
            _searchSlugPort = searchSlugPort;
            _getLinksOurPort = getLinksOurPort;
        }

        [HttpGet("links")]
        public async Task<IActionResult> GetQrsAsync(int? page = 0, int? pageSize = 10, string? filter = "all")
        {
            await _getLinksPort.GetPortsAsync(page.Value, pageSize.Value, filter);
            var links = _getLinksOurPort.Content;
            return links?.Links.Count > 0 ? Ok(links) : NoContent();
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
            await _createQRPort.CreateQRAsync(generateQr);
            return Created();
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateQRAsync(int id, [FromBody] JsonPatchDocument<QRDTO> patchDocument)
        {
            if (!ModelState.IsValid) return BadRequest();

            await _updateQREditorPort.UpdateQREditorPortAsync(id, patchDocument);
            return Ok();

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLinkAsync(int id)
        {
            await _deleteLinkPort.DeleteLinkPortAsync(id);
            return Ok();
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
