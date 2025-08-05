
namespace Personnel.Client.Client.Pages.Forgot
{
    public partial class ForgotPassword
    {
        [Inject] public IJSRuntime JS { get; set; } = default!;
        [Inject] public IProxy Proxy { get; set; } = default!;
        [Inject] public ISnackbar Snackbar { get; set; } = default!;

        ForgotPasswordDTO model = new();
        private bool _processing = false;
        private bool _loader = true;
        private bool _send = false;

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                _loader = false;
                StateHasChanged();
            }
        }

        private async Task OnValidSubmit(EditContext context)
        {
            _processing = true;
            try
            {
                model.Email = model.Email.Trim();
                var send = await Proxy.PostAsync<Response, ForgotPasswordDTO>("api/v1/auth/forgot-password", model);

                _send = true;


            }
            catch (Exception ex) { }
            _processing = false;
        }
    }
}
