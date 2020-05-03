﻿using System.Runtime.InteropServices;

namespace uRADMonitorX.Windows
{
    internal static class NativeMethods
    {
        internal enum WM
        {
            SYSCOMMAND = 0x0112
        };

        internal enum SC
        {
            MINIMIZE = 0xF020
        }

        [DllImport("user32.dll")]
        internal static extern bool SetProcessDPIAware();

        [DllImport("ShCore.dll")]
        internal static extern int SetProcessDpiAwareness([MarshalAs(UnmanagedType.U4)] ProcessDpiAwareness processDpiAwareness);

        internal enum ProcessDpiAwareness : uint
        {
            /// <summary>
            /// DPI unaware. This app does not scale for DPI changes and is always assumed
            /// to have a scale factor of 100% (96 DPI). It will be automatically scaled
            /// by the system on any other DPI setting.
            /// </summary>
            ProcessDpiUnaware = 0,

            /// <summary>
            /// System DPI aware. This app does not scale for DPI changes. It will query
            /// for the DPI once and use that value for the lifetime of the app. If the 
            /// DPI changes, the app will not adjust to the new DPI value. It will be
            /// automatically scaled up or down by the system when the DPI changes
            /// from the system value.
            /// </summary>
            ProcessSystemDpiAware = 1,

            /// <summary>
            /// Per monitor DPI aware. This app checks for the DPI when it is created and
            /// adjusts the scale factor whenever the DPI changes. These applications
            /// are not automatically scaled by the system.
            /// </summary>
            ProcessPerMonitorDpiAware = 2
        }
    }
}
