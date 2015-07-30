using System;
using uRADMonitorX.Core.Device;

namespace uRADMonitorX.Core.Fetchers {

    public class DeviceDataFetcherEventArgs : EventArgs {

        public DeviceData DeviceData { get; private set; }

        public DeviceDataFetcherEventArgs(DeviceData deviceData) {
            this.DeviceData = deviceData;
        }
    }
}
