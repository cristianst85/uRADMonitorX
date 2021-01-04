using uRADMonitorX.uRADMonitor.Configuration;

namespace uRADMonitorX.Configuration
{
    public class DeviceDataClientConfiguration : IDeviceDataClientConfiguration
    {
        public string GetEndpointUrl()
        {
            return Program.Settings.uRADMonitorEndpointUrl;
        }

        public string GetUserAgent()
        {
            return Program.Settings.UserAgent;
        }
    }
}
