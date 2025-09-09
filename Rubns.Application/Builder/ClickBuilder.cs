
namespace Rubns.Application.Builder
{
    public class ClickBuilder
    {
        private int? _id;
        private int _linkId;
        private string _country = string.Empty;
        private string _region = string.Empty;
        private string _city = string.Empty;
        private string _deviceType = string.Empty;
        private string _os = string.Empty;
        private string _browser = string.Empty;
        private string? _ipAddress = string.Empty;


        public ClickBuilder Withd(int Id) { _id = Id; return this; }
        public ClickBuilder WithLinkId(int linkId) { _linkId = linkId; return this; }
        public ClickBuilder WithCountry(string country) { _country = country; return this; }
        public ClickBuilder WithRegion(string region) { _region = region; return this; }
        public ClickBuilder WithCity(string city) { _city = city; return this; }
        public ClickBuilder WithDeviceType(string deviceType) { _deviceType = deviceType; return this; }
        public ClickBuilder WithOs(string os) { _os = os; return this; }
        public ClickBuilder WithBrowser(string browser) { _browser = browser; return this; }
        public ClickBuilder WithIPAddress(string? ipAddress) { _ipAddress = ipAddress; return this; }
        public ClickBuilder WithUserAgent(StringValues userAgent, Dictionary<string, string> headers)
        {
            var clientHints = ClientHints.Factory(headers);
            var dd = new DeviceDetector(userAgent, clientHints);
            dd.Parse();
            _deviceType = dd.GetDeviceName();
            _os = dd.GetOs().Match.Name;
            _browser = dd.GetClient().Match.Name;

            return this;

        }
        public Click Build()
        {
            Click click = new Click();
            if (_id.HasValue) { click.ID = _id.Value; }
            click.LinkId = _linkId;
            click.Country = _country;
            click.Region = _region;
            click.City = _city;
            click.DeviceType = _deviceType;
            click.OS = _os;
            click.Browser = _browser;
            click.IpAddress = _ipAddress;
            return click;
        }


    }
}
