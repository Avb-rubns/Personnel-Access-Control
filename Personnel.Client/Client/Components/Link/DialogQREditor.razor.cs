namespace Personnel.Client.Client.Components.Link
{
    public partial class DialogQREditor
    {
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
        [Parameter] public LinkDetailDTO link { get; set; }
        [Parameter] public QRCode qrCode { get; set; } = new();
        [Inject] public IJSRuntime JS { get; set; } = default!;
        [Inject] public IProxy Proxy { get; set; } = default!;
        [Inject] public ISnackbar Snackbar { get; set; } = default!;


        bool _processing = false;
        bool _openPanel1;
        bool _openPanel2;

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

        private async Task UpdataQRAsync()
        {
            _processing = true;
            try
            {
                QRUpdateDTO qr = new()
                {
                    ColorDark = _backUp.ColorDark != qrCode.ColorDark ? qrCode.ColorDark : null,
                    ColorLight = _backUp.ColorLight != qrCode.ColorLight ? qrCode.ColorLight : null,
                    QuietZone = qrCode.QuietZone,
                    DotScale = _backUp.DotScale != qrCode.DotScale ? qrCode.DotScale : double.NaN,
                };
                var response = await Proxy.PatchAsync<Response, QRUpdateDTO>($"api/v1/link/qr/{link.QR.Id}", qr);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        Snackbar.Add("Cambios realizado", Severity.Success);
                        MudDialog.Close(DialogResult.Ok(true));
                        break;
                    case HttpStatusCode.BadRequest:
                        Snackbar.Add("Error al realizar cambio, intente de nuevo", Severity.Error);
                        break;
                    case HttpStatusCode.NoContent:
                        Snackbar.Add("Error el QR no existe", Severity.Error);
                        break;
                }

            }
            catch (Exception e) { }

            _processing = false;
        }
        private void Cancel() => MudDialog.Cancel();
        private async Task UpdateSelectedColor(MudColor color)
        {
            if (qrCode is null)
            {
                return;
            }
            qrCode.ColorLight = color.Value;
            await JS.InvokeVoidAsync("createQR", "qrcode", qrCode);
        }
        private async Task UpdateSelectedColorDot(MudColor color)
        {
            if (qrCode is null)
            {
                return;
            }
            qrCode.ColorDark = color.Value;
            qrCode.PI = color.Value;
            qrCode.PO = color.Value;

            await JS.InvokeVoidAsync("createQR", "qrcode", qrCode);
        }
        private async Task UpdateSliderPaper(int quietZone)
        {
            if (qrCode is null)
            {
                return;
            }
            qrCode.QuietZone = quietZone;

            await JS.InvokeVoidAsync("createQR", "qrcode", qrCode);
        }
        public async Task UpdateSliderDot(double dotSize)
        {
            if (qrCode is null)
            {
                return;
            }
            qrCode.DotScale = dotSize;
            qrCode.DotScaleTiming = dotSize;
            qrCode.DotScaleA = dotSize;
            await JS.InvokeVoidAsync("createQR", "qrcode", qrCode);
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
        private async Task ResetQrCode()
        {
            qrCode = _backUp;
            await JS.InvokeVoidAsync("createQR", "qrcode", qrCode);
            await JS.InvokeVoidAsync("createQR", "qrcode", qrCode);
        }
    }
}
