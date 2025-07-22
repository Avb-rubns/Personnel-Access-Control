namespace Personnel.Client.Client.Pages.Admin
{
    public partial class User
    {
        [Inject] public IProxy Proxy { get; set; } = default!;
        [Inject] public ISnackbar Snackbar { get; set; } = default!;


        List<RolDTO> RolDTOs { get; set; } = new();
        RegisterUserDTO UserDTO { get; set; } = new();
        bool _processing = false;


        protected override async Task OnInitializedAsync()
        {
            var response = await Proxy.GetAsync<ResponseData<List<RolDTO>>>("api/v1/rols");
            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    RolDTOs = response.Data;
                    break;

            }

        }

        private async Task RegisterUserAsync()
        {
            _processing = true;
            try
            {
                var response = await Proxy.PostAsync<Response, RegisterUserDTO>("api/v1/register", UserDTO);
                switch (response.StatusCode)
                {
                    case System.Net.HttpStatusCode.Created:
                        Snackbar.Add("Registro exitoso", Severity.Success);
                        UserDTO = new RegisterUserDTO();
                        break;
                    case System.Net.HttpStatusCode.BadRequest:
                        Snackbar.Add(response.Message, Severity.Error);
                        break;
                    default:
                        Snackbar.Add("Ocurrió un error inesperado, informe a su jefe", Severity.Error);
                        break;
                }

            }
            catch (Exception ex) { }
            _processing = false;
        }

        void ClearFormUser()
        {
            UserDTO = new();
        }

    }
}
