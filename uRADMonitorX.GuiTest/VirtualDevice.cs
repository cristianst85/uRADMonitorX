using System;
using System.Collections.Generic;
using uRADMonitorX.Commons;
using uRADMonitorX.Commons.Networking;
using uRADMonitorX.Core.Device;

namespace uRADMonitorX.GuiTest {

    public class VirtualDevice {

        public String DeviceId { get; private set; }
        public String DetectorName { get; private set; }
        public int FirmwareVersion { get; private set; }
        public int HardwareVersion { get; private set; }
        public DeviceModelType Model { get; private set; }

        private IList<DeviceReadings> readings;

        public String IPAddress { get; private set; }
        public String ServerIPAddress { get; private set; }

        private DateTime? startTime;
        private volatile bool started = false;

        public HttpStatus ServerResponseCode { get; set; }

        public VirtualDevice(String deviceId, RadiationDetector detector, int fwVersion, int hwVersion, DeviceModelType model, String ipAddress, String serverIpAddres, ICollection<DeviceReadings> readings) {
            this.DeviceId = deviceId;
            this.DetectorName = detector.Name;
            this.FirmwareVersion = fwVersion;
            this.HardwareVersion = hwVersion;
            this.Model = model;
            this.IPAddress = ipAddress;
            this.ServerIPAddress = serverIpAddres;
            this.readings = new List<DeviceReadings>(readings);
            this.ServerResponseCode = HttpStatus.OK;
        }

        public void Start() {
            if (this.started) {
                throw new Exception("Device already started.");
            }

            this.startTime = DateTime.Now;
            this.started = true;
        }

        public void Stop() {

            if (!this.started) {
                throw new Exception("Device already stopped.");
            }

            this.startTime = null;
            this.started = false;
        }

        public DeviceData ReadData() {

            if (!started) {
                throw new Exception("Device is not started.");
            }

            DeviceData deviceData = new DeviceData();
            deviceData.DeviceInformation.DeviceID = this.DeviceId;
            deviceData.DeviceInformation.Detector = this.DetectorName;
            deviceData.DeviceInformation.FirmwareVersion = this.FirmwareVersion;
            deviceData.DeviceInformation.HardwareVersion = this.HardwareVersion;
            deviceData.DeviceInformation.DeviceModel = this.Model;

            int secondsElapsedFromStartTime = (int)DateTime.Now.Subtract(this.startTime.Value).TotalSeconds;
           
            //  0s ...  59s  1st reading
            // 60s ... 119s  2nd reading
            // .........................
            // ............  Nth reading
            int index = (secondsElapsedFromStartTime / 60) % this.readings.Count;

            DeviceReadings deviceReadings = this.readings[index];

            deviceData.Temperature = deviceReadings.Temperature;
            deviceData.Radiation = deviceReadings.Radiation;
            deviceData.RadiationAverage = deviceReadings.RadiationAverage;
            deviceData.Pressure = deviceReadings.Pressure;
            deviceData.Voltage = deviceReadings.Voltage;
            deviceData.VoltagePercent = deviceReadings.VoltagePercent;
            deviceData.Uptime = secondsElapsedFromStartTime;
            deviceData.WDT = secondsElapsedFromStartTime % 60;
            deviceData.ServerResponseCode = this.ServerResponseCode.Code.ToString();
            deviceData.IPAddress = this.IPAddress;
            deviceData.ServerIPAddress = this.ServerIPAddress;

            return deviceData;
        }
    }
}
