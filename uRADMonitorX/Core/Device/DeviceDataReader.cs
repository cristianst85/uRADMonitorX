using System;
using System.Globalization;

namespace uRADMonitorX.Core.Device {

    public abstract class DeviceDataReader {

        private NumberFormatInfo numberFormatInfo = new CultureInfo("en-US").NumberFormat;

        protected virtual DeviceData internalParse(String rawText) {
            DeviceData deviceData = new DeviceData();
            String[] tokens = rawText.Split(new String[] { "<br>", "<hr>", " ", "<b>", "</b>" }, StringSplitOptions.None);
            for (int i = 0; i < tokens.Length; i++) {
                String token = tokens[i].Trim();
                if (token.Length > 0) {
                    if (token.StartsWith("uRADMonitor", StringComparison.OrdinalIgnoreCase)) {
                        deviceData.DeviceInformation.DeviceID = tokens[i + 1];
                        i++;
                    }
                    else if (token.StartsWith("type", StringComparison.OrdinalIgnoreCase)) {
                        deviceData.DeviceInformation.DeviceModel = (DeviceModel)int.Parse(token.Split(':')[1]);
                    }
                    else if (token.StartsWith("hw", StringComparison.OrdinalIgnoreCase)) {
                        deviceData.DeviceInformation.HardwareVersion = int.Parse(token.Split(':')[1]);
                    }
                    else if (token.StartsWith("sw", StringComparison.OrdinalIgnoreCase)) {
                        deviceData.DeviceInformation.FirmwareVersion = int.Parse(token.Split(':')[1]);
                        deviceData.DeviceInformation.Detector = tokens[i + 1];
                        i++;
                    }
                    else if (token.StartsWith("radiation", StringComparison.OrdinalIgnoreCase)) {
                        String radiation = token.Split(':')[1];
                        deviceData.Radiation = int.Parse(radiation.Substring(0, radiation.IndexOf("CPM", StringComparison.OrdinalIgnoreCase)));
                    }
                    else if (token.StartsWith("average", StringComparison.OrdinalIgnoreCase)) {
                        String radiationAvg = token.Split(':')[1];
                        deviceData.RadiationAverage = double.Parse(radiationAvg.Substring(0, radiationAvg.IndexOf("CPM", StringComparison.OrdinalIgnoreCase)), NumberStyles.AllowDecimalPoint, numberFormatInfo);
                    }
                    else if (token.StartsWith("temp", StringComparison.OrdinalIgnoreCase)) {
                        String temperature = token.Split(':')[1];
                        deviceData.Temperature = double.Parse(temperature.Substring(0, temperature.IndexOf("C", StringComparison.OrdinalIgnoreCase)), NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, numberFormatInfo);
                    }
                    else if (token.StartsWith("P", StringComparison.OrdinalIgnoreCase)) {
                        String pressure = token.Split(':')[1];
                        deviceData.Pressure = int.Parse(pressure.Substring(0, pressure.IndexOf("Pa", StringComparison.OrdinalIgnoreCase)));
                    }
                    else if (token.StartsWith("voltage", StringComparison.OrdinalIgnoreCase)) {
                        String voltage = token.Split(':')[1];
                        deviceData.Voltage = int.Parse(voltage.Substring(0, voltage.IndexOf("V", StringComparison.OrdinalIgnoreCase)));
                        deviceData.VoltagePercent = int.Parse(tokens[i + 1].TrimStart('(').TrimEnd(')').TrimEnd('%'));
                        i++;
                    }
                    else if (token.StartsWith("uptime", StringComparison.OrdinalIgnoreCase)) {
                        String uptime = token.Split(':')[1];
                        deviceData.Uptime = int.Parse(uptime.Substring(0, uptime.IndexOf("s", StringComparison.OrdinalIgnoreCase)));
                    }
                    else if (token.StartsWith("wdt", StringComparison.OrdinalIgnoreCase)) {
                        String wdt = token.Split(':')[1];
                        deviceData.WDT = int.Parse(wdt.Substring(0, wdt.IndexOf("s", StringComparison.OrdinalIgnoreCase)));
                    }
                    else if (token.StartsWith("ip", StringComparison.OrdinalIgnoreCase)) {
                        deviceData.IPAddress = token.Split(':')[1];
                    }
                    else if (token.StartsWith("server", StringComparison.OrdinalIgnoreCase)) {
                        deviceData.ServerIPAddress = token.Split(':')[1];
                    }
                    else if (token.StartsWith("http", StringComparison.OrdinalIgnoreCase)) {
                        deviceData.ServerResponseCode = token.Split(':')[1];
                    }
                }
            }
            return deviceData;
        }
    }
}
