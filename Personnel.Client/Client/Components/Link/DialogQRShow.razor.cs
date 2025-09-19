namespace Personnel.Client.Client.Components.Link
{
    public partial class DialogQRShow
    {
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
        [Parameter] public LinkDTO link { get; set; }
        [Parameter] public QRCode qrCode { get; set; } = new();
        [Inject] public IJSRuntime JS { get; set; } = default!;
        [Inject] public ISnackbar Snackbar { get; set; } = default!;

        QRCode _backUp = new();
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _backUp.Text = qrCode.Text;
                _backUp.ColorDark = qrCode.ColorDark;
                _backUp.PO = qrCode.PO;
                _backUp.PI = qrCode.PI;
                _backUp.ColorLight = qrCode.ColorLight;
                _backUp.DotScale = qrCode.DotScale;
                _backUp.DotScaleTiming = qrCode.DotScaleTiming;
                _backUp.DotScaleA = qrCode.DotScaleA;
                _backUp.QuietZone = qrCode.QuietZone;
                await JS.InvokeVoidAsync("createQR", "qrcode", qrCode);
            }
        }
        private async Task DownloadQrCode()
        {
            if (qrCode is null)
            {
                return;
            }
            await JS.InvokeVoidAsync("downloadQRcode", "qrcode", link.Name);
        }
        private async Task ShareQrCode()
        {
            if (qrCode is null)
            {
                return;
            }
            await JS.InvokeAsync<bool>("shareHelper.shareQR", "qrcode", "Mi dibujo", "Mira lo que hice en Blazor 😎", $"{link.Name}.png");
        }

    }
}
