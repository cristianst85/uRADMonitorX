using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace uRADMonitorX.Extensions
{
    public static class PointExtensions
    {
        public static bool IsOnScreenWorkingArea(this Point topLeft, int atLeast = 0)
        {
            return Screen.AllScreens.Any(s => s.WorkingArea.Contains(new Point(topLeft.X + atLeast, topLeft.Y + atLeast)));
        }
    }
}
