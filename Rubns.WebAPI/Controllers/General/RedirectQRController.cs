namespace Rubns.WebAPI.Controllers.General
{

    [Route("qr/{slug}")]
    [ApiController]
    public class RedirectQRController : ControllerBase
    {
        private readonly IGetURLDestinationBySlugUseCase _getLinkbySlugUseCase;
        private readonly IGetURLDestinationBySlugOutputPort _getLinkbySlugOutputPort;

        public RedirectQRController(IGetURLDestinationBySlugUseCase getLinkforSlugInPort,
            IGetURLDestinationBySlugOutputPort linkforSlugOutport)
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
