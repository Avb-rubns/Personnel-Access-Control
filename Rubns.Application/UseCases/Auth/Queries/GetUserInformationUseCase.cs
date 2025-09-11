namespace Rubns.Application.UseCases.Auth.Queries
{
    internal sealed class GetUserInformationUseCase(IJWTService jWTService,
        IGetUserInformationOutputPort userInformationOutPort,
        ILogger logger)
        : IGetUserInformationUseCase
    {
        private readonly IJWTService _jWTService = jWTService;
        private readonly IGetUserInformationOutputPort _userInformationOutPort = userInformationOutPort;
        private readonly ILogger _logger = logger;

        public async Task ExecuteAsync(string JWT)
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
                await _userInformationOutPort.ExecuteAsync(user);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error in UserInformationUseCase:{error}", ex.Message);
                throw;
            }


        }
    }
}
