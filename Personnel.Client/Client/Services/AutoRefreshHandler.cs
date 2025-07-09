namespace Personnel.Client.Client.Services
{
    public class AutoRefreshHandler(AuthService authService) : DelegatingHandler
    {
        private readonly AuthService AuthService = authService;

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage req, CancellationToken ct)
        {
            var response = await base.SendAsync(req, ct);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                // Llama al refresh endpoint (asegúrate que usa un HttpClient sin handlers)
                var refreshOk = await AuthService.TryRefreshTokenAsync();

                if (refreshOk)
                {
                    // Clona la petición original
                    var newRequest = await CloneHttpRequestMessageAsync(req);

                    // Reintenta la solicitud con nuevo token (que vendrá como cookie)
                    return await base.SendAsync(newRequest, ct);
                }
            }

            return response;
        }

        private async Task<HttpRequestMessage> CloneHttpRequestMessageAsync(HttpRequestMessage request)
        {
            var clone = new HttpRequestMessage(request.Method, request.RequestUri)
            {
                Content = request.Content != null
                    ? new StringContent(await request.Content.ReadAsStringAsync())
                    : null,
                Version = request.Version
            };

            foreach (var header in request.Headers)
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);

            foreach (var prop in request.Options)
                clone.Options.TryAdd(prop.Key, prop.Value);

            return clone;
        }



    }
}
