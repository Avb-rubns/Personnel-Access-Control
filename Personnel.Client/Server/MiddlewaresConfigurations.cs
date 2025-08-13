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

            app.MapWhen(
               context =>
               {
                   var path = context.Request.Path.Value?.ToLower() ?? "";
                   return path.StartsWith("/api");
               },
               apiApp =>
               {
                   apiApp.UseRouting();
                   apiApp.UseMiddleware<JwtValidationMiddleware>();
                   apiApp.UseAuthorization();
                   apiApp.UseEndpoints(endpoints =>
                   {
                       endpoints.MapControllers();
                   });
               }
           );


            app.UseRouting();
            app.UseCors(option =>
            {
                option.AllowAnyOrigin();
                option.AllowAnyMethod();
                option.AllowAnyHeader();
            });


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // Esto permite que rutas como /r/{slug} lleguen a su controlador
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
