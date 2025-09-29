namespace Personnel.Client.Client.Shared
{
    public partial class LogIn
    {

        [Inject] public IProxy Proxy { get; set; } = default!;
        [Inject] public ISnackbar Snackbar { get; set; } = default!;
        [Inject] public AuthService AuthJWT { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;

        bool _loader = false;
        bool _processing = false;
        bool isShow;

        InputType PasswordInput = InputType.Password;
        string PasswordInputIcon = Icons.Material.Filled.VisibilityOff;

        private LoginRequestDTO model = new();

        protected override void OnInitialized()
        {
            NavigationManager.NavigateTo("/login", replace: true);
        }


        void Visibility()
        {
            if (isShow)
            {
                isShow = false;
                PasswordInputIcon = Icons.Material.Filled.VisibilityOff;
                PasswordInput = InputType.Password;
            }
            else
            {
                isShow = true;
                PasswordInputIcon = Icons.Material.Filled.Visibility;
                PasswordInput = InputType.Text;
            }
        }

        private async Task OnValidSubmit(EditContext context)
        {
            _processing = true;
            try
            {
                var response = await Proxy.PostAsync<ResponseData<AuthResponseDTO>, LoginRequestDTO>("api/v1/login", model);
                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.OK:
                        model = new();
                        AuthJWT.Login();
                        NavigationManager.NavigateTo("", true);
                        break;
                    case System.Net.HttpStatusCode.Unauthorized:
                        Snackbar.Add("Credenciales incorrectas o usuario no registrado.", Severity.Error);
                        break;
                    case System.Net.HttpStatusCode.BadRequest:
                        Snackbar.Add("Credenciales incorrectas o usuario no registrado.", Severity.Error);
                        break;
                    default:
                        Snackbar.Add("Ocurrió un error inesperado, informe a su jefe", Severity.Error);
                        break;
                }
                _processing = false;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            _processing = false;
            StateHasChanged();
        }
    }
}
