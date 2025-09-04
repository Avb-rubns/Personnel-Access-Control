namespace Rubns.WebAPI.Controllers.General
{

    [Route("qr/{slug}")]
    [ApiController]
    public class RedirectQRController : ControllerBase
    {
        private readonly IGetLinkforSlugInPort _getLinkforSlugInPort;
        private readonly IGetLinkforSlugOutport _linkforSlugOutport;

        public RedirectQRController(IGetLinkforSlugInPort getLinkforSlugInPort,
            IGetLinkforSlugOutport linkforSlugOutport)
        {
            _getLinkforSlugInPort = getLinkforSlugInPort;
            _linkforSlugOutport = linkforSlugOutport;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string slug)
        {
            try
            {
                await _getLinkforSlugInPort.SearchLinkforSlug(HttpContext.Request, slug);
                var url = _linkforSlugOutport.Content;
                return Redirect(url);

            }
            catch (ResourceInactiveException)
            {
                return NotFound();
            }
            catch (NotFoundException)
            {
                return NotFound();
            }
            catch
            {

                return StatusCode(StatusCodes.Status500InternalServerError, new ProblemDetails
                {
                    Title = "Error interno",
                    Detail = "Ocurrió un error inesperado. Intente nuevamente más tarde.",
                    Status = StatusCodes.Status500InternalServerError,
                    Type = "https://httpstatuses.com/500"
                });
            }

        }
    }

}
