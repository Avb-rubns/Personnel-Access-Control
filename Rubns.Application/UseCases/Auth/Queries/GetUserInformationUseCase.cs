namespace Rubns.Application.UseCases.Auth.Queries
{
    internal sealed class GetUserInformationUseCase(IJWTService jWTService,
        IGetUserInformationOutputPort userInformationOutPort,
        ISqidService sqidService,
        ILogger logger)
        : IGetUserInformationUseCase
    {
        private readonly IJWTService _jWTService = jWTService;
        private readonly IGetUserInformationOutputPort _userInformationOutPort = userInformationOutPort;
        private readonly ILogger _logger = logger;
        private readonly ISqidService _sqidService = sqidService;

        public async Task ExecuteAsync(string JWT)
        {
            try
            {
                var user = _jWTService.JWTtoUserInfo(JWT);
                user.ID = _sqidService.Decode(user.FrindlyId);
                user.RolID = _sqidService.Decode(user.FrindlyRolId);

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
