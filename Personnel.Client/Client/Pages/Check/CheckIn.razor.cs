namespace Personnel.Client.Client.Pages.Check
{
    public partial class CheckIn : IDisposable
    {

        [Inject] public IJSRuntime JS { get; set; } = default!;
        [Inject] public IProxy Proxy { get; set; } = default!;
        [Inject] public ISnackbar Snackbar { get; set; } = default!;

        CheckInDTO model = new();
        private DotNetObjectReference<CheckIn>? _dotnetRef;
        private bool _processing = false;
        private bool _loader = true;
        private bool _checinSuccess = false;
        private string userName = string.Empty;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _dotnetRef = DotNetObjectReference.Create(this);
                await JS.InvokeVoidAsync("getGeoLocation", _dotnetRef);
                _loader = false;
            }

        }

        [JSInvokable]
        public void SetPosition(CheckInDTO pos)
        {
            model.Latitude = pos.Latitude;
            model.Longitude = pos.Longitude;
            StateHasChanged();
        }

        [JSInvokable]
        public void SetError(LocationError err)
        {
            Snackbar.Add($"Fallo el checkin: {err.message}", Severity.Error);
        }

        private async Task OnValidSubmit(EditContext context)
        {
            _processing = true;
            try
            {
                var response = await Proxy.PostAsync<Response, CheckInDTO>("api/v1/ticket/check-in", model);

                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.Created:
                        Snackbar.Add("Registro exitoso", Severity.Success);
                        model = new CheckInDTO();
                        userName = response.Message;
                        _checinSuccess = true;
                        break;
                    case System.Net.HttpStatusCode.NotFound:
                        Snackbar.Add("Compruebe su correo y télefono", Severity.Error);
                        break;
                    case System.Net.HttpStatusCode.BadRequest:
                        Snackbar.Add("Intente de nuevo", Severity.Error);
                        break;
                    default:
                        Snackbar.Add("Ocurrió un error inesperado, informe a su jefe", Severity.Error);
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            _processing = false;

        }

        public void Dispose()
        {
            _dotnetRef?.Dispose();
        }
    }
}
