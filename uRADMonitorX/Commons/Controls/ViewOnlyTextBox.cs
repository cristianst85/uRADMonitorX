using System.Security.Permissions;
using System.Windows.Forms;

namespace uRADMonitorX.Commons.Controls {

    public class ViewOnlyTextBox : System.Windows.Forms.TextBox {

        const int WM_SETFOCUS = 0x0007;
        const int WM_KILLFOCUS = 0x0008;
        const int WM_SETCURSOR = 0x0020;
        const int WM_MOUSEHOVER = 0x02A1;
        const int WM_NCMOUSEHOVER = 0x02A2;
        const int WM_NCMOUSEMOVE = 0x00A0;
        const int WM_MOUSEMOVE = 0x0200;

        public ViewOnlyTextBox()
            : base() {
            base.Enabled = true;
            base.ReadOnly = true;
            base.BorderStyle = BorderStyle.Fixed3D;
            base.ContextMenu = new ContextMenu(); // Disable context menu on right click.
            base.ShortcutsEnabled = false;
        }

        public new bool Enabled {
            get {
                return base.Enabled;
            }
            set {
                ; // nothing;
            }
        }

        public new bool ReadOnly {
            get {
                return base.ReadOnly;
            }
            set {
                ; // nothing;
            }
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m) {
            if (m.Msg == WM_SETFOCUS) {
                m.Msg = WM_KILLFOCUS;
            }

            if (m.Msg == WM_SETCURSOR) {
                return;
            }

            // Disable border highlight.
            if (m.Msg == WM_MOUSEHOVER || m.Msg == WM_NCMOUSEHOVER || m.Msg == WM_MOUSEMOVE || m.Msg == WM_NCMOUSEMOVE) {
                return;
            }
            base.WndProc(ref m);
        }
    }
}
