using System;
using System.Reflection;

namespace uRADMonitorX.Commons {

    public static class EnvironmentUtils {

        public static Boolean IsMonoRuntime() {
            return Type.GetType("Mono.Runtime") != null;
        }

        public static String GetMonoRuntimeVersion() {
            Type type = Type.GetType("Mono.Runtime");
            if (type != null) {
                MethodInfo displayName = type.GetMethod("GetDisplayName", BindingFlags.NonPublic | BindingFlags.Static);
                if (displayName != null) {
                    return String.Format("{0}", displayName.Invoke(null, null));
                }
                else {
                    return null;
                }
            }
            else {
                return null;
            }
        }

        public static bool IsUnix() {
            int pId = (int)Environment.OSVersion.Platform;
            return ((pId == 4) || (pId == 6) || (pId == 128));
        }
    }
}
