namespace Personnel.Client.Client.Services
{
    public class AuthService(ISnackbar snackbar
        , HttpClient httpClient) : AuthenticationStateProvider
    {
        public ISnackbar Snackbar { get; set; } = snackbar;
        private readonly HttpClient HttpClient = httpClient;
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {

                var httpResponse = await HttpClient.GetAsync("api/v1/auth/me");

                switch (httpResponse.StatusCode)
                {
                    case HttpStatusCode.OK:
                        var content = await httpResponse.Content.ReadAsStringAsync();
                        var response = JsonSerializer.Deserialize<UserInfoDTO>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                        if (response is { ID: > 0 })
                        {
                            var claims = new List<Claim> {
                        new Claim(ClaimTypes.NameIdentifier, response.ID.ToString()),
                        new Claim(ClaimTypes.Name, response.FirstName),
                        new Claim(ClaimTypes.Email, response.Email),
                        new Claim(ClaimTypes.Expiration, response.Expiration.ToString()),
                        new Claim("Status", response.Status.ToString()),
                        new Claim(ClaimTypes.Role, response.Role)
                        };
                            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "cookie"));
                            return new AuthenticationState(user);

                        }
                        break;
                    case HttpStatusCode.Unauthorized:
                        await TryRefreshTokenAsync();
                        break;

                }

            }
            catch (Exception ex)
            {

            }
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }
        public async Task MarkUserAsLoggedOutAsync()
        {
            NotifyAuthenticationStateChanged(Task.FromResult(
                new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));
        }
        public void Login()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task<bool> TryRefreshTokenAsync()
        {
            bool IsRefresh = false;
            try
            {

                var response = await HttpClient.PostAsync("api/v1/auth/refresh", null);
                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        IsRefresh = true;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return IsRefresh;
        }
    }
}
