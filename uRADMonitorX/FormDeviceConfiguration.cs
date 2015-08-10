using System;
using System.Windows.Forms;
using uRADMonitorX.Commons.Networking;
using uRADMonitorX.Configuration;
using uRADMonitorX.Core;

namespace uRADMonitorX {

    public partial class FormDeviceConfiguration : Form {

        private ISettings settings;

        public FormDeviceConfiguration(ISettings settings) {
            InitializeComponent();

            this.settings = settings;

            // Init.
            this.buttonSave.Enabled = false;

            this.textBoxIPAddress.Text = settings.DeviceIPAddress;
            this.textBoxIPAddress.SelectionStart = this.textBoxIPAddress.Text.Length; // Don't select text content.

            if (settings.PollingType == Core.PollingType.WDTSync) {
                this.radioButtonPollingTypeWDTSync.Checked = true;
                this.textBoxPollingInterval.Enabled = false;
                this.labelPollingIntervalUnits.Enabled = false;
            }
            else {
                this.radioButtonPollingTypeInterval.Checked = true;
            }
            this.textBoxPollingInterval.Text = String.Format("{0}", settings.PollingInterval);

            this.textBoxIPAddress.TextChanged += new EventHandler(textBoxIPAddress_TextChanged);
            this.radioButtonPollingTypeWDTSync.CheckedChanged += new EventHandler(radioButtons_CheckedChanged);
            this.radioButtonPollingTypeInterval.CheckedChanged += new EventHandler(radioButtons_CheckedChanged);
            this.textBoxPollingInterval.TextChanged += new EventHandler(textBoxPollingInterval_TextChanged);
        }

        private void textBoxIPAddress_TextChanged(object sender, EventArgs e) {
            if (!this.ipAddressIsValid(this.textBoxIPAddress.Text)) {
                ToolTip toolTip = new ToolTip();
                toolTip.IsBalloon = true;
                toolTip.ToolTipIcon = ToolTipIcon.Error;
                toolTip.ToolTipTitle = "Incorrect value";
                toolTip.Show("Value for IP Address must be a valid IPv4 address.", this.textBoxIPAddress, 0, -74, 3000);
            }
            this.updateSaveButtonState();
        }

        private void radioButtons_CheckedChanged(object sender, EventArgs e) {
            this.textBoxPollingInterval.Enabled = !this.radioButtonPollingTypeWDTSync.Checked;
            this.labelPollingIntervalUnits.Enabled = !this.radioButtonPollingTypeWDTSync.Checked;
            this.updateSaveButtonState();
        }

        private void textBoxPollingInterval_TextChanged(object sender, EventArgs e) {
            if (!this.pollingIntervalIsValid(this.textBoxPollingInterval.Text)) {
                ToolTip toolTip = new ToolTip();
                toolTip.IsBalloon = true;
                toolTip.ToolTipIcon = ToolTipIcon.Error;
                toolTip.ToolTipTitle = "Incorrect value";
                toolTip.Show("Value for Polling Interval must be a number between 1 and 999.", this.textBoxPollingInterval, 0, -74, 5000);
            }
            this.updateSaveButtonState();
        }

        private void updateSaveButtonState() {
            this.buttonSave.Enabled = inputControlsValuesAreValid();
        }

        private bool inputControlsValuesAreValid() {
            return
                this.ipAddressIsValid(this.textBoxIPAddress.Text) && (
                    this.radioButtonPollingTypeWDTSync.Checked ||
                    (this.radioButtonPollingTypeInterval.Checked &&
                    this.pollingIntervalIsValid(this.textBoxPollingInterval.Text))
                );
        }

        private bool ipAddressIsValid(String ipAddress) {
            return IPAddress.IsValidFormat(ipAddress) || IPEndPoint.IsValidFormat(ipAddress);
        }

        private bool pollingIntervalIsValid(String pollingInterval) {
            int value = 0;
            bool isInteger = int.TryParse(pollingInterval, out value);
            return (isInteger && (value > 0 && value < 1000));
        }

        private void buttonSave_Click(object sender, EventArgs e) {
            this.settings.DeviceIPAddress = this.textBoxIPAddress.Text;
            this.settings.PollingType = (this.radioButtonPollingTypeWDTSync.Checked) ? PollingType.WDTSync : PollingType.FixedInterval;
            if (this.radioButtonPollingTypeInterval.Checked) {
                this.settings.PollingInterval = int.Parse(this.textBoxPollingInterval.Text);
            }
            this.settings.Commit();
            this.Close();
        }
    }
}
