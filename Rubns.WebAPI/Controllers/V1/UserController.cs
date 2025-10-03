namespace Rubns.WebAPI.Controllers.V1
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class UserController : ControllerBase
    {

        private readonly IGetUsersUseCase _getUsersUseCase;
        private readonly IGetUsersOutputPort _getUsersOutputPort;
        private readonly IUpdateUserUseCase _updateUserUseCase;


        public UserController(IGetUsersUseCase getUsersInputPort,
            IUpdateUserUseCase patchUserPort,
            IGetUsersOutputPort getUsersOutPort)
        {

            _getUsersUseCase = getUsersInputPort;
            _updateUserUseCase = patchUserPort;
            _getUsersOutputPort = getUsersOutPort;
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUsersAsync(string search = "", int? page = 1, int? pageSize = 10)
        {
            TableUserDTO tableUser = new();

            await _getUsersUseCase.ExecuteAsync(search, page, pageSize);
            tableUser = _getUsersOutputPort.Result;

            return tableUser.Pagination.Total > 0 ? Ok(tableUser) : NoContent();

        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateUserAsync(string id, [FromBody] JsonPatchDocument<UserRegistedDTO> patchDoc)
        {
            await _updateUserUseCase.Executeasync(id, patchDoc);
            return Ok();

        }
    }
}
