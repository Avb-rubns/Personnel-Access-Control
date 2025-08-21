using Personnel.Client.Client.Components.Link;

namespace Personnel.Client.Client.Pages.Link
{
    public partial class Links
    {
        [CascadingParameter] private Task<AuthenticationState>? _authenticationState { get; set; }
        [Inject] public IDialogService Dialog { get; set; } = default!;
        [Inject] public IProxy Proxy { get; set; } = default!;

        private MudTable<QRDTO> table { get; set; } = new();
        private List<QRDTO> links { get; set; } = new();
        private int totalItems;
        private int _userID = default!;

        private readonly DialogOptions dialogOptions = new() { BackdropClick = false, FullWidth = true };

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
        private async Task<TableData<QRDTO>> ServerReload(TableState state, CancellationToken token)
        {
            var data = await Proxy.GetAsync<ResponseData<List<QRDTO>>>("/api/v1/qr/qrs");
            switch (data.StatusCode)
            {
                case HttpStatusCode.OK:
                    totalItems = data.Data.Count;
                    links = data.Data;
                    break;
                case System.Net.HttpStatusCode.NoContent:
                    totalItems = 0;
                    break;
            }

            return new TableData<QRDTO>() { TotalItems = totalItems, Items = links };
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
                await table.ReloadServerData();
            }
        }
    }
}
