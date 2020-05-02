using System;
using System.Reflection;
using System.Windows.Forms;

namespace uRADMonitorX.Commons.Controls
{
    /// <summary>
    /// Contains a fix for the 63 character NotifyIcon ToolTip text limit problem in .NET Framework.
    /// </summary>
    public static class NotifyIconUtils
    {
        /// <summary>
        /// Sets the ToolTip text for the specified NotifyIcon up to 127 characters in length.
        /// </summary>
        /// <param name="notifyIcon"></param>
        /// <param name="text"></param>
        public static void SetText(NotifyIcon notifyIcon, String text)
        {
            if (notifyIcon == null)
            {
                throw new ArgumentNullException("notifyIcon");
            }

            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            if (text.Length >= 128)
            {
                throw new ArgumentOutOfRangeException("Text length must be less than 128 characters long.");
            }

            var t = typeof(NotifyIcon);
            var flags = BindingFlags.NonPublic | BindingFlags.Instance;

            t.GetField("text", flags).SetValue(notifyIcon, text);

            if (!EnvironmentUtils.IsMonoRuntime())
            {
                if ((bool)t.GetField("added", flags).GetValue(notifyIcon))
                {
                    t.GetMethod("UpdateIcon", flags).Invoke(notifyIcon, new object[] { true });
                }
            }
            else
            {
                t.GetMethod("Recalculate", flags).Invoke(notifyIcon, null);
            }
        }
    }
}
