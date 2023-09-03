using FluentScheduler;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using uRADMonitorX.Commons;
using uRADMonitorX.Commons.Controls;
using uRADMonitorX.Commons.Formatting;
using uRADMonitorX.Commons.Logging;
using uRADMonitorX.Commons.Networking;
using uRADMonitorX.Configuration;
using uRADMonitorX.Core;
using uRADMonitorX.Core.Device;
using uRADMonitorX.Core.Jobs;
using uRADMonitorX.Extensions;
using uRADMonitorX.uRADMonitor.Services;
using uRADMonitorX.Windows;

namespace uRADMonitorX
{
    public partial class FormMain : Form
    {
        public event SettingsChangedEventHandler SettingsChangedEventHandler;

        private readonly ITimeSpanFormatter timeSpanFormatter = new TimeSpanFormatter();

        private ILogger logger = null;
        private ISettings settings = null;

        private IDeviceDataReaderFactory deviceDataReaderFactory = null;
        private IDeviceDataJobFactory deviceDataJobFactory = null;
        private IDeviceDataJob deviceDataJob = null;

        private IDeviceServiceFactory deviceServiceFactory = null;

        private volatile bool _isClosing;
        public bool IsClosing
        {
            get
            {
                return _isClosing;
            }
            private set
            {
                _isClosing = value;
            }
        }

        private volatile bool _isReady;
        public bool IsReady
        {
            get
            {
                return _isReady;
            }
            private set
            {
                _isReady = value;
            }
        }

        private Point WindowPosition { get; set; }

        private volatile bool allowVisible;

        private DateTime? notifyIconBalloonLastShownAt = null;
        private DateTime? lastDataReadingTimestamp = null;

        public FormMain(IDeviceDataReaderFactory deviceDataReaderFactory,
            IDeviceDataJobFactory deviceDataJobFactory,
            IDeviceServiceFactory deviceServiceFactory,
            ISettings settings,
            ILogger logger)
        {
            try
            {
                InitializeComponent();

                this.deviceDataReaderFactory = deviceDataReaderFactory;
                this.deviceDataJobFactory = deviceDataJobFactory;
                this.deviceServiceFactory = deviceServiceFactory;
                this.settings = settings;
                this.logger = logger;

                logger.Write($"Application is starting...");

                this.notifyIcon.Icon = Properties.Resources.RadiationDisabled;

                // Fix status strip right padding.
                this.statusStrip.Padding = new Padding(3, this.statusStrip.Padding.Top, 3, this.statusStrip.Padding.Bottom);

                this.toolStripStatusLabelDeviceStatus.Text = "Loading...";
                this.toolStripStatusLabelDeviceUptime.Text = "n/a";

                var version = AssemblyUtils.GetVersion();
                this.Text = this.Text.Replace("{version}", version.ToString(3));

                logger.Write($"Application version is {AssemblyUtils.GetProductVersion()}");

                bool isDebug = false;
                Debug.Assert(isDebug = true);

                if (isDebug && EnvironmentUtils.IsMonoRuntime())
                {
                    this.Text += " (Mono)";
                }

                // Pre-initialization.
                var deviceSettings = this.settings.Devices.First();

                // Update controls based on current settings.
                if (deviceSettings.EndpointUrl.IsNullOrEmpty())
                {
                    this.enablePollingToolStripMenuItem.Enabled = false;
                    this.viewDeviceWebpageToolStripMenuItem.Enabled = false;
                    this.viewDeviceWebpageContextMenuToolStripMenuItem.Enabled = false;
                }

                this.enablePollingToolStripMenuItem.Checked = this.settings.IsPollingEnabled;

                var deviceCapabilities = deviceSettings.GetDeviceCapabilities();

                this.labelPressure.Enabled = deviceCapabilities.HasFlag(DeviceCapability.Pressure);
                this.pressureToolStripMenuItem.Enabled = deviceCapabilities.HasFlag(DeviceCapability.Pressure);

                this.StartPosition = FormStartPosition.Manual;
                this.WindowPosition = this.settings.Display.WindowPosition;
                RestoreWindowOnScreen();

                Debug.WriteLine($"[{Program.ApplicationName}] [{nameof(FormMain)}] Settings.StartMinimized: {this.settings.Display.StartMinimized.ToString().ToLowerInvariant()}");
                Debug.WriteLine($"[{Program.ApplicationName}] [{nameof(FormMain)}] Settings.ShowInTaskbar: {this.settings.Display.ShowInTaskbar.ToString().ToLowerInvariant()}");

                this.allowVisible = !this.settings.Display.StartMinimized;

                if (this.settings.Display.StartMinimized)
                {
                    this.WindowState = FormWindowState.Minimized;
                    this.ShowInTaskbar = false;
                }
                else
                {
                    this.WindowState = FormWindowState.Normal;
                    this.ShowInTaskbar = this.settings.Display.ShowInTaskbar;
                }

                this.viewDeviceOnlineDataToolStripMenuItem.Enabled = false;

                // Handlers.
                this.FormClosing += new FormClosingEventHandler(this.FormMain_Closing);
                this.enablePollingToolStripMenuItem.CheckedChanged += new EventHandler(EnablePollingToolStripMenuItem_CheckedChanged);
                this.viewOnlyTextBoxId.TextChanged += new EventHandler(ViewOnlyTextBoxId_TextChanged);

                var startupThread = new Thread(new ThreadStart(delegate { this.InitDevice(false); }))
                {
                    Name = "InitDeviceThread"
                };

                startupThread.Start();

                JobManager.Initialize(new Registry());

                ConfigureCheckForUpdatesJob();

                logger.Write($"Application was started successfully");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[{Program.ApplicationName}] [{nameof(FormMain)}] Exception: {ex.ToString()}");

                this.logger.Write(string.Format("Exception: {0}", ex.ToString()));
            }
            finally
            {
                this.IsReady = true;
            }
        }

        private void ConfigureCheckForUpdatesJob()
        {
            if (settings.General.AutomaticallyCheckForUpdates)
            {
                if (JobManager.GetSchedule("checkForUpdates").IsNull())
                {
                    JobManager.AddJob(
                        () =>
                        {
                            try
                            {
                                var updateInfo = Program.ApplicationUpdater.Check(Program.UpdateUrl);

                                if (updateInfo.IsNewVersionAvailable(AssemblyUtils.GetVersion()))
                                {
                                    this.notifyIcon.ShowBalloonTip(10000, "uRADMonitorX Update Available", string.Format("A new version of uRADMonitorX ({0}) is available.", updateInfo.Version), ToolTipIcon.Info);
                                }
                            }
                            catch
                            {
                                // Silently ignore all errors when automatically checking for updates.
                            }
                        },
                        (job) => job.WithName("checkForUpdates").ToRunOnceAt(DateTime.Now.AddMinutes(2)).AndEvery(Program.UpdaterInterval).Minutes()
                    );
                }
            }
            else
            {
                JobManager.RemoveJob("checkForUpdates");
            }
        }

        protected override void SetVisibleCore(bool value)
        {
            Debug.WriteLine($"[{Program.ApplicationName}] [{nameof(FormMain)}] SetVisibleCore to {value.ToString().ToLowerInvariant()}");
            Debug.WriteLine($"[{Program.ApplicationName}] [{nameof(FormMain)}] AllowVisible is {allowVisible.ToString().ToLowerInvariant()}");

            if (value && !this.IsHandleCreated)
            {
                CreateHandle();
            }

            if (!allowVisible)
            {
                value = false;
            }

            base.SetVisibleCore(value);
            allowVisible = true;
        }

        private void InitDevice(bool isRestart)
        {
            while (!this.IsReady)
            {
                Thread.Sleep(100);
            }

            if (!IsClosing)
            {
                if (this.deviceDataJob.IsNotNull())
                {
                    this.deviceDataJob.Stop();
                    this.deviceDataJob.DeviceDataJobEventHandler -= DeviceDataJob_DeviceDataJobEventHandler;
                    this.deviceDataJob.DeviceDataJobErrorEventHandler -= DeviceDataJob_DeviceDataJobErrorEventHandler;
                    this.deviceDataJob.Dispose();
                }

                if (this.settings.Devices.First().EndpointUrl.IsNotNullOrEmpty())
                {
                    deviceDataJob = deviceDataJobFactory.Create();
                    deviceDataJob.DeviceDataJobEventHandler += new DeviceDataJobEventHandler(DeviceDataJob_DeviceDataJobEventHandler);
                    deviceDataJob.DeviceDataJobErrorEventHandler += new DeviceDataJobErrorEventHandler(DeviceDataJob_DeviceDataJobErrorEventHandler);

                    if (this.enablePollingToolStripMenuItem.Checked)
                    {
                        deviceDataJob.Start();

                        if (!isRestart || this.toolStripStatusLabelDeviceStatus.Text.Equals(Properties.Resources.DeviceIsNotConfigured))
                        {
                            this.UpdateDeviceStatus(string.Format("Connecting to {0}...", this.settings.Devices.First().EndpointUrl));
                        }

                        this.notifyIcon.Icon = Properties.Resources.RadiationIcon;
                    }
                    else
                    {
                        this.UpdateDeviceStatus(Properties.Resources.PollingIsDisabled);
                        this.UpdateNotifyIconText(Properties.Resources.PollingIsDisabled);
                        this.notifyIcon.Icon = Properties.Resources.RadiationDisabled;
                    }
                }
                else
                {
                    this.UpdateDeviceStatus(Properties.Resources.DeviceIsNotConfigured);
                    this.UpdateNotifyIconText(Properties.Resources.DeviceIsNotConfigured);
                    this.notifyIcon.Icon = Properties.Resources.RadiationDisabled;
                }
            }
        }

        private void UpdateDeviceInformation(DeviceInformation deviceInformation)
        {
            this.viewOnlyTextBoxId.Text = deviceInformation.DeviceID;
            this.viewOnlyTextBoxModel.Text = string.Format("{0} ({1})", (int)deviceInformation.DeviceModel, deviceInformation.DeviceModel);
            this.viewOnlyTextBoxHardware.Text = string.Format("{0}", deviceInformation.HardwareVersion);
            this.viewOnlyTextBoxFirmware.Text = string.Format("{0}", deviceInformation.FirmwareVersion);
            this.viewOnlyTextBoxDetector.Text = deviceInformation.Detector;
        }

        private void UpdateDeviceUptime(int seconds)
        {
            this.toolStripStatusLabelDeviceUptime.Text = string.Format("{0}", this.timeSpanFormatter.Format(TimeSpan.FromSeconds(seconds)));
            this.statusStrip.Update();
        }

        private delegate void updateDeviceStatusCallback(string status);

        private void UpdateDeviceStatus(string status)
        {
            if (!this.IsClosing)
            {
                if (((ISynchronizeInvoke)this.statusStrip).InvokeRequired)
                {
                    this.Invoke(new updateDeviceStatusCallback(UpdateDeviceStatus), new object[] { status });
                }
                else
                {
                    this.toolStripStatusLabelDeviceStatus.Text = status;
                    this.statusStrip.Update();
                }
            }
        }

        private void DeviceDataJob_DeviceDataJobErrorEventHandler(object sender, DeviceDataJobErrorEventArgs e)
        {
            if (!this.IsClosing)
            {
                this.Invoke(new updateDeviceStatusOnErrorCallback(UpdateDeviceStatusOnError), new object[] { e.Exception });
            }
        }

        private delegate void updateDeviceStatusOnErrorCallback(Exception ex);

        private void UpdateDeviceStatusOnError(Exception ex)
        {
            // Do not update UI if user disables polling before first update.
            if (!this.settings.IsPollingEnabled)
            {
                return;
            }

            this.logger.Write(string.Format("Device data fetch error. Exception: {0}", ex.ToString()));

            this.toolStripStatusLabelDeviceStatus.Text = "Device data fetch error.";
            this.statusStrip.Update();

            // Update icon and ToolTip text for notification area icon.
            this.notifyIcon.Icon = Properties.Resources.RadiationWithError;
            var notifyText = "Device data fetch error.";

            if (this.lastDataReadingTimestamp.HasValue)
            {
                notifyText += string.Format("\nLast successful data fetch was at {0}", this.lastDataReadingTimestamp.Value.ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CurrentCulture));
            }

            this.UpdateNotifyIconText(notifyText);
        }

        private void UpdateNotifyIconText()
        {
            this.UpdateNotifyIconText(null);
        }

        private void UpdateNotifyIconText(String message)
        {
            // Get form title text.
            var notifyIconText = new StringBuilder(this.Text);

            if (message != null)
            {
                notifyIconText.Append(string.Format("\n{0}", message));
            }

            NotifyIconUtils.SetText(this.notifyIcon, notifyIconText.ToString());
        }

        private void DeviceDataJob_DeviceDataJobEventHandler(object sender, DeviceDataJobEventArgs e)
        {
            if (!this.IsClosing)
            {
                this.Invoke(new updateDeviceDataCallback(UpdateDeviceData), new object[] { e.DeviceData });
            }
        }

        private delegate void updateDeviceDataCallback(DeviceData deviceData);

        private void UpdateDeviceData(DeviceData deviceData)
        {
            // Do not update data if user disables polling before first update.
            if (!this.settings.IsPollingEnabled)
            {
                return;
            }

            DateTime now = DateTime.UtcNow;
            DateTime dataReadingsTimeStamp = now.AddSeconds(-(deviceData.WDT % 60));

            this.UpdateDeviceInformation(deviceData.DeviceInformation);

            try
            {
                if (this.settings.Logging.IsEnabled && this.settings.Logging.DataLogging.IsEnabled)
                {
                    if (!this.lastDataReadingTimestamp.HasValue || (this.lastDataReadingTimestamp.HasValue && now.Subtract(this.lastDataReadingTimestamp.Value).TotalSeconds >= 60))
                    {
                        this.lastDataReadingTimestamp = dataReadingsTimeStamp;

                        if (this.settings.Logging.DataLogging.UseSeparateFile)
                        {
                            LoggerManager.GetInstance().GetLogger(Program.DataLoggerName).Write(JsonConvert.SerializeObject(deviceData));
                        }
                        else
                        {
                            LoggerManager.GetInstance().GetLogger(Program.LoggerName).Write(JsonConvert.SerializeObject(deviceData));
                        }
                    }
                }
            }
            catch (Exception loggingException)
            {
                Debug.WriteLine($"[{Program.ApplicationName}] [{nameof(FormMain)}] UpdateDeviceData Error: {loggingException.ToString()}");
            }

            // Try to get the radiation detector to perform the conversion.
            var radiationDetectorIsKnown = RadiationDetector.TryGetByName(deviceData.DeviceInformation.Detector, out RadiationDetector radiationDetector);

            if (!radiationDetectorIsKnown || this.settings.Misc.RadiationUnitType == RadiationUnitType.Cpm)
            {
                this.viewOnlyTextBoxRadiation.Text = Radiation.FromCpm(deviceData.Radiation).ToString();
                this.viewOnlyTextBoxRadiationAverage.Text = deviceData.RadiationAverage.HasValue ? Radiation.FromCpm(deviceData.RadiationAverage).ToString() : string.Empty;
            }
            else
            {
                var decimalDigitsNumber = this.settings.Misc.RadiationUnitType == RadiationUnitType.uSvH ? 4 : 2;

                this.viewOnlyTextBoxRadiation.Text = Radiation.FromCpm(deviceData.Radiation).ConvertTo(this.settings.Misc.RadiationUnitType, radiationDetector.ConversionFactor.Value).ToString(decimalDigitsNumber);
                this.viewOnlyTextBoxRadiationAverage.Text = deviceData.RadiationAverage.HasValue ? Radiation.FromCpm(deviceData.RadiationAverage).ConvertTo(this.settings.Misc.RadiationUnitType, radiationDetector.ConversionFactor.Value).ToString(decimalDigitsNumber) : string.Empty;
            }

            this.labelRadiationAverage.Enabled = deviceData.RadiationAverage.HasValue;

            // Temperature conversion.
            this.viewOnlyTextBoxTemperature.Text = Temperature.FromCelsius(deviceData.Temperature).ConvertTo(this.settings.Misc.TemperatureUnitType).ToString();

            // Pressure conversion.
            if (deviceData.Pressure.HasValue)
            {
                this.labelPressure.Enabled = true;
                this.pressureToolStripMenuItem.Enabled = true;

                var decimalDigitsNumber = this.settings.Misc.PressureUnitType == PressureUnitType.Pa ? 0 : 3;

                this.viewOnlyTextBoxPressure.Text = Pressure.FromPascals(deviceData.Pressure.Value).ConvertTo(this.settings.Misc.PressureUnitType).ToString(decimalDigitsNumber);
            }
            else
            {
                this.labelPressure.Enabled = false;
                this.pressureToolStripMenuItem.Enabled = false;
                this.viewOnlyTextBoxPressure.Text = string.Empty;
            }

            // Update settings.
            var saveSettings = false;

            var deviceSettings = this.settings.Devices.First();
            var deviceCapabilities = deviceSettings.GetDeviceCapabilities();

            if (deviceCapabilities.HasFlag(DeviceCapability.Pressure) != deviceData.Pressure.HasValue)
            {
                var deviceCapabilitiesRegister = new EnumRegister<DeviceCapability>(deviceSettings.GetDeviceCapabilities());
                deviceSettings.SetDeviceCapabilities(deviceCapabilitiesRegister.InvertFlag(DeviceCapability.Pressure));
                saveSettings = true;
            }

            var radiationDetectorName = RadiationDetector.Normalize(deviceData.DeviceInformation.Detector);

            if (!radiationDetectorName.Equals(deviceSettings.GetRadiationDetectorName()))
            {
                deviceSettings.SetRadiationDetectorName(radiationDetectorName);
                saveSettings = true;
            }

            if (saveSettings)
            {
                this.settings.Save();
            }

            this.viewOnlyTextBoxVoltage.Text = string.Format("{0} V ({1}%)", deviceData.Voltage, deviceData.VoltagePercentage);
            this.viewOnlyTextBoxWDT.Text = string.Format("{0} s", deviceData.WDT);

            this.UpdateDeviceStatus(string.Format("Device received {0} from server.", (HttpStatus.GetReason(int.Parse(deviceData.ServerResponseCode)) != null) ? string.Format("{0} ({1})", deviceData.ServerResponseCode, HttpStatus.GetReason(int.Parse(deviceData.ServerResponseCode))) : deviceData.ServerResponseCode));

            // Update ToolTip text for notification area icon.
            var notifyIconText = new StringBuilder(string.Format("Radiation: {0}, Average: {1}\nTemperature: {2}", this.viewOnlyTextBoxRadiation.Text, this.viewOnlyTextBoxRadiationAverage.Text, this.viewOnlyTextBoxTemperature.Text));

            if (deviceSettings.GetDeviceCapabilities().HasFlag(DeviceCapability.Pressure))
            {
                notifyIconText.Append(string.Format(", Pressure: {0}", this.viewOnlyTextBoxPressure.Text));
            }

            this.UpdateNotifyIconText(notifyIconText.ToString());

            this.UpdateDeviceUptime(deviceData.Uptime);

            if (deviceData.ServerResponseCode != HttpStatus.OK.Code.ToString())
            {
                this.logger.Write(string.Format("Device received {0} from server (WDT={1})", deviceData.ServerResponseCode, deviceData.WDT));

                this.notifyIcon.Icon = Properties.Resources.RadiationWithError;

                if (deviceData.WDT > 59)
                {
                    this.UpdateNotifyIconText("Device cannot send data to server...");
                }
                else
                {
                    // TODO: Ask Radu about this strange behavior (deviceData.ServerResponseCode == 0 && (WDT == 59 || WDT == 60)).
                }
            }
            else
            {
                this.notifyIcon.Icon = Properties.Resources.RadiationIcon;
            }

            if (this.settings.Notifications.IsEnabled)
            {
                var currentRadiation = Radiation.FromCpm(deviceData.Radiation);
                var showRadiationNotification = true;

                if (this.settings.Notifications.RadiationThreshold.MeasurementUnit != RadiationUnitType.Cpm)
                {
                    if (radiationDetectorIsKnown)
                    {
                        currentRadiation = currentRadiation.ConvertTo(this.settings.Notifications.RadiationThreshold.MeasurementUnit, radiationDetector.ConversionFactor.Value);
                    }
                    else
                    {
                        // Disable radiation notification if the radiation value cannot be converted from cpm to uSv/h or uRem/h.
                        showRadiationNotification = false;
                    }
                }

                var showBalloon = false;
                var balloonTitle = string.Empty;
                var balloonMessage = string.Empty;

                var currentTemperature = Temperature.FromCelsius(deviceData.Temperature).ConvertTo(this.settings.Notifications.TemperatureThreshold.MeasurementUnit);

                if (this.settings.Notifications.TemperatureThreshold.HighValue.HasValue &&
                    currentTemperature.Value >= this.settings.Notifications.TemperatureThreshold.HighValue)
                {
                    balloonTitle = "High Temperature";
                    balloonMessage = string.Format("Temperature: {0}", currentTemperature.ToString());
                    showBalloon = true;
                }

                if (showRadiationNotification &&
                    this.settings.Notifications.RadiationThreshold.HighValue.HasValue &&
                    currentRadiation.Value >= this.settings.Notifications.RadiationThreshold.HighValue)
                {
                    if (balloonTitle.Length > 0)
                    {
                        balloonTitle += "/";
                    }

                    if (balloonMessage.Length > 0)
                    {
                        balloonMessage += "\n";
                    }

                    balloonTitle += "Radiation";
                    balloonMessage += string.Format("Radiation: {0}", currentRadiation.ToString());

                    showBalloon = true;
                }

                if (showBalloon)
                {
                    balloonTitle += " Alert";

                    // Add the date and time when the event occurs to notification message. This is useful because
                    // Windows queues notifications when user is away from computer (e.g.: screen is locked).
                    // LINK: https://msdn.microsoft.com/en-us/library/windows/desktop/ee330740%28v=vs.85%29.aspx
                    balloonMessage += string.Format("\nEvent occurred at {0}.", now.ToLocalTime().ToString("dd/MM/yyyy HH:mm:ss", CultureInfo.CurrentCulture));

                    // Do not show multiple notifications for the same readings. Usually when polling interval is lower than the device refresh period (60 seconds).
                    if (!notifyIconBalloonLastShownAt.HasValue || (this.notifyIconBalloonLastShownAt.HasValue && now.Subtract(this.notifyIconBalloonLastShownAt.Value).TotalSeconds >= 60))
                    {
                        this.notifyIcon.ShowBalloonTip(10000, balloonTitle, balloonMessage, ToolTipIcon.Info);
                        this.notifyIconBalloonLastShownAt = dataReadingsTimeStamp;
                    }
                }
            }
        }

        private void EnablePollingToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            if (this.enablePollingToolStripMenuItem.Checked)
            {
                deviceDataJob.Start();

                this.UpdateDeviceStatus(string.Format("Connecting to {0}...", this.settings.Devices.First().EndpointUrl));
                this.UpdateNotifyIconText(string.Format("Connecting to {0}...", this.settings.Devices.First().EndpointUrl));
            }
            else
            {
                deviceDataJob.Stop();

                this.UpdateDeviceStatus(Properties.Resources.PollingIsDisabled);
                this.UpdateNotifyIconText(Properties.Resources.PollingIsDisabled);
            }
        }

        // Toggle 'Enable polling'.
        private void EnablePollingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.enablePollingToolStripMenuItem.Checked = !this.enablePollingToolStripMenuItem.Checked;
            this.settings.IsPollingEnabled = this.enablePollingToolStripMenuItem.Checked;

            var deviceSettings = this.settings.Devices.FirstOrDefault();

            if (deviceSettings.IsNotNull())
            {
                deviceSettings.Polling.IsEnabled = this.settings.IsPollingEnabled;
            }

            this.settings.Save();

            this.notifyIcon.Icon = this.settings.IsPollingEnabled ? Properties.Resources.RadiationIcon : Properties.Resources.RadiationDisabled;
        }

        private void ConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new FormDeviceConfiguration(this.settings))
            {
                var result = form.ShowDialog(this);

                if (result == DialogResult.OK)
                {
                    if (this.settings.Devices.First().EndpointUrl.IsNotNullOrEmpty())
                    {
                        this.enablePollingToolStripMenuItem.Enabled = true;
                        this.viewDeviceWebpageToolStripMenuItem.Enabled = true;
                        this.viewDeviceWebpageContextMenuToolStripMenuItem.Enabled = true;

                        this.InitDevice(true);
                    }
                }
            }
        }

        private void OptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new FormOptions(this.settings))
            {
                form.SettingsChangedEventHandler += new SettingsChangedEventHandler(Form_SettingsChangedEventHandler);
                form.ShowDialog(this);
            }
        }

        private void Form_SettingsChangedEventHandler(object sender, SettingsChangedEventArgs e)
        {
            SettingsChangedEventHandler?.Invoke(this, new SettingsChangedEventArgs(this.settings));

            ConfigureCheckForUpdatesJob();
        }

        private void CloseApplication(object sender, EventArgs e)
        {
            IsClosing = true;
            Close();
        }

        private void FormMain_Closing(object sender, FormClosingEventArgs e)
        {
            Debug.WriteLine($"[{Program.ApplicationName}] [{nameof(FormMain)}] FormMain_Closing() CloseReason: {e.CloseReason}");

            if (WindowState != FormWindowState.Minimized)
            {
                UpdateWindowPositionAndSaveToSettings();
            }

            if (e.CloseReason == CloseReason.UserClosing)
            {
                if (IsClosing)
                {
                    this.logger.Write($"Application was closed. Reason: {e.CloseReason}.");
                    return;
                }

                if (settings.Display.CloseToSystemTray)
                {
                    MinimizeToTray();

                    e.Cancel = true;
                    ResetCloseReasonToNone();
                    return;
                }
            }

            logger.Write($"Application was closed. Reason: {e.CloseReason}.");
            IsClosing = true;
        }

        private void ResetCloseReasonToNone()
        {
            var fieldInfo = typeof(Form).GetField("closeReason", BindingFlags.Instance | BindingFlags.NonPublic);
            fieldInfo.SetValue(this, CloseReason.None);
        }

        private void RestoreWindowOnScreen()
        {
            Debug.WriteLine($"[{Program.ApplicationName}] [{nameof(FormMain)}] RestoreWindowOnScreen()");

            if (WindowPosition.IsOnScreenWorkingArea(atLeast: 10))
            {
                Top = WindowPosition.Y;
                Left = WindowPosition.X;
                return;
            }

            var screenWorkingArea = Screen.PrimaryScreen?.WorkingArea;

            if (screenWorkingArea.HasValue)
            {
                var x = screenWorkingArea.Value.Width - Size.Width - 10;
                var y = screenWorkingArea.Value.Height - Size.Height - 10;

                Top = y;
                Left = x;

                if (WindowState != FormWindowState.Minimized)
                {
                    UpdateWindowPositionAndSaveToSettings();
                }
            }
        }

        private void UpdateWindowPositionAndSaveToSettings()
        {
            Debug.WriteLine($"[{Program.ApplicationName}] [{nameof(FormMain)}] UpdateWindowPositionAndSaveToSettings()");

            var currentWindowPosition = new Point(Left, Top);

            if (WindowPosition != currentWindowPosition)
            {
                WindowPosition = currentWindowPosition;
                settings.Display.WindowPosition = WindowPosition;
                settings.Save();
            }
        }

        private void ShowWindow()
        {
            Debug.WriteLine($"[{Program.ApplicationName}] [{nameof(FormMain)}] ShowWindow()");

            if (WindowState == FormWindowState.Minimized)
            {
                ShowInTaskbar = settings.Display.ShowInTaskbar;
                Show();
                BringToFront();
                WindowState = FormWindowState.Normal;
                Visible = true;

                RestoreWindowOnScreen();
            }
            else
            {
                TopMost = true; // Force to come on top.
                Show();
                BringToFront();
            }

            // Don't stay on top forever. Allow it to go in background.
            TopMost = false;
        }

        private void MinimizeToTray()
        {
            Debug.WriteLine($"[{Program.ApplicationName}] [{nameof(FormMain)}] MinimizeToTray()");

            allowVisible = false;
            ShowInTaskbar = settings.Display.ShowInTaskbar;
            Hide();
            Visible = false;
            WindowState = FormWindowState.Minimized;
        }

        private void ToggleWindow()
        {
            Debug.WriteLine($"[{Program.ApplicationName}] [{nameof(FormMain)}] ToggleWindow()");

            ShowInTaskbar = settings.Display.ShowInTaskbar;

            if (WindowState == FormWindowState.Minimized)
            {
                Show();
                BringToFront();
                WindowState = FormWindowState.Normal;
                Visible = true;

                RestoreWindowOnScreen();
                return;
            }

            Hide();
            UpdateWindowPositionAndSaveToSettings();
            Visible = false;
            WindowState = FormWindowState.Minimized;
        }

        [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)NativeMethods.WM.SYSCOMMAND && m.WParam.ToInt32() == (int)NativeMethods.SC.MINIMIZE)
            {
                ToggleWindow();
                return;
            }

            base.WndProc(ref m);
        }

        private void ShowHideContextMenuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleWindow();
        }

        private void ShowHideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToggleWindow();
        }

        private void NotifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ShowWindow();
            }
        }

        private void ViewDeviceWebpageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(string.Format("http://{0}/", this.settings.Devices.First().EndpointUrl));
        }

        private void ViewDeviceWebpageToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Process.Start(string.Format("http://{0}/", this.settings.Devices.First().EndpointUrl));
        }

        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new FormAbout())
            {
                form.Text = form.Text.Replace("{title}", Application.ProductName);
                form.Update();

                form.ShowDialog(this);
            }
        }

        [SuppressMessage(category: "Style", checkId: "IDE1006")]
        private void uRADMonitorWebsiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.uradmonitor.com/");
        }

        [SuppressMessage(category: "Style", checkId: "IDE1006")]
        private void uRADMonitorForumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("http://www.uradmonitor.com/forums/");
        }

        private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // FIXME: Don't crash (right-click on the notification icon) when running on Mono.
            if (!EnvironmentUtils.IsMonoRuntime())
            {
                this.contextMenuStrip.Show(Cursor.Position, ToolStripDropDownDirection.AboveRight);
            }

            if (this.WindowState == FormWindowState.Normal)
            {
                this.ShowWindow();
            }
        }

        private void ViewOnlyTextBoxId_TextChanged(object sender, EventArgs e)
        {
            this.viewDeviceOnlineDataToolStripMenuItem.Enabled = this.viewOnlyTextBoxId.Text.Length > 0;
        }

        private void ViewDeviceDashboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.viewOnlyTextBoxId.HasText())
            {
                uRADMonitorHelper.OpenDashboardUrl(this.viewOnlyTextBoxId.Text);
            }
        }

        private void ViewDeviceGraphDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.viewOnlyTextBoxId.HasText())
            {
                uRADMonitorHelper.OpenGraphUrl(this.viewOnlyTextBoxId.Text, ((ToolStripMenuItem)sender).Tag.ToString().ToLower());
            }
        }

        private void CheckForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new FormUpdate(this.logger))
            {
                form.Update();
                form.ShowDialog(this);
            }
        }

        private void NetworkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new FormNetwork(settings, deviceServiceFactory))
            {
                var result = form.ShowDialog(this);
            }
        }
    }
}
