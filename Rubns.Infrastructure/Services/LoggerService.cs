
namespace Rubns.Infrastructure.Services
{
    public static class LoggerService
    {
        public static IServiceCollection AddLoggerMSSQL(this IServiceCollection services, string? connectionString)
        {

            services.TryAddSingleton<ILogger>(
                new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.MSSqlServer(connectionString,
                    new MSSqlServerSinkOptions
                    {
                        SchemaName = "dbo",
                        TableName = "Logger",
                        AutoCreateSqlTable = true
                    }
                    , restrictedToMinimumLevel: LevelAlias.Minimum)
                .WriteTo.File("{Directory.GetCurrentDirectory()}\\ApplicationLog.txt"
                        , rollingInterval: RollingInterval.Day
                        , retainedFileCountLimit: 7)
                .CreateLogger()
            );


            return services;
        }
    }
}
