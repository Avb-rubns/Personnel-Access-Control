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

            app.MapControllers();
            app.MapOpenApi();
            app.MapScalarApiReference();

            return app;
        }

    }
}
