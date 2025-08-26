namespace Personnel.Server.Tests.Unit;

[TestClass]
public class EncryptionServiceTests
{
    private ServiceProvider _provider = null!;

    [TestInitialize]
    public void Init()
    {
        var inMemorySettings = new Dictionary<string, string>
        {
            ["WordSecret"] = "serverSecret",
            ["WordSecretPass"] = "passSecret",
            ["WordSecretForgotPass"] = "forgotSecret",
            ["ConnectionStrings:DefaultConnection"] = "Server=(localdb)\\mssqllocaldb;Database=DevDB;Trusted_Connection=True;"
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        var services = new ServiceCollection();
        services.AddInfrastructureAsync(configuration);

        var descriptor = services.First(d =>
        d.ServiceType == typeof(DbContextOptions<AuthDbContextEFC>));
        services.Remove(descriptor);

        services.AddDbContext<AuthDbContextEFC>(opts =>
        opts.UseInMemoryDatabase("TestDb"));

        // 3) Construye el proveedor
        _provider = services.BuildServiceProvider();


    }


    [TestMethod]
    public void EncryptionService_ResolveViaIoC_CanGenerateAndValidate()
    {
        var svc = _provider.GetRequiredService<IEncryptionService>();

        // Arrange
        var dto = new RegisterDTO { NameApp = "MyApp", WordSecretUser = "userSalt" };
        var apiKey = svc.GenerateApiKey(dto);

        // Act & Assert
        Assert.IsFalse(string.IsNullOrEmpty(apiKey));
        // Y prueba ValidatePass con GenerateNewPass...
        var pass = "Test123!";
        var hash = svc.GenerateNewPass(pass);
        Assert.IsTrue(svc.ValidatePass(pass, hash));
    }
}
