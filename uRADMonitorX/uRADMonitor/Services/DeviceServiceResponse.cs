using System.Collections.Generic;
using uRADMonitorX.uRADMonitor.Domain;

namespace uRADMonitorX.uRADMonitor.Services
{
    public class DeviceServiceResponse
    {
        public ICollection<Device> Devices { get; }

        public DeviceServiceResponse()
        {
            this.Devices = new List<Device>();
        }

        public void SetError(string error)
        {
            this.HasError = true;
            this.Error = error;
        }

        public string Error { get; private set; }

        public bool HasError { get; private set; }
    }
}
