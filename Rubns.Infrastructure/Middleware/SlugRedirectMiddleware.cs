using Microsoft.Extensions.Caching.Memory;
using Rubns.Core.Ports.Link;

namespace Rubns.Infrastructure.Middleware
{
    public class SlugRedirectMiddleware(RequestDelegate requestDelegate
        , ILinkRepositoryEFC repositoryEFC
        , IMemoryCache memoryCache)
    {
        private readonly RequestDelegate _next = requestDelegate;
        private readonly ILinkRepositoryEFC qRRepositoryEFC = repositoryEFC;
        private readonly IMemoryCache _cache = memoryCache;

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value?.Trim('/');
            if (path?.StartsWith("r/") == true)
            {
                var slug = path.Substring(2);
                if (!_cache.TryGetValue(slug, out var urlRecord))
                {
                    urlRecord = await qRRepositoryEFC.FindSlugAsync(slug);
                    if (urlRecord != null)
                    {
                        _cache.Set(slug, urlRecord, TimeSpan.FromMinutes(10));
                        context.Request.Path = $"/r/{slug}";
                        await _next(context);
                        return;
                    }
                }
            }
        }

    }
}
