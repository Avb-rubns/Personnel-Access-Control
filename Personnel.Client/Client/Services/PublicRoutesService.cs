namespace Personnel.Client.Client.Services
{
    public class PublicRoutesService
    {
        private static readonly string[] PublicRoutes =
    {
        "/reset-password",
        "/forgot-password",
        "/register"
    };

        public bool IsPublicRoute(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return false;

            return PublicRoutes.Any(route => url.Contains(route, StringComparison.OrdinalIgnoreCase));
        }
    }
}
