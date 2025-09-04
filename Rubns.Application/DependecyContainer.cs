
using Microsoft.Extensions.DependencyInjection.Extensions;
using Rubns.Application.Builder;

namespace Rubns.Application
{
    public static class DependecyContainer
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.TryAddScoped<ClickBuilder>();
            return services.ConfigureServices(Assembly.GetExecutingAssembly().Location);
        }
    }
}
