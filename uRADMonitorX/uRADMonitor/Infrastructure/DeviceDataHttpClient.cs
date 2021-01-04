using uRADMonitorX.Commons;
using uRADMonitorX.uRADMonitor.Configuration;

namespace uRADMonitorX.uRADMonitor.Infrastructure
{
    public class DeviceDataHttpClient : IDeviceDataClient
    {
        private readonly IDeviceDataClientConfiguration deviceDataClientConfiguration;
        private readonly IHttpClient httpClient;

        public DeviceDataHttpClient(IDeviceDataClientConfiguration deviceDataClientConfiguration, IHttpClient httpClient)
        {
            this.deviceDataClientConfiguration = deviceDataClientConfiguration;
            this.httpClient = httpClient;
        }

        public string Get()
        {
            return this.httpClient.Get(deviceDataClientConfiguration.GetEndpointUrl());
        }
    }
}
