using Microsoft.Extensions.DependencyInjection;
using ServicesRegister;
using System.Reflection;
namespace Rubns.WebAPI
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddPresenters(this IServiceCollection services)
        {

            return services.ConfigureServices(Assembly.GetExecutingAssembly().Location);

        }
    }
}
