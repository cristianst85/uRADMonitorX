using System;
using System.Diagnostics;
using System.Windows.Forms;
using uRADMonitorX.Commons;

namespace uRADMonitorX
{
    public partial class FormAbout : Form
    {
        public FormAbout()
        {
            InitializeComponent();

            var version = AssemblyUtils.GetProductVersion();

            this.label2.Text = this.label2.Text.Replace("{version}", version);

            this.richTextBoxCopyright.Text = this.richTextBoxCopyright.Text.Replace("{fluentSchedulerLibVersion}", AssemblyUtils.GetVersion("FluentScheduler").ToString());
            this.richTextBoxCopyright.Text = this.richTextBoxCopyright.Text.Replace("{newtonsoftJsonLibVersion}", AssemblyUtils.GetVersion("Newtonsoft.Json").ToString());
            this.richTextBoxCopyright.Text = this.richTextBoxCopyright.Text.Replace("{nlogLibVersion}", AssemblyUtils.GetVersion("NLog").ToString());

            this.KeyDown += new KeyEventHandler(FormAbout_KeyPress);
            this.richTextBoxCopyright.LinkClicked += new LinkClickedEventHandler(RichTextBoxCopyright_LinkClicked);
        }

        private void LinkLabelContact_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(String.Format("mailto:{0}?subject=About {1} v{2}", this.linkLabelContact.Text, Application.ProductName, Application.ProductVersion));
        }

        private void RichTextBoxCopyright_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start(e.LinkText);
        }

        private void FormAbout_KeyPress(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

        private void LinkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/cristianst85/uRADMonitorX");
        }
    }
}
