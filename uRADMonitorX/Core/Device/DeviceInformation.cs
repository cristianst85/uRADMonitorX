using System;

namespace uRADMonitorX.Core.Device {

    public class DeviceInformation {

        public String DeviceID { get; set; }
        public DeviceModel DeviceModel { get; set; }
        public int HardwareVersion { get; set; }
        public int FirmwareVersion { get; set; }
        public String Detector { get; set; }
    }
}
