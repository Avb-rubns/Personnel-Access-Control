namespace Rubns.WebAPI.Controllers.General
{

    [Route("qr/{slug}")]
    [ApiController]
    public class RedirectQRController : ControllerBase
    {
        private readonly IGetLinkbySlugUseCase _getLinkbySlugUseCase;
        private readonly IGetLinkbySlugOutputPort _getLinkbySlugOutputPort;

        public RedirectQRController(IGetLinkbySlugUseCase getLinkforSlugInPort,
            IGetLinkbySlugOutputPort linkforSlugOutport)
        {
            _getLinkbySlugUseCase = getLinkforSlugInPort;
            _getLinkbySlugOutputPort = linkforSlugOutport;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string slug)
        {
            await _getLinkbySlugUseCase.ExecuteAsync(HttpContext.Request, slug);
            var url = _getLinkbySlugOutputPort.Result;
            return Redirect(url);

        }
    }

}
