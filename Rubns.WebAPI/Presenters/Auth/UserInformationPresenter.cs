using Rubns.Application.Interface.Auth.Queries;

namespace Rubns.WebAPI.Presenters.Auth
{
    internal sealed class UserInformationPresenter : IGetUserInformationOutputPort
    {
        public UserInfoDTO Result { get; set; }

        public Task ExecuteAsync(UserClaim userClaim)
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
            Result = userInfo;
            return Task.CompletedTask;
        }
    }
}
