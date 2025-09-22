namespace Rubns.Application.UseCases.Clicks.Commands
{
    internal class CreateClickUseCase(IClickRepositoryEFC clickRepositoryEFC,
        ClickBuilder clickBuilder,
        IProxyServer proxyServer,
        IConfiguration configuration,
        ILogger logger) : ICreateClickUseCase
    {

        private IClickRepositoryEFC _clickRepositoryEFC = clickRepositoryEFC;
        private readonly ILogger _logger = logger;
        private readonly ClickBuilder _clickBuilder = clickBuilder;
        private readonly IProxyServer _proxyServer = proxyServer;
        private readonly IConfiguration _configuration = configuration;


        public async Task ExecuteAsync(HttpRequest request, int idLink)
        {
            try
            {
                ResponseData<IPAPI> ipInfo;

                var headers = request.Headers.ToDictionary(a => a.Key, a => a.Value.ToArray().FirstOrDefault());

                var click = _clickBuilder
                     .WithUserAgent(request.Headers["User-Agent"], headers)
                     .WithLinkId(idLink)
                     .WithIPAddress(request.HttpContext.Connection.RemoteIpAddress?.ToString())
                     .Build();


                if (_configuration["Enviroment"].ToString() == "dev")
                {
                    Random random = new Random();

                    var _ipTest = _configuration.GetSection("IpsTest").GetChildren()
                        .Select(s => s.Value)
                        .ToList() ?? new();

                    string ipTest = _ipTest[random.Next(0, 23)];

                    ipInfo = await _proxyServer.GetAsync<ResponseData<IPAPI>>("ip-api", ipTest);
                    click.IpAddress = ipTest;
                }
                else
                {
                    ipInfo = await _proxyServer.GetAsync<ResponseData<IPAPI>>("ip-api", click.IpAddress);
                }


                if (ipInfo.Data is not null)
                {
                    click.Region = ipInfo.Data.RegionName;
                    click.Country = ipInfo.Data.Country;
                    click.City = ipInfo.Data.City;
                }


                var data = await _clickRepositoryEFC.InsertAsync(click);

                if (data <= 0)
                {
                    _logger.Warning("No se estan registrando los click.");
                }

            }
            catch (Exception e)
            {
                _logger.Error(e, "Error CreateClick:{error}", e.Message);
            }
        }
    }
}
