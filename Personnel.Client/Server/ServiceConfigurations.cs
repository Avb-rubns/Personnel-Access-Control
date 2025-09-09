namespace Personnel.Client.Server
{
    internal static class ServiceConfigurations
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            if (builder.Configuration.GetSection(EnvironmentOptions.SectionKey).Get<EnvironmentOptions>() is { } environmentOptions)
            {
                builder.Configuration.AddJsonFile($"appsettings.{environmentOptions.EnvironmentName}.json", optional: true, reloadOnChange: true);
            }
            builder.Services.AddServices(builder.Configuration);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>())
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials(); // 👈 esto habilita cookies / credenciales

                });
            });
            builder.Services.AddControllers()
                    .AddNewtonsoftJson();
            builder.Services.AddOpenApi();
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();
            builder.Services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<BrotliCompressionProvider>();
                options.Providers.Add<GzipCompressionProvider>();
                options.MimeTypes = ResponseCompressionDefaults.MimeTypes;
            });


            builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });

            builder.Services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Optimal;
            });

            builder.Services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            });

            builder.Services.AddHttpClient("maileroo", c =>
            {
                c.BaseAddress = new Uri("https://smtp.maileroo.com");
                c.DefaultRequestHeaders.Add("X-API-Key", builder.Configuration.GetSection("Maileroo")["ApiKey"]);
            });
            builder.Services.AddHttpClient("clean");
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminActive", policy =>
                {
                    policy.RequireRole("Administrator");
                    policy.RequireClaim("Status", "Active");
                });
                options.AddPolicy("RootActive", policy =>
                {
                    policy.RequireRole("root");
                    policy.RequireClaim("Status", "Active");
                });
            });




            return builder.Build();
        }
    }
}
