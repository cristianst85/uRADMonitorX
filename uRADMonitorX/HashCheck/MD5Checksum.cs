using System;
using uRADMonitorX.Commons;

namespace HashCheck {
    
    public class MD5Checksum : IChecksum {

        public const int Length = 32;

        public String Value { get; private set; }

        public static MD5Checksum FromFile(String filePath) {
            return new MD5Checksum(ChecksumUtils.ComputeMD5(filePath));
        }

        public static MD5Checksum Parse(String value) {
            // TODO: regex ?!
            if (value.Length != MD5Checksum.Length) {
                throw new Exception("Invalid checksum length.");
            }
            return new MD5Checksum(value.ToLower());
        }

        private MD5Checksum(String value) {
            this.Value = value;
        }

        public bool Verify(String filePath) {
            String newChecksum = ChecksumUtils.ComputeMD5(filePath);
            return newChecksum.Equals(this.Value, StringComparison.OrdinalIgnoreCase);
        }
    }
}
