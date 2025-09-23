namespace Personnel.Client.Client.Services
{
    public class Utils(NavigationManager navigationManager
        , IJSRuntime js
        , ISnackbar snackbar)
    {
        private readonly IJSRuntime _js = js;
        private readonly ISnackbar _snackbar = snackbar;

        private readonly NavigationManager _navigationManager = navigationManager;
        public string PathURL(string url)
        {
            string result = string.Empty;

            var uri = url.Split("qr");
            result = uri[1];


            return result;
        }

        public async Task CopyToClipboardAsync(string text)
        {
            var result = await _js.InvokeAsync<bool>("copyText", text);

            if (result)
                _snackbar.Add("URL copiado al portapapeles", Severity.Success);
            else
                _snackbar.Add("Su equipo no permite copiar al portapapeles", Severity.Error);
        }
    }
}
