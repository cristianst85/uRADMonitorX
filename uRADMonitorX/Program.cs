using System;
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
using uRADMonitorX.Extensions;
using uRADMonitorX.Helpers;
using uRADMonitorX.Updater;
using uRADMonitorX.uRADMonitor.Domain;
using uRADMonitorX.uRADMonitor.Infrastructure;
using uRADMonitorX.uRADMonitor.Services;
using uRADMonitorX.Windows;

namespace uRADMonitorX
{
    internal static partial class Program
    {
        internal static readonly string ApplicationName = "uRADMonitorX";
        internal static readonly string LoggerName = "fileLogger";
        internal static readonly string DataLoggerName = "dataLogger";
        internal static readonly string LoggerFileName = "uRADMonitorX.log";
        internal static readonly string DataLoggerFileName = "data.log";

        /// <summary>
        /// Specifies the URL application uses to check 
        /// for updates. Only use secure URLs (HTTPS).
        /// </summary>
        internal static readonly string UpdateUrl = "https://api.github.com/repos/cristianst85/uRADMonitorX/releases/latest";

        /// <summary>
        /// Specifies the interval in minutes at which the
        /// application automatically checks for updates.
        /// </summary>
        internal static readonly int UpdaterInterval = 720; // 12 hours.

        internal static readonly IWebUpdater ApplicationUpdater = new GitHubUpdater();

        private static ProgramArguments programArguments = null;
        private static ISettings settings = null;
        private static ILogger logger = null;
        private static Mutex mutex = null;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyUtils.AssemblyResolver);
            InternalMain(args);
        }

        static void InternalMain(string[] args)
        {
            if (!EnvironmentUtils.IsUnix())
            {
                if (EnvironmentUtils.IsAtLeastWindows10())
                {
                    NativeMethods.SetProcessDpiAwareness(NativeMethods.ProcessDpiAwareness.ProcessPerMonitorDpiAware);
                }
                else if (EnvironmentUtils.IsAtLeastWindowsVista())
                {
                    NativeMethods.SetProcessDPIAware();
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (!ProgramArguments.TryParse(args, out programArguments))
            {
                // Get a new instance with default values.
                programArguments = new ProgramArguments();
            }

            if (programArguments.CleanupUpdate || EnvironmentUtils.IsMonoRuntime())
            {
                if (!EnvironmentUtils.IsMonoRuntime())
                {
                    // Wait one second to allow the other instance to exit.
                    Thread.Sleep(1000);
                }

                var applicationTemporaryFilePath = string.Format("{0}.tmp", AssemblyUtils.GetApplicationPath());

                // Remove the old executable file.
                if (File.Exists(applicationTemporaryFilePath))
                {
                    File.Delete(applicationTemporaryFilePath);
                }
            }

            if (!programArguments.AllowMultipleInstances)
            {
                var guidAttribute = (GuidAttribute)(typeof(Program).Assembly).GetCustomAttributes(typeof(GuidAttribute), true)[0];
                mutex = new Mutex(false, string.Format("{0}{1}", Program.ApplicationName, guidAttribute.Value), out bool applicationInstanceIsNotRunning);

                if (!applicationInstanceIsNotRunning)
                {
                    MessageBox.Show(string.Format("An instance of this program is already running."), Program.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }
            }

            // Load settings.
            var settingsFilePath = Path.Combine(AssemblyUtils.GetApplicationDirPath(), Program.Settings.Files.JsonFileName);

            // Migrate settings from XML to JSON file format.
            if (!File.Exists(settingsFilePath))
            {
                var xmlSettingsFilePath = Path.Combine(AssemblyUtils.GetApplicationDirPath(), Program.Settings.Files.XmlFileName);

                try
                {
                    if (File.Exists(xmlSettingsFilePath))
                    {
                        settings = JsonSettings.LoadFromXmlFile(xmlSettingsFilePath).SaveAs(settingsFilePath);
                    };
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Cannot migrate settings to JSON format.\n\nError details: {0}", ex.Message), Program.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }
            }

            if (settings.IsNull())
            {
                try
                {
                    if (!File.Exists(settingsFilePath))
                    {
                        try
                        {
                            JsonSettings.CreateFile(settingsFilePath);
                        }
                        catch (Exception createSettingsFileException)
                        {
                            MessageBox.Show(string.Format("Cannot create settings file {0}.\n\nError details: {1}", settingsFilePath, createSettingsFileException.Message), Program.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                            return;
                        }
                    }

                    settings = JsonSettings.LoadFromFile(settingsFilePath);

                    // Fixes an issue with data log directory name on Linux due to trailing slash.
                    if (settings.Logging.DataLogging.DirectoryPath.IsNotNull() && settings.Logging.DataLogging.DirectoryPath.EndsWith("\\"))
                    {
                        settings.Logging.DataLogging.DirectoryPath = settings.Logging.DataLogging.DirectoryPath.TrimEnd('\\');
                        settings.Save();
                    }
                }
                catch (Exception loadSettingsFileException)
                {
                    MessageBox.Show(string.Format("Cannot load settings from file {0}.\n\nError details: {1}", settingsFilePath, loadSettingsFileException.Message), Program.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                }
            }

            var loggerFilePath = GetLoggerPath(settings.Logging.DirectoryPath, Program.LoggerFileName, false);
            var dataLoggerFilePath = GetLoggerPath(settings.Logging.DataLogging.UseSeparateFile ? settings.Logging.DataLogging.DirectoryPath : settings.Logging.DirectoryPath, Program.DataLoggerFileName, settings.IsPollingEnabled && settings.Logging.DataLogging.IsEnabled && settings.Logging.DataLogging.UseSeparateFile);

            LoggerManager.GetInstance().Add(Program.LoggerName,
                                            new ThreadSafeLogger(
                                                new Logger(
                                                    new ReconfigurableFileAppender(loggerFilePath),
                                                    new DateTimeFormatter()
                                                )
                                                {
                                                    Enabled = settings.Logging.IsEnabled
                                                }
                                            ));

            LoggerManager.GetInstance().Add(Program.DataLoggerName,
                                          new ThreadSafeLogger(
                                                new Logger(
                                                    new NLogDailyFileAppender(dataLoggerFilePath),
                                                    new DateTimeFormatter()
                                                )
                                                {
                                                    Enabled = settings.Logging.IsEnabled &&
                                                    settings.Logging.DataLogging.IsEnabled &&
                                                    settings.Logging.DataLogging.UseSeparateFile
                                                }
                                            ));

            logger = LoggerManager.GetInstance().GetLogger(Program.LoggerName);

            var deviceDataReaderFactory = new DeviceDataHttpReaderFactory(settings);
            var deviceDataJobFactory = new DeviceDataJobFactory(settings, deviceDataReaderFactory);

            var deviceDataClientConfiguration = new DeviceDataClientConfiguration();
            var httpClientConfiguration = new HttpClientConfiguration();

            var httpClientFactory = new uRADMonitorHttpClientFactory(httpClientConfiguration);
            var deviceDataClientFactory = new DeviceDataHttpClientFactory(deviceDataClientConfiguration, httpClientFactory);
            var deviceServiceFactory = new DeviceServiceFactory(new DeviceFactory(), deviceDataClientFactory);

            if (!programArguments.IgnoreRegisteringAtWindowsStartup && !EnvironmentUtils.IsUnix())
            {
                RegisterAtWindowsStartup();
            }

            var formMain = new FormMain(deviceDataReaderFactory, deviceDataJobFactory, deviceServiceFactory, settings, logger);
            formMain.SettingsChangedEventHandler += new SettingsChangedEventHandler(FormMain_SettingsChangedEventHandler);

            // Both uRADMonitor API and GitHub API requires TLS v1.2.
            ServicePointManagerHelper.SetSecurityProtocolToTls12();

            Application.Run(formMain);
        }

        private static void FormMain_SettingsChangedEventHandler(object sender, SettingsChangedEventArgs e)
        {
            if (logger != null)
            {
                ConfigLogger(
                    LoggerManager.GetInstance().GetLogger(Program.LoggerName),
                    GetLoggerPath(settings.Logging.DirectoryPath, Program.LoggerFileName, false),
                    settings.Logging.IsEnabled
                );

                ConfigLogger(
                    LoggerManager.GetInstance().GetLogger(Program.DataLoggerName),
                    GetLoggerPath(settings.Logging.DataLogging.UseSeparateFile ? settings.Logging.DataLogging.DirectoryPath : settings.Logging.DirectoryPath, Program.DataLoggerFileName, true),
                    settings.Logging.IsEnabled && settings.Logging.DataLogging.IsEnabled && settings.Logging.DataLogging.UseSeparateFile
                );
            }

            if (!programArguments.IgnoreRegisteringAtWindowsStartup && !EnvironmentUtils.IsUnix())
            {
                RegisterAtWindowsStartup();
            }
        }

        private static string GetLoggerPath(string loggerDirectoryPath, string loggerFileName, bool createIfNotExists)
        {
            loggerDirectoryPath = loggerDirectoryPath.TrimStart('\\').TrimEnd('\\');
            string loggerFilePath = null;

            if (Path.IsPathRooted(loggerDirectoryPath))
            {
                loggerFilePath = Path.Combine(loggerDirectoryPath, loggerFileName);
            }
            else
            {
                if (loggerDirectoryPath.Length > 0)
                {
                    loggerFilePath = Path.Combine(Path.GetDirectoryName(AssemblyUtils.GetApplicationPath()), loggerDirectoryPath, loggerFileName);
                }
                else
                {
                    loggerFilePath = Path.Combine(Path.GetDirectoryName(AssemblyUtils.GetApplicationPath()), loggerFileName);
                }
            }

            if (createIfNotExists)
            {
                var dirPath = Path.GetDirectoryName(loggerFilePath);

                if (!Directory.Exists(dirPath))
                {
                    Directory.CreateDirectory(dirPath);
                }
            }

            return loggerFilePath;
        }

        private static void ConfigLogger(ILogger logger, string loggerNewFilePath, bool enable)
        {
            logger.Enabled = enable;

            try
            {
                if (logger.Appender is IReconfigurableFileAppender)
                {
                    ((IReconfigurableFileAppender)logger.Appender).Reconfigure(loggerNewFilePath);
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                logger.Write(string.Format("Cannot reconfigure logger appender. Exception: {0}", ex.ToString()));
            }
        }

        private static void RegisterAtWindowsStartup()
        {
            try
            {
                if (settings.General.StartWithWindows)
                {
                    RegistryUtils.RegisterAtWindowsStartup(Application.ProductName, string.Format("\"{0}\"", new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath));
                }
                else
                {
                    RegistryUtils.UnRegisterAtWindowsStartup(Application.ProductName);
                }
            }
            catch (Exception e)
            {
                if (logger != null)
                {
                    logger.Write(string.Format("Error registering application to run at Windows Startup. Exception: {0}", e.ToString()));
                }
            }
        }
    }
}
