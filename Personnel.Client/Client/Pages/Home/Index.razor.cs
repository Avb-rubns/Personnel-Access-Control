namespace Personnel.Client.Client.Pages.Home
{
    public partial class Index
    {

        [Inject] public IProxy Proxy { get; set; } = default!;


        bool _loader = false;

        List<CheckUserTodayDTO> checkUsers = new List<CheckUserTodayDTO>();
        private MudTable<CheckUserTodayDTO> table { get; set; } = new();
        private int totalItems;
        private string searchString = null;


        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                _loader = false;
            }
        }


        private async Task<TableData<CheckUserTodayDTO>> ServerReload(TableState state, CancellationToken token)
        {
            var data = await Proxy.GetAsync<ResponseData<List<CheckUserTodayDTO>>>($"api/v1/dashboard");

            switch (data.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    checkUsers = data.Data;
                    totalItems = data.Data.Count();
                    break;
                case System.Net.HttpStatusCode.NoContent:
                    totalItems = 0;
                    break;

            }
            return new TableData<CheckUserTodayDTO>() { TotalItems = totalItems, Items = checkUsers };
        }
        private void OnSearch(string text)
        {
            searchString = text;
            if (!string.IsNullOrEmpty(searchString))
            {
                table.ReloadServerData();
            }
        }
    }
}
