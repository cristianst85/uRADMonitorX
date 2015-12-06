using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using uRADMonitorX.Commons;
using uRADMonitorX.Commons.Logging;
using uRADMonitorX.Commons.Logging.Appenders;
using uRADMonitorX.Commons.Logging.Formatters;
using uRADMonitorX.Configuration;
using uRADMonitorX.Core;
using uRADMonitorX.Windows;

namespace uRADMonitorX {

    static class Program {

        public static readonly String LoggerName = "fileLogger";
        public static readonly String LoggerFilePath = "uRADMonitorX.log";
        public static readonly String SettingsFileName = "config.xml";
        public static readonly String UserAgent = "uRADMonitorX/1.0";
        // Download update.xml only using HTTPS.
        public static readonly String UpdaterUrl = "https://github.com/cristianst85/uRADMonitorX/update.xml";

        private static ProgramArguments arguments = null;
        private static ISettings settings = null;
        private static ILogger logger = null;

        private static Mutex mutex;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {

            bool success = ProgramArguments.TryParse(args, out arguments);

            if (!success) {
                arguments = new ProgramArguments(); // Get a new instance with default values.
            }

            if (arguments.CleanupUpdate) {
                // Wait one second to allow the other instance to exit.
                Thread.Sleep(1000);
                // Remove the old executable file.
                File.Delete(String.Format("{0}.tmp", AssemblyUtils.GetApplicationPath()));
            }

            if (!arguments.AllowMultipleInstances) {
                var assembly = typeof(Program).Assembly;
                GuidAttribute guidAttribute = (GuidAttribute)assembly.GetCustomAttributes(typeof(GuidAttribute), true)[0];
                bool applicationInstanceIsNotRunning;
                mutex = new Mutex(false, String.Format("{0}{1}", Application.ProductName, guidAttribute.Value), out applicationInstanceIsNotRunning);
                if (!applicationInstanceIsNotRunning) {
                    MessageBox.Show(String.Format("An instance of this program is already running."), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // Load settings.
            String settingsFilePath = String.Format("{0}{1}{2}", Path.GetDirectoryName(new Uri(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath), Path.DirectorySeparatorChar, Program.SettingsFileName);
            if (File.Exists(settingsFilePath)) {
                settings = XMLSettings.LoadFromFile(settingsFilePath);
            }
            else {
                XMLSettings.CreateFile(settingsFilePath);
                settings = XMLSettings.LoadFromFile(settingsFilePath);
            }

            String loggerFilePath = null;
            if (settings.LogDirectoryPath.Length > 0) {
                loggerFilePath = Path.Combine(settings.LogDirectoryPath, Program.LoggerFilePath);
            }
            else {
                loggerFilePath = Path.Combine(Path.GetDirectoryName(AssemblyUtils.GetApplicationPath()), Program.LoggerFilePath);
            }

            LoggerManager.GetInstance().Add(Program.LoggerName,
                                            new ThreadSafeLogger(
                                                new FileAppender(Path.Combine((Path.GetDirectoryName(AssemblyUtils.GetApplicationPath())), Program.LoggerFilePath)) { Enabled = true },
                                                new SimpleFormatter()) { Enabled = settings.IsLoggingEnabled }
                                           );
            logger = LoggerManager.GetInstance().GetLogger(Program.LoggerName);

            IDeviceDataReaderFactory deviceDataReaderFactory = new DeviceDataHttpReaderFactory(settings);

            if (!arguments.IgnoreRegisteringAtWindowsStartup && !EnvironmentUtils.IsUnix()) {
                registerAtWindowsStartup();
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            FormMain formMain = new FormMain(deviceDataReaderFactory, settings, logger);
            formMain.SettingsChangedEventHandler += new SettingsChangedEventHandler(formMain_SettingsChangedEventHandler);

            Application.Run(formMain);
        }

        private static void formMain_SettingsChangedEventHandler(object sender, SettingsChangedEventArgs e) {
            if (logger != null) {
                configLogger();
            }
            if (!arguments.IgnoreRegisteringAtWindowsStartup && !EnvironmentUtils.IsUnix()) {
                registerAtWindowsStartup();
            }
        }

        private static void configLogger() {
            logger.Enabled = settings.IsLoggingEnabled;
            try {
                ILoggerAppender appender = LoggerManager.GetInstance().GetLogger(Program.LoggerName).Appender;
                if (appender is ICanReconfigureAppender) {
                    // TODO: Verify if logger path is in application root directory.
                    if (settings.LogDirectoryPath.Length > 0) {
                        ((ICanReconfigureAppender)appender).Reconfigure(Path.Combine(settings.LogDirectoryPath, Program.LoggerFilePath));
                    }
                    else {
                        ((ICanReconfigureAppender)appender).Reconfigure(Path.Combine(Path.GetDirectoryName(AssemblyUtils.GetApplicationPath()), Program.LoggerFilePath));
                    }
                }
            }
            catch (UnauthorizedAccessException ex) {
                logger.Write(String.Format("Cannot reconfigure logger appender. Exception: {0}", ex.ToString()));
            }
        }

        private static void registerAtWindowsStartup() {
            try {
                if (settings.StartWithWindows) {
                    Registry.RegisterAtWindowsStartup(Application.ProductName, String.Format("\"{0}\"", new Uri(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath));
                }
                else {
                    Registry.UnRegisterAtWindowsStartup(Application.ProductName);
                }
            }
            catch (Exception e) {
                if (logger != null) {
                    logger.Write(String.Format("Error registering application to start at Windows startup. Exception: {0}", e.ToString()));
                }
            }
        }
    }
}
