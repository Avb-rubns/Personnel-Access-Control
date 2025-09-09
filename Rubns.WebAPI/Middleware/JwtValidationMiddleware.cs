namespace Rubns.WebAPI.Middleware
{
    public class JwtValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;
        private readonly ILogger<JwtValidationMiddleware> _logger;
        private readonly List<string> _publicRoutes;
        private readonly string _jwtSecret;
        private readonly string _cookieName;

        public JwtValidationMiddleware(RequestDelegate next
            , IConfiguration config
            , ILogger<JwtValidationMiddleware> logger)
        {
            _next = next;
            _config = config;
            _logger = logger;

            _publicRoutes = _config.GetSection("PublicRoutes").Get<List<string>>() ?? new();

            _jwtSecret = config["Jwt:Secret"] ?? config["WordSecretJWT"] ??
                throw new InvalidOperationException("Jwt secret no configurado.");

            _cookieName = config["Jwt:CookieName"] ?? "accessToken";
        }

        public async Task Invoke(HttpContext context, IUserContextService userContext)
        {

            var path = context.Request.Path.Value?.ToLower();

            if (_publicRoutes.Any(r => path.StartsWith(r, StringComparison.OrdinalIgnoreCase)))
            {
                await _next(context);
                return;
            }


            string? token = null;
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (!string.IsNullOrWhiteSpace(authHeader)
                && authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            {
                token = authHeader.Substring("Bearer ".Length).Trim();
            }
            else if (context.Request.Cookies.TryGetValue(_cookieName, out var cookieToken)
                && !string.IsNullOrEmpty(cookieToken))
            {
                token = cookieToken;
            }

            if (string.IsNullOrWhiteSpace(token))
            {
                _logger.LogWarning("No token provided for path {Path}", path);
                throw new UnauthorizedException("Falta el token de autorización.", "No autorizado");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSecret);

            try
            {
                var validations = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RoleClaimType = ClaimTypes.Role,
                    NameClaimType = ClaimTypes.Name,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };

                tokenHandler.ValidateToken(token, validations, out _);

                var principal = tokenHandler.ValidateToken(token, validations, out var validatedToken);




                if (validatedToken is JwtSecurityToken jwt
                    && !jwt.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogWarning("Token algorithm no permitido: {Alg}", jwt.Header.Alg);
                    throw new UnauthorizedException("Falta el token de autorización.", "No autorizado");
                }

                context.User = principal;

                if (principal.Identity?.IsAuthenticated == true)
                {
                    var roles = principal.Claims
                                .Where(c => c.Type == ClaimTypes.Role || c.Type.Equals("role", StringComparison.OrdinalIgnoreCase))
                                .Select(c => c.Value)
                                .ToList();
                    var statusClaim = principal.Claims.FirstOrDefault(c => c.Type.Equals("Status", StringComparison.OrdinalIgnoreCase)
                                                           || c.Type.Equals("status", StringComparison.OrdinalIgnoreCase)
                                                           || c.Type.Equals("isActive", StringComparison.OrdinalIgnoreCase));

                    var iD = principal.Claims
                                        .FirstOrDefault(c => c.Type.Equals("userId", StringComparison.OrdinalIgnoreCase));

                    var name = principal.Claims
                                        .FirstOrDefault(c => c.Type.Equals("firstName", StringComparison.OrdinalIgnoreCase));

                    userContext.UserId = iD.Value;
                    userContext.Roles = roles;
                    userContext.Status = statusClaim.Value;
                    userContext.Name = name.Value;
                }

                await _next(context);
            }
            catch (SecurityTokenExpiredException)
            {
                _logger.LogInformation("Token expirado para path {Path}", path);
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Title = "Token vencido",
                    Detail = "El token a vencido, genere un nuevos tokens.",
                    Status = StatusCodes.Status401Unauthorized,
                    Type = "https://httpstatuses.com/401"
                });
                return;
            }
            catch (SecurityTokenValidationException)
            {
                _logger.LogInformation("Token no valido  para path {Path}", path);
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Title = "Token no valido",
                    Detail = "El token a vencido, genere un nuevos tokens.",
                    Status = StatusCodes.Status401Unauthorized,
                    Type = "https://httpstatuses.com/404"
                });
                return;
            }
        }
    }

}
