﻿using Microsoft.Win32;

namespace uRADMonitorX.Windows
{
    public static class RegistryUtils
    {
        public static void RegisterAtWindowsStartup(string applicationName, string applicationPath)
        {
            using (var registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                registryKey.SetValue(applicationName, applicationPath);
                registryKey.Flush();
            }
        }

        public static void UnRegisterAtWindowsStartup(string applicationName)
        {
            using (var registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                registryKey.DeleteValue(applicationName, false);
                registryKey.Flush();
            }
        }

        public static string GetCurrentMajorVersionNumber()
        {
            using (var registryKey = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", false))
            {
                return registryKey.GetValue("CurrentMajorVersionNumber", string.Empty).ToString();
            }
        }
    }
}
