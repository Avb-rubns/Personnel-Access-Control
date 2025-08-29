namespace Rubns.WebAPI.Presenters.Auth
{
    internal sealed class UserInformationOutPort : IUserInformationOutPort
    {
        public UserInfoDTO Content { get; set; }

        public Task Handler(UserClaim userClaim)
        {
            UserInfoDTO userInfo = new UserInfoDTO()
            {
                FirstName = userClaim.FirstName,
                Email = userClaim.Email,
                Phone = userClaim.Phone,
                ID = userClaim.ID,
                Role = userClaim.Role,
                RolID = userClaim.RolID,
                Status = userClaim.Status,
                Expiration = userClaim.Expiration,
            };
            Content = userInfo;
            return Task.CompletedTask;
        }
    }
}
