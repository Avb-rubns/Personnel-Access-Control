using Personnel.Client.Shared.DTOs.Clik;
using Rubns.Core.Entities.Clicks;

namespace Rubns.WebAPI.Presenters.Clicks
{
    internal class ClicksPresenter : IGetClicksOutputPort
    {
        public ClicksDashboardDto Result { get; set; }

        public Task Success(ClicksDashboard clicks)
        {
            ClicksDashboardDto Results = new ClicksDashboardDto();

            Results.TotalClicks = clicks.TotalClicks;
            Results.Id = clicks.FriendlyId;
            Results.LinkId = clicks.FriendlyLinkId;
            Results.Slug = clicks.Slug;
            Results.URL = clicks.URL;

            Results.ByDevice = clicks.ByDevice;
            Results.ByRegion = clicks.ByRegion;
            Results.ByCountry = clicks.ByCountry;
            Results.ByBrowser = clicks.ByBrowser;
            Results.ByOS = clicks.ByOS;
            Results.ByCity = clicks.ByCity;
            Results.ByDate = clicks.ByDate;

            Result = Results;
            return Task.CompletedTask;
        }
    }
}
