using Microsoft.AspNetCore.Components.Forms;

namespace Personnel.Client.Client.Pages.Check
{
    public partial class Check : IDisposable
    {

        [Inject] public IJSRuntime JS { get; set; } = default!;
        [Inject] public IProxy Proxy { get; set; } = default!;




        CheckDTO model = new();
        private DotNetObjectReference<Check>? _dotnetRef;
        private bool _processing = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
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
            Console.WriteLine($"Geolocation error: {err.message}");
        }

        private async Task OnValidSubmit(EditContext context)
        {
            _processing = true;
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(model));

            var response = await Proxy.PostAsync<Response, CheckDTO>("api/v1/ticket/check", model);

            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(response));
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Ticket checked successfully.");
            }
            else
            {
                Console.WriteLine("Failed to check ticket.");
            }
            _processing = false;
        }

        public void Dispose()
        {
            _dotnetRef?.Dispose();
        }
    }
}
