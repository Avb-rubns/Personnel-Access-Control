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
        private string _filter = "all";

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

                    var id = user.Claims.FirstOrDefault(c => c.Type.Equals("userId", StringComparison.OrdinalIgnoreCase));
                    _userID = int.TryParse(id.Value, out int idUser) ? idUser : 0;
                }
            }
        }
        private async Task<TableData<LinkDTO>> ServerReload(TableState state, CancellationToken token)
        {
            int page = state.Page == 0 ? 1 : state.Page;
            var data = await Proxy.GetAsync<ResponseData<LinksDTO>>($"/api/v1/link/links?page={page}&pageSize={state.PageSize}&filter={_filter}");
            switch (data.StatusCode)
            {
                case HttpStatusCode.OK:
                    totalItems = data.Data.Pagination.Total;
                    _links = data.Data.Links;
                    break;
                case System.Net.HttpStatusCode.NoContent:
                    totalItems = 0;
                    _links = new();
                    break;
            }

            switch (state.SortLabel)
            {
                case "name":
                    _links = _links.OrderByDirection(state.SortDirection, o => o.Name).ToList();
                    break;
                case "url":
                    _links = _links.OrderByDirection(state.SortDirection, o => o.Slug).ToList();
                    break;
                case "destiny":
                    _links = _links.OrderByDirection(state.SortDirection, o => o.Content).ToList();
                    break;
                case "clicks":
                    _links = _links.OrderByDirection(state.SortDirection, o => o.Clicks).ToList();
                    break;
                case "status":
                    _links = _links.OrderByDirection(state.SortDirection, o => o.Status).ToList();
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
        private async Task QREditor(LinkDTO link)
        {
            QRCode qrCode = new()
            {
                Text = link.Url,
                ColorDark = link.QR.ColorDark,
                PO = link.QR.ColorDark,
                PI = link.QR.ColorDark,
                ColorLight = link.QR.ColorLight,
                DotScale = link.QR.DotScale,
                DotScaleA = link.QR.DotScale,
                DotScaleTiming = link.QR.DotScale,
                QuietZone = link.QR.QuietZone,

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
        private async Task DeleteLinkAsync(string id)
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
            var result = await JS.InvokeAsync<bool>("copyText", url);
            if (result)
            {
                Snackbar.Add("URL copiado al portapapeles", severity: Severity.Success);

            }
            else { Snackbar.Add("Su equipo no permite copiar al portapapeles", severity: Severity.Error); }
        }

        private async Task UpdateFilter(string mudSelect)
        {
            _filter = mudSelect;
            await _table.ReloadServerData();
        }

    }
}
