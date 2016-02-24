using System;
using System.IO;
using NLog;
using NLog.Config;
using NLog.Targets;
using uRADMonitorX.Commons.Logging.Appenders;

namespace uRADMonitorX {

    public class NLogDailyFileAppender : ILoggerAppender {

        private Logger logger;

        public NLogDailyFileAppender(String filePath) {
            var target = new FileTarget();
            target.Encoding = System.Text.Encoding.UTF8;
            target.FileName = filePath;
            target.ArchiveFileName = Path.GetFileNameWithoutExtension(filePath) + "-{#}" + Path.GetExtension(filePath);
            target.ArchiveNumbering = ArchiveNumberingMode.Date;
            target.ArchiveEvery = NLog.Targets.FileArchivePeriod.Day;
            target.Layout = @"${message}";
            target.AutoFlush = true;

            var config = new LoggingConfiguration();

            config.AddTarget("fileTarget", target);
            
            var rule = new LoggingRule("fileLogger", LogLevel.Info, target);
            config.LoggingRules.Add(rule);

            LogManager.Configuration = config;
            logger = LogManager.GetLogger("fileLogger");
        }

        public void Append(string message) {
            this.logger.Info(message);
        }
    }
}
