namespace Rubns.WebAPI.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ScrapperController : ControllerBase
    {

        private readonly IGetMetasUseCase _getMetasUseCase;
        private readonly IGetMetasOutputPort _getMetasOutputPort;

        private readonly IGetHeadUseCase _getHeadUseCase;
        private readonly IGetHeadOutputPort _getHeadOutputPort;

        private readonly IGetTitleUseCase _getTitleUseCase;
        private readonly IGetTitleOutputPort _getTitleOutputPort;

        public ScrapperController(IGetMetasUseCase metaProxyPort,
            IGetMetasOutputPort metasOutputPort,
            IGetHeadUseCase headPort,
            IGetHeadOutputPort headOutputPort,
            IGetTitleUseCase titlePort,
            IGetTitleOutputPort titleOutputPort)
        {
            _getMetasUseCase = metaProxyPort;
            _getMetasOutputPort = metasOutputPort;
            _getHeadUseCase = headPort;
            _getTitleUseCase = titlePort;
            _getHeadOutputPort = headOutputPort;
            _getTitleOutputPort = titleOutputPort;


        }

        [HttpGet("metas")]
        public async Task<IActionResult> GetMeta([FromQuery] string url)
        {
            await _getMetasUseCase.ExecuteAsync(url);
            var data = _getMetasOutputPort.Result;
            return Ok(data);
        }

        [HttpGet("head")]
        public async Task<IActionResult> GetHeadAsync([FromQuery] string url)
        {

            await _getHeadUseCase.ExecuteAsync(url);
            var data = _getHeadOutputPort.Result;
            return Ok(data);

        }

        [HttpGet("title")]
        public async Task<IActionResult> GetTitleAsync([FromQuery] string url)
        {

            await _getTitleUseCase.ExecuteAsync(url);
            var data = _getTitleOutputPort.Result;
            return Ok(data);

        }

    }
}
