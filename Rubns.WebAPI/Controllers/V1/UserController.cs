namespace Rubns.WebAPI.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class UserController(IGetUsersPort getUsersPort, IPatchUserPort patchUserPort) : ControllerBase
    {

        private readonly IGetUsersPort _getUsersPort = getUsersPort;
        private readonly IPatchUserPort _patchUserPort = patchUserPort;

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
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateUserAsync(int id, [FromBody] JsonPatchDocument<UserRegistedDTO> patchDoc)
        {
            if (!ModelState.IsValid) return BadRequest();

            var result = await _patchUserPort.PatchUserAsync(id, patchDoc);
            switch (result)
            {
                case > 1: return Ok();
                case 0: return NoContent();
                default: return BadRequest();
            }

        }
    }
}
