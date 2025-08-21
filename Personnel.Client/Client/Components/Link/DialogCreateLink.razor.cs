namespace Personnel.Client.Client.Components.Link
{
    public partial class DialogCreateLink
    {
        [CascadingParameter] private IMudDialogInstance MudDialog { get; set; }
        [Parameter] public int UserId { get; set; }

        [Inject] public IJSRuntime JS { get; set; } = default!;
        [Inject] public IProxy Proxy { get; set; } = default!;
        [Inject] public ISnackbar Snackbar { get; set; } = default!;

        QRCreateDTO model = new();
        bool _scanURL = false;
        bool _processing = false;

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                model.UserIDRegistered = UserId;
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


        private async Task CreateAsync(EditContext context)
        {
            _processing = true;
            try
            {
                var response = await Proxy.PostAsync<ResponseData<QRDTO>, QRCreateDTO>("/api/v1/qr/create", model);

                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        MudDialog.Close(DialogResult.Ok(true));
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
        private void Cancel() => MudDialog.Cancel();
    }
}
