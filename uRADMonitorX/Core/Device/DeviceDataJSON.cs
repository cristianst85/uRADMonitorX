using System;

namespace uRADMonitorX.Core.Device {

    public class DeviceDataJSON {

        public String Id { get; set; }
        public DeviceModelType Type { get; set; }
        public String Detector { get; set; }
        public int Cpm { get; set; }
        public int Temperature { get; set; }
        public int Pressure { get; set; }
        public int Uptime { get; set; }

        private DeviceDataJSON() {
        }
    }
}
