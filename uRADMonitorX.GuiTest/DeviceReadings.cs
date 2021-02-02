namespace uRADMonitorX.GuiTest
{
    public class DeviceReadings
    {
        public int Radiation { get; set; }

        public decimal? RadiationAverage { get; set; }

        public decimal Temperature { get; set; }

        public int? Pressure { get; set; }

        public int Voltage { get; set; }

        public int VoltagePercentage { get; set; }

        public DeviceReadings()
        {
        }
    }
}
