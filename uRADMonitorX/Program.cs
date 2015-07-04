using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using uRADMonitorX.Commons;
using uRADMonitorX.Commons.Logging;
using uRADMonitorX.Commons.Logging.Appenders;
using uRADMonitorX.Commons.Logging.Formatters;

namespace uRADMonitorX {

    static class Program {

        public static readonly String LoggerName = "fileLogger";
        public static readonly String LoggerFilePath = "uRADMonitorX.log";
        public static readonly String SettingsFileName = "config.xml";

        private static Mutex mutex;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {

            ProgramArguments arguments = null;
            bool success = ProgramArguments.TryParse(args, out arguments);

            if (!success) {
                arguments = new ProgramArguments(); // Get a new instance with default values.
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

            LoggerManager.GetInstance().Add(Program.LoggerName,
                                                new ThreadSafeLogger(
                                                    new FileAppender(Path.Combine((Path.GetDirectoryName(AssemblyUtils.GetApplicationPath())), Program.LoggerFilePath)) { Enabled = true },
                                                    new SimpleFormatter()) { Enabled = false }
                                            );
            ILogger logger = LoggerManager.GetInstance().GetLogger(Program.LoggerName);
            logger.Write("Application is starting...");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain(arguments.IgnoreRegisteringAtWindowsStartup));
        }
    }
}
