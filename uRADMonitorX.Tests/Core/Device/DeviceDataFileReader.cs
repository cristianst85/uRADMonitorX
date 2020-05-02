using System;
using System.IO;
using uRADMonitorX.Core.Device;
using uRADMonitorX.Extensions;

namespace uRADMonitorX.Tests.Core.Device
{
    public class DeviceDataFileReader : DeviceDataReader, IDeviceDataReader
    {
        public string FilePath { get; private set; }

        public DeviceDataFileReader(string filePath)
        {
            if (filePath.IsNullOrEmpty())
            {
                throw new ArgumentNullException("filePath");
            }

            this.FilePath = filePath;
        }

        public override DeviceData Read()
        {
            string fileContent = File.ReadAllText(this.FilePath);

            return this.Parse(fileContent);
        }
    }
}