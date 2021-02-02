namespace uRADMonitorX.Core.Device
{
    public class DeviceData
    {
        public DeviceInformation DeviceInformation { get; set; }

        public int Radiation { get; set; }

        public decimal? RadiationAverage { get; set; }

        public decimal Temperature { get; set; }

        public decimal? Pressure { get; set; }

        public int Voltage { get; set; }

        public int VoltagePercentage { get; set; }

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
