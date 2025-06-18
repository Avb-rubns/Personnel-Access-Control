
using Microsoft.JSInterop;

namespace Personnel.Client.Client.Pages.Check
{
    public partial class Check : IDisposable
    {

        [Inject] public IJSRuntime JS { get; set; } = default!;

        CheckDTO model = new();
        private bool _firstRender = true;
        private DotNetObjectReference<Check>? _dotnetRef;



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


        public void Dispose()
        {
            _dotnetRef?.Dispose();
        }
    }
}
