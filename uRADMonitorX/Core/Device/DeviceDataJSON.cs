using System;

namespace uRADMonitorX.Core.Device {

    public class DeviceDataJSON {

        public String DeviceID { get; set; }
        public DeviceType DeviceType { get; set; }
        public String Detector { get; set; }
        public int Radiation { get; set; }
        public int Temperature { get; set; }
        public int Pressure { get; set; }
        public int Uptime { get; set; }

        private DeviceDataJSON() {
        }
    }
}
