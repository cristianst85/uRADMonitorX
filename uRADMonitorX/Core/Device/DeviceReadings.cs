namespace uRADMonitorX.Core.Device {
    
    public class DeviceReadings {

        public int Radiation { get; set; }
        public double? RadiationAverage { get; set; }
        public double Temperature { get; set; }
        public int? Pressure { get; set; }
        public int Voltage { get; set; }
        public int VoltagePercent { get; set; }

        public DeviceReadings() {
        }
    }
}
