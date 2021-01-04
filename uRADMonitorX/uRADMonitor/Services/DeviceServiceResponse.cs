using System.Collections.Generic;
using uRADMonitorX.Extensions;
using uRADMonitorX.uRADMonitor.Domain;

namespace uRADMonitorX.uRADMonitor.Services
{
    public class DeviceServiceResponse
    {
        public ICollection<Device> Devices { get; set; }

        public string Error { get; set; }

        public bool HasData
        {
            get
            {
                return this.Error.IsNullOrEmpty();
            }
        }
    }
}
