using System;

namespace HashCheck {

    public class ChecksumEntry {

        public String FileName { get; private set; }
        public IChecksum Checksum { get; private set; }

        public ChecksumEntry(String fileName, IChecksum checksum) {
            this.FileName = fileName;
            this.Checksum = checksum;
        }
    }
}
