namespace Rubns.WebAPI.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [RoleAndStatusAuth("Administrador,root")]
    public class LinkController : ControllerBase
    {
        private readonly IGetLinksUseCase _getLinksUseCase;
        private readonly IGetLinksOutputPort _getLinksOutputPort;
        private readonly IGenerateQRPort<MemoryStream> _generateQRPort;
        private readonly ICreateLinkUseCase _createLinkUseCase;
        private readonly IUpdateQRUseCase _updateQRUseCase;
        private readonly IDeleteLinkUseCase _deleteLinkUseCase;
        private readonly ISearchSlugUseCase _searchSlugUseCase;

        public LinkController(IGenerateQRPort<MemoryStream> qrGeneratePort
            , ICreateLinkUseCase qrCreatePort
            , IGetLinksUseCase qRsGetPort
            , IUpdateQRUseCase updateQREditorPort
            , IDeleteLinkUseCase deleteLinkPort
            , ISearchSlugUseCase searchSlugPort
            , IGetLinksOutputPort getLinksOurPort)
        {
            _generateQRPort = qrGeneratePort;
            _createLinkUseCase = qrCreatePort;
            _getLinksUseCase = qRsGetPort;
            _updateQRUseCase = updateQREditorPort;
            _deleteLinkUseCase = deleteLinkPort;
            _searchSlugUseCase = searchSlugPort;
            _getLinksOutputPort = getLinksOurPort;
        }

        [HttpGet("links")]
        public async Task<IActionResult> GetQrsAsync(int? page = 1, int? pageSize = 10, string? filter = "all")
        {
            await _getLinksUseCase.ExecuteAsync(page.Value, pageSize.Value, filter);
            var links = _getLinksOutputPort.Result;
            return links?.Links.Count > 0 ? Ok(links) : NoContent();
        }
        [HttpPost("generate")]
        public async Task<IActionResult> GenerateCode(GenerateQrDTO generateQr)
        {
            var result = await _generateQRPort.GenerateQRCodeAsync(generateQr);
            return File(result, "image/png", $"{generateQr.Content}.png");
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreateLinkAsync(LinkCreateDTO generateQr)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            generateQr.Url = baseUrl;
            await _createLinkUseCase.ExecuteAsync(generateQr);
            return Created();
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateQRAsync(int id, [FromBody] JsonPatchDocument<QRDTO> patchDocument)
        {
            await _updateQRUseCase.ExecuteAsync(id, patchDocument);
            return Ok();

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLinkAsync(int id)
        {
            await _deleteLinkUseCase.ExecuteAsync(id);
            return Ok();
        }
        [HttpGet("check")]
        public async Task<IActionResult> CheckSlugAsync(string slug)
        {
            await _searchSlugUseCase.SearchSlugAsync(slug, true);
            return Ok();
        }
    }
}
