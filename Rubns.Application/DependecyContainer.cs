
namespace Rubns.Application
{
    public static class DependecyContainer
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            return services.ConfigureServices(Assembly.GetExecutingAssembly().Location);
        }
    }
}
