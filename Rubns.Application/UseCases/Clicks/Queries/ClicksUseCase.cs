namespace Rubns.Application.UseCases.Clicks.Queries
{
    internal class ClicksUseCase(ILogger logger,
        IClickRepositoryEFC clickRepositoryEFC,
        ISqidService sqidService,
        ILinkRepositoryEFC linkRepositoryEFC,
        IGetClicksOutputPort getClicksOutputPort)
        : IGetClicksUseCase
    {

        private readonly IClickRepositoryEFC _clickRepositoryEFC = clickRepositoryEFC;
        private readonly ISqidService _sqidService = sqidService;
        private readonly ILogger _logger = logger;
        private readonly IGetClicksOutputPort _getClicksOutputPort = getClicksOutputPort;
        private readonly ILinkRepositoryEFC _linkRepositoryEFC = linkRepositoryEFC;

        public async Task ExecuteAsync(string id, DateTime? starDate, DateTime? endDate)
        {
            try
            {
                DateTime StarDate = DateTime.Today;
                DateTime EndDate = DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59).AddMilliseconds(999);

                if (starDate.HasValue && endDate.HasValue)
                {
                    StarDate = starDate.Value;
                    EndDate = endDate.Value;
                }

                if (StarDate > EndDate)
                {
                    throw new ArgumentException("El rango de fechas no es correcto.");
                }


                ClicksDashboard clicks = new();

                if (string.IsNullOrEmpty(id))
                {
                    throw new ArgumentException("Datos invalidos");
                }

                var IdClic = _sqidService.Decode(id);

                var clics = await _clickRepositoryEFC.GetClicksAsync(IdClic, StarDate, EndDate);

                if (clics.FirstOrDefault() is { LinkId: < 0 })
                {
                    throw new NotFoundException("No se encontro el enlace.", "Sin informacion");
                }


                if (clics.Count() > 0)
                {
                    var LinkInfo = await _linkRepositoryEFC.GetLinkByIdAsync(clics[0].LinkId);

                    clicks.Slug = LinkInfo.Slug;
                    clicks.URL = LinkInfo.Url;

                    clicks.TotalClicks = clics.Count();
                    clicks.FriendlyId = _sqidService.Encode(IdClic);
                    clicks.FriendlyLinkId = _sqidService.Encode(clics[0].LinkId);

                    clicks.ByDate = clics.GroupBy(clic => clic.ClickedAt.Date)
                        .ToDictionary(g => g.Key.Date.ToString("d"), g => g.Count());

                    clicks.ByDevice = clics.GroupBy(clic => string.IsNullOrEmpty(clic.DeviceType) ?
                    "Unknown" : clic.DeviceType)
                        .ToDictionary(g => g.Key, g => g.Count());

                    clicks.ByRegion = clics.GroupBy(c => string.IsNullOrEmpty(c.Region) ?
                    "Unknown" : c.Region)
                        .ToDictionary(g => g.Key, g => g.Count());

                    clicks.ByCountry = clics.GroupBy(c => string.IsNullOrEmpty(c.Country) ?
                    "Unknown" : c.Country)
                        .ToDictionary(g => g.Key, g => g.Count());

                    clicks.ByBrowser = clics.GroupBy(c => string.IsNullOrEmpty(c.Browser) ?
                    "Unknown" : c.Browser)
                        .ToDictionary(g => g.Key, g => g.Count());

                    clicks.ByOS = clics.GroupBy(c => string.IsNullOrEmpty(c.OS) ?
                    "Unknown" : c.OS)
                        .ToDictionary(g => g.Key, g => g.Count());

                    clicks.ByCity = clics.GroupBy(c => string.IsNullOrEmpty(c.City) ?
                    "Unknown" : c.City)
                        .ToDictionary(g => g.Key, g => g.Count());

                }

                await _getClicksOutputPort.Success(clicks);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error ClicksUseCase:{error}", ex.Message);
                throw;
            }

        }
    }
}
