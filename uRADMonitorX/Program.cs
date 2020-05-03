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
using uRADMonitorX.Updater;
using uRADMonitorX.Windows;

namespace uRADMonitorX
{
    internal static class Program
    {
        internal static readonly string ApplicationName = "uRADMonitorX";
        internal static readonly string LoggerName = "fileLogger";
        internal static readonly string DataLoggerName = "dataLogger";
        internal static readonly string LoggerFileName = "uRADMonitorX.log";
        internal static readonly string DataLoggerFileName = "data.log";
        internal static readonly string SettingsFileName = "config.xml";
        internal static readonly string UserAgent = "uRADMonitorX/1.0";

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
                    NativeMethods.SetProcessDpiAwareness(NativeMethods.ProcessDpiAwareness.ProcessSystemDpiAware);
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
            var settingsFilePath = string.Format("{0}{1}{2}", Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().GetName().CodeBase).LocalPath), Path.DirectorySeparatorChar, Program.SettingsFileName);

            try
            {
                if (!File.Exists(settingsFilePath))
                {
                    try
                    {
                        XMLSettings.CreateFile(settingsFilePath);
                    }
                    catch (Exception createSettingsFileException)
                    {
                        MessageBox.Show(string.Format("Cannot create settings file {0}.\n\nError details: {1}", settingsFilePath, createSettingsFileException.Message), Program.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return;
                    }
                }

                settings = XMLSettings.LoadFromFile(settingsFilePath);

                // Fixes an issue with data log directory name on Linux due to trailing slash.
                if (settings.DataLogDirectoryPath.IsNotNull() && settings.DataLogDirectoryPath.EndsWith("\\"))
                {
                    settings.DataLogDirectoryPath = settings.DataLogDirectoryPath.TrimEnd('\\');
                    settings.Commit();
                }
            }
            catch (Exception loadSettingsFileException)
            {
                MessageBox.Show(string.Format("Cannot load settings from file {0}.\n\nError details: {1}", settingsFilePath, loadSettingsFileException.Message), Program.ApplicationName, MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            var loggerFilePath = GetLoggerPath(settings.LogDirectoryPath, Program.LoggerFileName, false);
            var dataLoggerFilePath = GetLoggerPath(settings.DataLoggingToSeparateFile ? settings.DataLogDirectoryPath : settings.LogDirectoryPath, Program.DataLoggerFileName, settings.IsLoggingEnabled && settings.IsDataLoggingEnabled && settings.DataLoggingToSeparateFile);

            LoggerManager.GetInstance().Add(Program.LoggerName,
                                            new ThreadSafeLogger(
                                                new Logger(
                                                    new ReconfigurableFileAppender(loggerFilePath),
                                                    new DateTimeFormatter()
                                                )
                                                {
                                                    Enabled = settings.IsLoggingEnabled
                                                }
                                            ));

            LoggerManager.GetInstance().Add(Program.DataLoggerName,
                                          new ThreadSafeLogger(
                                                new Logger(
                                                    new NLogDailyFileAppender(dataLoggerFilePath),
                                                    new DateTimeFormatter()
                                                )
                                                {
                                                    Enabled = settings.IsLoggingEnabled &&
                                                    settings.IsDataLoggingEnabled &&
                                                    settings.DataLoggingToSeparateFile
                                                }
                                            ));

            logger = LoggerManager.GetInstance().GetLogger(Program.LoggerName);

            var deviceDataReaderFactory = new DeviceDataHttpReaderFactory(settings);
            var deviceDataJobFactory = new DeviceDataJobFactory(settings, deviceDataReaderFactory);

            if (!programArguments.IgnoreRegisteringAtWindowsStartup && !EnvironmentUtils.IsUnix())
            {
                RegisterAtWindowsStartup();
            }

            var formMain = new FormMain(deviceDataReaderFactory, deviceDataJobFactory, settings, logger);
            formMain.SettingsChangedEventHandler += new SettingsChangedEventHandler(FormMain_SettingsChangedEventHandler);

            Application.Run(formMain);
        }

        private static void FormMain_SettingsChangedEventHandler(object sender, SettingsChangedEventArgs e)
        {
            if (logger != null)
            {
                ConfigLogger(
                    LoggerManager.GetInstance().GetLogger(Program.LoggerName),
                    GetLoggerPath(settings.LogDirectoryPath, Program.LoggerFileName, false),
                    settings.IsLoggingEnabled
                );

                ConfigLogger(
                    LoggerManager.GetInstance().GetLogger(Program.DataLoggerName),
                    GetLoggerPath(settings.DataLoggingToSeparateFile ? settings.DataLogDirectoryPath : settings.LogDirectoryPath, Program.DataLoggerFileName, true),
                    settings.IsLoggingEnabled && settings.IsDataLoggingEnabled && settings.DataLoggingToSeparateFile
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
                if (settings.StartWithWindows)
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
