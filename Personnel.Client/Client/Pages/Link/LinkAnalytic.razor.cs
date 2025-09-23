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
        private bool _previousIsDarkMode;
        private List<DataPoint> dataPoints = new();
        private ApexChartOptions<DataPoint> _optionsAPEXChart;
        private ApexChart<DataPoint> _chart;


        protected override async Task OnParametersSetAsync()
        {
            if (_previousIsDarkMode != IsDarkMode)
            {
                _previousIsDarkMode = IsDarkMode;

                ApplyTheme();

                if (_chart is not null)
                {
                    StateHasChanged();
                    await _chart.RenderAsync();
                }
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
            var global = ApexChartService.GlobalOptions;
            _optionsAPEXChart = new ApexChartOptions<DataPoint>
            {
                Theme = global.Theme,
                Chart = new Chart
                {
                    Animations = new Animations
                    {
                        Easing = Easing.Linear,
                        DynamicAnimation = new DynamicAnimation { Speed = 900 }
                    }
                },
                NoData = new NoData { Text = "Sin información" },
                Yaxis = new List<YAxis>
                {
                    new YAxis()
                    {
                        DecimalsInFloat = 0
                    }
                }
            };


            await GetDataAsync();
            _isLoading = false;
        }

        private async Task LineApexChart()
        {
            dataPoints = _click.ByDate
                .Select(kv => new DataPoint
                {
                    Date = DateTime.ParseExact(kv.Key, "dd/MM/yyyy", null),
                    Value = kv.Value
                })
            .OrderBy(p => p.Date)
            .ToList();


            _optionsAPEXChart.Chart = new ApexCharts.Chart
            {
                DropShadow = new DropShadow
                {
                    Enabled = true,
                    Top = 18,
                    Left = 7,
                    Blur = 10,
                    Opacity = 0.2d
                }
            };

            _optionsAPEXChart.Xaxis = new XAxis
            {
                Title = new AxisTitle
                {
                    Text = DateTime.Parse(_filterOption.Date).ToString("MMMM")
                }
            };

            _optionsAPEXChart.Markers = new Markers { Shape = MarkerShape.Circle, Size = 5, FillOpacity = new Opacity(0.8d) };

            _optionsAPEXChart.Stroke = new Stroke { Curve = Curve.Smooth };
            _optionsAPEXChart.Legend = new Legend
            {
                Position = LegendPosition.Top,
                HorizontalAlign = ApexCharts.Align.Right,
                Floating = true,
                OffsetX = -5,
                OffsetY = -25,
            };
            await _chart.RenderAsync();
        }

        private async Task ChangeRange(FilterOption select)
        {
            _filterOption = select;
            await GetDataAsync();
            StateHasChanged();
        }

        private async Task GetDataAsync()
        {
            var data = await Proxy.GetAsync<ResponseData<ClicksDashboardDto>>($"/api/v1/click/{Slug}?starDate={_filterOption.Date}&endDate={Filters[0].Date}");
            switch (data.StatusCode)
            {
                case HttpStatusCode.OK:
                    _click = data.Data;
                    LineApexChart();
                    break;
                default:
                    Snackbar.Add("Error al obtener la informacion, recargue de nuevo, si el error persiste contacte a un administrador.", Severity.Error);
                    break;
            }
        }
    }
}
