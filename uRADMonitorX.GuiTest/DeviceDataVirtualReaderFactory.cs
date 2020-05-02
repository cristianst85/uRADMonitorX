using uRADMonitorX.Core;
using uRADMonitorX.Core.Device;

namespace uRADMonitorX.GuiTest
{
    public class DeviceDataVirtualReaderFactory : IDeviceDataReaderFactory
    {
        private readonly VirtualDevice device;

        public DeviceDataVirtualReaderFactory(VirtualDevice device)
        {
            this.device = device;
        }

        public IDeviceDataReader Create()
        {
            return new DeviceDataVirtualReader(device);
        }
    }
}
