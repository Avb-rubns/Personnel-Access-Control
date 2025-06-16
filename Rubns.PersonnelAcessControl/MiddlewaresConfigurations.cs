using Rubns.Infrastructure.Services;
using Scalar.AspNetCore;

namespace Rubns.Auth.API
{
    internal static class MiddlewaresConfigurations
    {
        public static WebApplication ConfigureMiddlewares(this WebApplication app)
        {

            app.UseRouting();
            app.UseCors(option =>
            {
                option.AllowAnyOrigin();
                option.AllowAnyMethod();
                option.AllowAnyHeader();
            });

            app.UseMiddleware<JwtValidationMiddleware>();

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.MapControllers();
            app.MapOpenApi();
            app.MapScalarApiReference();
            app.MapRazorPages();
            app.MapFallbackToFile("index.html");

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

            return app;
        }

    }
}
