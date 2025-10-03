namespace Personnel.Client.Client.Pages.Admin
{
    public partial class Users
    {
        [Inject] public IProxy Proxy { get; set; } = default!;
        [Inject] public ISnackbar Snackbar { get; set; } = default!;

        List<RolDTO> RolDTOs { get; set; } = new();
        RegisterUserDTO UserDTO { get; set; } = new();
        private List<UserRegistedDTO> _users { get; set; }
        private MudTable<UserRegistedDTO> table { get; set; } = new();
        private UserRegistedDTO selectedItem1 = null;
        private UserRegistedDTO elementBeforeEdit = new();

        bool _loading = true;
        bool _processing = false;
        int page = 1;
        bool IsFinishPage = false;
        private int totalItems;
        private string searchString = null;


        protected override async Task OnInitializedAsync()
        {
            var response = await Proxy.GetAsync<ResponseData<List<RolDTO>>>("api/v1/rol/rols");

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    RolDTOs = response.Data;
                    break;

            }

            _loading = false;

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
                    case HttpStatusCode.Created:
                        Snackbar.Add("Registro exitoso", Severity.Success);
                        UserDTO = new RegisterUserDTO();
                        await table.ReloadServerData();
                        break;
                    case HttpStatusCode.BadRequest:
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
            int page = state.Page + 1;
            var data = await Proxy.GetAsync<ResponseData<TableUserDTO>>($"api/v1/user/users?page={page}&pageSize={state.PageSize}&search={searchString}");

            switch (data.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    _users = data.Data.RegisteredUsers;
                    _users.ForEach(s =>
                    {
                        s.LevelPermission = RolDTOs.Single(f => f.RolID == s.RolID).LevelPermission;
                    });
                    totalItems = data.Data.Pagination.Total;
                    break;
                case HttpStatusCode.NoContent:
                    IsFinishPage = true;
                    break;

            }
            return new TableData<UserRegistedDTO>() { TotalItems = totalItems, Items = _users };
        }

        private void OnSearch(string text)
        {
            searchString = text;
            table.ReloadServerData();
        }

        private void BackupItem(object element)
        {
            elementBeforeEdit = new()
            {
                UserName = ((UserRegistedDTO)element).UserName,
                LastName = ((UserRegistedDTO)element).LastName,
                Phone = ((UserRegistedDTO)element).Phone,
                Email = ((UserRegistedDTO)element).Email,
                RolID = ((UserRegistedDTO)element).RolID,
                Status = ((UserRegistedDTO)element).Status,
                LevelPermission = ((UserRegistedDTO)element).LevelPermission,
            };
        }


        private void ResetItemToOriginalValues(object element)
        {
            ((UserRegistedDTO)element).UserName = elementBeforeEdit.UserName;
            ((UserRegistedDTO)element).LastName = elementBeforeEdit.LastName;
            ((UserRegistedDTO)element).Phone = elementBeforeEdit.Phone;
            ((UserRegistedDTO)element).Email = elementBeforeEdit.Email;
            ((UserRegistedDTO)element).RolID = elementBeforeEdit.RolID;
            ((UserRegistedDTO)element).Status = elementBeforeEdit.Status;
        }
        private async Task SendEditUserAsync()
        {
            var data = await Proxy.PatchAsync<Response, UserRegistedDTO>($"api/v1/user/{selectedItem1.UserID}", selectedItem1);

            switch (data.StatusCode)
            {
                case HttpStatusCode.OK:
                    Snackbar.Add("Cambios realizado", Severity.Success);
                    break;
                case HttpStatusCode.BadRequest:
                    Snackbar.Add("Error al realizar cambio, intente de nuevo", Severity.Error);
                    ResetItemToOriginalValues(selectedItem1);
                    break;
                case HttpStatusCode.NoContent:
                    Snackbar.Add("Error el usuario no existe", Severity.Error);
                    break;
            }
        }
    }
}
