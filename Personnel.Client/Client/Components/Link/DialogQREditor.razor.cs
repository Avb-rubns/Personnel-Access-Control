namespace Personnel.Client.Client.Components.Link
{
    public partial class DialogQREditor : IDisposable
    {
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; } = default!;
        [Parameter] public LinkDetailDTO link { get; set; }
        [Parameter] public QRCode qrCode { get; set; } = new();
        [Parameter] public int Width { get; set; } = new();
        [Inject] public IJSRuntime JS { get; set; } = default!;
        [Inject] public IProxy Proxy { get; set; } = default!;
        [Inject] public ISnackbar Snackbar { get; set; } = default!;
        [Inject] public ContrastChecker ContrastChecker { get; set; } = default!;

        private bool _processing = false;
        private bool _openPanel1;
        private bool _openPanel2;
        private bool _change = true;

        private bool _checkContrast = false;

        private QRCode _backUp = new();
        private EditContext _editContext;

        protected override void OnInitialized()
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
            _editContext = new EditContext(qrCode);
            _editContext.OnFieldChanged += EditContextOnFieldChanged;
            DetectChanges();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JS.InvokeVoidAsync("createQR", "qrcode", qrCode);
            }
        }

        private void EditContextOnFieldChanged(object sender, FieldChangedEventArgs e)
        {
            DetectChanges();
        }

        private void DetectChanges()
        {

            var HaveChange =
                    !(qrCode.DotScale != _backUp.DotScale ||
                    qrCode.QuietZone != _backUp.QuietZone ||
                    qrCode.ColorDark != _backUp.ColorDark ||
                    qrCode.ColorLight != _backUp.ColorLight);

            _change = HaveChange;

        }

        public void Dispose()
        {
            if (_editContext != null)
                _editContext.OnFieldChanged -= EditContextOnFieldChanged;
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
            DetectChanges();
            _checkContrast = ContrastChecker.CheckContrast(qrCode.ColorLight, qrCode.ColorDark);
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
            DetectChanges();
            _checkContrast = ContrastChecker.CheckContrast(qrCode.ColorLight, qrCode.ColorDark);

        }
        private async Task UpdateSliderPaper(int quietZone)
        {
            if (qrCode is null)
            {
                return;
            }
            qrCode.QuietZone = quietZone;

            await JS.InvokeVoidAsync("createQR", "qrcode", qrCode);
            DetectChanges();
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
            DetectChanges();
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
            if (_editContext != null)
                _editContext.OnFieldChanged -= EditContextOnFieldChanged;

            qrCode.Text = _backUp.Text;
            qrCode.ColorDark = _backUp.ColorDark;
            qrCode.PO = _backUp.PO;
            qrCode.PI = _backUp.PI;
            qrCode.ColorLight = _backUp.ColorLight;
            qrCode.DotScale = _backUp.DotScale;
            qrCode.DotScaleTiming = _backUp.DotScaleTiming;
            qrCode.DotScaleA = _backUp.DotScaleA;
            qrCode.QuietZone = _backUp.QuietZone;
            _editContext = new EditContext(qrCode);
            _editContext.OnFieldChanged += EditContextOnFieldChanged;
            DetectChanges();

            await JS.InvokeVoidAsync("createQR", "qrcode", qrCode);
            await JS.InvokeVoidAsync("createQR", "qrcode", qrCode);
        }
    }
}
