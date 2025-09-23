namespace Personnel.Client.Client.Pages.Link
{
    public partial class LinkDetail : IDisposable
    {
        [Parameter] public string Slug { get; set; } = default!;
        [Inject] public IDialogService Dialog { get; set; } = default!;
        [Inject] public IProxy Proxy { get; set; } = default!;
        [Inject] public IJSRuntime JS { get; set; } = default!;
        [Inject] public ISnackbar Snackbar { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;


        private LinkDetailDTO _link = new();
        private EditContext _editContext;
        private LinkUpdateDTO _linkBackup = new LinkUpdateDTO();
        private LinkUpdateDTO _linkForm = new();
        private QRCode qrCode = new();
        private int _clicks = 0;
        private bool _processing;
        private bool _open;
        private bool _change = true;
        private bool _isLoading = true;
        private bool _qrInitialized = false;
        private int _width = 0;
        private bool refresh = false;
        private readonly DialogOptions dialogOptions = new()
        { BackdropClick = false, MaxWidth = MaxWidth.Medium, FullWidth = true };
        private readonly DialogOptions _fullScreen = new() { FullScreen = true };


        protected override async Task OnInitializedAsync()
        {

            await GetLinkDetail();
            if (!_isLoading)
            {
                _editContext = new EditContext(_linkForm);
                _editContext.OnFieldChanged += EditContextOnFieldChanged;
                DetectChanges();
                _width = await JS.InvokeAsync<int>("width");
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!_isLoading && !_qrInitialized)
            {
                await PaintQR();
                _qrInitialized = true;
            }
        }

        private void EditContextOnFieldChanged(object sender, FieldChangedEventArgs e)
        {
            DetectChanges();
        }

        private void DetectChanges()
        {

            var HaveChange =
                    !(_linkForm.Name != _linkBackup.Name ||
                    _linkForm.Content != _linkBackup.Content ||
                    _linkForm.Slug != _linkBackup.Slug);

            _change = HaveChange;

        }
        public void Dispose()
        {
            if (_editContext != null)
                _editContext.OnFieldChanged -= EditContextOnFieldChanged;
        }

        private void Reset()
        {
            if (_editContext != null)
                _editContext.OnFieldChanged -= EditContextOnFieldChanged;

            _linkForm.Name = _linkBackup.Name;
            _linkForm.Slug = _linkBackup.Slug;
            _linkForm.Content = _linkBackup.Content;
            _editContext = new EditContext(_linkForm);
            _editContext.OnFieldChanged += EditContextOnFieldChanged;
            DetectChanges();
        }

        private void Backup()
        {
            _linkForm = new LinkUpdateDTO()
            {
                Name = _link.Name,
                Slug = _link.Slug,
                Content = _link.Content
            };

            _linkBackup = new LinkUpdateDTO()
            {
                Name = _link.Name,
                Slug = _link.Slug,
                Content = _link.Content
            };
        }

        private async Task GetLinkDetail()
        {
            var data = await Proxy.GetAsync<ResponseData<LinkDetailDTO>>($"/api/v1/link/{Slug}");
            switch (data.StatusCode)
            {
                case HttpStatusCode.OK:
                    if (data.Data is not null)
                    {
                        _link = data.Data;
                        Backup();
                        _clicks = _link.Clicks.Count();
                        _isLoading = false;
                    }
                    break;
                default:
                    Snackbar.Add("Error al obtener la informacion, recargue de nuevo, si el error persiste contacte a un administrador.", Severity.Error);

                    break;
            }
        }

        private async Task PaintQR()
        {
            qrCode.Text = _link.Url;
            qrCode.ColorDark = _link.QR.ColorDark;
            qrCode.PO = _link.QR.ColorDark;
            qrCode.PI = _link.QR.ColorLight;
            qrCode.ColorLight = _link.QR.ColorLight;
            qrCode.DotScale = _link.QR.DotScale;
            qrCode.DotScaleTiming = _link.QR.DotScale;
            qrCode.DotScaleA = _link.QR.DotScale;
            qrCode.QuietZone = _link.QR.QuietZone;
            qrCode.Width = 100;
            qrCode.Height = 100;
            await JS.InvokeVoidAsync("createQR", "qrcodeLink", qrCode);
        }
        private async Task ClipboardCopy()
        {
            var result = await JS.InvokeAsync<bool>("copyText", _link.Url);
            if (result)
            {
                Snackbar.Add("URL copiado al portapapeles", severity: Severity.Success);

            }
            else { Snackbar.Add("Su equipo no permite copiar al portapapeles", severity: Severity.Error); }
        }
        private async Task QREdit()
        {
            QRCode qrCode = new()
            {
                Text = _link.Url,
                ColorDark = _link.QR.ColorDark,
                PO = _link.QR.ColorDark,
                PI = _link.QR.ColorDark,
                ColorLight = _link.QR.ColorLight,
                DotScale = _link.QR.DotScale,
                DotScaleA = _link.QR.DotScale,
                DotScaleTiming = _link.QR.DotScale,
                QuietZone = _link.QR.QuietZone,

            };
            var parameters = new DialogParameters<DialogQREditor>
            {
                {x => x.link, _link},
                {x => x.qrCode, qrCode},
                {x => x.Width, _width }
            };




            var dialog = await Dialog.ShowAsync<DialogQREditor>("Show", parameters, _width < 550 ? _fullScreen : dialogOptions);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                await GetLinkDetail();
                await PaintQR();
            }
        }
        private async Task SendUpdate()
        {
            _processing = true;

            try
            {
                LinkUpdateDTO update = new()
                {
                    Name = _linkBackup.Name != _linkForm.Name ? _linkForm.Name : string.Empty,
                    Content = _linkBackup.Content != _linkForm.Content ? _linkForm.Content : string.Empty,
                };

                if (_linkBackup.Slug != _linkForm.Slug)
                {
                    update.Slug = _linkForm.Slug;
                    refresh = true;
                }

                var response = await Proxy.PatchAsync<Response, LinkUpdateDTO>($"api/v1/link/{_link.ID}", update);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        Snackbar.Add("Cambios realizados", Severity.Success);
                        NavigationManager.NavigateTo($"/link/{update.Slug}", replace: refresh);
                        break;
                    case HttpStatusCode.BadRequest:
                        Snackbar.Add("Error al realizar cambio, intente de nuevo", Severity.Error);
                        break;
                    case HttpStatusCode.NoContent:
                        Snackbar.Add("Error el QR no existe", Severity.Error);
                        break;
                }
            }
            catch
            {

            }
            _processing = false;

        }
        private async Task DownloadQrCode()
        {
            if (qrCode is null)
            {
                return;
            }
            await JS.InvokeVoidAsync("downloadQRcode", "qrcodeLink", _link.Name);
        }
        private async Task ShareQrCode()
        {
            if (qrCode is null)
            {
                return;
            }
            await JS.InvokeAsync<bool>("shareHelper.shareQR", "qrcodeLink", "Mi dibujo", "Mira lo que hice en Blazor 😎", $"{_link.Name}.png");
        }

        private void Analytics(string slug)
        {
            NavigationManager.NavigateTo($"link/analytics/{slug}");
        }
        private void ToggleOpen() => _open = !_open;
    }
}
