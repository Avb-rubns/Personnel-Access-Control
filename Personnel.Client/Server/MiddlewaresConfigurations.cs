using Rubns.Infrastructure.Services;
using Scalar.AspNetCore;

namespace Personnel.Client.Server
{
    internal static class MiddlewaresConfigurations
    {
        public static WebApplication ConfigureMiddlewares(this WebApplication app)
        {
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.MapRazorPages();

            app.MapWhen(
               context =>
               {
                   var path = context.Request.Path.Value?.ToLower() ?? "";
                   return path.StartsWith("/api");
               },
               apiApp =>
               {
                   apiApp.UseMiddleware<JwtValidationMiddleware>();
                   apiApp.UseRouting();
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



            app.MapOpenApi();
            app.MapScalarApiReference();

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

            app.MapFallbackToFile("index.html");
            return app;
        }

    }
}
