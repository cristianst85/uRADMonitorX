using System;
using System.Text.RegularExpressions;

namespace uRADMonitorX.Commons.Networking
{
    public class IPEndPoint
    {
        private static readonly string IpEndPointPattern = @"^(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(\.(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)){3}:[0-9]+$";

        public string IPAddress { get; private set; }

        public int Port { get; private set; }

        private IPEndPoint(string ipAddress, int port)
        {
            this.IPAddress = ipAddress;
            this.Port = port;
        }

        public IPEndPoint Parse(string ipEndPoint)
        {
            throw new NotImplementedException();
        }

        public bool TryParse(string ipEndPoint, out IPEndPoint ipEndPointObj)
        {
            throw new NotImplementedException();
        }

        public static bool IsValidFormat(string ipEndPoint)
        {
            if (ipEndPoint == null)
            {
                throw new ArgumentNullException("ipEndPoint");
            }

            bool isRegexMatch = CreateRegex(IpEndPointPattern).IsMatch(ipEndPoint);

            if (!isRegexMatch)
            {
                return false;
            }

            int port = int.Parse(ipEndPoint.Substring(ipEndPoint.IndexOf(':') + 1));

            return isRegexMatch && IsValidPortNumber(port);
        }

        private static bool IsValidPortNumber(int portNumber)
        {
            return (portNumber > 0 && portNumber <= 65535);
        }

        private static Regex CreateRegex(string pattern)
        {
            return new Regex(pattern, RegexOptions.IgnoreCase);
        }
    }
}
