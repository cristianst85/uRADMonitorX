using System;
using System.Globalization;

namespace uRADMonitorX.Core.Device
{
    public abstract class DeviceDataReader : IDeviceDataReader
    {
        private readonly NumberFormatInfo numberFormatInfo = new CultureInfo("en-US").NumberFormat;
        private readonly string[] separators = new string[] { "<br>", "<hr>", " ", "<b>", "</b>" };

        public abstract DeviceData Read();

        protected virtual DeviceData Parse(string content)
        {
            var deviceData = new DeviceData();

            string[] tokens = content.Split(separators, StringSplitOptions.None);

            for (int i = 0; i < tokens.Length; i++)
            {
                string token = tokens[i].Trim();

                if (token.Length > 0)
                {
                    if (token.StartsWith("uRADMonitor", StringComparison.OrdinalIgnoreCase))
                    {
                        deviceData.DeviceInformation.DeviceID = tokens[i + 1];

                        i++;
                    }
                    else if (token.StartsWith("type", StringComparison.OrdinalIgnoreCase))
                    {
                        deviceData.DeviceInformation.DeviceModel = (DeviceModelType)int.Parse(token.Split(':')[1]);
                    }
                    else if (token.StartsWith("hw", StringComparison.OrdinalIgnoreCase))
                    {
                        deviceData.DeviceInformation.HardwareVersion = int.Parse(token.Split(':')[1]);
                    }
                    else if (token.StartsWith("sw", StringComparison.OrdinalIgnoreCase))
                    {
                        deviceData.DeviceInformation.FirmwareVersion = int.Parse(token.Split(':')[1]);
                        deviceData.DeviceInformation.Detector = tokens[i + 1];

                        i++;
                    }
                    else if (token.StartsWith("radiation", StringComparison.OrdinalIgnoreCase) || token.StartsWith("rad:", StringComparison.OrdinalIgnoreCase))
                    {
                        string radiation = token.Split(':')[1];
                        deviceData.Radiation = int.Parse(radiation.Substring(0, radiation.IndexOf("CPM", StringComparison.OrdinalIgnoreCase)));

                        if (tokens[i + 1].Contains("("))
                        {
                            string radiationAvg = tokens[++i];
                            deviceData.RadiationAverage = decimal.Parse(radiationAvg.Substring(1, radiationAvg.IndexOf("CPM", StringComparison.OrdinalIgnoreCase) - 1), NumberStyles.AllowDecimalPoint, numberFormatInfo);
                        }
                    }
                    else if (token.StartsWith("average", StringComparison.OrdinalIgnoreCase))
                    {
                        string radiationAvg = token.Split(':')[1];
                        deviceData.RadiationAverage = decimal.Parse(radiationAvg.Substring(0, radiationAvg.IndexOf("CPM", StringComparison.OrdinalIgnoreCase)), NumberStyles.AllowDecimalPoint, numberFormatInfo);
                    }
                    else if (token.StartsWith("temp", StringComparison.OrdinalIgnoreCase) || token.StartsWith("t:", StringComparison.OrdinalIgnoreCase))
                    {
                        string temperature = token.Split(':')[1];
                        deviceData.Temperature = decimal.Parse(temperature.Substring(0, temperature.IndexOf("C", StringComparison.OrdinalIgnoreCase)), NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, numberFormatInfo);
                    }
                    else if (token.StartsWith("P", StringComparison.OrdinalIgnoreCase) && token.EndsWith("Pa", StringComparison.OrdinalIgnoreCase))
                    {
                        string pressure = token.Split(':')[1];
                        deviceData.Pressure = decimal.Parse(pressure.Substring(0, pressure.IndexOf("Pa", StringComparison.OrdinalIgnoreCase)), NumberStyles.AllowDecimalPoint, numberFormatInfo);
                    }
                    else if (token.StartsWith("vol", StringComparison.OrdinalIgnoreCase))
                    {
                        string voltage = token.Split(':')[1];
                        deviceData.Voltage = int.Parse(voltage.Substring(0, voltage.IndexOf("V", StringComparison.OrdinalIgnoreCase)));

                        if (tokens[i + 1].Contains("/"))
                        {
                            deviceData.VoltagePercentage = int.Parse(tokens[i + 1].TrimStart('(').TrimEnd(')').Split('/')[0].TrimEnd('%'));
                        }
                        else
                        {
                            if (tokens[i + 1].StartsWith("duty"))
                            {
                                deviceData.VoltagePercentage = int.Parse(tokens[i + 1].Split(':')[1].TrimEnd('%'));
                            }
                            else
                            {
                                deviceData.VoltagePercentage = int.Parse(tokens[i + 1].TrimStart('(').TrimEnd(')').TrimEnd('%'));
                            }
                        }

                        i++;
                    }
                    else if (token.StartsWith("uptime", StringComparison.OrdinalIgnoreCase) || token.StartsWith("UP:", StringComparison.OrdinalIgnoreCase))
                    {
                        string uptime = token.Split(':')[1];
                        deviceData.Uptime = int.Parse(uptime.Substring(0, uptime.IndexOf("s", StringComparison.OrdinalIgnoreCase)));
                    }
                    else if (token.StartsWith("wdt", StringComparison.OrdinalIgnoreCase))
                    {
                        string wdt = token.Split(':')[1];
                        deviceData.WDT = int.Parse(wdt.Substring(0, wdt.IndexOf("s", StringComparison.OrdinalIgnoreCase)));
                    }
                    else if (token.StartsWith("ip", StringComparison.OrdinalIgnoreCase))
                    {
                        deviceData.IPAddress = token.Split(':')[1];
                    }
                    else if (token.StartsWith("server", StringComparison.OrdinalIgnoreCase))
                    {
                        deviceData.ServerIPAddress = token.Split(':')[1];
                    }
                    else if (token.StartsWith("eth", StringComparison.OrdinalIgnoreCase))
                    {
                        deviceData.IPAddress = token.Split(':')[1];
                        deviceData.ServerIPAddress = tokens[++i].TrimStart('(').TrimEnd(')');
                    }
                    else if (token.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                    {
                        deviceData.ServerResponseCode = token.Split(':')[1];
                    }
                }
            }

            return deviceData;
        }
    }
}
