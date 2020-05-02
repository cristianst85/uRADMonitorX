using System;
using System.Diagnostics;
using uRADMonitorX.Core.Device;

namespace uRADMonitorX.GuiTest
{
    public class DeviceDataVirtualReader : IDeviceDataReader
    {
        private readonly VirtualDevice device;

        public DeviceDataVirtualReader(VirtualDevice device)
        {
            this.device = device;
        }

        public DeviceData Read()
        {
            try
            {
                return this.device.ReadData();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());

                throw;
            }
        }
    }
}
