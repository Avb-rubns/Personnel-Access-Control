using Rubns.WebAPI.Middleware;

namespace Personnel.Client.Server
{
    internal static class MiddlewaresConfigurations
    {
        public static WebApplication ConfigureMiddlewares(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {

                app.UseDeveloperExceptionPage();
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();

            }
            app.UseMiddleware<GlobalExceptionMiddlewware>();
            app.UseRouting();
            app.UseCors("AllowFrontend");
            app.MapWhen(
               context =>
               {
                   var path = context.Request.Path.Value?.ToLower() ?? "";
                   return path.StartsWith("/api");
               },
               apiApp =>
               {
                   apiApp.UseRouting();
                   apiApp.UseCors("AllowFrontend");
                   apiApp.UseMiddleware<GlobalExceptionMiddlewware>();
                   apiApp.UseMiddleware<JwtValidationMiddleware>();
                   apiApp.UseAuthorization();
                   apiApp.UseEndpoints(endpoints =>
                   {
                       endpoints.MapControllers();
                   });
               }
           );
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // Esto permite que rutas como /qr/{slug} lleguen a su controlador
                endpoints.MapRazorPages();
            });

            app.MapOpenApi();
            app.MapScalarApiReference();

            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.MapRazorPages();
            app.UseResponseCompression();
            app.MapFallbackToFile("index.html");
            return app;
        }

    }
}
