using System;
using System.IO;
using System.Windows.Forms;
using uRADMonitorX.Commons;
using uRADMonitorX.Configuration;
using uRADMonitorX.Core;

namespace uRADMonitorX {

    public partial class FormOptions : Form {

        private ISettings settings;
        private bool areSettingsValid;

        public event SettingsChangedEventHandler SettingsChangedEventHandler;

        public FormOptions(ISettings settings) {
            InitializeComponent();

            this.settings = settings;

            this.checkBoxStartWithWindows.Checked = settings.StartWithWindows;

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
                // Other values defaults to Cpm.
                this.comboBoxRadiationUnit.SelectedIndex = 0;
            }

            this.comboBoxPressureUnit.Enabled = settings.HasPressureSensor;
            this.labelPressureUnit.Enabled = settings.HasPressureSensor;

            this.checkBoxStartWithWindows.CheckedChanged += new EventHandler(settingsChanged);
            this.checkBoxLoggingEnable.CheckedChanged += new EventHandler(settingsChanged);
            this.checkBoxStartMinimized.CheckedChanged += new EventHandler(settingsChanged);
            this.checkBoxCloseToSystemTray.CheckedChanged += new EventHandler(settingsChanged);
            this.checkBoxShowInTaskbar.CheckedChanged += new EventHandler(settingsChanged);
            this.comboBoxTemperatureUnit.SelectedIndexChanged += new EventHandler(settingsChanged);
            this.comboBoxPressureUnit.SelectedIndexChanged += new EventHandler(settingsChanged);
            this.comboBoxRadiationUnit.SelectedIndexChanged += new EventHandler(settingsChanged);

            this.textBoxLogDirectoryPath.TextChanged += new EventHandler(textBoxLogDirectoryPathTextChanged);
            this.buttonConfigureLogDirectoryPath.Click += new System.EventHandler(this.buttonConfigureLogDirectoryPath_Click);
            this.checkBoxLoggingEnable.CheckedChanged += new EventHandler(checkBoxLoggingEnable_CheckedChanged);

            this.checkBoxLoggingEnable_CheckedChanged(null, null);
            this.areSettingsValid = true;
            this.buttonApply.Enabled = false;
        }

        private void settingsChanged(Object sender, EventArgs e) {
            if (this.areSettingsValid) {
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
            if (this.checkBoxLoggingEnable.Checked && this.textBoxLogDirectoryPath.Text.Length > 0 && Directory.Exists(this.textBoxLogDirectoryPath.Text)) {
                this.pictureBoxLoggingInfo.Hide();
                this.labelLoggingInfo.Hide();
                this.areSettingsValid = true;
                this.settingsChanged(sender, e);
            }
            else if (!this.checkBoxLoggingEnable.Checked) {
                this.pictureBoxLoggingInfo.Hide();
                this.labelLoggingInfo.Hide();
                this.areSettingsValid = true;
                this.settingsChanged(sender, e);
            }
            else if (this.textBoxLogDirectoryPath.Text.Length > 0 && !Directory.Exists(this.textBoxLogDirectoryPath.Text)) {
                this.labelLoggingInfo.Text = "Directory does not exist.";
                this.pictureBoxLoggingInfo.Image = global::uRADMonitorX.Properties.Resources.error;
                this.pictureBoxLoggingInfo.Show();
                this.labelLoggingInfo.Show();
                this.areSettingsValid = false;
                this.settingsChanged(sender, e);
            }
            else {
                this.labelLoggingInfo.Text = "Using default application path.";
                this.pictureBoxLoggingInfo.Image = global::uRADMonitorX.Properties.Resources.information;
                this.pictureBoxLoggingInfo.Show();
                this.labelLoggingInfo.Show();
                this.areSettingsValid = true;
                this.settingsChanged(sender, e);
            }
        }

        private void saveSettings() {
            this.settings.StartWithWindows = this.checkBoxStartWithWindows.Checked;

            this.settings.IsLoggingEnabled = this.checkBoxLoggingEnable.Checked;
            this.settings.LogDirectoryPath = this.textBoxLogDirectoryPath.Text;

            this.settings.ShowInTaskbar = this.checkBoxShowInTaskbar.Checked;
            this.settings.StartMinimized = this.checkBoxStartMinimized.Checked;
            this.settings.CloseToSystemTray = this.checkBoxCloseToSystemTray.Checked;
            this.settings.TemperatureUnitType = (TemperatureUnitType)Enum.Parse(typeof(TemperatureUnitType), this.comboBoxTemperatureUnit.SelectedItem.ToString(), true);
            this.settings.PressureUnitType = (PressureUnitType)Enum.Parse(typeof(PressureUnitType), this.comboBoxPressureUnit.SelectedItem.ToString(), true);
            this.settings.RadiationUnitType = EnumHelper.GetEnumByDescription<RadiationUnitType>(this.comboBoxRadiationUnit.SelectedItem.ToString(), true);
            this.settings.Commit();

            this.onSettingsChanged();
        }

        private void checkBoxLoggingEnable_CheckedChanged(object sender, EventArgs e) {
            this.textBoxLogDirectoryPath.Enabled = this.checkBoxLoggingEnable.Checked;
            this.buttonConfigureLogDirectoryPath.Enabled = this.checkBoxLoggingEnable.Checked;
            textBoxLogDirectoryPathTextChanged(sender, e);
        }

        private void buttonConfigureLogDirectoryPath_Click(object sender, EventArgs e) {
            FolderBrowserDialog folderBrowseDialog = new FolderBrowserDialog();
            try {
                if (this.textBoxLogDirectoryPath.Text.Length == 0) {
                    folderBrowseDialog.SelectedPath = AppDomain.CurrentDomain.BaseDirectory;
                }
                else {
                    folderBrowseDialog.SelectedPath = this.textBoxLogDirectoryPath.Text;
                }
                folderBrowseDialog.ShowNewFolderButton = true;
                folderBrowseDialog.Description = "Select a directory to save log files.";
                DialogResult result = folderBrowseDialog.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK) {
                    this.textBoxLogDirectoryPath.Text = folderBrowseDialog.SelectedPath;
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
    }
}
