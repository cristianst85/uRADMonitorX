using System.ComponentModel;
using System.Windows.Forms;

namespace uRADMonitorX.Extensions
{
    public static class ControlExtensions
    {
        public static bool HasText(this Control control)
        {
            return control.Text.Length > 0;
        }

        public static void InvokeIfRequired(this Control control, MethodInvoker action)
        {
            ((ISynchronizeInvoke)control).InvokeIfRequired(action);
        }

        public static void InvokeIfRequired(this ISynchronizeInvoke obj, MethodInvoker action)
        {
            if (obj.InvokeRequired)
            {
                obj.Invoke(action, new object[0]);
            }
            else
            {
                action();
            }
        }
    }
}
