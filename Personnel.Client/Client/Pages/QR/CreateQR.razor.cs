using MudBlazor.Utilities;

namespace Personnel.Client.Client.Pages.QR
{
    public partial class CreateQR
    {

        [Inject] public IJSRuntime JS { get; set; } = default!;
        [Inject] public IProxy Proxy { get; set; } = default!;
        [Inject] public ISnackbar Snackbar { get; set; } = default!;

        QRCreateDTO model = new();
        QRCode qrCode = new();

        bool openPanel1;
        bool openPanel2;
        bool openPanel3;
        bool openPanel4;
        bool _processing = false;

        IList<IBrowserFile> _files = new List<IBrowserFile>();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JS.InvokeVoidAsync("createQR", "qrcode", qrCode);

            }
        }

        private async Task UploadFiles(IBrowserFile file)
        {
            _files.Clear();
            _files.Add(file);
            if (file.Size <= 5_000_000)
            {
                using var stream = file.OpenReadStream(maxAllowedSize: 5_000_000); // 5 MB
                using var ms = new MemoryStream();
                await stream.CopyToAsync(ms);
                var bytes = ms.ToArray();

                var previewUrl = $"data:{file.ContentType};base64,{Convert.ToBase64String(bytes)}";
                qrCode.Logo = previewUrl; // Ahora Logo es una URL embebida
                await JS.InvokeVoidAsync("createQR", "qrcode", qrCode);

            }
        }
        private async Task DeleteLogo()
        {
            _files.Clear();
            qrCode.Logo = string.Empty;
            await JS.InvokeVoidAsync("createQR", "qrcode", qrCode);
        }

        public async Task UpdateSelectedColor(MudColor color)
        {
            qrCode.ColorLight = color.Value;

            await JS.InvokeVoidAsync("createQR", "qrcode", qrCode);
        }

        public async Task UpdateSelectedColorDot(MudColor color)
        {
            qrCode.ColorDark = color.Value;
            qrCode.PI = color.Value;
            qrCode.PO = color.Value;

            await JS.InvokeVoidAsync("createQR", "qrcode", qrCode);
        }

        public async Task UpdateSliderPaper(int quietZone)
        {
            qrCode.QuietZone = quietZone;
            await JS.InvokeVoidAsync("createQR", "qrcode", qrCode);
        }
        public async Task UpdateSliderDot(double dotSize)
        {
            qrCode.DotScale = dotSize;
            qrCode.DotScaleTiming = dotSize;
            qrCode.DotScaleA = dotSize;
            await JS.InvokeVoidAsync("createQR", "qrcode", qrCode);
        }

    }
}
