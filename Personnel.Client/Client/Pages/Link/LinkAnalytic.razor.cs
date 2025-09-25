namespace Personnel.Client.Client.Pages.Link
{
    public partial class LinkAnalytic
    {
        [CascadingParameter(Name = "IsDarkMode")] public bool IsDarkMode { get; set; }
        [Parameter] public string Slug { get; set; } = default!;
        [Inject] public IDialogService Dialog { get; set; } = default!;
        [Inject] public IProxy Proxy { get; set; } = default!;
        [Inject] public IJSRuntime JS { get; set; } = default!;
        [Inject] public ISnackbar Snackbar { get; set; } = default!;
        [Inject] public NavigationManager NavigationManager { get; set; } = default!;
        [Inject] public Utils Utils { get; set; } = default!;
        [Inject] public IApexChartService ApexChartService { get; set; } = default!;


        private ClicksDashboardDto _click = new();
        private bool _isLoading = true;
        private FilterOption _filterOption =
            new FilterOption("month", "Último mes", DateTime.Today.AddMonths(-1).ToString("yyyy-MM-dd"));

        List<FilterOption> Filters = new()
        {
            new FilterOption("today", "Del día", DateTime.Today.AddDays(1).AddTicks(-1).ToString("yyyy-MM-dd")),
            new FilterOption("week", "Última semana", DateTime.Today.AddDays(-7).ToString("yyyy-MM-dd")),
            new FilterOption("month", "Último mes", DateTime.Today.AddMonths(-1).ToString("yyyy-MM-dd")),
            new FilterOption("custom", "Por rango", DateTime.Today.ToString("yyyy-MM-dd"))
        };

        private string _star = string.Empty;
        private string _end = string.Empty;
        private bool _DisablePickerRange = true;
        private DateRange _dateRange { get; set; } = new DateRange();
        private MudDateRangePicker _picker;

        private bool _previousIsDarkMode;

        private List<DataPoint> dataPoints;
        private ApexChartOptions<DataPoint> _optionsAPEXChart;
        private ApexChart<DataPoint> _chart;


        protected override async Task OnParametersSetAsync()
        {
            if (_previousIsDarkMode != IsDarkMode)
            {
                _previousIsDarkMode = IsDarkMode;
                ApplyTheme();
                await ForceRenderChart();
            }

        }

        private async Task ForceRenderChart()
        {
            if (_chart is not null)
            {
                StateHasChanged();
                await _chart.UpdateSeriesAsync(true);
                await _chart.RenderAsync();
            }
        }

        private void ApplyTheme()
        {
            var theme = new Theme { Mode = IsDarkMode ? Mode.Dark : Mode.Light };
            ApexChartService.GlobalOptions.Theme = theme;
            _optionsAPEXChart.Theme = theme;
        }

        protected override async Task OnInitializedAsync()
        {
            _dateRange.Start = DateTime.Parse(_filterOption.Date);
            _dateRange.End = DateTime.Parse(Filters[0].Date);

            _star = _filterOption.Date;
            _end = Filters[0].Date;
            SetupChartOptions();
            await LoadDataAsync();
            _isLoading = false;
        }

        private void SetupChartOptions()
        {
            var theme = new Theme { Mode = IsDarkMode ? Mode.Dark : Mode.Light };

            _optionsAPEXChart = new ApexChartOptions<DataPoint>
            {
                Theme = theme,
                Chart = new Chart
                {
                    Animations = new Animations
                    {
                        Easing = Easing.Linear,
                        DynamicAnimation = new DynamicAnimation { Speed = 900 }
                    },
                    DropShadow = new DropShadow
                    {
                        Enabled = true,
                        Top = 18,
                        Left = 7,
                        Blur = 10,
                        Opacity = 0.2d
                    }
                },
                NoData = new NoData { Text = "Sin información" },
                Yaxis = new List<YAxis> { new YAxis { DecimalsInFloat = 0 } },
                Markers = new Markers { Shape = MarkerShape.Circle, Size = 5, FillOpacity = 0.8d },
                Stroke = new Stroke { Curve = Curve.Smooth },
                Legend = new Legend
                {
                    Position = LegendPosition.Top,
                    HorizontalAlign = ApexCharts.Align.Right,
                    Floating = true,
                    OffsetX = -5,
                    OffsetY = -25,
                }
            };
        }


        private async Task ChangeRange(FilterOption select)
        {
            _filterOption = select;
            if (select.Key.Equals("custom"))
            {
                _DisablePickerRange = false;
                StateHasChanged();
                await _picker.OpenAsync();
                _star = _dateRange.Start?.ToString("yyyy-MM-dd");
                _end = _dateRange.End?.ToString("yyyy-MM-dd");
            }
            else
            {
                _DisablePickerRange = true;
                _dateRange.Start = DateTime.Parse(select.Date);
                _dateRange.End = DateTime.Parse(Filters[0].Date);
                _filterOption = select;
                _star = _dateRange.Start?.ToString("yyyy-MM-dd");
                _end = _dateRange.End?.ToString("yyyy-MM-dd");

            }
            await LoadDataAsync();
            await ForceRenderChart();
        }

        private async Task LoadDataAsync()
        {

            var data = await Proxy.GetAsync<ResponseData<ClicksDashboardDto>>($"/api/v1/click/{Slug}?starDate={_star}&endDate={_end}");
            switch (data.StatusCode)
            {
                case HttpStatusCode.OK:
                    _click = data.Data;
                    dataPoints = _click.ByDate

                    .Select(kv => new DataPoint
                    {
                        Date = DateTime.ParseExact(kv.Key, "dd/MM/yyyy", null),
                        Value = kv.Value
                    })
                    .OrderBy(p => p.Date)
                    .ToList();
                    break;
                default:
                    Snackbar.Add("Error al obtener la informacion, recargue de nuevo, si el error persiste contacte a un administrador.", Severity.Error);
                    break;
            }
        }
    }
}
