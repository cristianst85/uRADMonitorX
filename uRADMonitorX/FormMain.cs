using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using FluentScheduler;
using uRADMonitorX.Commons;
using uRADMonitorX.Commons.Controls;
using uRADMonitorX.Commons.Formatting;
using uRADMonitorX.Commons.Logging;
using uRADMonitorX.Commons.Networking;
using uRADMonitorX.Configuration;
using uRADMonitorX.Core;
using uRADMonitorX.Core.Device;
using uRADMonitorX.Core.Fetchers;
using uRADMonitorX.Updater;
using uRADMonitorX.Windows;
using FluentScheduler.Model;

namespace uRADMonitorX {

    public partial class FormMain : Form {

        public event SettingsChangedEventHandler SettingsChangedEventHandler;

        private IDeviceDataFetcher deviceDataFetcher = null;
        private ITimeSpanFormatter timeSpanFormatter = new TimeSpanFormatter();
        private ILogger logger = null;
        private ISettings settings = null;
        private IDeviceDataReaderFactory deviceDataReaderFactory = null;

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

        private int mLastWindowXPos;
        private int mLastWindowYPos;
        private volatile bool allowVisible;

        private DateTime? notifyIconBalloonLastShownAt = null;

        public FormMain(IDeviceDataReaderFactory deviceDataReaderFactory, ISettings settings, ILogger logger) {
            try {
                InitializeComponent();

                this.deviceDataReaderFactory = deviceDataReaderFactory;
                this.settings = settings;
                this.logger = logger;

                this.notifyIcon.Icon = (System.Drawing.Icon)global::uRADMonitorX.Properties.Resources.RadiationDisabled;

                // Fix status strip right padding.
                this.statusStrip.Padding = new Padding(3, this.statusStrip.Padding.Top, 3, this.statusStrip.Padding.Bottom);

                this.toolStripStatusLabelDeviceStatus.Text = "Loading...";
                this.toolStripStatusLabelDeviceUptime.Text = "n/a";

                Version version = AssemblyUtils.GetVersion();
                this.Text = this.Text.Replace("{version}", String.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Build));
#if DEBUG
                if (EnvironmentUtils.IsMonoRuntime()) {
                    this.Text += " (Mono)";
                }
#endif

                // Pre-init.
                // From settings.
                if (String.IsNullOrEmpty(this.settings.DeviceIPAddress)) {
                    this.enablePollingToolStripMenuItem.Enabled = false;
                    this.viewDeviceWebpageToolStripMenuItem.Enabled = false;
                    this.viewDeviceWebpageToolStripMenuItem1.Enabled = false;
                }

                this.enablePollingToolStripMenuItem.Checked = this.settings.IsPollingEnabled;
                this.labelPressure.Enabled = this.settings.HasPressureSensor;
                this.pressureToolStripMenuItem.Enabled = this.settings.HasPressureSensor;

                this.StartPosition = FormStartPosition.Manual;
                this.restoreWindowPosition(this.settings.LastWindowXPos, this.settings.LastWindowYPos);

                Debug.WriteLine(String.Format("FormMain > Settings.StartMinimized: {0}", this.settings.StartMinimized.ToString().ToLower()));
                Debug.WriteLine(String.Format("FormMain > Settings.ShowInTaskbar: {0}", this.settings.ShowInTaskbar.ToString().ToLower()));

                this.allowVisible = !this.settings.StartMinimized;

                if (this.settings.StartMinimized) {
                    this.WindowState = FormWindowState.Minimized;
                    this.ShowInTaskbar = false;
                }
                else {
                    this.WindowState = FormWindowState.Normal;
                    this.ShowInTaskbar = this.settings.ShowInTaskbar;
                }

                this.viewDeviceOnlineDataToolStripMenuItem.Enabled = false;

                // Handlers
                this.FormClosing += new FormClosingEventHandler(this.formMain_Closing);
                this.enablePollingToolStripMenuItem.CheckedChanged += new EventHandler(enablePollingToolStripMenuItem_CheckedChanged);
                this.viewOnlyTextBoxId.TextChanged += new EventHandler(viewOnlyTextBoxId_TextChanged);

                Thread startupThread = new Thread(new ThreadStart(delegate { this.initDevice(false); }));
                startupThread.Name = "initDeviceThread";
                startupThread.Start();

                TaskManager.Initialize(new Registry());
                configureCheckForUpdatesTask();
            }
            catch (Exception ex) {
                Debug.WriteLine(String.Format("FormMain > Exception: {0}", ex.ToString()));
                this.logger.Write(String.Format("Exception: {0}", ex.ToString()));
            }
            finally {
                this.IsReady = true;
            }
        }

        private void configureCheckForUpdatesTask() {
            if (settings.AutomaticallyCheckForUpdates) {
                if (TaskManager.GetSchedule("checkForUpdates") == null) {
                    TaskManager.AddTask(
                        () => {
                            try {
                                GitHubApplicationUpdater applicationUpdater = new GitHubApplicationUpdater(Program.UpdaterUrl);
                                ApplicationUpdateInfo applicationUpdateInfo = applicationUpdater.Check();
                                if (applicationUpdateInfo.IsNewVersionAvailable(AssemblyUtils.GetVersion())) {
                                    this.notifyIcon.ShowBalloonTip(10000, "uRADMonitorX Update Available", String.Format("A new version of uRADMonitorX ({0}) is available.", applicationUpdateInfo.Version), ToolTipIcon.Info);
                                }
                            }
                            catch {
                                // Silently ignore all errors when automatically checking for updates.
                                // There's no need to annoy users with these.
                            }
                        },
                        (task) => task.WithName("checkForUpdates").ToRunOnceAt(DateTime.Now.AddMinutes(2)).AndEvery(Program.UpdaterInterval).Minutes()
                    );
                }
            }
            else {
                TaskManager.RemoveTask("checkForUpdates");
            }
        }

        protected override void SetVisibleCore(bool value) {
            Debug.WriteLine(String.Format("FormMain > SetVisibleCore({0})", value.ToString().ToLower()));
            Debug.WriteLine(String.Format("FormMain > allowVisible: {0}", this.allowVisible.ToString().ToLower()));
            if (value && !this.IsHandleCreated) {
                CreateHandle();
            }
            if (!this.allowVisible) {
                value = false;
            }
            base.SetVisibleCore(value);
            this.allowVisible = true;
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

                if (!String.IsNullOrEmpty(this.settings.DeviceIPAddress)) {
                    IDeviceDataReader deviceDataReader = deviceDataReaderFactory.Create();
                    IPollingStrategy pollingStrategy = null;
                    if (this.settings.PollingType == Core.PollingType.WDTSync) {
                        pollingStrategy = new WDTSyncPollingStrategy(60);
                    }
                    else {
                        pollingStrategy = new FixedIntervalPollingStrategy(this.settings.PollingInterval);
                    }
                    deviceDataFetcher = new DeviceDataFetcher(deviceDataReader, pollingStrategy);
                    deviceDataFetcher.DeviceDataFetcherEventHandler += new DeviceDataFetcherEventHandler(deviceDataFetcher_DeviceDataFetcherEventHandler);
                    deviceDataFetcher.DeviceDataFetcherErrorEventHandler += new DeviceDataFetcherErrorEventHandler(deviceDataFetcher_DeviceDataFetcherErrorEventHandler);
                    if (this.enablePollingToolStripMenuItem.Checked) {
                        deviceDataFetcher.Start();
                        if (!isRestart) {
                            this.updateDeviceStatus(String.Format("Connecting to {0}...", this.settings.DeviceIPAddress));
                        }
                        this.notifyIcon.Icon = (System.Drawing.Icon)global::uRADMonitorX.Properties.Resources.RadiationIcon;
                    }
                    else {
                        this.updateDeviceStatus("Polling is disabled.");
                        this.updateNotifyIconText("Polling is disabled.");
                        this.notifyIcon.Icon = (System.Drawing.Icon)global::uRADMonitorX.Properties.Resources.RadiationDisabled;
                    }
                }
                else {
                    this.updateDeviceStatus("Device is not configured.");
                    this.updateNotifyIconText("Device is not configured.");
                    this.notifyIcon.Icon = (System.Drawing.Icon)global::uRADMonitorX.Properties.Resources.RadiationDisabled;
                }
            }
        }

        private void updateDeviceInformation(DeviceInformation deviceInformation) {
            this.viewOnlyTextBoxId.Text = deviceInformation.DeviceID;
            this.viewOnlyTextBoxModel.Text = String.Format("{0} ({1})", (int)deviceInformation.DeviceModel, deviceInformation.DeviceModel);
            this.viewOnlyTextBoxHardware.Text = String.Format("{0}", deviceInformation.HardwareVersion);
            this.viewOnlyTextBoxFirmware.Text = String.Format("{0}", deviceInformation.FirmwareVersion);
            this.viewOnlyTextBoxDetector.Text = deviceInformation.Detector;
        }

        private void updateDeviceUptime(int seconds) {
            this.toolStripStatusLabelDeviceUptime.Text = String.Format("{0}", this.timeSpanFormatter.Format(TimeSpan.FromSeconds(seconds)));
            this.statusStrip.Update();
        }

        private delegate void updateDeviceStatusCallback(String status);

        private void updateDeviceStatus(String status) {
            if (!this.IsClosing) {
                if (((ISynchronizeInvoke)this.statusStrip).InvokeRequired) {
                    this.Invoke(new updateDeviceStatusCallback(updateDeviceStatus), new object[] { status });
                }
                else {
                    this.toolStripStatusLabelDeviceStatus.Text = status;
                    this.statusStrip.Update();
                }
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
            // Update icon and ToolTip text for notification area icon.
            this.notifyIcon.Icon = (System.Drawing.Icon)global::uRADMonitorX.Properties.Resources.RadiationWithError;
            this.updateNotifyIconText("Device data fetch error.");
        }

        private void updateNotifyIconText() {
            this.updateNotifyIconText(null);
        }

        private void updateNotifyIconText(String message) {
            StringBuilder notifyIconText = new StringBuilder(this.Text); // Get form title text.
            if (message != null) {
                notifyIconText.Append(String.Format("\n{0}", message));
            }
            NotifyIconUtils.SetText(this.notifyIcon, notifyIconText.ToString());
        }

        private void deviceDataFetcher_DeviceDataFetcherEventHandler(object sender, DeviceDataFetcherEventArgs e) {
            if (!this.IsClosing) {
                this.Invoke(new updateDeviceDataCallback(updateDeviceData), new object[] { e.DeviceData });
            }
        }

        private delegate void updateDeviceDataCallback(DeviceData deviceData);

        private void updateDeviceData(DeviceData deviceData) {
            // Do not update data if user disables polling before first update.
            if (!this.settings.IsPollingEnabled) {
                return;
            }
            this.updateDeviceInformation(deviceData.DeviceInformation);

            // Use normalized name only for conversions.
            String radiationDetectorName = RadiationDetector.Normalize(deviceData.DeviceInformation.Detector);
            if (this.settings.RadiationUnitType == Core.RadiationUnitType.Cpm) {
                this.viewOnlyTextBoxRadiation.Text = String.Format("{0} cpm", deviceData.Radiation);
                this.viewOnlyTextBoxRadiationAverage.Text = String.Format("{0} cpm", deviceData.RadiationAverage);
            }
            else {
                if (RadiationDetector.IsKnown(radiationDetectorName)) {
                    RadiationDetector radiationDetector = RadiationDetector.GetByName(radiationDetectorName);
                    if (this.settings.RadiationUnitType == Core.RadiationUnitType.uSvH) {
                        this.viewOnlyTextBoxRadiation.Text = String.Format("{0} µSv/h", MathX.Truncate(Radiation.CpmToMicroSvPerHour(deviceData.Radiation, radiationDetector.Factor), 4));
                        this.viewOnlyTextBoxRadiationAverage.Text = String.Format("{0} µSv/h", MathX.Truncate(Radiation.CpmToMicroSvPerHour((double)deviceData.RadiationAverage, radiationDetector.Factor), 4));
                    }
                    else if (this.settings.RadiationUnitType == Core.RadiationUnitType.uRemH) {
                        this.viewOnlyTextBoxRadiation.Text = String.Format("{0} µrem/h", MathX.Truncate(Radiation.CpmToMicroRemPerHour(deviceData.Radiation, radiationDetector.Factor), 2));
                        this.viewOnlyTextBoxRadiationAverage.Text = String.Format("{0} µrem/h", MathX.Truncate(Radiation.CpmToMicroRemPerHour((double)deviceData.RadiationAverage, radiationDetector.Factor), 2));
                    }
                    else {
                        // If conversion to other radiation unit type is not implemented fallback silently to cpm.
                        this.settings.RadiationUnitType = Core.RadiationUnitType.Cpm;
                        this.settings.Commit();
                        this.viewOnlyTextBoxRadiation.Text = String.Format("{0} cpm", deviceData.Radiation);
                        this.viewOnlyTextBoxRadiationAverage.Text = String.Format("{0} cpm", deviceData.RadiationAverage);
                    }
                }
                else {
                    // If detector is not known fallback silently to cpm.
                    this.settings.RadiationUnitType = Core.RadiationUnitType.Cpm;
                    this.settings.Commit();
                    this.viewOnlyTextBoxRadiation.Text = String.Format("{0} cpm", deviceData.Radiation);
                    this.viewOnlyTextBoxRadiationAverage.Text = String.Format("{0} cpm", deviceData.RadiationAverage);
                }
            }

            if (this.settings.TemperatureUnitType == Core.TemperatureUnitType.Celsius) {
                this.viewOnlyTextBoxTemperature.Text = String.Format("{0} °C", deviceData.Temperature);
            }
            else if (this.settings.TemperatureUnitType == Core.TemperatureUnitType.Fahrenheit) {
                this.viewOnlyTextBoxTemperature.Text = String.Format("{0} °F", Temperature.CelsiusToFahrenheit(deviceData.Temperature));
            }
            else {
                // If conversion to other temperature unit type is not implemented fallback silently to default (Celsius).
                this.settings.TemperatureUnitType = Core.TemperatureUnitType.Celsius;
                this.settings.Commit();
                this.viewOnlyTextBoxTemperature.Text = String.Format("{0} °C", deviceData.Temperature);
            }
            if (deviceData.Pressure.HasValue) {
                this.labelPressure.Enabled = true;
                this.pressureToolStripMenuItem.Enabled = true;
                if (this.settings.PressureUnitType == Core.PressureUnitType.Pa) {
                    this.viewOnlyTextBoxPressure.Text = String.Format("{0} Pa", deviceData.Pressure.Value);
                }
                else if (this.settings.PressureUnitType == Core.PressureUnitType.kPa) {
                    this.viewOnlyTextBoxPressure.Text = String.Format("{0} kPa", Pressure.PascalToKiloPascal(deviceData.Pressure.Value));
                }
                else {
                    // If conversion to other pressure unit type is not implemented fallback silently to default (Pascal).
                    this.settings.PressureUnitType = Core.PressureUnitType.Pa;
                    this.settings.Commit();
                    this.viewOnlyTextBoxPressure.Text = String.Format("{0} Pa", deviceData.Pressure.Value);
                }
            }
            else {
                this.labelPressure.Enabled = false;
                this.pressureToolStripMenuItem.Enabled = false;
                this.viewOnlyTextBoxPressure.Text = String.Empty;
            }

            // Update Settings.
            bool commitSettings = false;
            if (this.settings.HasPressureSensor != deviceData.Pressure.HasValue) {
                this.settings.HasPressureSensor = deviceData.Pressure.HasValue;
                commitSettings = true;
            }
            if (!(radiationDetectorName).Equals(this.settings.DetectorName)) {
                this.settings.DetectorName = radiationDetectorName;
                // If the detector is known and the radiation notification value was not set fallback to default values.
                if (this.settings.DetectorName != null &&
                        RadiationDetector.IsKnown(this.settings.DetectorName) &&
                        this.settings.RadiationNotificationUnitType == RadiationUnitType.Cpm &&
                        this.settings.RadiationNotificationValue == 0) {
                    this.settings.RadiationNotificationValue = DefaultSettings.RadiationNotificationValue;
                    this.settings.RadiationNotificationUnitType = DefaultSettings.RadiationNotificationUnitType;
                }
                commitSettings = true;
            }
            if (commitSettings) {
                this.settings.Commit();
            }

            this.viewOnlyTextBoxVoltage.Text = String.Format("{0} V ({1}%)", deviceData.Voltage, deviceData.VoltagePercent);
            this.viewOnlyTextBoxWDT.Text = String.Format("{0} s", deviceData.WDT);
            this.updateDeviceStatus(String.Format("Device received {0} from server.", (HttpStatus.GetReason(int.Parse(deviceData.ServerResponseCode)) != null) ? String.Format("{0} ({1})", deviceData.ServerResponseCode, HttpStatus.GetReason(int.Parse(deviceData.ServerResponseCode))) : deviceData.ServerResponseCode));

            // Update ToolTip text for notification area icon.
            StringBuilder notifyIconText = new StringBuilder(String.Format("Radiation: {0}, Average: {1}\nTemperature: {2}", this.viewOnlyTextBoxRadiation.Text, this.viewOnlyTextBoxRadiationAverage.Text, this.viewOnlyTextBoxTemperature.Text));
            if (settings.HasPressureSensor) {
                notifyIconText.Append(String.Format(", Pressure: {0}", this.viewOnlyTextBoxPressure.Text));
            }
            this.updateNotifyIconText(notifyIconText.ToString());

            this.updateDeviceUptime(deviceData.Uptime);

            if (deviceData.ServerResponseCode != HttpStatus.OK.Code.ToString()) {
                this.logger.Write(String.Format("Device received {0} from server (WDT={1})", deviceData.ServerResponseCode, deviceData.WDT));
                this.notifyIcon.Icon = (System.Drawing.Icon)global::uRADMonitorX.Properties.Resources.RadiationWithError;
                if (deviceData.WDT > 59) {
                    this.updateNotifyIconText("Device cannot send data to server...");
                }
                else {
                    // TODO: Ask Radu about this strange behavior (deviceData.ServerResponseCode == 0 && (WDT == 59 || WDT == 60)).
                }
            }
            else {
                this.notifyIcon.Icon = (System.Drawing.Icon)global::uRADMonitorX.Properties.Resources.RadiationIcon;
            }

            if (this.settings.AreNotificationsEnabled) {
                double currentTemperature = deviceData.Temperature;
                String currentTemperatureUnit = "C";

                if (this.settings.TemperatureNotificationUnitType == TemperatureUnitType.Fahrenheit) {
                    currentTemperature = Temperature.CelsiusToFahrenheit(deviceData.Temperature);
                    currentTemperatureUnit = "F";
                }

                double currentRadiation = deviceData.Radiation;
                String currentRadiationUnit = EnumHelper.GetEnumDescription<RadiationUnitType>(RadiationUnitType.Cpm);
                if (RadiationDetector.IsKnown(radiationDetectorName)) {
                    RadiationDetector radiationDetector = RadiationDetector.GetByName(radiationDetectorName);
                    if (this.settings.RadiationNotificationUnitType == RadiationUnitType.uSvH) {
                        currentRadiation = Radiation.CpmToMicroSvPerHour(currentRadiation, radiationDetector.Factor);
                    }
                    else if (this.settings.RadiationNotificationUnitType == RadiationUnitType.uRemH) {
                        currentRadiation = Radiation.CpmToMicroRemPerHour(currentRadiation, radiationDetector.Factor);
                    }
                    currentRadiationUnit = EnumHelper.GetEnumDescription<RadiationUnitType>(this.settings.RadiationNotificationUnitType);
                }
                else {
                    if (this.settings.RadiationNotificationUnitType != RadiationUnitType.Cpm) {
                        currentRadiation = 0; // Disable radiation notification if radiation value cannot be converted to uSv/h or uRem/h.
                    }
                }

                bool showBalloon = false;
                String balloonTitle = String.Empty;
                String balloonMessage = String.Empty;
                if (currentTemperature >= this.settings.HighTemperatureNotificationValue) {
                    balloonTitle = "High Temperature";
                    balloonMessage = String.Format("Temperature: {0} °{1}", currentTemperature, currentTemperatureUnit);
                    showBalloon = true;
                }
                if (this.settings.RadiationNotificationValue != 0 && currentRadiation >= this.settings.RadiationNotificationValue) {
                    if (balloonTitle.Length > 0) {
                        balloonTitle += "/";
                    }
                    if (balloonMessage.Length > 0) {
                        balloonMessage += "\n";
                    }
                    balloonTitle += "Radiation";
                    balloonMessage += String.Format("Radiation: {0} {1}", currentRadiation, currentRadiationUnit);
                    showBalloon = true;
                }
                if (showBalloon) {
                    balloonTitle += " Alert";

                    DateTime now = DateTime.UtcNow;
                    DateTime dataReadingsTimeStamp = now.AddSeconds(-(deviceData.WDT % 60));

                    // Add the date and time when the event occurs to notification message. This is useful because
                    // Windows queues notifications when user is away from computer (e.g.: screen is locked).
                    // LINK: https://msdn.microsoft.com/en-us/library/windows/desktop/ee330740%28v=vs.85%29.aspx
                    balloonMessage += String.Format("\nEvent occurred at {0}.", now.ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CurrentCulture));

                    // Do not show multiple notifications for the same readings. Usually when polling interval is lower than the device refresh period (60 seconds).
                    if (!notifyIconBalloonLastShownAt.HasValue || (this.notifyIconBalloonLastShownAt.HasValue && now.Subtract(this.notifyIconBalloonLastShownAt.Value).TotalSeconds >= 60)) {
                        this.notifyIcon.ShowBalloonTip(10000, balloonTitle, balloonMessage, ToolTipIcon.Info);
                        this.notifyIconBalloonLastShownAt = dataReadingsTimeStamp;
                    }
                }
            }
        }

        private void enablePollingToolStripMenuItem_CheckedChanged(object sender, EventArgs e) {
            if (this.enablePollingToolStripMenuItem.Checked) {
                deviceDataFetcher.Start();
                this.updateDeviceStatus(String.Format("Connecting to {0}...", this.settings.DeviceIPAddress));
                this.updateNotifyIconText(String.Format("Connecting to {0}...", this.settings.DeviceIPAddress));
            }
            else {
                deviceDataFetcher.Stop();
                this.updateDeviceStatus("Polling is disabled.");
                this.updateNotifyIconText("Polling is disabled.");
            }
        }

        // Toogle 'Enable polling'.
        private void enablePollingToolStripMenuItem_Click(object sender, EventArgs e) {
            this.enablePollingToolStripMenuItem.Checked = !this.enablePollingToolStripMenuItem.Checked;
            this.settings.IsPollingEnabled = this.enablePollingToolStripMenuItem.Checked;
            this.settings.Commit();
            this.notifyIcon.Icon = this.settings.IsPollingEnabled ?
                ((System.Drawing.Icon)global::uRADMonitorX.Properties.Resources.RadiationIcon) :
                ((System.Drawing.Icon)global::uRADMonitorX.Properties.Resources.RadiationDisabled);
        }

        private void configurationToolStripMenuItem_Click(object sender, EventArgs e) {
            using (FormDeviceConfiguration form = new FormDeviceConfiguration(this.settings)) {
                DialogResult result = form.ShowDialog(this);
                if (result == System.Windows.Forms.DialogResult.OK) {
                    if (!String.IsNullOrEmpty(this.settings.DeviceIPAddress)) {
                        this.enablePollingToolStripMenuItem.Enabled = true;
                        this.viewDeviceWebpageToolStripMenuItem.Enabled = true;
                        this.viewDeviceWebpageToolStripMenuItem1.Enabled = true;
                        this.initDevice(true);
                    }
                }
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e) {
            using (FormOptions form = new FormOptions(this.settings)) {
                form.SettingsChangedEventHandler += new SettingsChangedEventHandler(form_SettingsChangedEventHandler);
                DialogResult result = form.ShowDialog(this);
            }
        }

        private void form_SettingsChangedEventHandler(object sender, SettingsChangedEventArgs e) {
            // this.ShowInTaskbar = this.settings.ShowInTaskbar; // TODO: fix this not to flicker.
            SettingsChangedEventHandler handler = SettingsChangedEventHandler;
            if (handler != null) {
                handler(this, new SettingsChangedEventArgs(this.settings));
            }
            configureCheckForUpdatesTask();
        }

        private void closeApplication(object sender, EventArgs e) {
            this.IsClosing = true;
            this.contextMenuStrip.Dispose();
            this.Close();
        }

        private void formMain_Closing(object sender, FormClosingEventArgs e) {
            Debug.WriteLine("FormMain > formMain_Closing()");
            this.saveWindowPosition();
            if (e.CloseReason != CloseReason.UserClosing) {
                this.IsClosing = true;
                this.saveWindowPosition(true);
                return;
            }
            if (this.settings.CloseToSystemTray && !this.IsClosing) {
                this.minimizeToTray();
                this.Hide();
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
                this.toogleWindow();
            }
            else {
                base.WndProc(ref m);
            }
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
                this.settings.LastWindowXPos = mLastWindowXPos;
                this.settings.LastWindowYPos = mLastWindowYPos;
                this.settings.Commit();
            }
        }

        private void showWindow() {
            if (this.WindowState == FormWindowState.Minimized) {
                this.ShowInTaskbar = this.settings.ShowInTaskbar;
                this.Show();
                this.BringToFront();
                this.WindowState = FormWindowState.Normal;
                this.Visible = true;
                this.restoreWindowPosition();
            }
            else {
                this.TopMost = true; // Force to come on top.
                this.Show();
                this.BringToFront();
            }
            this.TopMost = false; // Don't stay on top forever. Allow it to go in background.
        }

        private void minimizeToTray() {
            Debug.WriteLine("FormMain > minimizeToTray()");
            this.ShowInTaskbar = this.settings.ShowInTaskbar;
            this.WindowState = FormWindowState.Minimized;
            this.Visible = false;
            this.Hide();
        }

        private void toogleWindow() {
            Debug.WriteLine("FormMain > toogleWindow()");
            this.ShowInTaskbar = this.settings.ShowInTaskbar;
            if (this.WindowState == FormWindowState.Minimized) {
                this.Show();
                this.BringToFront();
                this.WindowState = FormWindowState.Normal;
                this.Visible = true;
                this.restoreWindowPosition();
            }
            else {
                this.Hide();
                this.saveWindowPosition();
                this.Visible = false;
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
                this.showWindow();
            }
        }

        private void viewDeviceWebpageToolStripMenuItem_Click(object sender, EventArgs e) {
            System.Diagnostics.Process.Start(String.Format("http://{0}/", this.settings.DeviceIPAddress));
        }

        private void viewDeviceWebpageToolStripMenuItem1_Click(object sender, EventArgs e) {
            System.Diagnostics.Process.Start(String.Format("http://{0}/", this.settings.DeviceIPAddress));
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

        private void contextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e) {
            // FIXME: Don't crash (right-click on the notification icon) when running on Mono.
            if (!EnvironmentUtils.IsMonoRuntime()) {
                this.contextMenuStrip.Show(Cursor.Position, ToolStripDropDownDirection.AboveRight);
            }
            if (this.WindowState == FormWindowState.Normal) {
                this.showWindow();
            }
        }

        private void viewOnlyTextBoxId_TextChanged(object sender, EventArgs e) {
            this.viewDeviceOnlineDataToolStripMenuItem.Enabled = this.viewOnlyTextBoxId.Text.Length > 0;
        }

        private void graphToolStripMenuItem_Click(object sender, EventArgs e) {
            if (this.viewOnlyTextBoxId.Text.Length > 0) {
                System.Diagnostics.Process.Start(String.Format("http://www.uradmonitor.com?open={0}", this.viewOnlyTextBoxId.Text));
            }
        }

        private void viewDeviceOnlineDataToolStripMenuItem_Click(object sender, EventArgs e) {
            if (this.viewOnlyTextBoxId.Text.Length > 0) {
                openDataAPILink(this.viewOnlyTextBoxId.Text, ((ToolStripMenuItem)sender).Text.ToLower());
            }
        }

        private void openDataAPILink(String deviceId, String sensorData) {
            System.Diagnostics.Process.Start(String.Format("http://data.uradmonitor.com/api/v1/devices/{0}/{1}", deviceId, sensorData));
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e) {
            using (FormUpdate form = new FormUpdate(this.logger)) {
                form.Update();
                DialogResult result = form.ShowDialog(this);
            }
        }
    }
}