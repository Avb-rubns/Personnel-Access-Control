namespace Rubns.Infrastructure
{
    public static class DependecyContainer
    {
        private const string V = "Enviroment";
        public static IServiceCollection AddInfrastructureAsync(this IServiceCollection services, IConfiguration configuration)
        {

            int level = 120;

            using var connection = new SqlConnection(configuration.GetConnectionString("dbAuth"));

            connection.Open();

            var query = $"SELECT compatibility_level FROM sys.databases where name = '{connection.Database}'";

            var levelDB = connection.QueryFirstOrDefault<int>(query, commandType: CommandType.Text);
            connection.Close();

            level = levelDB.CompareTo(level) < 0 ? level : levelDB;

            services.AddDbContext<AuthDbContextEFC>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("dbAuth"), options =>
                {
                    options.EnableRetryOnFailure();
                    options.UseCompatibilityLevel(level);
                    options.CommandTimeout(240);
                });
                if (configuration[V].ToString() == "dev")
                {
                    options.EnableSensitiveDataLogging();
                    options.EnableDetailedErrors();
                }
                options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            });



            return services.ConfigureServices(Assembly.GetExecutingAssembly().Location);

        }
    }
}
