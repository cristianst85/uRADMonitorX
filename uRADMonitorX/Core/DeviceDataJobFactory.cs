using System;
using System.Linq;
using uRADMonitorX.Configuration;
using uRADMonitorX.Core.Jobs;
using uRADMonitorX.Extensions;

namespace uRADMonitorX.Core
{
    public class DeviceDataJobFactory : IDeviceDataJobFactory
    {
        private readonly ISettings settings;
        private readonly IDeviceDataReaderFactory deviceDataReaderFactory;

        public DeviceDataJobFactory(ISettings settings, IDeviceDataReaderFactory deviceDataReaderFactory)
        {
            if (settings.IsNull())
            {
                throw new ArgumentNullException("settings");
            }

            if (deviceDataReaderFactory.IsNull())
            {
                throw new ArgumentNullException("deviceDataReaderFactory");
            }

            this.settings = settings;
            this.deviceDataReaderFactory = deviceDataReaderFactory;
        }

        public IDeviceDataJob Create()
        {
            IPollingStrategy pollingStrategy;

            var pollingSettings = this.settings.Devices.First().Polling;

            if (pollingSettings.Type == PollingType.WDTSync)
            {
                pollingStrategy = new WDTSyncPollingStrategy(WDTSyncPollingStrategy.DefaultWDTInterval);
            }
            else
            {
                pollingStrategy = new FixedIntervalPollingStrategy(pollingSettings.Interval.Value);
            }

            var deviceDataReader = this.deviceDataReaderFactory.Create();

            return new DeviceDataJob(deviceDataReader, pollingStrategy);
        }
    }
}
