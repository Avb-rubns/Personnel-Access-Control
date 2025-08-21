namespace Rubns.WebAPI.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ScrapperController(IMetaProxyPort metaProxyPort,
        IHeadPort<List<Dictionary<string, string>>> headPort,
        ITitlePort<Dictionary<string, string>> titlePort)
        : ControllerBase
    {
        readonly IMetaProxyPort _metaProxyPort = metaProxyPort;
        readonly IHeadPort<List<Dictionary<string, string>>> _headPort = headPort;
        readonly ITitlePort<Dictionary<string, string>> _titlePort = titlePort;

        [HttpGet("metas")]
        public async Task<IActionResult> GetMeta([FromQuery] string url)
        {

            if (string.IsNullOrEmpty(url))
            {
                return BadRequest();
            }
            var data = await _metaProxyPort.GetMetaAsync(url);
            if (data.Count() > 0)
            {
                return Ok(data);
            }
            return NoContent();

        }

        [HttpGet("head")]
        public async Task<IActionResult> GetHeadAsync([FromQuery] string url)
        {

            if (string.IsNullOrEmpty(url))
            {
                return BadRequest();
            }
            var data = await _headPort.GetHeadAsync(url);
            if (data.Count() > 0)
            {
                return Ok(data);
            }
            return NoContent();

        }

        [HttpGet("title")]
        public async Task<IActionResult> GetTitleAsync([FromQuery] string url)
        {

            if (string.IsNullOrEmpty(url))
            {
                return BadRequest();
            }
            var data = await _titlePort.GetPortPort(url);
            if (data.Count() > 0)
            {
                return Ok(data);
            }
            return NoContent();

        }

    }
}
