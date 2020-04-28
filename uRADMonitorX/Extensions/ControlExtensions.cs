using System.Windows.Forms;

namespace uRADMonitorX.Extensions
{
    public static class ControlExtensions
    {
        public static bool HasText(this Control control)
        {
            return control.Text.Length > 0;
        }
    }
}
