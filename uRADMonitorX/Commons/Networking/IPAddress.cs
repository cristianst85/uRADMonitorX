using System;
using System.Text.RegularExpressions;

namespace uRADMonitorX.Commons.Networking
{
    public class IPAddress
    {
        private static readonly string IpAddressPattern = @"^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3}$";

        public string Value { get; private set; }

        private IPAddress(string ipAddress)
        {
            this.Value = ipAddress;
        }

        public static IPAddress Parse(string ipAddress)
        {
            if (IsValidFormat(ipAddress))
            {
                return new IPAddress(ipAddress);
            }

            throw new FormatException("Invalid IP address format.");
        }

        public static bool TryParse(string ipAddress, out IPAddress ipAddressObj)
        {
            if (IsValidFormat(ipAddress))
            {
                ipAddressObj = new IPAddress(ipAddress);

                return true;
            }

            ipAddressObj = null;

            return false;
        }

        public static bool IsValidFormat(string ipAddress)
        {
            if (ipAddress == null)
            {
                throw new ArgumentNullException("ipAddress");
            }

            return CreateRegex(IpAddressPattern).IsMatch(ipAddress);
        }

        private static Regex CreateRegex(String pattern)
        {
            return new Regex(pattern, RegexOptions.IgnoreCase);
        }
    }
}

