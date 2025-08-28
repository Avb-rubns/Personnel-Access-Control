using DeviceDetectorNET;
using Microsoft.Extensions.Primitives;

namespace Personnel.Client.Shared.Builder
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
        public ClickDTO Build()
        {
            ClickDTO clickDTO = new ClickDTO();
            if (_id.HasValue) { clickDTO.ID = _id.Value; }
            clickDTO.LinkId = _linkId;
            clickDTO.Country = _country;
            clickDTO.Region = _region;
            clickDTO.City = _city;
            clickDTO.DeviceType = _deviceType;
            clickDTO.OS = _os;
            clickDTO.Browser = _browser;
            clickDTO.IpAddress = _ipAddress;
            return clickDTO;
        }


    }
}
