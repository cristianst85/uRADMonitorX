using System;
using uRADMonitorX.Commons;
using uRADMonitorX.Configuration;
using uRADMonitorX.Core.Device;
using uRADMonitorX.Extensions;

namespace uRADMonitorX.Core
{
    public class DeviceDataHttpReaderFactory : IDeviceDataReaderFactory
    {
        private readonly ISettings settings;

        public DeviceDataHttpReaderFactory(ISettings settings)
        {
            if (settings.IsNull())
            {
                throw new ArgumentNullException("settings");
            }

            this.settings = settings;
        }

        public IDeviceDataReader Create()
        {
            return new DeviceDataHttpReader(new HttpClient(Program.UserAgent), this.settings.DeviceIPAddress);
        }
    }
}
