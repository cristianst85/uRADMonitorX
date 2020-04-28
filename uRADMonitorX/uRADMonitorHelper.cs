using System.Diagnostics;

namespace uRADMonitorX
{
    public static class uRADMonitorHelper
    {
        public static void OpenDashboardUrl(string deviceId)
        {
            // Let server to redirect if HTTPS is supported.
            Process.Start(string.Format("http://uradmonitor.com/tools/dashboard-04/?open={0}", deviceId));
        }

        public static void OpenGraphUrl(string deviceId, string sensorDataType)
        {
            // Let server to redirect if HTTPS is supported.
            Process.Start(string.Format("http://uradmonitor.com?open={0}&sensor={1}", deviceId, sensorDataType));
        }

        public static void OpenAPIUrl(string deviceId, string sensorDataType)
        {
            // Let server to redirect if HTTPS is supported.
            Process.Start(string.Format("http://data.uradmonitor.com/api/v1/devices/{0}/{1}", deviceId, sensorDataType));
        }
    }
}
