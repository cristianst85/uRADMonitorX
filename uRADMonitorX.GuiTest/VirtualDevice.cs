using System;
using System.Collections.Generic;
using uRADMonitorX.Commons.Networking;
using uRADMonitorX.Core.Device;

namespace uRADMonitorX.GuiTest
{
    public class VirtualDevice
    {
        public string DeviceId { get; private set; }

        public string DetectorName { get; private set; }

        public int FirmwareVersion { get; private set; }

        public int HardwareVersion { get; private set; }

        public DeviceModelType Model { get; private set; }

        public string IPAddress { get; private set; }

        public HttpStatus ServerResponseCode { get; set; }

        private DateTime? startTime;
        private volatile bool started = false;
        private readonly IList<DeviceReadings> readings;

        public VirtualDevice(string deviceId, RadiationDetector detector, int fwVersion, int hwVersion, DeviceModelType model, string ipAddress, ICollection<DeviceReadings> readings)
        {
            this.DeviceId = deviceId;
            this.DetectorName = detector.Name;
            this.FirmwareVersion = fwVersion;
            this.HardwareVersion = hwVersion;
            this.Model = model;
            this.IPAddress = ipAddress;
            this.ServerResponseCode = HttpStatus.OK;
            this.readings = new List<DeviceReadings>(readings);
        }

        public void Start()
        {
            if (this.started)
            {
                throw new Exception("Device already started.");
            }

            this.startTime = DateTime.Now;
            this.started = true;
        }

        public void Stop()
        {
            if (!this.started)
            {
                throw new Exception("Device already stopped.");
            }

            this.startTime = null;
            this.started = false;
        }

        public DeviceData ReadData()
        {
            if (!started)
            {
                throw new Exception("Device is not started.");
            }

            var deviceData = new DeviceData();
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

            var deviceReadings = this.readings[index];

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

            return deviceData;
        }
    }
}
