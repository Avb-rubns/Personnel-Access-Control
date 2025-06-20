using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace Personnel.Client.Client.Pages.Check
{
    public partial class Check : IDisposable
    {

        [Inject] public IJSRuntime JS { get; set; } = default!;
        [Inject] public IProxy Proxy { get; set; } = default!;
        [Inject] public ISnackbar Snackbar { get; set; } = default!;

        CheckDTO model = new();
        private DotNetObjectReference<Check>? _dotnetRef;
        private bool _processing = false;
        private bool _loader = true;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _loader = false;
                _dotnetRef = DotNetObjectReference.Create(this);
                await JS.InvokeVoidAsync("getGeoLocation", _dotnetRef);
            }

        }

        [JSInvokable]
        public void SetPosition(CheckDTO pos)
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
            var response = await Proxy.PostAsync<Response, CheckDTO>("api/v1/ticket/check", model);

            _ = response.StatusCode switch
            {
                System.Net.HttpStatusCode.OK => Snackbar.Add("Registro exitoso", Severity.Success),
                System.Net.HttpStatusCode.NotFound => Snackbar.Add("Compruebe su correo y télefono", Severity.Error),
                System.Net.HttpStatusCode.BadRequest => Snackbar.Add("Intente de nuevo", Severity.Error),
                _ => Snackbar.Add("Ocurrió un error inesperado, informe a su jefe", Severity.Error)
            };
            _processing = false;
        }

        public void Dispose()
        {
            _dotnetRef?.Dispose();
        }
    }
}
