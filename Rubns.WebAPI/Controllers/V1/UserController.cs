namespace Rubns.WebAPI.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class UserController : ControllerBase
    {

        private readonly IGetUsersInputPort _getUsersInputPort;
        private readonly IGetUsersOutPort _getUsersOutPort;
        private readonly IPatchUserPort _patchUserPort;


        public UserController(IGetUsersInputPort getUsersInputPort,
            IPatchUserPort patchUserPort,
            IGetUsersOutPort getUsersOutPort)
        {

            _getUsersInputPort = getUsersInputPort;
            _patchUserPort = patchUserPort;
            _getUsersOutPort = getUsersOutPort;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsersforPageAsync(string search = "", int? page = 0, int? pageSize = 10)
        {
            TableUserDTO tableUser = new();

            await _getUsersInputPort.GetAllUsersforPageAsync(search, page, pageSize);

            tableUser = ((IPresenter<TableUserDTO>)_getUsersOutPort).Content;

            switch (tableUser.Total)
            {
                case > 0:
                    return Ok(tableUser);
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
                case >= 1: return Ok();
                case 0: return NoContent();
                default: return BadRequest();
            }

        }
    }
}
