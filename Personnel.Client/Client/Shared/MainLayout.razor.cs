namespace Personnel.Client.Client.Shared
{


    public partial class MainLayout
    {

        [Inject] public IJSRuntime JS { get; set; } = default!;
        private bool _isDarkMode;
        bool _drawerOpen = false;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JS.InvokeVoidAsync("detectColorScheme");
                var theme = await JS.InvokeAsync<string>("getFromLocalStorage","theme");
                _isDarkMode = theme.Equals("dark")? true:  false ;
                StateHasChanged();
            }
        }

        void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }
        async Task DarkModeAsync()
        {
            var newTheme = _isDarkMode ? "light" : "dark";
            await JS.InvokeVoidAsync("setToLocalStorage", "theme", newTheme);
            _isDarkMode = !_isDarkMode;

        }
    }
}
