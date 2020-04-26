using System.Net;

namespace uRADMonitorX.Helpers
{
    public static class ServicePointManagerHelper
    {
        public static void SetSecurityProtocolToTls12()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }
    }
}
