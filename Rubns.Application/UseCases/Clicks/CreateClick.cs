using Rubns.Application.Builder;
using Rubns.Core.Abstraccions.Clicks;

namespace Rubns.Application.UseCases.Clicks
{
    internal class CreateClick(IClickRepositoryEFC clickRepositoryEFC,
        ClickBuilder clickBuilder,
        ILogger logger) : ICreateClickPort
    {

        private IClickRepositoryEFC _clickRepositoryEFC = clickRepositoryEFC;
        private readonly ILogger _logger = logger;
        private readonly ClickBuilder _clickBuilder = clickBuilder;


        public async Task CreateAsync(HttpRequest request, int idLink)
        {
            try
            {
                var headers = request.Headers.ToDictionary(a => a.Key, a => a.Value.ToArray().FirstOrDefault());
                var click = _clickBuilder
                     .WithUserAgent(request.Headers["User-Agent"], headers)
                     .WithLinkId(idLink)
                     .WithIPAddress(request.HttpContext.Connection.RemoteIpAddress?.ToString())
                     .Build();

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
