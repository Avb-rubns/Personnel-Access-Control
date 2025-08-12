namespace Rubns.Infrastructure.Middleware
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class RoleAndStatusAuthAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly string[] _roles;
        public RoleAndStatusAuthAttribute(string roles)
        {
            _roles = roles.Split(',').Select(r => r.Trim()).ToArray();
        }

        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;

            if (user?.Identity == null || !user.Identity.IsAuthenticated)
            {
                context.Result = new UnauthorizedResult();
                return Task.CompletedTask;
            }

            // Obtener roles (soportamos ClaimTypes.Role y "role")
            var roles = user.Claims
                .Where(c => c.Type == ClaimTypes.Role || c.Type.Equals("role", StringComparison.OrdinalIgnoreCase))
                .Select(c => c.Value)
                .ToList();

            // Obtener status (puede venir como "Status", "status", "isActive", etc. adaptalo)
            var statusClaim = user.Claims.FirstOrDefault(c => c.Type.Equals("Status", StringComparison.OrdinalIgnoreCase)
                                                           || c.Type.Equals("status", StringComparison.OrdinalIgnoreCase)
                                                           || c.Type.Equals("isActive", StringComparison.OrdinalIgnoreCase));

            var statusValue = statusClaim?.Value ?? string.Empty;

            bool isActive = statusValue.Equals("Active", StringComparison.OrdinalIgnoreCase)
                            || statusValue.Equals("1")
                            || statusValue.Equals("true", StringComparison.OrdinalIgnoreCase);

            if (!isActive)
            {
                context.Result = new ForbidResult(); // usuario no activo
                return Task.CompletedTask;
            }

            // Validar rol requerido
            var roleClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (roleClaim == null || !_roles.Contains(roleClaim, StringComparer.OrdinalIgnoreCase))
            {
                context.Result = new ForbidResult();
                return Task.CompletedTask;
            }

            // OK
            return Task.CompletedTask;
        }
    }
}
