using System;
using System.IO;

namespace uRADMonitorX.Commons.Logging.Appenders
{
    using NLog;
    using NLog.Config;
    using NLog.Targets;

    public class NLogDailyFileAppender : ILoggerAppender, IReconfigurableFileAppender
    {
        private readonly object _locker = new object();

        private readonly Logger logger;

        public NLogDailyFileAppender(String filePath)
        {
            var target = new FileTarget
            {
                Encoding = System.Text.Encoding.UTF8
            };

            ConfigFileTarget(target, filePath);

            target.ArchiveNumbering = ArchiveNumberingMode.Date;
            target.ArchiveEvery = FileArchivePeriod.Day;
            target.Layout = @"${message}";
            target.AutoFlush = true;

            var config = new LoggingConfiguration();

            config.AddTarget("fileTarget", target);

            var rule = new LoggingRule("fileLogger", LogLevel.Info, target);
            config.LoggingRules.Add(rule);

            LogManager.Configuration = config;
            this.logger = LogManager.GetLogger("fileLogger");
        }

        public void Append(string message)
        {
            lock (this._locker)
            {
                this.logger.Info(message);
            }
        }

        public void Reconfigure(string newFilePath)
        {
            lock (this._locker)
            {
                var target = (FileTarget)LogManager.Configuration.FindTargetByName("fileTarget");

                var logEventInfo = new LogEventInfo
                {
                    TimeStamp = DateTime.Now
                };

                var fileName = target.FileName.Render(logEventInfo);

                if (fileName.Equals(newFilePath, StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }

                string fileFullPath = Path.GetFullPath(fileName);
                string newFileFullPath = Path.GetFullPath(newFilePath);

                // Move or rename the current log file.
                if (File.Exists(fileFullPath) && !File.Exists(newFileFullPath))
                {
                    File.Move(fileFullPath, newFileFullPath); 
                }

                ConfigFileTarget(target, newFilePath);

                LogManager.ReconfigExistingLoggers();
            }
        }

        private void ConfigFileTarget(FileTarget target, String filePath)
        {
            target.FileName = filePath;
            target.ArchiveFileName = Path.Combine(Path.GetDirectoryName(filePath), Path.GetFileNameWithoutExtension(filePath) + "-{#}" + Path.GetExtension(filePath));
        }
    }
}
