using Rubns.Application.Interface.Users.Queries;

namespace Rubns.WebAPI.Presenters.Users
{
    public class GetUsers : IGetUsersOutputPort, IPresenter<TableUserDTO>
    {
        public TableUserDTO Result { get; set; }

        public Task Success(List<User> users, int total)
        {
            List<UserRegistedDTO> userRegistedDTOs = new List<UserRegistedDTO>();


            userRegistedDTOs = users.Select(u => new UserRegistedDTO
            {
                UserID = u.UserID,
                UserName = u.Name,
                LastName = u.LastName,
                Phone = u.Phone,
                Email = u.Email,
                RolID = u.RolID,
                Status = u.Status,
            }).ToList();


            Result = new TableUserDTO()
            {
                RegisteredUsers = userRegistedDTOs,
                Total = total
            };
            return Task.CompletedTask;
        }
    }
}
