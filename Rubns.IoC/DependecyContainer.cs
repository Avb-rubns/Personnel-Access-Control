using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Rubns.Application;
using Rubns.Infrastructure;
using Rubns.Infrastructure.Services;

namespace Rubns.IoC
{
    public static class DependecyContainer
    {
        public static IServiceCollection AddServices(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddInfrastructureAsync(configuration);
            service.AddApplication();
            service.AddLoggerMSSQL(configuration.GetConnectionString("dbAuth"));

            return service;
        }
    }
}
