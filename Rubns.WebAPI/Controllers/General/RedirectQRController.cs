namespace Rubns.WebAPI.Controllers.General
{

    [Route("qr/{slug}")]
    [ApiController]
    public class RedirectQRController : ControllerBase
    {
        private readonly IGetLinkforSlugInputPort _getLinkforSlugInPort;
        private readonly IGetLinkforSlugOutputport _linkforSlugOutport;

        public RedirectQRController(IGetLinkforSlugInputPort getLinkforSlugInPort,
            IGetLinkforSlugOutputport linkforSlugOutport)
        {
            _getLinkforSlugInPort = getLinkforSlugInPort;
            _linkforSlugOutport = linkforSlugOutport;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string slug)
        {
            await _getLinkforSlugInPort.SearchLinkforSlug(HttpContext.Request, slug);
            var url = _linkforSlugOutport.Content;
            return Redirect(url);

        }
    }

}
