using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using uRADMonitorX.Commons;
using uRADMonitorX.Commons.Formatting;
using uRADMonitorX.Commons.Logging;
using uRADMonitorX.Updater;

namespace uRADMonitorX {

    public partial class FormUpdate : Form {

        private ILogger logger = null;

        private IHttpApplicationUpdater httpApplicationUpdater = null;
        private IApplicationUpdateInfo applicationUpdateInfo = null;
        private String tempPath = null;

        public FormUpdate(ILogger logger) {
            InitializeComponent();

            this.logger = logger;

            this.buttonUpdate.Enabled = false;
            this.buttonGoToDownloadPage.Enabled = false;

            Thread asyncUpdateThread = new Thread(new ThreadStart(delegate { this.checkForUpdates(); }));
            asyncUpdateThread.Name = "checkForUpdatesThread";
            asyncUpdateThread.Start();
        }

        private void checkForUpdates() {
            this.toggleButtonState(this.buttonUpdate, false);
            this.updateButtonText(this.buttonUpdate, "Update");
            this.updateStatus(String.Format("Fetching{0}{1}, please wait a few seconds...", CharUtils.NonBreakingSpace, Program.UpdaterUrl));
            try {
                this.httpApplicationUpdater = new GitHubApplicationUpdater(Program.UpdaterUrl);
                this.applicationUpdateInfo = this.httpApplicationUpdater.Check();
                if (applicationUpdateInfo.IsNewVersionAvailable(AssemblyUtils.GetVersion())) {
                    this.updateStatus(String.Format("A new version of uRADMonitorX ({0}) is available.", applicationUpdateInfo.Version));
                    if (!String.IsNullOrEmpty(this.applicationUpdateInfo.DownloadPage)) {
                        this.toggleButtonState(this.buttonGoToDownloadPage, true);
                    }
                    this.toggleButtonState(this.buttonUpdate, true);
                }
                else {
                    this.updateStatus("You are using the latest version of uRADMonitorX.");
                }
            }
            catch (Exception ex) {
                this.pictureBoxStatus.Image = global::uRADMonitorX.Properties.Resources.exclamation;
                this.updateStatus(String.Format("An error occurred while checking for application update: {0}.", TextStyleFormatter.LowercaseFirst(ex.Message).TrimEnd('.')));
                this.logger.Write(String.Format("An error occurred while checking for application update. Exception: {0}", ex.ToString()));
                Debug.WriteLine(String.Format("FormUpdate > Exception: {0}", ex.ToString()));
                if (ex.Message.Equals("Unable to connect to the remote server", StringComparison.OrdinalIgnoreCase)) {
                    this.updateButtonText(this.buttonUpdate, "Retry");
                    this.toggleButtonState(this.buttonUpdate, true);
                }
            }
        }

        private void updateStatus(String status) {
            if (((ISynchronizeInvoke)this.labelStatus).InvokeRequired) {
                this.Invoke(new updateStatusCallback(this.updateStatus), new object[] { status });
            }
            else {
                this.labelStatus.Text = status;
                this.Refresh();
            }
        }

        private void toggleButtonState(Button button, bool isEnabled) {
            if (((ISynchronizeInvoke)button).InvokeRequired) {
                this.Invoke(new toggleButtonStateCallback(this.toggleButtonState), new object[] { button, isEnabled });
            }
            else {
                button.Enabled = isEnabled;
            }
        }

        private void updateButtonText(Button button, String text) {
            if (((ISynchronizeInvoke)button).InvokeRequired) {
                this.Invoke(new updateButtonTextCallback(this.updateButtonText), new object[] { button, text });
            }
            else {
                button.Text = text;
            }
        }

        private delegate void updateStatusCallback(String status);

        private delegate void toggleButtonStateCallback(Button button, bool isEnabled);

        private delegate void updateButtonTextCallback(Button button, String text);

        private void buttonClose_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void buttonGoToDownloadPage_Click(object sender, EventArgs e) {
            if (this.applicationUpdateInfo != null && !String.IsNullOrEmpty(this.applicationUpdateInfo.DownloadPage)) {
                System.Diagnostics.Process.Start(String.Format("{0}", this.applicationUpdateInfo.DownloadPage));
            }
        }

        private void buttonUpdate_Click(object sender, EventArgs e) {
            // TODO: Run this async.
            try {
                if (this.buttonUpdate.Text.Equals("Update")) {
                    if (this.httpApplicationUpdater != null) {
                        this.buttonUpdate.Enabled = false;
                        this.updateStatus(String.Format("Downloading update, please wait a few seconds..."));
                        tempPath = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());
                        // Download to temporary directory.
                        this.httpApplicationUpdater.Download(this.applicationUpdateInfo.DownloadUrl, tempPath);
                        // Verify the hash for downloaded file.
                        String checksum = ChecksumUtils.ComputeMD5(tempPath);
                        // TODO: Use SHA-1 instead.
                        if (!checksum.Equals(applicationUpdateInfo.FileHash, StringComparison.OrdinalIgnoreCase)) {
                            throw new Exception("Checksum of the downloaded file doesn't match the one found at the remote site.");
                        }
                        this.updateStatus(String.Format("Application needs to be restarted to complete the update process."));
                        if (!EnvironmentUtils.IsMonoRuntime()) {
                            this.buttonUpdate.Text = "Restart";
                            this.buttonUpdate.Enabled = true;
                        }
                        else {
                            String executingAssemblyPath = AssemblyUtils.GetApplicationPath();
                            // Rename executing assembly.
                            File.Move(executingAssemblyPath, executingAssemblyPath + ".tmp");
                            // Move temp file to current assembly path.
                            File.Move(tempPath, executingAssemblyPath);
                        }
                    }
                }
                else if (this.buttonUpdate.Text.Equals("Restart")) {
                    this.buttonUpdate.Enabled = false;
                    String executingAssemblyPath = AssemblyUtils.GetApplicationPath();
                    // Rename executing assembly.
                    File.Move(executingAssemblyPath, executingAssemblyPath + ".tmp");
                    // Move temp file to current assembly path.
                    File.Move(tempPath, executingAssemblyPath);
                    // Restart application with '--cleanup-update'
                    ProcessStartInfo processStartInfo = new ProcessStartInfo(executingAssemblyPath, "--allow-multiple-instances --cleanup-update");
                    processStartInfo.WorkingDirectory = Path.GetDirectoryName(executingAssemblyPath);
                    Process.Start(processStartInfo);
                    Application.Exit();
                }
                else if (this.buttonUpdate.Text.Equals("Retry")) {
                    Thread asyncUpdateThread = new Thread(new ThreadStart(delegate { this.checkForUpdates(); }));
                    asyncUpdateThread.Name = "checkForUpdatesThread";
                    asyncUpdateThread.Start();
                }
            }
            catch (Exception ex) {
                this.pictureBoxStatus.Image = global::uRADMonitorX.Properties.Resources.exclamation;
                this.updateStatus(String.Format("An error occurred while downloading application update: {0}.", TextStyleFormatter.LowercaseFirst(ex.Message).Trim().TrimEnd('.')));
                this.logger.Write(String.Format("An error occurred while downloading application update. Exception: {0}", ex.ToString()));
                Debug.WriteLine(String.Format("FormUpdate > Exception: {0}", ex.ToString()));
            }
        }
    }
}
