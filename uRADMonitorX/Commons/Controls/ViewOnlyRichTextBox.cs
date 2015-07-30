using System.Security.Permissions;
using System.Windows.Forms;

namespace uRADMonitorX.Commons.Controls {

    public class ViewOnlyRichTextBox : System.Windows.Forms.RichTextBox {

        const int WM_SETFOCUS = 0x0007;
        const int WM_KILLFOCUS = 0x0008;
        const int WM_SETCURSOR = 0x0020;

        public ViewOnlyRichTextBox()
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

            base.WndProc(ref m);
        }
    }
}