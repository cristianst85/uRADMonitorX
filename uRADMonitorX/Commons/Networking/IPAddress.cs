using System;
using System.Text.RegularExpressions;

namespace uRADMonitorX.Commons.Networking {

    public class IPAddress {

        private static String ipAddressPattern = @"^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3}$";

        public String Value { get; private set; }

        private IPAddress(String ipAddress) {
            this.Value = ipAddress;
        }

        public static IPAddress Parse(String ipAddress) {
            if (IsValidFormat(ipAddress)) {
                return new IPAddress(ipAddress);
            }
            else {
                throw new FormatException("Invalid IP address format.");
            }
        }

        public static bool TryParse(String ipAddress, out IPAddress ipAddressObj) {
            if (IsValidFormat(ipAddress)) {
                ipAddressObj = new IPAddress(ipAddress);
                return true;
            }
            else {
                ipAddressObj = null;
                return false;
            }
        }

        public static bool IsValidFormat(String ipAddress) {
            if (ipAddress == null) {
                throw new ArgumentNullException("ipAddress");
            }
            return createRegex(ipAddressPattern).IsMatch(ipAddress);
        }

        private static Regex createRegex(String pattern) {
            return new Regex(pattern, RegexOptions.IgnoreCase);
        }
    }
}
