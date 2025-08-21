namespace Personnel.Client.Client.Pages.Link
{
    public partial class CreateQR
    {
        [CascadingParameter]
        private Task<AuthenticationState>? authenticationState { get; set; }
        [Inject] public IJSRuntime JS { get; set; } = default!;
        [Inject] public IProxy Proxy { get; set; } = default!;
        [Inject] public ISnackbar Snackbar { get; set; } = default!;
        [Inject] public HttpClient HttpClient { get; set; } = default!;


        QRCreateDTO model = new();
        QRCode qrCode = new();
        QRDTO qrCreated = new();
        bool openPanel1;
        bool openPanel2;
        bool openPanel3;
        bool openPanel4;
        bool _processing = false;
        bool _scanURL = false;

        IList<IBrowserFile> _files = new List<IBrowserFile>();
        private Dictionary<string, string>? metaTags;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (authenticationState is not null)
                {
                    var authState = await authenticationState;
                    var user = authState?.User;

                    var id = user.Claims
                                        .FirstOrDefault(c => c.Type.Equals("userId", StringComparison.OrdinalIgnoreCase));
                    model.UserIDRegistered = int.TryParse(id.Value, out int idUser) ? idUser : 0;
                }

                await JS.InvokeVoidAsync("createQR", "qrcode", qrCode);

            }
        }

        private async Task OnValidSubmit(EditContext context)
        {
            _processing = true;
            try
            {
                var response = await Proxy.PostAsync<ResponseData<QRDTO>, QRCreateDTO>("/api/v1/qr/create", model);

                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        qrCreated = response.Data;
                        qrCode.Text = qrCreated.Url;
                        await JS.InvokeVoidAsync("createQR", "qrcode", qrCode);
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

            }
            _processing = false;
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

        private async Task GetTagUrl(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return;
            }

            _scanURL = true;
            try
            {
                content.Trim();
                content = content.StartsWith("https://") ? content : "https://" + content;

                var metas = await Proxy.GetAsync<ResponseData<Dictionary<string, string>>>
                    ($"/api/v1/scrapper/title?url={Uri.EscapeDataString(content)}");
                if (metas.StatusCode == HttpStatusCode.OK)
                {
                    model.Name = metas.Data.TryGetValue("title", out string title) ? title : string.Empty;
                    model.Slug = metas.Data.TryGetValue("slug", out string slug) ? slug : string.Empty;
                }
                model.Content = content;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            _scanURL = false;
            return;
        }

        private async Task DeleteLogo()
        {
            _files.Clear();
            qrCode.Logo = string.Empty;
            await JS.InvokeVoidAsync("createQR", "qrcode", qrCode);
        }

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
        private async Task ResetQrCode()
        {
            var reset = new QRCode();
            model = new();
            qrCode = reset;
            await JS.InvokeVoidAsync("createQR", "qrcode", qrCode);
        }
        private async Task DownloadQrCode()
        {
            if (qrCode is null)
            {
                return;
            }
            await JS.InvokeVoidAsync("downloadQRcode", "qrcode", "qr1");
        }
        private async Task ShareQrCode()
        {
            if (qrCode is null)
            {
                return;
            }
            await JS.InvokeVoidAsync("shareQRCode", "qrcode");
        }

    }
}
