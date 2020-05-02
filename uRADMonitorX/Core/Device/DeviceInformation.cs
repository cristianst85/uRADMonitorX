namespace uRADMonitorX.Core.Device
{
    public class DeviceInformation
    {
        public string DeviceID { get; set; }

        public DeviceModelType DeviceModel { get; set; }

        public int HardwareVersion { get; set; }

        public int FirmwareVersion { get; set; }

        public string Detector { get; set; }
    }
}
