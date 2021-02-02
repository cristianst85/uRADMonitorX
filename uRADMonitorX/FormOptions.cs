using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using uRADMonitorX.Commons;
using uRADMonitorX.Configuration;
using uRADMonitorX.Core;
using uRADMonitorX.Core.Device;
using uRADMonitorX.Extensions;

namespace uRADMonitorX
{
    public partial class FormOptions : Form
    {
        public event SettingsChangedEventHandler SettingsChangedEventHandler;

        private static NumberFormatInfo numberFormatInfo = CultureInfo.CurrentCulture.NumberFormat;

        private readonly ISettings settings;

        private int temperatureNotificationUnitSelectedIndex = -1;
        private int radiationNotificationUnitSelectedIndex = -1;

        public FormOptions(ISettings settings)
        {
            InitializeComponent();

            this.settings = settings;

            this.checkBoxStartWithWindows.Checked = this.settings.General.StartWithWindows;
            this.checkBoxStartWithWindows.Enabled = !EnvironmentUtils.IsUnix();
            this.checkBoxAutomaticallyCheckForUpdates.Checked = this.settings.General.AutomaticallyCheckForUpdates;

            this.checkBoxLoggingEnable.Checked = this.settings.Logging.IsEnabled;
            this.textBoxLogDirectoryPath.Text = this.settings.Logging.DirectoryPath;

            if (this.checkBoxLoggingEnable.Checked && this.textBoxLogDirectoryPath.Text.Length > 0 && Directory.Exists(this.textBoxLogDirectoryPath.Text))
            {
                this.pictureBoxLoggingInfo.Hide();
                this.labelLoggingInfo.Hide();
            }
            else if (this.textBoxLogDirectoryPath.Text.Length > 0 && !Directory.Exists(this.textBoxLogDirectoryPath.Text))
            {
                this.labelLoggingInfo.Text = "Directory does not exist.";
                this.pictureBoxLoggingInfo.Image = Properties.Resources.error;
            }
            else
            {
                this.labelLoggingInfo.Text = "Using default application path.";
                this.pictureBoxLoggingInfo.Image = Properties.Resources.information;
            }

            this.checkBoxDataLoggingEnable.Checked = this.settings.Logging.DataLogging.IsEnabled;
            this.checkBoxDataLoggingToSeparateFile.Checked = this.settings.Logging.DataLogging.UseSeparateFile;
            this.textBoxDataLogDirectoryPath.Text = this.settings.Logging.DataLogging.DirectoryPath;

            this.checkBoxShowInTaskbar.Checked = this.settings.Display.ShowInTaskbar;
            this.checkBoxStartMinimized.Checked = this.settings.Display.StartMinimized;
            this.checkBoxCloseToSystemTray.Checked = this.settings.Display.CloseToSystemTray;

            this.comboBoxTemperatureUnit.Items.Add(TemperatureUnitType.Celsius);
            this.comboBoxTemperatureUnit.Items.Add(TemperatureUnitType.Fahrenheit);

            this.comboBoxPressureUnit.Items.Add(PressureUnitType.Pa);
            this.comboBoxPressureUnit.Items.Add(PressureUnitType.hPa);
            this.comboBoxPressureUnit.Items.Add(PressureUnitType.kPa);
            this.comboBoxPressureUnit.Items.Add(PressureUnitType.mbar);

            this.comboBoxRadiationUnit.Items.Add(EnumHelper.GetEnumDescription(RadiationUnitType.Cpm));
            this.comboBoxRadiationUnit.Items.Add(EnumHelper.GetEnumDescription(RadiationUnitType.uSvH));
            this.comboBoxRadiationUnit.Items.Add(EnumHelper.GetEnumDescription(RadiationUnitType.uRemH));

            if (this.settings.Misc.TemperatureUnitType == TemperatureUnitType.Celsius)
            {
                this.comboBoxTemperatureUnit.SelectedIndex = 0;
            }
            else if (this.settings.Misc.TemperatureUnitType == TemperatureUnitType.Fahrenheit)
            {
                this.comboBoxTemperatureUnit.SelectedIndex = 1;
            }
            else
            {
                // Other values defaults to Celsius.
                this.comboBoxTemperatureUnit.SelectedIndex = 0;
            }

            if (this.settings.Misc.PressureUnitType == PressureUnitType.Pa)
            {
                this.comboBoxPressureUnit.SelectedIndex = 0;
            }
            else if (this.settings.Misc.PressureUnitType == PressureUnitType.hPa)
            {
                this.comboBoxPressureUnit.SelectedIndex = 1;
            }
            else if (this.settings.Misc.PressureUnitType == PressureUnitType.kPa)
            {
                this.comboBoxPressureUnit.SelectedIndex = 2;
            }
            else if (this.settings.Misc.PressureUnitType == PressureUnitType.mbar)
            {
                this.comboBoxPressureUnit.SelectedIndex = 3;
            }
            else
            {
                // Other values defaults to Pascal.
                this.comboBoxPressureUnit.SelectedIndex = 0;
            }

            if (this.settings.Misc.RadiationUnitType == RadiationUnitType.Cpm)
            {
                this.comboBoxRadiationUnit.SelectedIndex = 0;
            }
            else if (this.settings.Misc.RadiationUnitType == RadiationUnitType.uSvH)
            {
                this.comboBoxRadiationUnit.SelectedIndex = 1;
            }
            else if (this.settings.Misc.RadiationUnitType == RadiationUnitType.uRemH)
            {
                this.comboBoxRadiationUnit.SelectedIndex = 2;
            }
            else
            {
                // Other values defaults to cpm.
                this.comboBoxRadiationUnit.SelectedIndex = 0;
            }

            var deviceSettings = this.settings.Devices.FirstOrDefault();

            this.comboBoxPressureUnit.Enabled = deviceSettings.IsNotNull() && deviceSettings.GetDeviceCapabilities().HasFlag(DeviceCapability.Pressure);
            this.labelPressureUnit.Enabled = deviceSettings.IsNotNull() && deviceSettings.GetDeviceCapabilities().HasFlag(DeviceCapability.Pressure);

            this.comboBoxTemperatureNotificationUnit.Items.Add(TemperatureUnitType.Celsius);
            this.comboBoxTemperatureNotificationUnit.Items.Add(TemperatureUnitType.Fahrenheit);

            this.comboBoxRadiationNotificationUnit.Items.Add(EnumHelper.GetEnumDescription(RadiationUnitType.Cpm));
            this.comboBoxRadiationNotificationUnit.Items.Add(EnumHelper.GetEnumDescription(RadiationUnitType.uSvH));
            this.comboBoxRadiationNotificationUnit.Items.Add(EnumHelper.GetEnumDescription(RadiationUnitType.uRemH));

            this.textBoxHighTemperatureNotificationValue.Text = this.settings.Notifications.TemperatureThreshold.HighValue.ToString();

            this.checkBoxNotificationsEnable.Checked = this.settings.Notifications.IsEnabled;

            if (this.settings.Notifications.TemperatureThreshold.MeasurementUnit == TemperatureUnitType.Celsius)
            {
                this.comboBoxTemperatureNotificationUnit.SelectedIndex = 0;
            }
            else if (this.settings.Notifications.TemperatureThreshold.MeasurementUnit == TemperatureUnitType.Fahrenheit)
            {
                this.comboBoxTemperatureNotificationUnit.SelectedIndex = 1;
            }
            else
            {
                // Other values defaults to Celsius.
                this.comboBoxTemperatureNotificationUnit.SelectedIndex = 0;
            }

            this.temperatureNotificationUnitSelectedIndex = this.comboBoxTemperatureNotificationUnit.SelectedIndex;

            var radiationNotificationUnitType = this.settings.Notifications.RadiationThreshold.MeasurementUnit;
            var radiationNotificationValue = this.settings.Notifications.RadiationThreshold.HighValue;

            var radiationDetectorName = RadiationDetector.Normalize(deviceSettings.GetRadiationDetectorName());
            var isKnownRadiationDetector = radiationDetectorName.IsNotNull() && RadiationDetector.IsKnown(radiationDetectorName);

            // Override settings if radiation notification unit type is not in counts per minute (cpm) and the detector is unknown.
            if (radiationNotificationUnitType != RadiationUnitType.Cpm && !isKnownRadiationDetector)
            {
                radiationNotificationValue = 0;
                radiationNotificationUnitType = RadiationUnitType.Cpm;
            }

            this.textBoxRadiationNotificationValue.Text = radiationNotificationValue.ToString();

            if (radiationNotificationUnitType == RadiationUnitType.Cpm)
            {
                this.comboBoxRadiationNotificationUnit.SelectedIndex = 0;
            }
            else if (radiationNotificationUnitType == RadiationUnitType.uSvH)
            {
                this.comboBoxRadiationNotificationUnit.SelectedIndex = 1;
            }
            else if (radiationNotificationUnitType == RadiationUnitType.uRemH)
            {
                this.comboBoxRadiationNotificationUnit.SelectedIndex = 2;
            }
            else
            {
                // Other values defaults to cpm.
                this.comboBoxRadiationNotificationUnit.SelectedIndex = 0;
            }

            this.radiationNotificationUnitSelectedIndex = this.comboBoxRadiationNotificationUnit.SelectedIndex;

            this.checkBoxStartWithWindows.CheckedChanged += new EventHandler(SettingsChanged);
            this.checkBoxAutomaticallyCheckForUpdates.CheckedChanged += new EventHandler(SettingsChanged);
            this.checkBoxLoggingEnable.CheckedChanged += new EventHandler(SettingsChanged);
            this.checkBoxDataLoggingEnable.CheckedChanged += new EventHandler(SettingsChanged);
            this.checkBoxDataLoggingToSeparateFile.CheckedChanged += new EventHandler(SettingsChanged);
            this.checkBoxStartMinimized.CheckedChanged += new EventHandler(SettingsChanged);
            this.checkBoxCloseToSystemTray.CheckedChanged += new EventHandler(SettingsChanged);
            this.checkBoxShowInTaskbar.CheckedChanged += new EventHandler(SettingsChanged);
            this.comboBoxTemperatureUnit.SelectedIndexChanged += new EventHandler(SettingsChanged);
            this.comboBoxPressureUnit.SelectedIndexChanged += new EventHandler(SettingsChanged);
            this.comboBoxRadiationUnit.SelectedIndexChanged += new EventHandler(SettingsChanged);

            this.textBoxHighTemperatureNotificationValue.TextChanged += new EventHandler(HighTemperatureNotificationValue_TextChanged);
            this.comboBoxTemperatureNotificationUnit.SelectedIndexChanged += new EventHandler(SettingsChanged);
            this.comboBoxTemperatureNotificationUnit.SelectedIndexChanged += new EventHandler(ComboBoxTemperatureNotificationUnit_SelectedIndexChanged);
            this.textBoxRadiationNotificationValue.TextChanged += new EventHandler(RadiationNotificationValue_TextChanged);
            this.comboBoxRadiationNotificationUnit.SelectedIndexChanged += new EventHandler(SettingsChanged);
            this.comboBoxRadiationNotificationUnit.SelectedIndexChanged += new EventHandler(ComboBoxRadiationNotificationUnit_SelectedIndexChanged);

            this.textBoxLogDirectoryPath.TextChanged += new EventHandler(TextBoxLogDirectoryPathTextChanged);
            this.textBoxDataLogDirectoryPath.TextChanged += new EventHandler(TextBoxDataLogDirectoryPathTextChanged);
            this.buttonConfigureLogDirectoryPath.Click += new System.EventHandler(ButtonConfigureLogDirectoryPath_Click);
            this.checkBoxLoggingEnable.CheckedChanged += new EventHandler(CheckBoxLoggingEnable_CheckedChanged);
            this.checkBoxDataLoggingEnable.CheckedChanged += new EventHandler(CheckBoxDataLoggingEnable_CheckedChanged);
            this.checkBoxDataLoggingToSeparateFile.CheckedChanged += new EventHandler(CheckBoxDataLoggingToSeparateFile_CheckedChanged);

            this.checkBoxNotificationsEnable.CheckedChanged += new EventHandler(SettingsChanged);
            this.checkBoxNotificationsEnable.CheckedChanged += new EventHandler(CheckBoxNotificationsEnable_CheckedChanged);

            this.CheckBoxLoggingEnable_CheckedChanged(null, null);
            this.CheckBoxDataLoggingEnable_CheckedChanged(null, null);
            this.CheckBoxNotificationsEnable_CheckedChanged(null, null);

            this.buttonApply.Enabled = false;
        }

        private void SettingsChanged(Object sender, EventArgs e)
        {
            if (LoggingDirectoryIsValid() && LoggingDataDirectoryIsValid() && !this.checkBoxNotificationsEnable.Checked ||
                LoggingDirectoryIsValid() && LoggingDataDirectoryIsValid() && (this.checkBoxNotificationsEnable.Checked && this.InputControlsValuesAreValid())
                )
            {
                this.buttonApply.Enabled = true;
            }
            else
            {
                this.buttonApply.Enabled = false;
            }
        }

        private void ButtonOK_Click(object sender, System.EventArgs e)
        {
            if (this.buttonApply.Enabled)
            {
                this.SaveSettings();
            }

            this.Close();
        }

        private void ButtonApply_Click(object sender, System.EventArgs e)
        {
            this.SaveSettings();

            this.buttonApply.Enabled = false;
        }

        private void TextBoxLogDirectoryPathTextChanged(object sender, EventArgs e)
        {
            LoggingDirectoryIsValid();

            this.SettingsChanged(sender, e);
        }

        private void TextBoxDataLogDirectoryPathTextChanged(object sender, EventArgs e)
        {
            LoggingDataDirectoryIsValid();

            this.SettingsChanged(sender, e);
        }

        private bool LoggingDirectoryIsValid()
        {
            var logDirPath = PathUtils.GetFullPath(AssemblyUtils.GetApplicationDirPath(), this.textBoxLogDirectoryPath.Text);

            if (this.checkBoxLoggingEnable.Checked && this.textBoxLogDirectoryPath.Text.Length > 0 && Directory.Exists(logDirPath))
            {
                this.pictureBoxLoggingInfo.Hide();
                this.labelLoggingInfo.Hide();

                return true;
            }
            else if (!this.checkBoxLoggingEnable.Checked)
            {
                this.pictureBoxLoggingInfo.Hide();
                this.labelLoggingInfo.Hide();

                return true;
            }
            else if (this.textBoxLogDirectoryPath.Text.Length > 0 && !Directory.Exists(logDirPath))
            {
                this.labelLoggingInfo.Text = "Directory does not exist.";
                this.pictureBoxLoggingInfo.Image = Properties.Resources.error;
                this.pictureBoxLoggingInfo.Show();
                this.labelLoggingInfo.Show();

                return false;
            }
            else
            {
                this.labelLoggingInfo.Text = "Using default application path.";
                this.pictureBoxLoggingInfo.Image = Properties.Resources.information;
                this.pictureBoxLoggingInfo.Show();
                this.labelLoggingInfo.Show();

                return true;
            }
        }

        private bool LoggingDataDirectoryIsValid()
        {
            var logDirPath = PathUtils.GetFullPath(AssemblyUtils.GetApplicationDirPath(), this.textBoxDataLogDirectoryPath.Text);

            if (this.textBoxDataLogDirectoryPath.Enabled && this.textBoxDataLogDirectoryPath.Text.Length > 0 && Directory.Exists(logDirPath))
            {
                return true;
            }
            else if (!this.textBoxDataLogDirectoryPath.Enabled)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SaveSettings()
        {
            this.settings.General.StartWithWindows = this.checkBoxStartWithWindows.Checked;
            this.settings.General.AutomaticallyCheckForUpdates = this.checkBoxAutomaticallyCheckForUpdates.Checked;

            this.settings.Display.ShowInTaskbar = this.checkBoxShowInTaskbar.Checked;
            this.settings.Display.StartMinimized = this.checkBoxStartMinimized.Checked;
            this.settings.Display.CloseToSystemTray = this.checkBoxCloseToSystemTray.Checked;

            this.settings.Notifications.IsEnabled = this.checkBoxNotificationsEnable.Checked;
            this.settings.Notifications.TemperatureThreshold.HighValue = this.textBoxHighTemperatureNotificationValue.HasText() && MathX.IsDecimal(this.textBoxHighTemperatureNotificationValue.Text, numberFormatInfo) ? decimal.Parse(this.textBoxHighTemperatureNotificationValue.Text, NumberStyles.AllowDecimalPoint, numberFormatInfo) : default(decimal?);
            this.settings.Notifications.TemperatureThreshold.MeasurementUnit = (TemperatureUnitType)Enum.Parse(typeof(TemperatureUnitType), this.comboBoxTemperatureNotificationUnit.SelectedItem.ToString(), true);
            this.settings.Notifications.RadiationThreshold.HighValue = this.textBoxRadiationNotificationValue.HasText() && MathX.IsDecimal(this.textBoxRadiationNotificationValue.Text, numberFormatInfo) ? decimal.Parse(this.textBoxRadiationNotificationValue.Text, NumberStyles.AllowDecimalPoint, numberFormatInfo) : default(decimal?);
            this.settings.Notifications.RadiationThreshold.MeasurementUnit = EnumHelper.GetEnumByDescription<RadiationUnitType>(this.comboBoxRadiationNotificationUnit.SelectedItem.ToString(), true);

            this.settings.Logging.IsEnabled = this.checkBoxLoggingEnable.Checked;
            this.settings.Logging.DirectoryPath = this.textBoxLogDirectoryPath.Text;

            this.settings.Logging.DataLogging.IsEnabled = this.checkBoxDataLoggingEnable.Checked;
            this.settings.Logging.DataLogging.UseSeparateFile = this.checkBoxDataLoggingToSeparateFile.Checked;
            this.settings.Logging.DataLogging.DirectoryPath = this.textBoxDataLogDirectoryPath.Text;

            this.settings.Misc.TemperatureUnitType = (TemperatureUnitType)Enum.Parse(typeof(TemperatureUnitType), this.comboBoxTemperatureUnit.SelectedItem.ToString(), true);
            this.settings.Misc.PressureUnitType = (PressureUnitType)Enum.Parse(typeof(PressureUnitType), this.comboBoxPressureUnit.SelectedItem.ToString(), true);
            this.settings.Misc.RadiationUnitType = EnumHelper.GetEnumByDescription<RadiationUnitType>(this.comboBoxRadiationUnit.SelectedItem.ToString(), true);

            this.settings.Save();

            this.OnSettingsChanged();
        }

        private void CheckBoxLoggingEnable_CheckedChanged(object sender, EventArgs e)
        {
            this.textBoxLogDirectoryPath.Enabled = this.checkBoxLoggingEnable.Checked;
            this.buttonConfigureLogDirectoryPath.Enabled = this.checkBoxLoggingEnable.Checked;
            this.checkBoxDataLoggingEnable.Enabled = this.checkBoxLoggingEnable.Checked;

            TextBoxLogDirectoryPathTextChanged(sender, e);
            CheckBoxDataLoggingEnable_CheckedChanged(sender, e);
        }

        private void CheckBoxDataLoggingEnable_CheckedChanged(object sender, EventArgs e)
        {
            this.checkBoxDataLoggingToSeparateFile.Enabled = this.checkBoxLoggingEnable.Checked && this.checkBoxDataLoggingEnable.Checked;

            CheckBoxDataLoggingToSeparateFile_CheckedChanged(sender, e);
            TextBoxDataLogDirectoryPathTextChanged(sender, e);
        }

        private void CheckBoxDataLoggingToSeparateFile_CheckedChanged(object sender, EventArgs e)
        {
            this.textBoxDataLogDirectoryPath.Enabled = this.checkBoxDataLoggingToSeparateFile.Checked
                && this.checkBoxDataLoggingToSeparateFile.Enabled;
            this.buttonConfigureDataLogDirectoryPath.Enabled = this.checkBoxDataLoggingToSeparateFile.Checked
               && this.checkBoxDataLoggingToSeparateFile.Enabled;
        }

        private void ButtonConfigureLogDirectoryPath_Click(object sender, EventArgs e)
        {
            ButtonConfigureLogDirectoryPath_Click(sender, e, this.textBoxLogDirectoryPath, "log");
        }

        private void ButtonConfigureDataLogDirectoryPath_Click(object sender, EventArgs e)
        {
            ButtonConfigureLogDirectoryPath_Click(sender, e, this.textBoxDataLogDirectoryPath, "data log");
        }

        private void ButtonConfigureLogDirectoryPath_Click(object sender, EventArgs e, TextBox textbox, String customText)
        {
            using (var folderBrowseDialog = new FolderBrowserDialog())
            {
                if (textbox.Text.Length == 0)
                {
                    folderBrowseDialog.SelectedPath = Path.GetDirectoryName(AssemblyUtils.GetApplicationPath());
                }
                else
                {
                    folderBrowseDialog.SelectedPath = textbox.Text;
                }

                folderBrowseDialog.ShowNewFolderButton = true;
                folderBrowseDialog.Description = string.Format("Select a directory where to save the {0} files.", customText);

                var result = folderBrowseDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    // If the selected path is in a subdirectory of the application path then keep only the relative path.
                    if (folderBrowseDialog.SelectedPath.StartsWith(Path.GetDirectoryName(AssemblyUtils.GetApplicationPath())))
                    {
                        textbox.Text = folderBrowseDialog.SelectedPath.Substring(Path.GetDirectoryName(AssemblyUtils.GetApplicationPath()).Length);
                    }
                    else
                    {
                        textbox.Text = folderBrowseDialog.SelectedPath;
                    }
                }
            }
        }

        private void OnSettingsChanged()
        {
            SettingsChangedEventHandler?.Invoke(this, new SettingsChangedEventArgs(this.settings));
        }

        private void CheckBoxNotificationsEnable_CheckedChanged(object sender, EventArgs e)
        {
            this.labelTemperatureNotification.Enabled = this.checkBoxNotificationsEnable.Checked;
            this.labelRadiationNotification.Enabled = this.checkBoxNotificationsEnable.Checked;
            this.textBoxHighTemperatureNotificationValue.Enabled = this.checkBoxNotificationsEnable.Checked;
            this.textBoxRadiationNotificationValue.Enabled = this.checkBoxNotificationsEnable.Checked;
            this.comboBoxTemperatureNotificationUnit.Enabled = this.checkBoxNotificationsEnable.Checked;

            var deviceSettings = this.settings.Devices.First();

            if (deviceSettings.IsNull())
            {
                return;
            }

            var radiationDetectorName = RadiationDetector.Normalize(deviceSettings.GetRadiationDetectorName());
            var isKnownRadiationDetector = radiationDetectorName.IsNotNull() && RadiationDetector.IsKnown(radiationDetectorName);

            if (!isKnownRadiationDetector)
            {
                this.comboBoxRadiationNotificationUnit.Enabled = false;

                if (this.checkBoxNotificationsEnable.Checked)
                {
                    this.pictureBoxNotificationsInfo.Show();
                    this.labelNotificationsInfo.Show();
                }
                else
                {
                    this.pictureBoxNotificationsInfo.Hide();
                    this.labelNotificationsInfo.Hide();
                }
            }
            else
            {
                this.comboBoxRadiationNotificationUnit.Enabled = this.checkBoxNotificationsEnable.Checked;
                this.pictureBoxNotificationsInfo.Hide();
                this.labelNotificationsInfo.Hide();
            }

            if (this.checkBoxNotificationsEnable.Checked)
            {
                HighTemperatureNotificationValue_TextChanged(sender, e);
                RadiationNotificationValue_TextChanged(sender, e);
            }
        }

        private void HighTemperatureNotificationValue_TextChanged(object sender, EventArgs e)
        {
            bool isValid = this.TemperatureNotificationValueIsValid(this.textBoxHighTemperatureNotificationValue.Text, (TemperatureUnitType)Enum.Parse(typeof(TemperatureUnitType), this.comboBoxTemperatureNotificationUnit.SelectedItem.ToString(), true));

            if (!isValid)
            {
                var toolTip = new ToolTip
                {
                    IsBalloon = true,
                    ToolTipIcon = ToolTipIcon.Error,
                    ToolTipTitle = "Incorrect value"
                };

                var message = new StringBuilder("Value for Temperature must be a number between ");
                message.Append((this.comboBoxTemperatureNotificationUnit.SelectedIndex == 0) ? "0 and 60." : "32 and 140.");
                message.AppendFormat("\nDecimal sign ({0}) is allowed.", numberFormatInfo.NumberDecimalSeparator);

                toolTip.Show(message.ToString(), this.textBoxRadiationNotificationValue, 0, -102, 5000);
            }

            this.UpdateApplyButtonState();
        }

        private void RadiationNotificationValue_TextChanged(object sender, EventArgs e)
        {
            bool isValid = this.RadiationNotificationValueIsValid(this.textBoxRadiationNotificationValue.Text);

            if (!isValid)
            {
                var toolTip = new ToolTip
                {
                    IsBalloon = true,
                    ToolTipIcon = ToolTipIcon.Error,
                    ToolTipTitle = "Incorrect value"
                };

                toolTip.Show(string.Format("Value for Radiation must be a number between 0 and 999999.\nDecimal sign ({0}) is allowed.", numberFormatInfo.NumberDecimalSeparator), this.textBoxRadiationNotificationValue, 0, -90, 5000);
            }

            this.UpdateApplyButtonState();
        }

        private bool TemperatureNotificationValueIsValid(string temperature, TemperatureUnitType unit)
        {
            bool isDecimal = decimal.TryParse(temperature, NumberStyles.AllowDecimalPoint, numberFormatInfo, out decimal value);

            // Device working temperature is between -20 °C and +60 °C.
            // For High Temperature notification we allow only the positive part of the interval.
            return (isDecimal &&
                    ((unit == TemperatureUnitType.Celsius && value >= 0.0m && value <= 60.0m) ||
                    (unit == TemperatureUnitType.Fahrenheit && value >= 32.0m && value <= 140.0m)));
        }

        private bool RadiationNotificationValueIsValid(string radiation)
        {
            bool isDecimal = decimal.TryParse(radiation, NumberStyles.AllowDecimalPoint, numberFormatInfo, out decimal value);

            return (isDecimal && (value >= 0.0m && value <= 999999.9m));
        }

        private void UpdateApplyButtonState()
        {
            this.buttonApply.Enabled = InputControlsValuesAreValid();
        }

        private bool InputControlsValuesAreValid()
        {
            return this.LoggingDirectoryIsValid() &&
                this.TemperatureNotificationValueIsValid(this.textBoxHighTemperatureNotificationValue.Text, (TemperatureUnitType)Enum.Parse(typeof(TemperatureUnitType), this.comboBoxTemperatureNotificationUnit.SelectedItem.ToString(), true)) &&
                this.RadiationNotificationValueIsValid(this.textBoxRadiationNotificationValue.Text);
        }

        private void ComboBoxTemperatureNotificationUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MathX.IsInteger(this.textBoxHighTemperatureNotificationValue.Text))
            {
                if (temperatureNotificationUnitSelectedIndex == 0 && this.comboBoxTemperatureNotificationUnit.SelectedIndex == 1)
                {
                    // Round to the nearest integer.
                    // Try to maintain precision when converting back-and-forth to/from the same value.
                    this.textBoxHighTemperatureNotificationValue.Text = Math.Round(Temperature.CelsiusToFahrenheit(double.Parse(this.textBoxHighTemperatureNotificationValue.Text)), 0).ToString();
                }
                else if (temperatureNotificationUnitSelectedIndex == 1 && this.comboBoxTemperatureNotificationUnit.SelectedIndex == 0)
                {
                    // Round to the nearest integer. 
                    // Try to maintain precision when converting back-and-forth to/from the same value.
                    this.textBoxHighTemperatureNotificationValue.Text = Math.Round(Temperature.FahrenheitToCelsius(double.Parse(this.textBoxHighTemperatureNotificationValue.Text)), 0).ToString();
                }
                else
                {
                    // Leave the value unchanged.
                }
            }

            this.temperatureNotificationUnitSelectedIndex = this.comboBoxTemperatureNotificationUnit.SelectedIndex;
        }

        private void ComboBoxRadiationNotificationUnit_SelectedIndexChanged(object sender, EventArgs e)
        {
            var deviceSettings = this.settings.Devices.First();

            if (deviceSettings.IsNull())
            {
                return;
            }

            if (MathX.IsDecimal(this.textBoxRadiationNotificationValue.Text, numberFormatInfo))
            {
                var radiationDetector = RadiationDetector.GetByName(deviceSettings.GetRadiationDetectorName());

                if (radiationNotificationUnitSelectedIndex == 0 && this.comboBoxRadiationNotificationUnit.SelectedIndex == 1)
                {
                    this.textBoxRadiationNotificationValue.Text = MathX.Truncate(Radiation.CpmToMicroSvPerHour(double.Parse(this.textBoxRadiationNotificationValue.Text), radiationDetector.ConversionFactor), 4).ToString();
                }
                else if (radiationNotificationUnitSelectedIndex == 0 && this.comboBoxRadiationNotificationUnit.SelectedIndex == 2)
                {
                    this.textBoxRadiationNotificationValue.Text = MathX.Truncate(Radiation.CpmToMicroRemPerHour(double.Parse(this.textBoxRadiationNotificationValue.Text), radiationDetector.ConversionFactor), 2).ToString();
                }
                else if (radiationNotificationUnitSelectedIndex == 1 && this.comboBoxRadiationNotificationUnit.SelectedIndex == 0)
                {
                    // Round to the nearest integer when converting to cpm.
                    // Try to maintain precision when converting back-and-forth to/from the same value.
                    this.textBoxRadiationNotificationValue.Text = Math.Round(Radiation.MicroSvPerHourToCpm(double.Parse(this.textBoxRadiationNotificationValue.Text), radiationDetector.ConversionFactor), 0).ToString();
                }
                else if (radiationNotificationUnitSelectedIndex == 1 && this.comboBoxRadiationNotificationUnit.SelectedIndex == 2)
                {
                    this.textBoxRadiationNotificationValue.Text = MathX.Truncate(Radiation.MicroSvPerHourToMicroRemPerHour(double.Parse(this.textBoxRadiationNotificationValue.Text)), 2).ToString();
                }
                else if (radiationNotificationUnitSelectedIndex == 2 && this.comboBoxRadiationNotificationUnit.SelectedIndex == 0)
                {
                    // Round to the nearest integer when converting to cpm. 
                    // Try to maintain precision when converting back-and-forth to/from the same value.
                    this.textBoxRadiationNotificationValue.Text = Math.Round(Radiation.MicroRemPerHourToCpm(double.Parse(this.textBoxRadiationNotificationValue.Text), radiationDetector.ConversionFactor), 0).ToString();
                }
                else if (radiationNotificationUnitSelectedIndex == 2 && this.comboBoxRadiationNotificationUnit.SelectedIndex == 1)
                {
                    this.textBoxRadiationNotificationValue.Text = MathX.Truncate(Radiation.MicroRemPerHourToMicroSvPerHour(double.Parse(this.textBoxRadiationNotificationValue.Text)), 4).ToString();
                }
                else
                {
                    // Leave the value unchanged.
                }
            }

            this.radiationNotificationUnitSelectedIndex = this.comboBoxRadiationNotificationUnit.SelectedIndex;
        }
    }
}
