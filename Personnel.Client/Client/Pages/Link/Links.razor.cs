namespace Personnel.Client.Client.Pages.Link
{
    public partial class Links
    {
        [CascadingParameter] private Task<AuthenticationState>? _authenticationState { get; set; }
        [Inject] public IDialogService Dialog { get; set; } = default!;
        [Inject] public IProxy Proxy { get; set; } = default!;
        [Inject] public IJSRuntime JS { get; set; } = default!;
        [Inject] public ISnackbar Snackbar { get; set; } = default!;
        [Inject] public Utils Utils { get; set; } = default!;

        private MudTable<LinkDTO> _table { get; set; } = new();
        private List<LinkDTO> _links { get; set; } = new();
        private int totalItems;
        private int _userID = default!;

        private readonly DialogOptions dialogOptions = new()
        { BackdropClick = false, MaxWidth = MaxWidth.Medium, FullWidth = true };
        private readonly DialogOptions _fullScreen = new() { FullScreen = true };
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                if (_authenticationState is not null)
                {
                    var authState = await _authenticationState;
                    var user = authState?.User;

                    var id = user.Claims
                                        .FirstOrDefault(c => c.Type.Equals("userId", StringComparison.OrdinalIgnoreCase));
                    _userID = int.TryParse(id.Value, out int idUser) ? idUser : 0;
                }
            }
        }
        private async Task<TableData<LinkDTO>> ServerReload(TableState state, CancellationToken token)
        {
            var data = await Proxy.GetAsync<ResponseData<List<LinkDTO>>>("/api/v1/link/links");
            switch (data.StatusCode)
            {
                case HttpStatusCode.OK:
                    totalItems = data.Data.Count;
                    _links = data.Data;
                    break;
                case System.Net.HttpStatusCode.NoContent:
                    totalItems = 0;
                    break;
            }

            return new TableData<LinkDTO>() { TotalItems = totalItems, Items = _links };
        }
        private async Task CreateLink()
        {
            var parameters = new DialogParameters<DialogCreateLink>
            {
                {x => x.UserId,_userID},
            };

            var dialog = await Dialog.ShowAsync<DialogCreateLink>("Create", parameters, dialogOptions);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                await _table.ReloadServerData();
            }
        }
        private async Task QREditor(int IDQR)
        {
            var link = _links.Find(x => x.ID == IDQR);
            QRCode qrCode = new QRCode()
            {
                Text = link.Url,
                ColorDark = link.ColorDark,
                PO = link.ColorDark,
                PI = link.ColorDark,
                ColorLight = link.ColorLight,
                DotScale = link.DotScale,
                DotScaleA = link.DotScale,
                DotScaleTiming = link.DotScale,
                QuietZone = link.QuietZone,

            };
            var parameters = new DialogParameters<DialogQREditor>
            {
                {x => x.link, link},
                {x => x.qrCode, qrCode}
            };

            int width = await JS.InvokeAsync<int>("width");


            var dialog = await Dialog.ShowAsync<DialogQREditor>("Editor", parameters, width < 550 ? _fullScreen : dialogOptions);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                await _table.ReloadServerData();
            }
        }
        private async Task DeleteLinkAsync(int id)
        {
            var link = _links.Find(x => x.ID == id);

            var parameters = new DialogParameters<DialogConfirmDelete>
            {
                {x => x.Message, $"Eliminar Link{link.Name}"},
            };


            var dialog = await Dialog.ShowAsync<DialogConfirmDelete>("Borrar", parameters);
            var result = await dialog.Result;
            if (!result.Canceled)
            {
                var delete = await Proxy.DeleteAsync<Response>($"api/v1/link/{id}");
                switch (delete.StatusCode)
                {
                    case HttpStatusCode.OK:
                        Snackbar.Add("Link eliminado", severity: Severity.Success);
                        await _table.ReloadServerData();
                        StateHasChanged();
                        break;
                    case HttpStatusCode.NotFound:
                        Snackbar.Add("Error al eliminar link", severity: Severity.Error);
                        break;
                    default:
                        Snackbar.Add(delete.Message, severity: Severity.Warning);
                        break;
                }

            }
        }

        private async Task ClipboardCopy(string url)
        {
            await JS.InvokeVoidAsync("clipboardCopy.copyText", url);
        }
    }
}
