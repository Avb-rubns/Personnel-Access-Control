using Personnel.Client.Shared.DTOs.Link.Queries;

namespace Personnel.Client.Client.Pages.Link
{
    public partial class LinkEdit
    {
        [CascadingParameter] private Task<AuthenticationState>? _authenticationState { get; set; }
        [Inject] public IProxy Proxy { get; set; } = default!;
        [Inject] public IJSRuntime JS { get; set; } = default!;
        [Inject] public ISnackbar Snackbar { get; set; } = default!;

        private LinkDTO _link = new LinkDTO();
    }
}
