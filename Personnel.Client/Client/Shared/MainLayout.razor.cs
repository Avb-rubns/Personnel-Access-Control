using MudBlazor;

namespace Personnel.Client.Client.Shared
{


    public partial class MainLayout
    {
        private bool _isDarkMode;
        private MudThemeProvider _mudThemeProvider;
        bool _drawerOpen = true;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await _mudThemeProvider.WatchSystemDarkModeAsync(OnSystemDarkModeChanged);
                StateHasChanged();
            }
        }

        private Task OnSystemDarkModeChanged(bool newValue)
        {
            _isDarkMode = newValue;
            StateHasChanged();
            return Task.CompletedTask;
        }

        void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }
        void DarkMode()
        {
            _isDarkMode = !_isDarkMode;
        }
    }
}
