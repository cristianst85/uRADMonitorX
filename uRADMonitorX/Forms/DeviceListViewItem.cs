using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Windows.Forms;
using uRADMonitorX.uRADMonitor.Domain;

namespace uRADMonitorX.Forms
{
    [Serializable]
    public class DeviceListViewItem : ListViewItem
    {
        public Device Device { get; private set; }

        public DeviceListViewItem(Device device)
            : base(new List<string>()
            {
                device.Id,
                device.GetLocation(),
                device.GetStatus()
            }.ToArray())
        {
            this.Device = device;
        }

        protected DeviceListViewItem(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
