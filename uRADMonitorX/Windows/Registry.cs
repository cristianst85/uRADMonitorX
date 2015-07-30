using System;
using Microsoft.Win32;

namespace uRADMonitorX.Windows {

    public static class Registry {

        public static void RegisterAtWindowsStartup(String applicationName, String applicationPath) {
            using (RegistryKey regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true)) {
                regKey.SetValue(applicationName, applicationPath);
                regKey.Flush();
            }
        }

        public static void UnRegisterAtWindowsStartup(String applicationName) {
            using (RegistryKey regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true)) {
                regKey.DeleteValue(applicationName, false);
                regKey.Flush();
            }
        }
    }
}
