using uRADMonitorX.uRADMonitor.Configuration;

namespace uRADMonitorX.Configuration
{
    public class DeviceDataClientConfiguration : HttpClientConfiguration, IDeviceDataClientConfiguration
    {
        public string EndpointUrl { get; set; } 

        public DeviceDataClientConfiguration() : base()
        {
            this.EndpointUrl = Program.Settings.uRADMonitorEndpointUrl;
        }

        public virtual string GetEndpointUrl()
        {
            return this.EndpointUrl;
        }
    }
}
