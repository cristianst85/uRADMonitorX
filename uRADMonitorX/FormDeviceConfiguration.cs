using System;
using System.Windows.Forms;
using uRADMonitorX.Commons.Networking;
using uRADMonitorX.Configuration;
using uRADMonitorX.Core;

namespace uRADMonitorX
{
    public partial class FormDeviceConfiguration : Form
    {
        private readonly ISettings settings;

        public FormDeviceConfiguration(ISettings settings)
        {
            InitializeComponent();

            this.settings = settings;

            // Initialization.
            this.buttonSave.Enabled = false;

            this.textBoxIPAddress.Text = settings.DeviceIPAddress;
            this.textBoxIPAddress.SelectionStart = this.textBoxIPAddress.Text.Length; // Don't select text content.

            if (settings.PollingType == Core.PollingType.WDTSync)
            {
                this.radioButtonPollingTypeWDTSync.Checked = true;
                this.textBoxPollingInterval.Enabled = false;
                this.labelPollingIntervalUnits.Enabled = false;
            }
            else
            {
                this.radioButtonPollingTypeInterval.Checked = true;
            }

            this.textBoxPollingInterval.Text = string.Format("{0}", settings.PollingInterval);

            // Handlers.
            this.textBoxIPAddress.TextChanged += new EventHandler(TextBoxIPAddress_TextChanged);
            this.radioButtonPollingTypeWDTSync.CheckedChanged += new EventHandler(RadioButtons_CheckedChanged);
            this.radioButtonPollingTypeInterval.CheckedChanged += new EventHandler(RadioButtons_CheckedChanged);
            this.textBoxPollingInterval.TextChanged += new EventHandler(TextBoxPollingInterval_TextChanged);
        }

        private void TextBoxIPAddress_TextChanged(object sender, EventArgs e)
        {
            if (!this.IpAddressIsValid(this.textBoxIPAddress.Text))
            {
                var toolTip = new ToolTip
                {
                    IsBalloon = true,
                    ToolTipIcon = ToolTipIcon.Error,
                    ToolTipTitle = "Incorrect value"
                };

                toolTip.Show("Value for IP Address must be a valid IPv4 address.", this.textBoxIPAddress, 0, -74, 3000);
            }

            this.UpdateSaveButtonState();
        }

        private void RadioButtons_CheckedChanged(object sender, EventArgs e)
        {
            this.textBoxPollingInterval.Enabled = !this.radioButtonPollingTypeWDTSync.Checked;
            this.labelPollingIntervalUnits.Enabled = !this.radioButtonPollingTypeWDTSync.Checked;

            this.UpdateSaveButtonState();
        }

        private void TextBoxPollingInterval_TextChanged(object sender, EventArgs e)
        {
            if (!this.pollingIntervalIsValid(this.textBoxPollingInterval.Text))
            {
                var toolTip = new ToolTip
                {
                    IsBalloon = true,
                    ToolTipIcon = ToolTipIcon.Error,
                    ToolTipTitle = "Incorrect value"
                };

                toolTip.Show("Value for Polling Interval must be a number between 1 and 999.", this.textBoxPollingInterval, 0, -74, 5000);
            }

            this.UpdateSaveButtonState();
        }

        private void UpdateSaveButtonState()
        {
            this.buttonSave.Enabled = InputControlsValuesAreValid();
        }

        private bool InputControlsValuesAreValid()
        {
            return this.IpAddressIsValid(this.textBoxIPAddress.Text) && (
                    this.radioButtonPollingTypeWDTSync.Checked ||
                    (this.radioButtonPollingTypeInterval.Checked &&
                    this.pollingIntervalIsValid(this.textBoxPollingInterval.Text))
                );
        }

        private bool IpAddressIsValid(string ipAddress)
        {
            return IPAddress.IsValidFormat(ipAddress) || IPEndPoint.IsValidFormat(ipAddress);
        }

        private bool pollingIntervalIsValid(string pollingInterval)
        {
            bool isInteger = int.TryParse(pollingInterval, out int value);

            return (isInteger && (value > 0 && value < 1000));
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            this.settings.DeviceIPAddress = this.textBoxIPAddress.Text;
            this.settings.PollingType = (this.radioButtonPollingTypeWDTSync.Checked) ? PollingType.WDTSync : PollingType.FixedInterval;

            if (this.radioButtonPollingTypeInterval.Checked)
            {
                this.settings.PollingInterval = int.Parse(this.textBoxPollingInterval.Text);
            }

            this.settings.Commit();
            this.Close();
        }
    }
}
