using System;
using System.IO;

namespace uRADMonitorX.Commons.Logging.Appenders {

    public class ReconfigurableFileAppender : ILoggerAppender, IReconfigurableFileAppender {

        private object _locker = new object();

        public String FilePath { get; protected set; }

        public ReconfigurableFileAppender(String filePath) {
            this.FilePath = filePath;
        }

        public virtual void Append(String message) {
            // Don't miss messages during reconfiguration.
            lock (this._locker) {
                using (TextWriter writer = File.AppendText(this.FilePath)) {
                    writer.WriteLine(message);
                }
            }
        }

        public virtual void Reconfigure(String newFilePath) {
            lock (this._locker) {
                if (this.FilePath.Equals(newFilePath, StringComparison.OrdinalIgnoreCase)) {
                    return;
                }
                String fileFullPath = Path.GetFullPath(this.FilePath);
                String newFileFullPath = Path.GetFullPath(newFilePath);
                if (File.Exists(fileFullPath) && !File.Exists(newFileFullPath)) {
                    File.Move(fileFullPath, newFileFullPath); // Move or rename current log file.
                }
                this.FilePath = newFilePath;
            }
        }
    }
}
