namespace Rubns.Application.Click.Post
{
    internal class ClickUseCase(IClickRepositoryEFC clickRepository
        , ILinkRepositoryEFC linkRepositoryEFC
        , ILogger logger
        , ClickBuilder clickBuilder) : ICreateClick
    {

        private readonly IClickRepositoryEFC _clickRepositoryEFC = clickRepository;
        private readonly ILogger _logger = logger;
        private readonly ClickBuilder _clickBuilder = clickBuilder;
        private readonly ILinkRepositoryEFC _linkRepositoryEFC = linkRepositoryEFC;

        public async Task<string> CreateClick(HttpRequest request, string slug)
        {
            string Url = string.Empty;

            try
            {
                var link = await _linkRepositoryEFC.GetLinkForSlugAsync(slug);

                if (link is { ID: <= 0 })
                {
                    return string.Empty;
                }


                //Url = link.Content;
                //var headers = request.Headers.ToDictionary(a => a.Key, a => a.Value.ToArray().FirstOrDefault());
                //var click = _clickBuilder
                //     .WithUserAgent(request.Headers["User-Agent"], headers)
                //     .WithLinkId(link.ID)
                //     .WithIPAddress(request.HttpContext.Connection.RemoteIpAddress?.ToString())
                //     .Build();

                //var data = await _clickRepositoryEFC.InsertAsync(click);

                //if (data <= 0)
                //{
                //    _logger.Warning("No se estan registrando los click.");
                //}


            }
            catch (Exception e)
            {
                _logger.Error(e, "Error CreateClick", e.Message);
            }


            return Url;
        }
    }
}
