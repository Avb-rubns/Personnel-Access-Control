namespace Personnel.Client.Client.Services
{
    public class AuthService(ISnackbar snackbar
        , HttpClient httpClient
        , NavigationManager navigation
        , PublicRoutesService publicRoutesService) : AuthenticationStateProvider
    {
        public ISnackbar Snackbar { get; set; } = snackbar;
        private readonly HttpClient _httpClient = httpClient;
        private readonly NavigationManager _navigation = navigation;
        private readonly PublicRoutesService _publicRoutes = publicRoutesService;
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var currentUri = new Uri(_navigation.Uri).PathAndQuery;

                if (_publicRoutes.IsPublicRoute(currentUri))
                {
                    var anonymous = new ClaimsPrincipal(new ClaimsIdentity());
                    return new AuthenticationState(anonymous);
                }

                var httpResponse = await _httpClient.GetAsync("api/v1/auth/me");

                switch (httpResponse.StatusCode)
                {
                    case HttpStatusCode.OK:
                        var content = await httpResponse.Content.ReadAsStringAsync();
                        var response = JsonSerializer.Deserialize<UserInfoDTO>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                        if (response is { ID: > 0 })
                        {
                            var claims = CreateClaims(response);
                            var user = new ClaimsPrincipal(claims);
                            return new AuthenticationState(user);

                        }
                        break;
                    case HttpStatusCode.Unauthorized:
                        var token = await TryRefreshTokenAsync();
                        return new AuthenticationState(token);

                }

            }
            catch (Exception ex)
            {

            }
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
        public async Task<bool> MarkUserAsLoggedOutAsync()
        {
            bool closed = false;
            var response = await _httpClient.DeleteAsync("api/v1/auth/logout");
            switch (response.StatusCode)
            {
                case HttpStatusCode.OK:
                    closed = true;
                    break;
            }
            NotifyAuthenticationStateChanged(Task.FromResult(
                new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));

            return closed;
        }
        public void Login()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task<ClaimsPrincipal> TryRefreshTokenAsync()
        {
            try
            {

                var response = await _httpClient.PostAsync("api/v1/auth/refresh", null);
                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        return await RefreshTokensAsync();
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return new ClaimsPrincipal();
        }
        private async Task<ClaimsPrincipal> RefreshTokensAsync()
        {
            var httpResponse = await _httpClient.GetAsync("api/v1/auth/me");

            switch (httpResponse.StatusCode)
            {
                case HttpStatusCode.OK:
                    var content = await httpResponse.Content.ReadAsStringAsync();
                    var response = JsonSerializer.Deserialize<UserInfoDTO>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (response is { ID: > 0 })
                    {
                        var claims = CreateClaims(response);
                        return new ClaimsPrincipal(claims);

                    }
                    break;

            }

            return new ClaimsPrincipal();
        }

        private ClaimsIdentity CreateClaims(UserInfoDTO userInfo)
        {
            var claims = new List<Claim> {
                        new Claim(ClaimTypes.NameIdentifier, userInfo.ID.ToString()),
                        new Claim("userId", userInfo.ID.ToString()),
                        new Claim(ClaimTypes.Name, userInfo.FirstName),
                        new Claim(ClaimTypes.Email, userInfo.Email),
                        new Claim(ClaimTypes.Expiration, userInfo.Expiration.ToString()),
                        new Claim("Status", userInfo.Status.ToString()),
                        new Claim(ClaimTypes.Role, userInfo.Role)
                        };

            return new ClaimsIdentity(claims, "cookie");
        }
    }
}
