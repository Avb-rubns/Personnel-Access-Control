namespace Rubns.Auth.API
{
    internal static class ServiceConfigurations
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();


            builder.Services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            });


            if (builder.Configuration.GetSection(EnvironmentOptions.SectionKey).Get<EnvironmentOptions>() is { } environmentOptions)
            {
                builder.Configuration.AddJsonFile($"appsettings.{environmentOptions.EnvironmentName}.json", optional: true, reloadOnChange: true);
            }

            builder.Services.AddServices(builder.Configuration);

            return builder.Build();
        }
    }
}
