namespace Rubns.Application.UseCases.Auth.Get
{
    internal sealed class UserInformationUseCase(IJWTService jWTService,
        IUserInformationOutPort userInformationOutPort,
        ILogger logger)
        : IUserInformationInPort
    {
        private readonly IJWTService _jWTService = jWTService;
        private readonly IUserInformationOutPort _userInformationOutPort = userInformationOutPort;
        private readonly ILogger _logger = logger;

        public async Task UserInfo(string JWT)
        {
            try
            {
                var user = _jWTService.JWTtoUserInfo(JWT);
                if (user is { ID: <= 0 })
                {
                    throw new ArgumentNullException("Sin informacion el JWT");
                }
                if (user is { Status: false })
                {
                    throw new ArgumentException("Usuario desactivado");
                }
                await _userInformationOutPort.Handler(user);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in UserInformationUseCase:{error}", ex.Message);
                throw;
            }


        }
    }
}
