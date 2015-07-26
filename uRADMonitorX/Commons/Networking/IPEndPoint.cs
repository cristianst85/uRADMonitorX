using System;
using System.Text.RegularExpressions;

namespace uRADMonitorX.Commons.Networking {

    public class IPEndPoint {

        private static String ipEndPointPattern = @"^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3}:[0-9]+$";

        public String IPAddress { get; private set; }
        public int Port { get; private set; }

        private IPEndPoint(String ipAddress, int port) {
            this.IPAddress = ipAddress;
            this.Port = port;
        }

        public IPEndPoint Parse(String ipEndPoint) {
            throw new NotImplementedException();
        }

        public bool TryParse(String ipEndPoint, out IPEndPoint ipEndPointObj) {
            throw new NotImplementedException();
        }

        public static bool IsValidFormat(String ipEndPoint) {
            if (ipEndPoint == null) {
                throw new ArgumentNullException("ipEndPoint");
            }

            bool isRegexMatch = createRegex(ipEndPointPattern).IsMatch(ipEndPoint);

            if (!isRegexMatch) {
                return false;
            }
            else {
                int port = int.Parse(ipEndPoint.Substring(ipEndPoint.IndexOf(':') + 1));
                return isRegexMatch && isValidPortNumber(port);
            }
        }

        private static bool isValidPortNumber(int portNumber) {
            return (portNumber > 0 && portNumber <= 65535);
        }

        private static Regex createRegex(String pattern) {
            return new Regex(pattern, RegexOptions.IgnoreCase);
        }
    }
}
