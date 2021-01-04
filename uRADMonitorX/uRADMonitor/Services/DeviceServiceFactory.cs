using uRADMonitorX.uRADMonitor.Domain;
using uRADMonitorX.uRADMonitor.Infrastructure;

namespace uRADMonitorX.uRADMonitor.Services
{
    public class DeviceServiceFactory : IDeviceServiceFactory
    {
        private readonly IDeviceFactory deviceFactory;
        private readonly IDeviceDataClientFactory deviceDataClientFactory;

        public DeviceServiceFactory(IDeviceFactory deviceFactory, IDeviceDataClientFactory deviceDataClientFactory)
        {
            this.deviceFactory = deviceFactory;
            this.deviceDataClientFactory = deviceDataClientFactory;
        }

        public IDeviceService Create(string userId, string userKey)
        {
            return new DeviceService(deviceDataClientFactory.Create(userId, userKey), deviceFactory);
        }
    }
}
