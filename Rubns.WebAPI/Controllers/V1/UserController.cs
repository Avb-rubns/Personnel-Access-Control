namespace Rubns.WebAPI.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class UserController(IGetUsersPort getUsersPort) : ControllerBase
    {

        private readonly IGetUsersPort _getUsersPort = getUsersPort;


        [HttpGet("users")]
        public async Task<IActionResult> GetUsersforPageAsync(int? page, int? pageSize)
        {
            var data = await _getUsersPort.GetAllUsersforPageAsync(page, pageSize);

            switch (data.Count)
            {
                case > 0:
                    return Ok(data);
                case 0:
                    return NoContent();
            }

            return BadRequest();

        }
    }
}
