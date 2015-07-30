using System;
using System.IO;

namespace uRADMonitorX.Core.Device {

    public class DeviceDataFileReader : DeviceDataReader, IDeviceDataReader {

        public String FilePath { get; private set; }

        public DeviceDataFileReader(String filePath) {

            if (String.IsNullOrEmpty(filePath)) {
                throw new ArgumentNullException("filePath");
            }

            this.FilePath = filePath;
        }

        public DeviceData Read() {
            String fileContent = File.ReadAllText(this.FilePath);
            return this.internalParse(fileContent);
        }
    }
}
