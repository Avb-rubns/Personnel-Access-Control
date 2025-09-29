namespace Personnel.Client.Client.Pages.Home
{
    public partial class Index
    {

        [Inject] public IProxy Proxy { get; set; } = default!;

        private int totalItemschecks;
        private List<CheckUserTodayDTO> checkUsers = new();
        private MudTable<CheckUserTodayDTO> _tableChecker { get; set; } = new();

        private MudTable<LinkByDashboardDTO> _tableLinks { get; set; } = new();
        private List<LinkByDashboardDTO> _links = new();
        private int totalLinks;

        bool _loader = false;
        protected override void OnInitialized()
        {
            _loader = false;
        }


        private async Task<TableData<CheckUserTodayDTO>> ServerChecker(TableState state, CancellationToken token)
        {
            var data = await Proxy.GetAsync<ResponseData<List<CheckUserTodayDTO>>>($"api/v1/dashboard/checks");

            switch (data.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    checkUsers = data.Data;
                    totalItemschecks = data.Data.Count();
                    break;
                case System.Net.HttpStatusCode.NoContent:
                    totalItemschecks = 0;
                    break;

            }
            return new TableData<CheckUserTodayDTO>() { TotalItems = totalItemschecks, Items = checkUsers };
        }

        private async Task<TableData<LinkByDashboardDTO>> ServerLinks(TableState state, CancellationToken token)
        {
            int page = state.Page + 1;
            int pageSize = state.PageSize;

            var data = await Proxy.GetAsync<ResponseData<LinksTodayDTO>>($"api/v1/dashboard/links?page={page}&pagesize={pageSize}");

            switch (data.StatusCode)
            {
                case HttpStatusCode.OK:

                    _links = data.Data.LinkByDashboard;
                    totalLinks = data.Data.Pagination.Total;
                    break;
                case HttpStatusCode.NoContent:
                    totalLinks = 0;
                    break;
            }


            return new TableData<LinkByDashboardDTO> { TotalItems = totalLinks, Items = _links };

        }
    }
}
