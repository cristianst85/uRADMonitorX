using System;
using uRADMonitorX.Configuration;
using uRADMonitorX.Core.Device;

namespace uRADMonitorX.Core {

    public class DeviceDataHttpReaderFactory : IDeviceDataReaderFactory {

        private ISettings settings;

        public DeviceDataHttpReaderFactory(ISettings settings) {

            if (settings == null) {
                throw new ArgumentNullException("settings");
            }

            this.settings = settings;
        }

        public IDeviceDataReader Create() {
            return new DeviceDataHttpReader(this.settings.DeviceIPAddress);
        }
    }
}
