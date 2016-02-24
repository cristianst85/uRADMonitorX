using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using uRADMonitorX.Commons;
using uRADMonitorX.Configuration;
using uRADMonitorX.Core;

namespace uRADMonitorX {

    public partial class FormOptions : Form {

        private ISettings settings;

        public event SettingsChangedEventHandler SettingsChangedEventHandler;

        private static NumberFormatInfo numberFormatInfo = CultureInfo.CurrentCulture.NumberFormat;

        private int temperatureNotificationUnitSelectedIndex = -1;
        private int radiationNotificationUnitSelectedIndex = -1;

        public FormOptions(ISettings settings) {
            InitializeComponent();

            this.settings = settings;

            this.checkBoxStartWithWindows.Checked = settings.StartWithWindows;
            this.checkBoxStartWithWindows.Enabled = !EnvironmentUtils.IsUnix();
            this.checkBoxAutomaticallyCheckForUpdates.Checked = settings.AutomaticallyCheckForUpdates;

            this.checkBoxLoggingEnable.Checked = settings.IsLoggingEnabled;
            this.textBoxLogDirectoryPath.Text = settings.LogDirectoryPath;

            if (this.checkBoxLoggingEnable.Checked && this.textBoxLogDirectoryPath.Text.Length > 0 && Directory.Exists(this.textBoxLogDirectoryPath.Text)) {
                this.pictureBoxLoggingInfo.Hide();
                this.labelLoggingInfo.Hide();
            }
            else if (this.textBoxLogDirectoryPath.Text.Length > 0 && !Directory.Exists(this.textBoxLogDirectoryPath.Text)) {
                this.labelLoggingInfo.Text = "Directory does not exist.";
                this.pictureBoxLoggingInfo.Image = global::uRADMonitorX.Properties.Resources.error;
            }
            else {
                this.labelLoggingInfo.Text = "Using default application path.";
                this.pictureBoxLoggingInfo.Image = global::uRADMonitorX.Properties.Resources.information;
            }

            this.checkBoxDataLoggingEnable.Checked = settings.IsDataLoggingEnabled;
            this.checkBoxDataLoggingToSeparateFile.Checked = settings.DataLoggingToSeparateFile;
            this.textBoxDataLogDirectoryPath.Text = settings.DataLogDirectoryPath;

            this.checkBoxShowInTaskbar.Checked = settings.ShowInTaskbar;
            this.checkBoxStartMinimized.Checked = settings.StartMinimized;
            this.checkBoxCloseToSystemTray.Checked = settings.CloseToSystemTray;

            this.comboBoxTemperatureUnit.Items.Add(TemperatureUnitType.Celsius);
            this.comboBoxTemperatureUnit.Items.Add(TemperatureUnitType.Fahrenheit);

            this.comboBoxPressureUnit.Items.Add(PressureUnitType.Pa);
            this.comboBoxPressureUnit.Items.Add(PressureUnitType.kPa);

            this.comboBoxRadiationUnit.Items.Add(EnumHelper.GetEnumDescription<RadiationUnitType>(RadiationUnitType.Cpm));
            this.comboBoxRadiationUnit.Items.Add(EnumHelper.GetEnumDescription<RadiationUnitType>(RadiationUnitType.uSvH));
            this.comboBoxRadiationUnit.Items.Add(EnumHelper.GetEnumDescription<RadiationUnitType>(RadiationUnitType.uRemH));

            if (settings.TemperatureUnitType == TemperatureUnitType.Celsius) {
                this.comboBoxTemperatureUnit.SelectedIndex = 0;
            }
            else if (settings.TemperatureUnitType == TemperatureUnitType.Fahrenheit) {
                this.comboBoxTemperatureUnit.SelectedIndex = 1;
            }
            else {
                // Other values defaults to Celsius.
                this.comboBoxTemperatureUnit.SelectedIndex = 0;
            }

            if (settings.PressureUnitType == PressureUnitType.Pa) {
                this.comboBoxPressureUnit.SelectedIndex = 0;
            }
            else if (settings.PressureUnitType == PressureUnitType.kPa) {
                this.comboBoxPressureUnit.SelectedIndex = 1;
            }
            else {
                // Other values defaults to Pascal.
                this.comboBoxPressureUnit.SelectedIndex = 0;
            }

            if (settings.RadiationUnitType == RadiationUnitType.Cpm) {
                this.comboBoxRadiationUnit.SelectedIndex = 0;
            }
            else if (settings.RadiationUnitType == RadiationUnitType.uSvH) {
                this.comboBoxRadiationUnit.SelectedIndex = 1;
            }
            else if (settings.RadiationUnitType == RadiationUnitType.uRemH) {
                this.comboBoxRadiationUnit.SelectedIndex = 2;
            }
            else {
                // Other values defaults to cpm.
                this.comboBoxRadiationUnit.SelectedIndex = 0;
            }

            this.comboBoxPressureUnit.Enabled = settings.HasPressureSensor;
            this.labelPressureUnit.Enabled = settings.HasPressureSensor;

            this.comboBoxTemperatureNotificationUnit.Items.Add(TemperatureUnitType.Celsius);
            this.comboBoxTemperatureNotificationUnit.Items.Add(TemperatureUnitType.Fahrenheit);

            this.comboBoxRadiationNotificationUnit.Items.Add(EnumHelper.GetEnumDescription<RadiationUnitType>(RadiationUnitType.Cpm));
            this.comboBoxRadiationNotificationUnit.Items.Add(EnumHelper.GetEnumDescription<RadiationUnitType>(RadiationUnitType.uSvH));
            this.comboBoxRadiationNotificationUnit.Items.Add(EnumHelper.GetEnumDescription<RadiationUnitType>(RadiationUnitType.uRemH));

            this.textBoxHighTemperatureNotificationValue.Text = settings.HighTemperatureNotificationValue.ToString();

            this.checkBoxNotificationsEnable.Checked = settings.AreNotificationsEnabled;

            if (settings.TemperatureNotificationUnitType == TemperatureUnitType.Celsius) {
                this.comboBoxTemperatureNotificationUnit.SelectedIndex = 0;
            }
            else if (settings.TemperatureNotificationUnitType == TemperatureUnitType.Fahrenheit) {
                this.comboBoxTemperatureNotificationUnit.SelectedIndex = 1;
            }
            else {
                // Other values defaults to Celsius.
                this.comboBoxTemperatureNotificationUnit.SelectedIndex = 0;
            }

            this.temperatureNotificationUnitSelectedIndex = this.comboBoxTemperatureNotificationUnit.SelectedIndex;

            RadiationUnitType radiationNotificationUnitType = settings.RadiationNotificationUnitType;
            double radiationNotificationValue = settings.RadiationNotificationValue;

            // Override settings if radiation notification unit type is not in counts pe minute (cpm) and the detector is unknown.
            if (radiationNotificationUnitType != RadiationUnitType.Cpm &&
                    (String.IsNullOrEmpty(settings.DetectorName) ||
                    !RadiationDetector.IsKnown(RadiationDetector.Normalize(settings.DetectorName)))
                ) {
                radiationNotificationValue = 0;
                radiationNotificationUnitType = RadiationUnitType.Cpm;
            }
            this.textBoxRadiationNotificationValue.Text = radiationNotificationValue.ToString();

            if (radiationNotificationUnitType == RadiationUnitType.Cpm) {
                this.comboBoxRadiationNotificationUnit.SelectedIndex = 0;
            }
            else if (radiationNotificationUnitType == RadiationUnitType.uSvH) {
                this.comboBoxRadiationNotificationUnit.SelectedIndex = 1;
            }
            else if (radiationNotificationUnitType == RadiationUnitType.uRemH) {
                this.comboBoxRadiationNotificationUnit.SelectedIndex = 2;
            }
            else {
                // Other values defaults to cpm.
                this.comboBoxRadiationNotificationUnit.SelectedIndex = 0;
            }

            this.radiationNotificationUnitSelectedIndex = this.comboBoxRadiationNotificationUnit.SelectedIndex;

            this.checkBoxStartWithWindows.CheckedChanged += new EventHandler(settingsChanged);
            this.checkBoxAutomaticallyCheckForUpdates.CheckedChanged += new EventHandler(settingsChanged);
            this.checkBoxLoggingEnable.CheckedChanged += new EventHandler(settingsChanged);
            this.checkBoxDataLoggingEnable.CheckedChanged += new EventHandler(settingsChanged);
            this.checkBoxDataLoggingToSeparateFile.CheckedChanged += new EventHandler(settingsChanged);
            this.checkBoxStartMinimized.CheckedChanged += new EventHandler(settingsChanged);
            this.checkBoxCloseToSystemTray.CheckedChanged += new EventHandler(settingsChanged);
            this.checkBoxShowInTaskbar.CheckedChanged += new EventHandler(settingsChanged);
            this.comboBoxTemperatureUnit.SelectedIndexChanged += new EventHandler(settingsChanged);
            this.comboBoxPressureUnit.SelectedIndexChanged += new EventHandler(settingsChanged);
            this.comboBoxRadiationUnit.SelectedIndexChanged += new EventHandler(settingsChanged);

            this.textBoxHighTemperatureNotificationValue.TextChanged += new EventHandler(highTemperatureNotificationValue_TextChanged);
            this.comboBoxTemperatureNotificationUnit.SelectedIndexChanged += new EventHandler(settingsChanged);
            this.comboBoxTemperatureNotificationUnit.SelectedIndexChanged += new EventHandler(comboBoxTemperatureNotificationUnit_SelectedIndexChanged);
            this.textBoxRadiationNotificationValue.TextChanged += new EventHandler(radiationNotificationValue_TextChanged);
            this.comboBoxRadiationNotificationUnit.SelectedIndexChanged += new EventHandler(settingsChanged);
            this.comboBoxRadiationNotificationUnit.SelectedIndexChanged += new EventHandler(comboBoxRadiationNotificationUnit_SelectedIndexChanged);

            this.textBoxLogDirectoryPath.TextChanged += new EventHandler(textBoxLogDirectoryPathTextChanged);
            this.buttonConfigureLogDirectoryPath.Click += new System.EventHandler(buttonConfigureLogDirectoryPath_Click);
            this.checkBoxLoggingEnable.CheckedChanged += new EventHandler(checkBoxLoggingEnable_CheckedChanged);
            this.checkBoxDataLoggingEnable.CheckedChanged += new EventHandler(checkBoxDataLoggingEnable_CheckedChanged);
            this.checkBoxDataLoggingToSeparateFile.CheckedChanged += new EventHandler(checkBoxDataLoggingToSeparateFile_CheckedChanged);

            this.checkBoxNotificationsEnable.CheckedChanged += new EventHandler(settingsChanged);
            this.checkBoxNotificationsEnable.CheckedChanged += new EventHandler(checkBoxNotificationsEnable_CheckedChanged);

            this.checkBoxLoggingEnable_CheckedChanged(null, null);
            this.checkBoxDataLoggingEnable_CheckedChanged(null, null);
            this.checkBoxNotificationsEnable_CheckedChanged(null, null);
            this.buttonApply.Enabled = false;
        }

        private void settingsChanged(Object sender, EventArgs e) {
            if (loggingDirectoryIsValid() && loggingDataDirectoryIsValid() && !this.checkBoxNotificationsEnable.Checked ||
                loggingDirectoryIsValid() && loggingDataDirectoryIsValid() && (this.checkBoxNotificationsEnable.Checked && this.inputControlsValuesAreValid())
                ) {
                this.buttonApply.Enabled = true;
            }
            else {
                this.buttonApply.Enabled = false;
            }
        }

        private void buttonOK_Click(object sender, System.EventArgs e) {
            if (this.buttonApply.Enabled) {
                this.saveSettings();
            }
            this.Close();
        }

        private void buttonApply_Click(object sender, System.EventArgs e) {
            this.saveSettings();
            this.buttonApply.Enabled = false;
        }

        private void textBoxLogDirectoryPathTextChanged(object sender, EventArgs e) {
            loggingDirectoryIsValid();
            this.settingsChanged(sender, e);
        }

        private bool loggingDirectoryIsValid() {
            String logDirPath = PathUtils.GetFullPath(AssemblyUtils.GetApplicationDirPath(), this.textBoxLogDirectoryPath.Text);
            if (this.checkBoxLoggingEnable.Checked && this.textBoxLogDirectoryPath.Text.Length > 0 && Directory.Exists(logDirPath)) {
                this.pictureBoxLoggingInfo.Hide();
                this.labelLoggingInfo.Hide();
                return true;
            }
            else if (!this.checkBoxLoggingEnable.Checked) {
                this.pictureBoxLoggingInfo.Hide();
                this.labelLoggingInfo.Hide();
                return true;
            }
            else if (this.textBoxLogDirectoryPath.Text.Length > 0 && !Directory.Exists(logDirPath)) {
                this.labelLoggingInfo.Text = "Directory does not exist.";
                this.pictureBoxLoggingInfo.Image = global::uRADMonitorX.Properties.Resources.error;
                this.pictureBoxLoggingInfo.Show();
                this.labelLoggingInfo.Show();
                return false;
            }
            else {
                this.labelLoggingInfo.Text = "Using default application path.";
                this.pictureBoxLoggingInfo.Image = global::uRADMonitorX.Properties.Resources.information;
                this.pictureBoxLoggingInfo.Show();
                this.labelLoggingInfo.Show();
                return true;
            }
        }

        private bool loggingDataDirectoryIsValid() {
            String logDirPath = PathUtils.GetFullPath(AssemblyUtils.GetApplicationDirPath(), this.textBoxDataLogDirectoryPath.Text);
            if (this.textBoxDataLogDirectoryPath.Text.Length > 0 && Directory.Exists(logDirPath)) {
                return false;
            }
            else {
                return true;
            }
        }

        private void saveSettings() {
            this.settings.StartWithWindows = this.checkBoxStartWithWindows.Checked;
            this.settings.AutomaticallyCheckForUpdates = this.checkBoxAutomaticallyCheckForUpdates.Checked;

            if (this.checkBoxLoggingEnable.Checked) {
                this.settings.LogDirectoryPath = this.textBoxLogDirectoryPath.Text;
            }
            this.settings.IsLoggingEnabled = this.checkBoxLoggingEnable.Checked;

            if (this.checkBoxDataLoggingEnable.Checked) {
                this.settings.DataLogDirectoryPath = this.textBoxDataLogDirectoryPath.Text;
            }
            this.settings.IsDataLoggingEnabled = this.checkBoxDataLoggingEnable.Checked;
            this.settings.DataLoggingToSeparateFile = this.checkBoxDataLoggingToSeparateFile.Checked;


            this.settings.ShowInTaskbar = this.checkBoxShowInTaskbar.Checked;
            this.settings.StartMinimized = this.checkBoxStartMinimized.Checked;
            this.settings.CloseToSystemTray = this.checkBoxCloseToSystemTray.Checked;
            this.settings.TemperatureUnitType = (TemperatureUnitType)Enum.Parse(typeof(TemperatureUnitType), this.comboBoxTemperatureUnit.SelectedItem.ToString(), true);
            this.settings.PressureUnitType = (PressureUnitType)Enum.Parse(typeof(PressureUnitType), this.comboBoxPressureUnit.SelectedItem.ToString(), true);
            this.settings.RadiationUnitType = EnumHelper.GetEnumByDescription<RadiationUnitType>(this.comboBoxRadiationUnit.SelectedItem.ToString(), true);

            if (this.checkBoxNotificationsEnable.Checked) {
                this.settings.HighTemperatureNotificationValue = int.Parse(this.textBoxHighTemperatureNotificationValue.Text);
                this.settings.TemperatureNotificationUnitType = (TemperatureUnitType)Enum.Parse(typeof(TemperatureUnitType), this.comboBoxTemperatureNotificationUnit.SelectedItem.ToString(), true);
                this.settings.RadiationNotificationValue = double.Parse(this.textBoxRadiationNotificationValue.Text, NumberStyles.AllowDecimalPoint, numberFormatInfo);
                this.settings.RadiationNotificationUnitType = EnumHelper.GetEnumByDescription<RadiationUnitType>(this.comboBoxRadiationNotificationUnit.SelectedItem.ToString(), true);
            }
            this.settings.AreNotificationsEnabled = this.checkBoxNotificationsEnable.Checked;

            this.settings.Commit();

            this.onSettingsChanged();
        }

        private void checkBoxLoggingEnable_CheckedChanged(object sender, EventArgs e) {
            this.textBoxLogDirectoryPath.Enabled = this.checkBoxLoggingEnable.Checked;
            this.buttonConfigureLogDirectoryPath.Enabled = this.checkBoxLoggingEnable.Checked;
            this.checkBoxDataLoggingEnable.Enabled = this.checkBoxLoggingEnable.Checked;
            textBoxLogDirectoryPathTextChanged(sender, e);
            checkBoxDataLoggingEnable_CheckedChanged(sender, e);
        }

        private void checkBoxDataLoggingEnable_CheckedChanged(object sender, EventArgs e) {
            this.checkBoxDataLoggingToSeparateFile.Enabled = this.checkBoxLoggingEnable.Checked
                && this.checkBoxDataLoggingEnable.Checked;
            checkBoxDataLoggingToSeparateFile_CheckedChanged(sender, e);
            // TODO: textBoxDataLogDirectoryPathTextChanged(sender, e);
        }

        private void checkBoxDataLoggingToSeparateFile_CheckedChanged(object sender, EventArgs e) {
            this.textBoxDataLogDirectoryPath.Enabled = this.checkBoxDataLoggingToSeparateFile.Checked
                && this.checkBoxDataLoggingToSeparateFile.Enabled;
            this.buttonConfigureDataLogDirectoryPath.Enabled = this.checkBoxDataLoggingToSeparateFile.Checked
               && this.checkBoxDataLoggingToSeparateFile.Enabled;
        }

        private void buttonConfigureLogDirectoryPath_Click(object sender, EventArgs e) {
            FolderBrowserDialog folderBrowseDialog = new FolderBrowserDialog();
            try {
                if (this.textBoxLogDirectoryPath.Text.Length == 0) {
                    folderBrowseDialog.SelectedPath = Path.GetDirectoryName(AssemblyUtils.GetApplicationPath());
                }
                else {
                    folderBrowseDialog.SelectedPath = this.textBoxLogDirectoryPath.Text;
                }
                folderBrowseDialog.ShowNewFolderButton = true;
                folderBrowseDialog.Description = "Select a directory where to save the log files.";
                DialogResult result = folderBrowseDialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK) {
                    // If the selected path is in a subdirectory of the application path then keep only the relative path.
                    if (folderBrowseDialog.SelectedPath.StartsWith(Path.GetDirectoryName(AssemblyUtils.GetApplicationPath()))) {
                        this.textBoxLogDirectoryPath.Text = folderBrowseDialog.SelectedPath.Substring(Path.GetDirectoryName(AssemblyUtils.GetApplicationPath()).Length);
                    }
                    else {
                        this.textBoxLogDirectoryPath.Text = folderBrowseDialog.SelectedPath;
                    }
                }
            }
            finally {
                folderBrowseDialog.Dispose();
            }
        }

        private void onSettingsChanged() {
            SettingsChangedEventHandler handler = SettingsChangedEventHandler;
            if (handler != null) {
                handler(this, new SettingsChangedEventArgs(this.settings));
            }
        }

        private void checkBoxNotificationsEnable_CheckedChanged(object sender, EventArgs e) {
            this.labelTemperatureNotification.Enabled = this.checkBoxNotificationsEnable.Checked;
            this.labelRadiationNotification.Enabled = this.checkBoxNotificationsEnable.Checked;
            this.textBoxHighTemperatureNotificationValue.Enabled = this.checkBoxNotificationsEnable.Checked;
            this.textBoxRadiationNotificationValue.Enabled = this.checkBoxNotificationsEnable.Checked;
            this.comboBoxTemperatureNotificationUnit.Enabled = this.checkBoxNotificationsEnable.Checked;
            Debug.WriteLine("checkBoxNotificationsEnable_CheckedChanged");
            if (String.IsNullOrEmpty(settings.DetectorName) ||
                !RadiationDetector.IsKnown(RadiationDetector.Normalize(settings.DetectorName))) {
                this.comboBoxRadiationNotificationUnit.Enabled = false;
                if (this.checkBoxNotificationsEnable.Checked) {
                    this.pictureBoxNotificationsInfo.Show();
                    this.labelNotificationsInfo.Show();
                }
                else {
                    this.pictureBoxNotificationsInfo.Hide();
                    this.labelNotificationsInfo.Hide();
                }
            }
            else {
                this.comboBoxRadiationNotificationUnit.Enabled = this.checkBoxNotificationsEnable.Checked;
                this.pictureBoxNotificationsInfo.Hide();
                this.labelNotificationsInfo.Hide();
            }
            if (this.checkBoxNotificationsEnable.Checked) {
                highTemperatureNotificationValue_TextChanged(sender, e);
                radiationNotificationValue_TextChanged(sender, e);
            }
        }

        private void highTemperatureNotificationValue_TextChanged(object sender, EventArgs e) {
            bool isValid = this.temperatureNotificationValueIsValid(this.textBoxHighTemperatureNotificationValue.Text, (TemperatureUnitType)Enum.Parse(typeof(TemperatureUnitType), this.comboBoxTemperatureNotificationUnit.SelectedItem.ToString(), true));
            if (!isValid) {
                ToolTip toolTip = new ToolTip();
                toolTip.IsBalloon = true;
                toolTip.ToolTipIcon = ToolTipIcon.Error;
                toolTip.ToolTipTitle = "Incorrect value";
                String message = "Value for Temperature must be a number between ";
                message += (this.comboBoxTemperatureNotificationUnit.SelectedIndex == 0) ? "0 and 60." : "32 and 140.";
                toolTip.Show(message, this.textBoxRadiationNotificationValue, 0, -102, 5000);
            }
            // this.comboBoxTemperatureNotificationUnit.Enabled = isValid;
            this.updateApplyButtonState();
        }

        private void radiationNotificationValue_TextChanged(object sender, EventArgs e) {
            bool isValid = this.radiationNotificationValueIsValid(this.textBoxRadiationNotificationValue.Text);
            if (!isValid) {
                ToolTip toolTip = new ToolTip();
                toolTip.IsBalloon = true;
                toolTip.ToolTipIcon = ToolTipIcon.Error;
                toolTip.ToolTipTitle = "Incorrect value";
                toolTip.Show(String.Format("Value for Radiation must be a number between 0 and 999999.\nDecimal sign ({0}) is allowed.", numberFormatInfo.NumberDecimalSeparator), this.textBoxRadiationNotificationValue, 0, -90, 5000);
            }
            // this.comboBoxRadiationNotificationUnit.Enabled = isValid;
            this.updateApplyButtonState();
        }

        private bool temperatureNotificationValueIsValid(String temperature, TemperatureUnitType unit) {
            int value = 0;
            bool isInteger = int.TryParse(temperature, out value);
            // Device working temperature is between -20 °C and +60 °C.
            // For High Temperature notification we allow only the positive part of the interval.
            return (isInteger &&
                    ((unit == TemperatureUnitType.Celsius && value >= 0 && value <= 60) ||
                    (unit == TemperatureUnitType.Fahrenheit && value >= 32 && value <= 140))
                   );
        }

        private bool radiationNotificationValueIsValid(String radiation) {
            double value = 0;
            bool isInteger = double.TryParse(radiation, NumberStyles.AllowDecimalPoint, numberFormatInfo, out value);
            return (isInteger && (value >= 0 && value <= 999999));
        }

        private void updateApplyButtonState() {
            this.buttonApply.Enabled = inputControlsValuesAreValid();
        }

        private bool inputControlsValuesAreValid() {
            return
                this.loggingDirectoryIsValid() &&
                this.temperatureNotificationValueIsValid(this.textBoxHighTemperatureNotificationValue.Text, (TemperatureUnitType)Enum.Parse(typeof(TemperatureUnitType), this.comboBoxTemperatureNotificationUnit.SelectedItem.ToString(), true)) &&
                this.radiationNotificationValueIsValid(this.textBoxRadiationNotificationValue.Text);
        }

        private void comboBoxTemperatureNotificationUnit_SelectedIndexChanged(object sender, EventArgs e) {
            if (MathX.IsInteger(this.textBoxHighTemperatureNotificationValue.Text)) {
                if (temperatureNotificationUnitSelectedIndex == 0 && this.comboBoxTemperatureNotificationUnit.SelectedIndex == 1) {
                    // Round to the nearest integer.
                    // Try to maintain precision when converting back-and-forth to/from the same value.
                    this.textBoxHighTemperatureNotificationValue.Text = Math.Round(Temperature.CelsiusToFahrenheit(double.Parse(this.textBoxHighTemperatureNotificationValue.Text)), 0).ToString();
                }
                else if (temperatureNotificationUnitSelectedIndex == 1 && this.comboBoxTemperatureNotificationUnit.SelectedIndex == 0) {
                    // Round to the nearest integer. 
                    // Try to maintain precision when converting back-and-forth to/from the same value.
                    this.textBoxHighTemperatureNotificationValue.Text = Math.Round(Temperature.FahrenheitToCelsius(double.Parse(this.textBoxHighTemperatureNotificationValue.Text)), 0).ToString();
                }
                else {
                    // Leave the value unchanged.
                }
            }
            this.temperatureNotificationUnitSelectedIndex = this.comboBoxTemperatureNotificationUnit.SelectedIndex;
        }

        private void comboBoxRadiationNotificationUnit_SelectedIndexChanged(object sender, EventArgs e) {
            if (MathX.IsDecimal(this.textBoxRadiationNotificationValue.Text, numberFormatInfo)) {
                RadiationDetector detector = RadiationDetector.GetByName(settings.DetectorName);
                if (radiationNotificationUnitSelectedIndex == 0 && this.comboBoxRadiationNotificationUnit.SelectedIndex == 1) {
                    this.textBoxRadiationNotificationValue.Text = MathX.Truncate(Radiation.CpmToMicroSvPerHour(double.Parse(this.textBoxRadiationNotificationValue.Text), detector.Factor), 4).ToString();
                }
                else if (radiationNotificationUnitSelectedIndex == 0 && this.comboBoxRadiationNotificationUnit.SelectedIndex == 2) {
                    this.textBoxRadiationNotificationValue.Text = MathX.Truncate(Radiation.CpmToMicroRemPerHour(double.Parse(this.textBoxRadiationNotificationValue.Text), detector.Factor), 2).ToString();
                }
                else if (radiationNotificationUnitSelectedIndex == 1 && this.comboBoxRadiationNotificationUnit.SelectedIndex == 0) {
                    // Round to the nearest integer when converting to cpm.
                    // Try to maintain precision when converting back-and-forth to/from the same value.
                    this.textBoxRadiationNotificationValue.Text = Math.Round(Radiation.MicroSvPerHourToCpm(double.Parse(this.textBoxRadiationNotificationValue.Text), detector.Factor), 0).ToString();
                }
                else if (radiationNotificationUnitSelectedIndex == 1 && this.comboBoxRadiationNotificationUnit.SelectedIndex == 2) {
                    this.textBoxRadiationNotificationValue.Text = MathX.Truncate(Radiation.MicroSvPerHourToMicroRemPerHour(double.Parse(this.textBoxRadiationNotificationValue.Text)), 2).ToString();
                }
                else if (radiationNotificationUnitSelectedIndex == 2 && this.comboBoxRadiationNotificationUnit.SelectedIndex == 0) {
                    // Round to the nearest integer when converting to cpm. 
                    // Try to maintain precision when converting back-and-forth to/from the same value.
                    this.textBoxRadiationNotificationValue.Text = Math.Round(Radiation.MicroRemPerHourToCpm(double.Parse(this.textBoxRadiationNotificationValue.Text), detector.Factor), 0).ToString();
                }
                else if (radiationNotificationUnitSelectedIndex == 2 && this.comboBoxRadiationNotificationUnit.SelectedIndex == 1) {
                    this.textBoxRadiationNotificationValue.Text = MathX.Truncate(Radiation.MicroRemPerHourToMicroSvPerHour(double.Parse(this.textBoxRadiationNotificationValue.Text)), 4).ToString();
                }
                else {
                    // Leave the value unchanged.
                }
            }
            this.radiationNotificationUnitSelectedIndex = this.comboBoxRadiationNotificationUnit.SelectedIndex;
        }
    }
}
