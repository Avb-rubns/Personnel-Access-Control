namespace Rubns.WebAPI.Controllers.V1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [RoleAndStatusAuth("Administrador,root")]
    public class LinkController : ControllerBase
    {
        private readonly IGenerateQRPort<MemoryStream> _generateQRPort;

        private readonly IGetLinksUseCase _getLinksUseCase;
        private readonly IGetLinksOutputPort _getLinksOutputPort;

        private readonly ICreateLinkUseCase _createLinkUseCase;

        private readonly IUpdateQRUseCase _updateQRUseCase;

        private readonly IDeleteLinkUseCase _deleteLinkUseCase;

        private readonly ISearchSlugUseCase _searchSlugUseCase;

        private readonly IGetLinkBySlugUseCase _getLinkBySlugUseCase;
        private readonly IGetLinkBySlugOutputPort _getLinkBySlugOutputPort;

        public LinkController(IGenerateQRPort<MemoryStream> qrGeneratePort
            , ICreateLinkUseCase qrCreatePort
            , IGetLinksUseCase qRsGetPort
            , IUpdateQRUseCase updateQREditorPort
            , IDeleteLinkUseCase deleteLinkPort
            , ISearchSlugUseCase searchSlugPort
            , IGetLinksOutputPort getLinksOurPort,
            IGetLinkBySlugUseCase getLinkUseCase,
            IGetLinkBySlugOutputPort getLinkOutputPort)
        {
            _generateQRPort = qrGeneratePort;
            _createLinkUseCase = qrCreatePort;
            _getLinksUseCase = qRsGetPort;
            _updateQRUseCase = updateQREditorPort;
            _deleteLinkUseCase = deleteLinkPort;
            _searchSlugUseCase = searchSlugPort;
            _getLinksOutputPort = getLinksOurPort;
            _getLinkBySlugUseCase = getLinkUseCase;
            _getLinkBySlugOutputPort = getLinkOutputPort;
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
        public async Task<IActionResult> UpdateQRAsync(string id, [FromBody] JsonPatchDocument<QRUpdateDTO> patchDocument)
        {
            await _updateQRUseCase.ExecuteAsync(id, patchDocument);
            return Ok();

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLinkAsync(string id)
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
        [HttpGet("{slug}")]
        public async Task<IActionResult> GetLinkAsync(string slug)
        {
            await _getLinkBySlugUseCase.ExecuteAsync(slug);
            var link = _getLinkBySlugOutputPort.Result;
            return Ok(link);
        }
    }
}
