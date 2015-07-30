using System;

namespace uRADMonitorX.Commons {

    public static class AssemblyUtils {

        public static String GetApplicationPath() {
            return new Uri(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath;
        }

        public static Version GetVersion() {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Reflection.AssemblyName assemblyName = assembly.GetName();
            return assemblyName.Version;
        }
    }
}
