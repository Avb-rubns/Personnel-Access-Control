WebApplication.CreateBuilder(args)
    .ConfigureServices()
    .ConfigureMiddlewares()
    .Run();