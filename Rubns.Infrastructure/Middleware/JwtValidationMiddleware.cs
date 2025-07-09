using Microsoft.AspNetCore.Http;

namespace Rubns.Infrastructure.Middleware
{
    public class JwtValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;

        public JwtValidationMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _config = config;
        }

        public async Task Invoke(HttpContext context)
        {
            var path = context.Request.Path.Value?.ToLower();

            if (path == "/api/v1/login" || path == "/api/v1/auth/refresh" || path == "/api/v1/ticket/check-in")
            {
                await _next(context);
                return;
            }

            // Verifica el token en el header
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (string.IsNullOrEmpty(token) && context.Request.Cookies.TryGetValue("accessToken", out var cookieToken))
            {
                token = cookieToken;
            }


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
