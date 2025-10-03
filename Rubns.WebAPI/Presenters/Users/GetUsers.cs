namespace Rubns.WebAPI.Presenters.Users
{
    public class GetUsers : IGetUsersOutputPort, IPresenter<TableUserDTO>
    {
        public TableUserDTO Result { get; set; }

        public Task Success(List<User> users, int total, int currentPage, int pageSize, bool hasNextPage, bool hasPreviousPage)
        {
            List<UserRegistedDTO> userRegistedDTOs = new List<UserRegistedDTO>();


            userRegistedDTOs = users.Select(u => new UserRegistedDTO
            {
                UserID = u.FriendlyUserID,
                UserName = u.Name,
                LastName = u.LastName,
                Phone = u.Phone,
                Email = u.Email,
                RolID = u.RolID,
                Status = u.Status,
            }).ToList();


            PaginationDTO paginationDTO = new PaginationDTO()
            {
                Total = total,
                CurrentPage = currentPage,
                PageSize = pageSize,
                HasNextPage = hasNextPage,
                HasPreviousPage = hasPreviousPage
            };

            Result = new TableUserDTO()
            {
                RegisteredUsers = userRegistedDTOs,
                Pagination = paginationDTO
            };
            return Task.CompletedTask;
        }
    }
}
