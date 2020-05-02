namespace uRADMonitorX.Core.Device
{
    public class DeviceData
    {
        public DeviceInformation DeviceInformation { get; set; }

        public int Radiation { get; set; }

        public double? RadiationAverage { get; set; }

        public double Temperature { get; set; }

        public double? Pressure { get; set; }

        public int Voltage { get; set; }

        public int VoltagePercent { get; set; }

        public int Uptime { get; set; }

        public int WDT { get; set; }

        public string IPAddress { get; set; }

        public string ServerIPAddress { get; set; }

        public string ServerResponseCode { get; set; }

        public DeviceData()
        {
            this.DeviceInformation = new DeviceInformation();
        }
    }
}
