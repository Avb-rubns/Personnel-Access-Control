namespace Rubns.WebAPI.Controllers.General
{

    [Route("qr/{slug}")]
    [ApiController]
    public class RedirectQRController : ControllerBase
    {
        private readonly ICreateClick _createClick;

        public RedirectQRController(ICreateClick createClick)
        {
            _createClick = createClick;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string slug)
        {
            var result = await _createClick.CreateClick(HttpContext.Request, slug);

            if (string.IsNullOrEmpty(result))
            {
                return NotFound();
            }
            return Redirect(result);
        }
    }

}
