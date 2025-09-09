namespace Rubns.WebAPI.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ScrapperController : ControllerBase
    {

        private readonly IMetaProxyInputPort _metaProxyPort;
        private readonly IMetasOutputPort _metasOutputPort;

        private readonly IHeadInputPort _headPort;
        private readonly IHeadOutputPort _headOutputPort;

        private readonly ITitleInputPort _titlePort;
        private readonly ITitleOutputPort _titleOutputPort;

        public ScrapperController(IMetaProxyInputPort metaProxyPort,
            IMetasOutputPort metasOutputPort,
            IHeadInputPort headPort,
            IHeadOutputPort headOutputPort,
            ITitleInputPort titlePort,
            ITitleOutputPort titleOutputPort)
        {
            _metaProxyPort = metaProxyPort;
            _metasOutputPort = metasOutputPort;
            _headPort = headPort;
            _titlePort = titlePort;
            _headOutputPort = headOutputPort;
            _titleOutputPort = titleOutputPort;


        }

        [HttpGet("metas")]
        public async Task<IActionResult> GetMeta([FromQuery] string url)
        {
            await _metaProxyPort.GetMetaAsync(url);
            var data = _metasOutputPort.Content;
            return Ok(data);
        }

        [HttpGet("head")]
        public async Task<IActionResult> GetHeadAsync([FromQuery] string url)
        {

            await _headPort.GetHeadAsync(url);
            var data = _headOutputPort.Content;
            return Ok(data);

        }

        [HttpGet("title")]
        public async Task<IActionResult> GetTitleAsync([FromQuery] string url)
        {

            await _titlePort.GetPortPort(url);
            var data = _titleOutputPort.Content;
            return Ok(data);

        }

    }
}
