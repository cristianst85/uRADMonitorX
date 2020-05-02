using System;
using System.IO;

namespace uRADMonitorX.Commons.Logging.Appenders
{
    public class ReconfigurableFileAppender : ILoggerAppender, IReconfigurableFileAppender
    {
        private readonly object _locker = new object();

        public string FilePath { get; protected set; }

        public ReconfigurableFileAppender(string filePath)
        {
            this.FilePath = filePath;
        }

        public virtual void Append(String message)
        {
            // Don't miss messages during reconfiguration.
            lock (this._locker)
            {
                using (var writer = File.AppendText(this.FilePath))
                {
                    writer.WriteLine(message);
                }
            }
        }

        public virtual void Reconfigure(String newFilePath)
        {
            lock (this._locker)
            {
                if (this.FilePath.Equals(newFilePath, StringComparison.OrdinalIgnoreCase))
                {
                    return;
                }

                string fileFullPath = Path.GetFullPath(this.FilePath);
                string newFileFullPath = Path.GetFullPath(newFilePath);

                // Move or rename the current log file.
                if (File.Exists(fileFullPath) && !File.Exists(newFileFullPath))
                {
                    File.Move(fileFullPath, newFileFullPath); 
                }

                this.FilePath = newFilePath;
            }
        }
    }
}
