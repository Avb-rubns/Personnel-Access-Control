using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace ServicesRegister
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection ConfigureServices(this IServiceCollection serviceCollection
            , string assemblyLocation
            , bool IsClass = false)
        {
            var service = Assembly.LoadFrom(assemblyLocation)
                     .GetTypes()
                     .Where(type => type.IsAbstract == false
                            && type.IsInterface == false
                            && type.IsEnum == false
                            && Regex.IsMatch(type.Name, "Nullable|Attribute|[<>]") == false
                            && type.BaseType?.Name != "DbContext");
            service
                    .ToList().ForEach(type =>
                        type.GetInterfaces().ToList()
                        .ForEach(interfaceType => serviceCollection.TryAddScoped(interfaceType, type)));
            if (IsClass)
            {
                service.ToList().ForEach(type =>
                            serviceCollection.TryAddScoped(type));
            }

            return serviceCollection;
        }
    }
}
