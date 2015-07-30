using System;
using System.IO;

namespace uRADMonitorX.Commons.Logging.Appenders {

    public class FileAppender : ILoggerAppender, ICanReconfigureAppender {

        private volatile bool _enabled;
        public bool Enabled {
            get {
                return _enabled;
            }
            set {
                _enabled = value;
            }
        }

        public String FilePath { get; protected set; }

        public FileAppender(String filePath) {
            this.FilePath = filePath;
        }

        public virtual void Append(String message) {
            if (this.Enabled) {
                using (TextWriter writer = File.AppendText(this.FilePath)) {
                    writer.WriteLine(message);
                }
            }
        }

        public virtual void Reconfigure(String filePath) {
            bool wasEnabled = this.Enabled;
            try {
                if (this.FilePath.Equals(filePath, StringComparison.OrdinalIgnoreCase)) {
                    return;
                }
                this.Enabled = false;
                String fileFullPath = Path.GetFullPath(this.FilePath);
                String newFileFullPath = Path.GetFullPath(filePath);
                if (File.Exists(fileFullPath) && !File.Exists(newFileFullPath)) {
                    File.Move(fileFullPath, newFileFullPath); // Move or rename current log file.
                }
                this.FilePath = filePath;
            }
            finally {
                this.Enabled = wasEnabled;
            }
        }
    }
}
