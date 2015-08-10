using System;

namespace uRADMonitorX.Core.Device {

    public class DeviceData {

        public DeviceInformation DeviceInformation { get; set; }
        public int Radiation { get; set; }
        public double RadiationAverage { get; set; }
        public double Temperature { get; set; }
        public int? Pressure { get; set; }
        public int Voltage { get; set; }
        public int VoltagePercent { get; set; }
        public int Uptime { get; set; }
        public int WDT { get; set; }
        public String IPAddress { get; set; }
        public String ServerIPAddress { get; set; }
        public String ServerResponseCode { get; set; }

        public DeviceData() {
            this.DeviceInformation = new DeviceInformation();
        }
    }
}
