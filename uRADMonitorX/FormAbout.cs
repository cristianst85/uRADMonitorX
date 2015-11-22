using System;
using System.Windows.Forms;
using uRADMonitorX.Commons;

namespace uRADMonitorX {

    public partial class FormAbout : Form {

        public FormAbout() {

            InitializeComponent();

            Version version = AssemblyUtils.GetVersion();

            this.label2.Text = this.label2.Text
                            .Replace("{version}", String.Format("{0}.{1}", version.Major, version.Minor))
                            .Replace("{build}", version.Build.ToString());
#if DEBUG
            this.label2.Text = this.label2.Text
                            .Replace("{revision}", version.Revision.ToString());
#endif
            this.label2.Text = this.label2.Text
                            .Replace("{revision}", String.Empty).TrimEnd('.');

            this.KeyDown += new KeyEventHandler(formAbout_KeyPress);
        }

        private void linkLabelContact_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            System.Diagnostics.Process.Start(String.Format("mailto:{0}?subject=About {1} v{2}", this.linkLabelContact.Text, Application.ProductName, Application.ProductVersion));
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            System.Diagnostics.Process.Start("http://www.aha-soft.com/");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            System.Diagnostics.Process.Start("http://www.famfamfam.com/lab/icons/silk/");
        }

        private void formAbout_KeyPress(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Escape) {
                this.Close();
            }
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            System.Diagnostics.Process.Start("https://github.com/cristianst85/uRADMonitorX");
        }
    }
}
