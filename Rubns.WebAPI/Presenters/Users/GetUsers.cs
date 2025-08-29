namespace Rubns.WebAPI.Presenters.Users
{
    public class GetUsers : IGetUsersOutPort, IPresenter<TableUserDTO>
    {
        public TableUserDTO Content { get; set; }

        public Task Handler(List<User> users, int total)
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


            Content = new TableUserDTO()
            {
                RegisteredUsers = userRegistedDTOs,
                Total = total
            };
            return Task.CompletedTask;
        }
    }
}
