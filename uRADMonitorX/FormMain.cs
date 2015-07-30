using System;
using System.Diagnostics;
using System.IO;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms;
using uRADMonitorX.Commons;
using uRADMonitorX.Commons.Formatting;
using uRADMonitorX.Commons.Logging;
using uRADMonitorX.Commons.Logging.Appenders;
using uRADMonitorX.Commons.Networking;
using uRADMonitorX.Configuration;
using uRADMonitorX.Core.Device;
using uRADMonitorX.Core.Fetchers;
using uRADMonitorX.Windows;

namespace uRADMonitorX {

    public partial class FormMain : Form {

        public static String SettingsFilePath { get; private set; }
        public static ISettings Settings { get; private set; }

        private IDeviceDataFetcher deviceDataFetcher = null;
        private ITimeSpanFormatter timeSpanFormatter = new TimeSpanFormatter();
        private ILogger logger = null;

        private volatile bool _isClosing;
        public bool IsClosing {
            get {
                return _isClosing;
            }
            private set {
                _isClosing = value;
            }
        }

        private volatile bool _isReady;
        public bool IsReady {
            get {
                return _isReady;
            }
            private set {
                _isReady = value;
            }
        }

        private FormWindowState mLastState;
        private int mLastWindowXPos;
        private int mLastWindowYPos;

        public FormMain() {
            try {
                InitializeComponent();

                this.notifyIcon.Icon = ((System.Drawing.Icon)global::uRADMonitorX.Properties.Resources.RadiationColor);

                // Fix status strip right padding.
                this.statusStrip.Padding = new Padding(3, this.statusStrip.Padding.Top, 3, this.statusStrip.Padding.Bottom);

                this.toolStripStatusLabelDeviceStatus.Text = "Loading...";
                this.toolStripStatusLabelDeviceUptime.Text = "n/a";

                if (AssemblyUtils.GetVersion().Major == 0) {
                    this.Text += " - Preview";
                }

                // Pre-init.
                SettingsFilePath = String.Format("{0}{1}{2}", Path.GetDirectoryName(new Uri(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath), Path.DirectorySeparatorChar, Program.SettingsFileName);
                if (File.Exists(SettingsFilePath)) {
                    Settings = XMLSettings.LoadFromFile(SettingsFilePath);
                }
                else {
                    XMLSettings.CreateFile(SettingsFilePath);
                    Settings = XMLSettings.LoadFromFile(SettingsFilePath);
                }

                this.logger = LoggerManager.GetInstance().GetLogger(Program.LoggerName);
                this.logger.Enabled = Settings.IsLoggingEnabled;
                this.configLogger(this.logger);

                // Pre-init.
                // From settings.
                if (String.IsNullOrEmpty(Settings.DeviceIPAddress)) {
                    this.enablePollingToolStripMenuItem.Enabled = false;
                    this.viewDeviceWebpageToolStripMenuItem.Enabled = false;
                    this.viewDeviceWebpageToolStripMenuItem1.Enabled = false;
                    this.notifyIcon.Icon = ((System.Drawing.Icon)global::uRADMonitorX.Properties.Resources.RadiationGray_v3);
                }
                this.enablePollingToolStripMenuItem.Checked = Settings.IsPollingEnabled;
                this.labelPressure.Enabled = Settings.HasPressureSensor;

                this.mLastState = this.WindowState;
                if (Settings.StartMinimized) {
                    this.WindowState = FormWindowState.Minimized;
                }
                this.StartPosition = FormStartPosition.Manual;
                this.restoreWindowPosition(Settings.LastWindowXPos, Settings.LastWindowYPos);
                this.ShowInTaskbar = Settings.ShowInTaskbar;

                // Handlers
                this.FormClosing += new FormClosingEventHandler(this.formMain_Closing);
                this.enablePollingToolStripMenuItem.CheckedChanged += new EventHandler(enablePollingToolStripMenuItem_CheckedChanged);

                this.registerAtWindowsStartup();

                Thread startupThread = new Thread(new ThreadStart(delegate { this.initDevice(false); }));
                startupThread.Name = "startupThread";
                startupThread.Start();
            }
            catch (Exception ex) {
                Debug.WriteLine(String.Format("Exception: {0}", ex.ToString()));
                this.logger.Write(String.Format("Exception: {0}", ex.ToString()));
            }
            finally {
                this.IsReady = true;
            }
        }

        private void configLogger(ILogger logger) {
            try {
                ILoggerAppender appender = LoggerManager.GetInstance().GetLogger(Program.LoggerName).Appender;
                if (appender is ICanReconfigureAppender) {
                    // TODO: Verify if logger path is in application root directory.
                    if (Settings.LogDirectoryPath.Length > 0) {
                        ((ICanReconfigureAppender)appender).Reconfigure(Path.Combine(Settings.LogDirectoryPath, Program.LoggerFilePath));
                    }
                    else {
                        ((ICanReconfigureAppender)appender).Reconfigure(Program.LoggerFilePath);
                    }
                }
            }
            catch (UnauthorizedAccessException ex) {
                logger.Write(String.Format("Cannot reconfigure logger appender. Exception: {0}", ex.ToString()));
            }
        }

        private void registerAtWindowsStartup() {
            try {
                if (Settings.StartWithWindows) {
                    Registry.RegisterAtWindowsStartup(Application.ProductName, String.Format("\"{0}\"", new Uri(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath));
                }
                else {
                    Registry.UnRegisterAtWindowsStartup(Application.CompanyName);
                }
            }
            catch (Exception e) {
                logger.Write(String.Format("Error registering application to start at windows startup. Exception: {0}", e.ToString()));
            }
        }

        private void initDevice(bool isRestart) {
            while (!this.IsReady) {
                Thread.Sleep(100);
            }

            if (!IsClosing) {
                if (this.deviceDataFetcher != null) {
                    this.deviceDataFetcher.Stop();
                    this.deviceDataFetcher.DeviceDataFetcherEventHandler -= deviceDataFetcher_DeviceDataFetcherEventHandler;
                    this.deviceDataFetcher.DeviceDataFetcherErrorEventHandler -= deviceDataFetcher_DeviceDataFetcherErrorEventHandler;
                }

                if (!String.IsNullOrEmpty(Settings.DeviceIPAddress)) {
                    IDeviceDataReader deviceDataReader = new DeviceDataHttpReader(Settings.DeviceIPAddress);
                    IPollingStrategy pollingStrategy = null;
                    if (Settings.PollingType == Core.PollingType.WDTSync) {
                        pollingStrategy = new WDTSyncPollingStrategy(60);
                    }
                    else {
                        pollingStrategy = new FixedIntervalPollingStrategy(Settings.PollingInterval);
                    }
                    deviceDataFetcher = new DeviceDataFetcher(deviceDataReader, pollingStrategy);
                    deviceDataFetcher.DeviceDataFetcherEventHandler += new DeviceDataFetcherEventHandler(deviceDataFetcher_DeviceDataFetcherEventHandler);
                    deviceDataFetcher.DeviceDataFetcherErrorEventHandler += new DeviceDataFetcherErrorEventHandler(deviceDataFetcher_DeviceDataFetcherErrorEventHandler);
                    if (this.enablePollingToolStripMenuItem.Checked) {
                        deviceDataFetcher.Start();
                        if (!isRestart) {
                            this.updateDeviceStatus(String.Format("Connecting to {0}...", Settings.DeviceIPAddress));
                        }
                        this.notifyIcon.Icon = ((System.Drawing.Icon)global::uRADMonitorX.Properties.Resources.RadiationColor);
                    }
                    else {
                        this.updateDeviceStatus("Polling is disabled.");
                        this.notifyIcon.Icon = ((System.Drawing.Icon)global::uRADMonitorX.Properties.Resources.RadiationGray_v3);
                    }
                }
                else {
                    this.updateDeviceStatus("Device is not configured.");
                    this.notifyIcon.Icon = ((System.Drawing.Icon)global::uRADMonitorX.Properties.Resources.RadiationGray_v3);
                }
            }
        }

        private void updateDeviceInformation(DeviceInformation deviceInformation) {
            this.viewOnlyTextBoxId.Text = deviceInformation.DeviceID;
            this.viewOnlyTextBoxModel.Text = String.Format("{0}", deviceInformation.DeviceType);
            this.viewOnlyTextBoxHardware.Text = String.Format("{0}", deviceInformation.HardwareVersion);
            this.viewOnlyTextBoxFirmware.Text = String.Format("{0}", deviceInformation.FirmwareVersion);
            this.viewOnlyTextBoxDetector.Text = deviceInformation.Detector;
        }

        private void updateDeviceUptime(int seconds) {
            this.toolStripStatusLabelDeviceUptime.Text = String.Format("{0}", this.timeSpanFormatter.Format(TimeSpan.FromSeconds(seconds)));
            this.statusStrip.Update();
        }

        private void updateDeviceStatus(String status) {
            if (!this.IsClosing) {
                this.Invoke(new updateDeviceStatusCallback(updateDeviceStatusTS), new object[] { status });
            }
        }

        private void deviceDataFetcher_DeviceDataFetcherErrorEventHandler(object sender, DeviceDataFetcherErrorEventArgs e) {
            if (!this.IsClosing) {
                this.Invoke(new updateDeviceStatusOnErrorCallback(updateDeviceStatusOnError), new object[] { e.Exception });
            }
        }

        private delegate void updateDeviceStatusOnErrorCallback(Exception ex);

        private void updateDeviceStatusOnError(Exception ex) {
            this.logger.Write(String.Format("Device data fetch error. Exception: {0}", ex.ToString()));
            this.toolStripStatusLabelDeviceStatus.Text = "Device data fetch error.";
            this.statusStrip.Update();
        }

        private delegate void updateDeviceStatusCallback(String status);

        public void updateDeviceStatusTS(String status) {
            this.toolStripStatusLabelDeviceStatus.Text = status;
            this.statusStrip.Update();
        }

        private void deviceDataFetcher_DeviceDataFetcherEventHandler(object sender, DeviceDataFetcherEventArgs e) {
            if (!this.IsClosing) {
                this.Invoke(new updateDeviceDataCallback(updateDeviceData), new object[] { e.DeviceData });
            }
        }

        private delegate void updateDeviceDataCallback(DeviceData deviceData);

        private void updateDeviceData(DeviceData deviceData) {
            // Do not update data if user disables polling before first update.
            if (!Settings.IsPollingEnabled) {
                return;
            }
            this.updateDeviceInformation(deviceData.DeviceInformation);
            if (Settings.RadiationUnitType == Core.RadiationUnitType.Cpm) {
                this.viewOnlyTextBoxRadiation.Text = String.Format("{0} cpm", deviceData.Radiation);
                this.viewOnlyTextBoxRadiationAverage.Text = String.Format("{0} cpm", deviceData.RadiationAverage);
            }
            else if (Settings.RadiationUnitType == Core.RadiationUnitType.uSvH && RadiationDetector.IsKnown(deviceData.DeviceInformation.Detector)) {
                this.viewOnlyTextBoxRadiation.Text = String.Format("{0} µSv/h", MathX.Truncate(Radiation.CpmToMicroSvPerHour(deviceData.Radiation, RadiationDetector.GetByName(deviceData.DeviceInformation.Detector).Factor), 4));
                this.viewOnlyTextBoxRadiationAverage.Text = String.Format("{0} µSv/h", MathX.Truncate(Radiation.CpmToMicroSvPerHour((double)deviceData.RadiationAverage, RadiationDetector.GetByName(deviceData.DeviceInformation.Detector).Factor), 4));
            }
            else if (Settings.RadiationUnitType == Core.RadiationUnitType.uRemH && RadiationDetector.IsKnown(deviceData.DeviceInformation.Detector)) {
                this.viewOnlyTextBoxRadiation.Text = String.Format("{0} µrem/h", MathX.Truncate(Radiation.CpmToMicroRemPerHour(deviceData.Radiation, RadiationDetector.GetByName(deviceData.DeviceInformation.Detector).Factor), 2));
                this.viewOnlyTextBoxRadiationAverage.Text = String.Format("{0} µrem/h", MathX.Truncate(Radiation.CpmToMicroRemPerHour((double)deviceData.RadiationAverage, RadiationDetector.GetByName(deviceData.DeviceInformation.Detector).Factor), 2));
            }
            else {
                // Other values defaults to Cpm.
                Settings.RadiationUnitType = Core.RadiationUnitType.Cpm;
                Settings.Commit();
                this.viewOnlyTextBoxRadiation.Text = String.Format("{0} CPM", deviceData.Radiation);
                this.viewOnlyTextBoxRadiationAverage.Text = String.Format("{0} CPM", deviceData.RadiationAverage);
            }

            if (Settings.TemperatureUnitType == Core.TemperatureUnitType.Celsius) {
                this.viewOnlyTextBoxTemperature.Text = String.Format("{0} C", deviceData.Temperature);
            }
            else if (Settings.TemperatureUnitType == Core.TemperatureUnitType.Fahrenheit) {
                this.viewOnlyTextBoxTemperature.Text = String.Format("{0} F", Temperature.CelsiusToFahrenheit(deviceData.Temperature));
            }
            else {
                // Other values defaults to Celsius.
                Settings.TemperatureUnitType = Core.TemperatureUnitType.Celsius;
                Settings.Commit();
                this.viewOnlyTextBoxTemperature.Text = String.Format("{0} C", deviceData.Temperature);
            }
            if (deviceData.Pressure.HasValue) {
                this.labelPressure.Enabled = true;
                if (Settings.PressureUnitType == Core.PressureUnitType.Pa) {
                    this.viewOnlyTextBoxPressure.Text = String.Format("{0} Pa", deviceData.Pressure.Value);
                }
                else if (Settings.PressureUnitType == Core.PressureUnitType.kPa) {
                    this.viewOnlyTextBoxPressure.Text = String.Format("{0} kPa", Pressure.PascalToKiloPascal(deviceData.Pressure.Value));
                }
                else {
                    // Other values defaults to Pascal.
                    Settings.PressureUnitType = Core.PressureUnitType.Pa;
                    Settings.Commit();
                    this.viewOnlyTextBoxPressure.Text = String.Format("{0} Pa", deviceData.Pressure.Value);
                }
            }
            else {
                this.labelPressure.Enabled = true;
                this.viewOnlyTextBoxPressure.Text = String.Empty;
            }

            // Update settings.
            if (Settings.HasPressureSensor != deviceData.Pressure.HasValue) {
                Settings.HasPressureSensor = deviceData.Pressure.HasValue;
                Settings.Commit();
            }

            this.viewOnlyTextBoxVoltage.Text = String.Format("{0} V ({1}%)", deviceData.Voltage, deviceData.VoltagePercent);
            this.viewOnlyTextBoxWDT.Text = String.Format("{0} s", deviceData.WDT);
            this.updateDeviceStatus(String.Format("Device received {0} from server.", (HttpStatus.GetReason(int.Parse(deviceData.ServerResponseCode)) != null) ? String.Format("{0} ({1})", deviceData.ServerResponseCode, HttpStatus.GetReason(int.Parse(deviceData.ServerResponseCode))) : deviceData.ServerResponseCode));
            this.updateDeviceUptime(deviceData.Uptime);
        }

        private void enablePollingToolStripMenuItem_CheckedChanged(object sender, EventArgs e) {
            if (this.enablePollingToolStripMenuItem.Checked) {
                deviceDataFetcher.Start();
                this.updateDeviceStatus(String.Format("Connecting to {0}...", Settings.DeviceIPAddress));
            }
            else {
                deviceDataFetcher.Stop();
                this.updateDeviceStatus("Polling is disabled.");
            }
        }

        // Toogle 'Enable polling'.
        private void enablePollingToolStripMenuItem_Click(object sender, EventArgs e) {
            this.enablePollingToolStripMenuItem.Checked = !this.enablePollingToolStripMenuItem.Checked;
            Settings.IsPollingEnabled = this.enablePollingToolStripMenuItem.Checked;
            Settings.Commit();
            this.notifyIcon.Icon = Settings.IsPollingEnabled ?
                ((System.Drawing.Icon)global::uRADMonitorX.Properties.Resources.RadiationColor) :
                ((System.Drawing.Icon)global::uRADMonitorX.Properties.Resources.RadiationGray_v3);
        }

        private void configurationToolStripMenuItem_Click(object sender, EventArgs e) {
            using (FormDeviceConfiguration form = new FormDeviceConfiguration(Settings)) {
                DialogResult result = form.ShowDialog(this);
                if (result == System.Windows.Forms.DialogResult.OK) {
                    if (!String.IsNullOrEmpty(Settings.DeviceIPAddress)) {
                        this.enablePollingToolStripMenuItem.Enabled = true;
                        this.viewDeviceWebpageToolStripMenuItem.Enabled = true;
                        this.viewDeviceWebpageToolStripMenuItem1.Enabled = true;
                        this.initDevice(true);
                    }
                }
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e) {
            using (FormOptions form = new FormOptions(Settings)) {
                form.SettingsChangedEventHandler += new SettingsChangedEventHandler(form_SettingsChangedEventHandler);
                DialogResult result = form.ShowDialog(this);
            }
        }

        private void form_SettingsChangedEventHandler(object sender, SettingsChangedEventArgs e) {
            this.registerAtWindowsStartup();
            this.logger.Enabled = e.Settings.IsLoggingEnabled;
            this.configLogger(this.logger);
        }

        private void closeApplication(object sender, EventArgs e) {
            this.IsClosing = true;
            this.contextMenuStrip.Dispose();
            this.Close();
        }

        private void formMain_Closing(object sender, FormClosingEventArgs e) {
            this.saveWindowPosition();
            if (e.CloseReason != CloseReason.UserClosing) {
                this.IsClosing = true;
                this.saveWindowPosition(true);
                return;
            }

            if (Settings.CloseToSystemTray && !this.IsClosing) {
                this.toogleWindow();
                e.Cancel = true;
            }
            else {
                this.IsClosing = true;
                this.saveWindowPosition(true);
            }
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref System.Windows.Forms.Message m) {
            if (m.Msg == (int)NativeMethods.WM.SYSCOMMAND && m.WParam.ToInt32() == (int)NativeMethods.SC.MINIMIZE) {
                this.saveWindowPosition();
            }
            base.WndProc(ref m);
        }

        protected override void OnClientSizeChanged(EventArgs e) {
            if (Settings != null) {
                this.ShowInTaskbar = Settings.ShowInTaskbar;
                if (this.WindowState == FormWindowState.Minimized) {
                    // If form is not shown in the taskbar then set visible to false.
                    if (!this.ShowInTaskbar) {
                        this.Visible = false;
                    }
                }
                else {
                    this.mLastState = this.WindowState;
                }
            }
            base.OnClientSizeChanged(e);
        }

        private void restoreWindowPosition() {
            this.restoreWindowPosition(mLastWindowXPos, mLastWindowYPos);
        }
        private void restoreWindowPosition(int x, int y) {
            this.Top = y;
            this.Left = x;
            this.mLastWindowYPos = this.Top;
            this.mLastWindowXPos = this.Left;
        }

        private void saveWindowPosition() {
            this.saveWindowPosition(false);
        }

        private void saveWindowPosition(bool commitToSettings) {
            if (this.WindowState != FormWindowState.Minimized) {
                this.mLastWindowYPos = this.Top;
                this.mLastWindowXPos = this.Left;
            }
            if (commitToSettings) {
                Settings.LastWindowXPos = mLastWindowXPos;
                Settings.LastWindowYPos = mLastWindowYPos;
                Settings.Commit();
            }
        }

        private void toogleWindow() {
            this.ShowInTaskbar = Settings.ShowInTaskbar;
            if (this.WindowState == FormWindowState.Minimized) {
                this.Show();
                this.BringToFront();
                this.WindowState = this.mLastState;
                this.Visible = true;
                this.TopMost = true;
                this.restoreWindowPosition();
            }
            else {
                this.TopMost = false;
                this.Visible = false;
                this.Hide();
                this.mLastState = this.WindowState;
                this.WindowState = FormWindowState.Minimized;
            }
        }

        private void showHideToolStripMenuItem_Click(object sender, EventArgs e) {
            toogleWindow();
        }

        private void showHideToolStripMenuItem1_Click(object sender, EventArgs e) {
            toogleWindow();
        }

        private void notifyIcon_MouseClick(object sender, MouseEventArgs e) {
            if (e.Button == System.Windows.Forms.MouseButtons.Left) {
                this.toogleWindow();
            }
        }

        private void viewDeviceWebpageToolStripMenuItem_Click(object sender, EventArgs e) {
            System.Diagnostics.Process.Start(String.Format("http://{0}/", Settings.DeviceIPAddress));
        }

        private void viewDeviceWebpageToolStripMenuItem1_Click(object sender, EventArgs e) {
            System.Diagnostics.Process.Start(String.Format("http://{0}/", Settings.DeviceIPAddress));
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            using (FormAbout form = new FormAbout()) {
                form.Update();
                DialogResult result = form.ShowDialog(this);
            }
        }

        private void uRADMonitorWebsiteToolStripMenuItem_Click(object sender, EventArgs e) {
            System.Diagnostics.Process.Start("http://www.uradmonitor.com/");
        }

        private void uRADMonitorForumToolStripMenuItem_Click(object sender, EventArgs e) {
            System.Diagnostics.Process.Start("http://www.uradmonitor.com/forums/");
        }
    }
}