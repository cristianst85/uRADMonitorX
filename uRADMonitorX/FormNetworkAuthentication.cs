using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using uRADMonitorX.Configuration;
using uRADMonitorX.uRADMonitor.Services;

namespace uRADMonitorX
{
    public partial class FormNetworkAuthentication : Form
    {
        private volatile bool IsClosing;

        private readonly ISettings settings;
        private readonly IDeviceServiceFactory deviceServiceFactory;

        public FormNetworkAuthentication(ISettings settings, IDeviceServiceFactory deviceServiceFactory)
        {
            InitializeComponent();

            this.settings = settings;
            this.deviceServiceFactory = deviceServiceFactory;

            this.textBoxUserId.Text = settings.uRADMonitorNetwork.UserId;
            this.textBoxUserKey.Text = settings.uRADMonitorNetwork.UserKey;

            this.textBoxUserId.TextChanged += ToggleButtons;
            this.textBoxUserKey.TextChanged += ToggleButtons;

            this.buttonOK.Click += ButtonOK_Click;
            this.buttonTest.Click += ButtonTest_Click;

            ToggleButtons(null, null);

            // Handlers.
            this.FormClosing += new FormClosingEventHandler(this.FormNetworkAuthentication_Closing);
        }

        private void ToggleButtons(object sender, EventArgs e)
        {
            var enable = !(string.IsNullOrEmpty(textBoxUserId.Text) && string.IsNullOrEmpty(textBoxUserKey.Text));

            buttonTest.Enabled = enable;
            buttonOK.Enabled = sender != null;
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            settings.uRADMonitorNetwork.UserId = textBoxUserId.Text;
            settings.uRADMonitorNetwork.UserKey = textBoxUserKey.Text;
            settings.Save();

            buttonOK.Enabled = false;

            this.Close();
        }

        private async void ButtonTest_Click(object sender, EventArgs e)
        {
            try
            {
                DeviceServiceResponse result = null;

                await Task.Run(() =>
                {
                    var deviceService = deviceServiceFactory.Create(textBoxUserId.Text, textBoxUserKey.Text);
                    result = deviceService.GetAll();
                });

                if (result.HasData)
                {
                    ShowMessageBox("A successful connection was made to the data.uradmonitor.com server.", MessageBoxIcon.Information);
                }
                else
                {
                    ShowMessageBox($"Error: {result.Error}.", MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Error: {ex.Message}.", MessageBoxIcon.Error);
            }
        }

        private void ShowMessageBox(string message, MessageBoxIcon messageBoxIcon)
        {
            if (!IsClosing)
            {
                MessageBox.Show(this, message, Program.ApplicationName, MessageBoxButtons.OK, messageBoxIcon);
            }
        }

        private void FormNetworkAuthentication_Closing(object sender, FormClosingEventArgs e)
        {
            this.IsClosing = true;
        }
    }
}
