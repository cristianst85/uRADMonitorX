using System;
using System.Reflection;

namespace uRADMonitorX.Commons
{
    public static class EnvironmentUtils
    {
        public static bool IsMonoRuntime()
        {
            return Type.GetType("Mono.Runtime") != null;
        }

        public static string GetMonoRuntimeVersion()
        {
            var type = Type.GetType("Mono.Runtime");

            if (type == null)
            {
                return null;
            }

            var displayName = type.GetMethod("GetDisplayName", BindingFlags.NonPublic | BindingFlags.Static);

            if (displayName == null)
            {
                return null;
            }

            return string.Format("{0}", displayName.Invoke(null, null));
        }

        public static bool IsUnix()
        {
            var pId = (int)Environment.OSVersion.Platform;

            return ((pId == 4) || (pId == 6) || (pId == 128));
        }
    }
}
