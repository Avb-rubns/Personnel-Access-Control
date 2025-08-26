var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMudServices();
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.TryAddScoped<AuthenticationStateProvider, AuthService>();
builder.Services.TryAddScoped<AuthService>();
builder.Services.TryAddScoped<Utils>();
builder.Services.TryAddScoped<PublicRoutesService>();
builder.Services.TryAddScoped<IProxy, Proxy>();

await builder.Build().RunAsync();
