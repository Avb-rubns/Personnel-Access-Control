namespace Personnel.Client.Client.Services
{
    public class AuthService(IProxy proxy, ISnackbar snackbar) : AuthenticationStateProvider
    {
        private IProxy Proxy { get; set; } = proxy;
        public ISnackbar Snackbar { get; set; } = snackbar;
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var response = await Proxy.GetAsync<ResponseData<UserInfoDTO>>("api/v1/auth/me");

                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        var claims = new List<Claim> {
                        new Claim(ClaimTypes.NameIdentifier, response.Data.ID.ToString()),
                        new Claim(ClaimTypes.Name, response.Data.FirstName),
                        new Claim(ClaimTypes.Email, response.Data.Email),
                        new Claim(ClaimTypes.Expiration, response.Data.Expiration.ToString()),
                        new Claim("Status", response.Data.Status.ToString()),
                        new Claim(ClaimTypes.Role, response.Data.Role)
                        };
                        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "cookie"));
                        return new AuthenticationState(user);
                    default:
                        Snackbar.Add("Ocurrió un error inesperado, informe a su jefe", Severity.Error);
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
            await Proxy.PostAsync<Response>("api/v1/auth/logout");
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
                var response = await Proxy.PostAsync<Response>("api/v1/auth/me");
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
