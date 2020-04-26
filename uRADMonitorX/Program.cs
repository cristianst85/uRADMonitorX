using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using uRADMonitorX.Commons;
using uRADMonitorX.Commons.Logging;
using uRADMonitorX.Commons.Logging.Appenders;
using uRADMonitorX.Commons.Logging.Formatters;
using uRADMonitorX.Configuration;
using uRADMonitorX.Core;
using uRADMonitorX.Updater;
using uRADMonitorX.Windows;

namespace uRADMonitorX {

    internal static class Program {

        public static readonly String LoggerName = "fileLogger";
        public static readonly String DataLoggerName = "dataLogger";
        public static readonly String LoggerFileName = "uRADMonitorX.log";
        public static readonly String DataLoggerFileName = "data.log";
        public static readonly String SettingsFileName = "config.xml";
        public static readonly String UserAgent = "uRADMonitorX/1.0";
        /// <summary>
        /// Specifies the URL application uses to check 
        /// for updates. Only use secure URLs (HTTPS).
        /// </summary>
        public static readonly String UpdateUrl = "https://api.github.com/repos/cristianst85/uRADMonitorX/releases/latest";
        /// <summary>
        /// Specifies the interval in minutes at which the
        /// application automatically checks for updates.
        /// </summary>
        public static readonly int UpdaterInterval = 720; // 12 hours.

        private static ProgramArguments arguments = null;
        private static ISettings settings = null;
        private static ILogger logger = null;

        private static Mutex mutex;

        internal static readonly IWebUpdater ApplicationUpdater = new GitHubUpdater();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyUtils.AssemblyResolver);
            internalMain(args);
        }

        static void internalMain(string[] args) {

            bool success = ProgramArguments.TryParse(args, out arguments);

            if (!success) {
                arguments = new ProgramArguments(); // Get a new instance with default values.
            }

            if (arguments.CleanupUpdate || EnvironmentUtils.IsMonoRuntime()) {
                if (!EnvironmentUtils.IsMonoRuntime()) {
                    // Wait one second to allow the other instance to exit.
                    Thread.Sleep(1000);
                }
                // Remove the old executable file.
                if (File.Exists(String.Format("{0}.tmp", AssemblyUtils.GetApplicationPath()))) {
                    File.Delete(String.Format("{0}.tmp", AssemblyUtils.GetApplicationPath()));
                }
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
            try {
                if (!File.Exists(settingsFilePath)) {
                    try {
                        XMLSettings.CreateFile(settingsFilePath);
                    }
                    catch (Exception createSettingsFileException) {
                        MessageBox.Show(String.Format("Cannot create settings file {0}.\n\nError details: {1}", settingsFilePath, createSettingsFileException.Message), "uRADMonitorX", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                settings = XMLSettings.LoadFromFile(settingsFilePath);
                // Fixes an issue with data log directory name on Linux due to trailing slash.
                settings.DataLogDirectoryPath = settings.DataLogDirectoryPath.TrimEnd('\\');
                settings.Commit();
            }
            catch (Exception loadSettingsFileException) {
                MessageBox.Show(String.Format("Cannot load settings from file {0}.\n\nError details: {1}", settingsFilePath, loadSettingsFileException.Message), "uRADMonitorX", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var loggerFilePath = getLoggerPath(settings.LogDirectoryPath, Program.LoggerFileName, false);
            var dataLoggerFilePath = getLoggerPath(settings.DataLoggingToSeparateFile ? settings.DataLogDirectoryPath : settings.LogDirectoryPath, Program.DataLoggerFileName, settings.IsLoggingEnabled && settings.IsDataLoggingEnabled && settings.DataLoggingToSeparateFile);

            LoggerManager.GetInstance().Add(Program.LoggerName,
                                            new ThreadSafeLogger(
                                                new Logger(
                                                    new ReconfigurableFileAppender(loggerFilePath),
                                                    new DateTimeFormatter()
                                                ) {
                                                    Enabled = settings.IsLoggingEnabled
                                                }
                                            ));
            LoggerManager.GetInstance().Add(Program.DataLoggerName,
                                          new ThreadSafeLogger(
                                                new Logger(
                                                    new NLogDailyFileAppender(dataLoggerFilePath),
                                                    new DateTimeFormatter()
                                                ) {
                                                    Enabled = settings.IsLoggingEnabled &&
                                                    settings.IsDataLoggingEnabled &&
                                                    settings.DataLoggingToSeparateFile
                                                }
                                            ));
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
                configLogger(
                    LoggerManager.GetInstance().GetLogger(Program.LoggerName),
                    getLoggerPath(settings.LogDirectoryPath, Program.LoggerFileName, false),
                    settings.IsLoggingEnabled
                );
                configLogger(
                    LoggerManager.GetInstance().GetLogger(Program.DataLoggerName),
                    getLoggerPath(settings.DataLoggingToSeparateFile ? settings.DataLogDirectoryPath : settings.LogDirectoryPath, Program.DataLoggerFileName, true),
                    settings.IsLoggingEnabled && settings.IsDataLoggingEnabled && settings.DataLoggingToSeparateFile
                );
            }
            if (!arguments.IgnoreRegisteringAtWindowsStartup && !EnvironmentUtils.IsUnix()) {
                registerAtWindowsStartup();
            }
        }

        private static String getLoggerPath(String loggerDirectoryPath, String loggerFileName, bool createIfNotExists) {
            loggerDirectoryPath = loggerDirectoryPath.TrimStart('\\').TrimEnd('\\');
            String loggerFilePath = null;
            if (Path.IsPathRooted(loggerDirectoryPath)) {
                loggerFilePath = Path.Combine(loggerDirectoryPath, loggerFileName);
            }
            else {
                if (loggerDirectoryPath.Length > 0) {
                    loggerFilePath = Path.Combine(Path.GetDirectoryName(AssemblyUtils.GetApplicationPath()), loggerDirectoryPath, loggerFileName);
                }
                else {
                    loggerFilePath = Path.Combine(Path.GetDirectoryName(AssemblyUtils.GetApplicationPath()), loggerFileName);
                }
            }

            if (createIfNotExists) {
                var dirPath = Path.GetDirectoryName(loggerFilePath);
                if (!Directory.Exists(dirPath)) {
                    Directory.CreateDirectory(dirPath);
                }
            }

            return loggerFilePath;
        }

        private static void configLogger(ILogger logger, String loggerNewFilePath, bool enable) {
            logger.Enabled = enable;
            Debug.WriteLine(loggerNewFilePath);
            try {
                ILoggerAppender appender = logger.Appender;
                if (appender is IReconfigurableFileAppender) {
                    ((IReconfigurableFileAppender)appender).Reconfigure(loggerNewFilePath);
                }
            }
            catch (UnauthorizedAccessException ex) {
                logger.Write(String.Format("Cannot reconfigure logger appender. Exception: {0}", ex.ToString()));
            }
        }

        private static void registerAtWindowsStartup() {
            try {
                if (settings.StartWithWindows) {
                    RegistryUtils.RegisterAtWindowsStartup(Application.ProductName, String.Format("\"{0}\"", new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath));
                }
                else {
                    RegistryUtils.UnRegisterAtWindowsStartup(Application.ProductName);
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
