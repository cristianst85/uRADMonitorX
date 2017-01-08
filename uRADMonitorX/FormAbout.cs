using System;
using System.Diagnostics;
using System.Windows.Forms;
using uRADMonitorX.Commons;

namespace uRADMonitorX {

    public partial class FormAbout : Form {

        public FormAbout() {

            InitializeComponent();

            Version version = AssemblyUtils.GetVersion();

            bool isDebug = false;
            Debug.Assert(isDebug = true);
            var strVersion = isDebug ? version.ToString() : version.ToString(3);
            this.label2.Text = this.label2.Text.Replace("{version}", strVersion);

            this.richTextBoxCopyright.Text = this.richTextBoxCopyright.Text.Replace("{fluentSchedulerLibVersion}", AssemblyUtils.GetVersion("FluentScheduler").ToString());
            this.richTextBoxCopyright.Text = this.richTextBoxCopyright.Text.Replace("{newtonsoftJsonLibVersion}", AssemblyUtils.GetVersion("Newtonsoft.Json").ToString());
            this.richTextBoxCopyright.Text = this.richTextBoxCopyright.Text.Replace("{nlogLibVersion}", AssemblyUtils.GetVersion("NLog").ToString());

            this.KeyDown += new KeyEventHandler(formAbout_KeyPress);
            this.richTextBoxCopyright.LinkClicked += new LinkClickedEventHandler(richTextBoxCopyright_LinkClicked);
        }

        private void linkLabelContact_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            System.Diagnostics.Process.Start(String.Format("mailto:{0}?subject=About {1} v{2}", this.linkLabelContact.Text, Application.ProductName, Application.ProductVersion));
        }

        private void richTextBoxCopyright_LinkClicked(object sender, LinkClickedEventArgs e) {
            System.Diagnostics.Process.Start(e.LinkText);
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
