namespace Personnel.Client.Client.Pages.Forgot
{
    public partial class ResetPassword
    {
        [Inject] public NavigationManager Navigation { get; set; } = default!;
        [Inject] public IProxy Proxy { get; set; } = default!;

        [Parameter] public string Token { get; set; } = string.Empty;

        ResetPasswordRequestDTO model = new();
        bool isValidToken = false;
        bool _loader = true;
        bool _send = false;
        bool _processing = false;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var result = await Proxy.GetAsync<Response>($"api/v1/auth/reset-password/validate?token={Token}");

                switch (result.StatusCode)
                {
                    case HttpStatusCode.OK:
                        isValidToken = true;
                        break;
                    default:
                        isValidToken = false;
                        break;
                }
            }
            catch (Exception ex)
            {

            }

            _loader = false;
        }

        private async Task OnValidSubmit(EditContext context)
        {
            _processing = true;
            try
            {

                var send = await Proxy.PostAsync<Response, ResetPasswordRequestDTO>("api/v1/auth/forgot-password", model);

                _send = true;


            }
            catch (Exception ex) { }
            _processing = false;
        }

    }
}
