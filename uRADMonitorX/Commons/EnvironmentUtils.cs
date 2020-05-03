using System;
using System.Reflection;
using uRADMonitorX.Windows;

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

        public static bool IsAtLeastWindowsVista()
        {
            return Environment.OSVersion.Version.Major >= 6;
        }

        public static bool IsAtLeastWindows10()
        {
            if (int.TryParse(RegistryUtils.GetCurrentMajorVersionNumber(), out int majorVersionNumber))
            {
                return majorVersionNumber >= 10;
            }

            return false;
        }
    }
}
