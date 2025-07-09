namespace Rubns.Application.Auth.Get
{
    internal sealed class UserInformationUseCase(IJWTService jWTService) : IUserInformationPort
    {
        private IJWTService JWTService { get; } = jWTService;

        public UserInfoDTO UserInfo(string JWT)
        {
            var user = JWTService.JWTtoUserInfo(JWT);

            return user;
        }
    }
}
