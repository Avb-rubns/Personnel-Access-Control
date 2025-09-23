namespace Personnel.Client.Client.Shared
{
    public partial class MainLayout()
    {

        [Inject] public IJSRuntime JS { get; set; } = default!;
        [Inject] public AuthService AuthService { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;
        [Inject] public IApexChartService ApexChartService { get; set; } = default!;
        private bool _isDarkMode;
        bool _drawerOpen = false;
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var theme = await JS.InvokeAsync<string>("getFromLocalStorage", "theme");

                if (string.IsNullOrEmpty(theme))
                {
                    await JS.InvokeVoidAsync("detectColorScheme");
                    theme = await JS.InvokeAsync<string>("getFromLocalStorage", "theme");
                }

                _isDarkMode = theme == "dark";

                var global = ApexChartService.GlobalOptions;

                global.Theme = new Theme { Mode = _isDarkMode ? Mode.Dark : Mode.Light, Palette = PaletteType.Palette3 };
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
            var global = ApexChartService.GlobalOptions;
            global.Theme = new Theme { Mode = _isDarkMode ? Mode.Dark : Mode.Light };
            StateHasChanged();

        }

        async Task Logout()
        {
            var closedSession = await AuthService.MarkUserAsLoggedOutAsync();
            if (closedSession)
            {
                NavigationManager.NavigateTo("/", true);
            }
        }
    }
}
