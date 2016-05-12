using System;

namespace uRADMonitorX.Updater {

    public class GitHubApplicationUpdateInfo : IApplicationUpdateInfo {

        public String Version { get; set; }
        public String DownloadUrl { get; set; }
        public String DownloadPage { get; set; }

        private String fileHash = null;
        public String FileHash {
            get {
                if (this.fileHash == null) {
                    this.fileHash = this.updater.GetChecksum();
                }
                return fileHash;
            }
            set {
                this.fileHash = value;
            }
        }

        private GitHubApplicationUpdater updater;

        public GitHubApplicationUpdateInfo(GitHubApplicationUpdater updater) {
            this.updater = updater;
        }

        public bool IsNewVersionAvailable(Version currentVersion) {
            Version availableVersion = new Version(this.Version);
            return (availableVersion.CompareTo(currentVersion) == 1);
        }
    }
}
