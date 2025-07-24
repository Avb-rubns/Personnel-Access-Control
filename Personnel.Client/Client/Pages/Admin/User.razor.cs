namespace Personnel.Client.Client.Pages.Admin
{
    public partial class User
    {
        [Inject] public IProxy Proxy { get; set; } = default!;
        [Inject] public ISnackbar Snackbar { get; set; } = default!;

        List<RolDTO> RolDTOs { get; set; } = new();
        RegisterUserDTO UserDTO { get; set; } = new();

        List<UserRegistedDTO> Users { get; set; }
        bool _processing = false;
        int page = 1;
        bool IsFinishPage = false;
        private int totalItems;
        private string searchString = null;
        private MudTable<UserRegistedDTO> table { get; set; } = new();
        private IEnumerable<UserRegistedDTO> pagedData;
        private UserRegistedDTO selectedItem1 = null;
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
                FormatFormUser();
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
        void FormatFormUser()
        {
            UserDTO.Email.Trim();
            UserDTO.UserName.Trim();
            UserDTO.LastName.Trim();
            UserDTO.Phone.Trim();
        }

        private async Task<TableData<UserRegistedDTO>> ServerReload(TableState state, CancellationToken token)
        {
            var data = await Proxy.GetAsync<ResponseData<List<UserRegistedDTO>>>($"api/v1/user/users?page={state.Page}&pageSize={state.PageSize}");

            switch (data.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    Users = data.Data.Skip(state.Page * state.PageSize).Take(state.PageSize).ToList();
                    totalItems = data.Data.Count();
                    break;
                case HttpStatusCode.NoContent:
                    IsFinishPage = true;
                    break;

            }
            return new TableData<UserRegistedDTO>() { TotalItems = totalItems, Items = Users };
        }

        private void OnSearch(string text)
        {
            searchString = text;
            table.ReloadServerData();
        }

    }
}
