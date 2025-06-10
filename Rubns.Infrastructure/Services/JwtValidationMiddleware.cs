using Microsoft.AspNetCore.Http;

namespace Rubns.Infrastructure.Services
{
    public class JwtValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;

        // Lista de rutas públicas (sin autenticación)
        private readonly List<string> _publicRoutes = new()
        {
            "/api/v1/login",
            "/scalar",
            "/openapi",
            "/api/v1/auth/refresh"
        };

        public JwtValidationMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _config = config;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower();

            // Si la ruta es pública, pasa directo al siguiente middleware
            if (_publicRoutes.Any(r => path.StartsWith(r)))
            {
                await _next(context);
                return;
            }

            // Verifica el token en el header
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Falta el token.");
                return;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["WordSecretJWT"]);

            try
            {
                var validations = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                tokenHandler.ValidateToken(token, validations, out _);
            }
            catch
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Token inválido.");
                return;
            }

            await _next(context);
        }
    }

}
