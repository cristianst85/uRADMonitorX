using uRADMonitorX.Commons;
using uRADMonitorX.uRADMonitor.Configuration;

namespace uRADMonitorX.uRADMonitor.Infrastructure
{
    public class DeviceDataHttpClientFactory : IDeviceDataClientFactory
    {
        private readonly IDeviceDataClientConfiguration deviceDataClientConfiguration;
        private readonly IHttpClientFactory httpClientFactory;

        public DeviceDataHttpClientFactory(IDeviceDataClientConfiguration deviceDataClientConfiguration, IHttpClientFactory httpClientFactory)
        {
            this.deviceDataClientConfiguration = deviceDataClientConfiguration;
            this.httpClientFactory = httpClientFactory;
        }

        public IDeviceDataClient Create(string userId, string userKey)
        {
            return new DeviceDataHttpClient(deviceDataClientConfiguration, httpClientFactory.Create(userId, userKey));
        }
    }
}
