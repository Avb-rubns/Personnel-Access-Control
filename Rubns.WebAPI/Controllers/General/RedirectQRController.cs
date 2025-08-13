namespace Rubns.WebAPI.Controllers.General
{

    [Route("qr/{slug}")]
    [ApiController]
    public class RedirectQRController : ControllerBase
    {
        private readonly ISearchSlugPort _searchSlugPort;

        public RedirectQRController(ISearchSlugPort searchSlugPort)
        {
            _searchSlugPort = searchSlugPort;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string slug)
        {
            var result = await _searchSlugPort.SearchSlugAsync(slug);

            if (string.IsNullOrEmpty(result))
            {
                return NotFound();
            }
            return Redirect(result);
        }
    }

}
