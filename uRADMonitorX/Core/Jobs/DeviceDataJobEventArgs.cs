using System;
using uRADMonitorX.Core.Device;

namespace uRADMonitorX.Core.Jobs
{
    public class DeviceDataJobEventArgs : EventArgs
    {
        public DeviceData DeviceData { get; private set; }

        public DeviceDataJobEventArgs(DeviceData deviceData)
        {
            this.DeviceData = deviceData;
        }
    }
}
