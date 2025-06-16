using Personnel.Client.Server;

WebApplication.CreateBuilder(args)
    .ConfigureServices()
    .ConfigureMiddlewares()
    .Run();