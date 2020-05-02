using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using uRADMonitorX.Commons;
using uRADMonitorX.Commons.Formatting;
using uRADMonitorX.Commons.Logging;
using uRADMonitorX.Extensions;
using uRADMonitorX.Updater;

namespace uRADMonitorX
{
    public partial class FormUpdate : Form
    {
        private ILogger logger;
        private IUpdateInfo updateInfo;

        private string updateFileTempPath;

        public FormUpdate(ILogger logger)
        {
            InitializeComponent();

            this.logger = logger;

            this.buttonUpdate.Enabled = false;
            this.buttonGoToDownloadPage.Enabled = false;

            var asyncUpdateThread = new Thread(new ThreadStart(delegate { this.CheckForUpdates(); }))
            {
                Name = "checkForUpdatesThread"
            };

            asyncUpdateThread.Start();
        }

        private void CheckForUpdates()
        {
            this.ToggleButtonState(this.buttonUpdate, false);

            this.UpdateButtonText(this.buttonUpdate, "Update");
            this.UpdateStatus("Searching for updates, please wait a few seconds...");

            try
            {
                var currentVersion = AssemblyUtils.GetVersion();

                this.updateInfo = Program.ApplicationUpdater.Check(Program.UpdateUrl);

                if (this.updateInfo.IsNewVersionAvailable(currentVersion))
                {
                    this.UpdateStatus(string.Format("A new version of uRADMonitorX ({0}) is available.", updateInfo.Version));

                    if (this.updateInfo.DownloadPage.IsNotNullOrEmpty())
                    {
                        this.ToggleButtonState(this.buttonGoToDownloadPage, true);
                    }

                    this.ToggleButtonState(this.buttonUpdate, true);
                }
                else if (this.updateInfo.IsCurrentVersionNewer(currentVersion))
                {
                    this.UpdateStatus("You are using a newer version of uRADMonitorX.");
                }
                else
                {
                    this.UpdateStatus("You are using the latest version of uRADMonitorX.");
                }
            }
            catch (Exception ex)
            {
                this.pictureBoxStatus.Image = global::uRADMonitorX.Properties.Resources.exclamation;

                this.UpdateStatus(string.Format("An error occurred while checking for updates: {0}.", TextStyleFormatter.LowercaseFirst(ex.Message).TrimEnd('.')));
                this.logger.Write(string.Format("An error occurred while checking for updates. Exception: {0}", ex.ToString()));

                Debug.WriteLine($"[{Program.ApplicationName}] [{nameof(FormUpdate)}] Exception: {ex.ToString()}");

                if (ex.Message.Equals("Unable to connect to the remote server", StringComparison.OrdinalIgnoreCase))
                {
                    this.UpdateButtonText(this.buttonUpdate, "Retry");
                    this.ToggleButtonState(this.buttonUpdate, true);
                }
            }
        }

        private void UpdateStatus(string status)
        {
            if (((ISynchronizeInvoke)this.labelStatus).InvokeRequired)
            {
                this.Invoke(new UpdateStatusCallback(this.UpdateStatus), new object[] { status });
            }
            else
            {
                this.labelStatus.Text = status;
                this.Refresh();
            }
        }

        private delegate void UpdateStatusCallback(string status);

        private void ToggleButtonState(Button button, bool isEnabled)
        {
            if (((ISynchronizeInvoke)button).InvokeRequired)
            {
                this.Invoke(new ToggleButtonStateCallback(this.ToggleButtonState), new object[] { button, isEnabled });
            }
            else
            {
                button.Enabled = isEnabled;
            }
        }

        private delegate void ToggleButtonStateCallback(Button button, bool isEnabled);

        private void UpdateButtonText(Button button, string text)
        {
            if (((ISynchronizeInvoke)button).InvokeRequired)
            {
                this.Invoke(new UpdateButtonTextCallback(this.UpdateButtonText), new object[] { button, text });
            }
            else
            {
                button.Text = text;
            }
        }

        private delegate void UpdateButtonTextCallback(Button button, String text);

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ButtonGoToDownloadPage_Click(object sender, EventArgs e)
        {
            if (this.updateInfo.IsNotNull() && this.updateInfo.DownloadPage.IsNotNullOrEmpty())
            {
                Process.Start(this.updateInfo.DownloadPage);
            }
        }

        private void ButtonUpdate_Click(object sender, EventArgs e)
        {
            // TODO: Run this async.
            try
            {
                if (this.buttonUpdate.Text.Equals("Update"))
                {
                    if (this.updateInfo.IsNotNull())
                    {
                        this.buttonUpdate.Enabled = false;

                        this.UpdateStatus(string.Format("Downloading update, please wait a few seconds..."));

                        // Retrieve the new file.
                        var fileContent = Program.ApplicationUpdater.Download(this.updateInfo.DownloadUrl);

                        // Save file to temporary directory.
                        this.updateFileTempPath = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());

                        File.WriteAllBytes(updateFileTempPath, fileContent);

                        if (!updateInfo.Checksum.Verify(updateFileTempPath))
                        {
                            throw new Exception("Downloaded file checksum does not match.");
                        }

                        this.UpdateStatus(String.Format("Application needs to be restarted to complete the update process."));

                        if (!EnvironmentUtils.IsMonoRuntime())
                        {
                            this.buttonUpdate.Text = "Restart";
                            this.buttonUpdate.Enabled = true;
                        }
                        else
                        {
                            string executingAssemblyPath = AssemblyUtils.GetApplicationPath();

                            // Rename executing assembly.
                            File.Move(executingAssemblyPath, executingAssemblyPath + ".tmp");

                            // Move temp file to current assembly path.
                            File.Move(updateFileTempPath, executingAssemblyPath);
                        }
                    }
                }
                else if (this.buttonUpdate.Text.Equals("Restart"))
                {
                    this.buttonUpdate.Enabled = false;

                    var executingAssemblyPath = AssemblyUtils.GetApplicationPath();

                    // Rename executing assembly.
                    File.Move(executingAssemblyPath, executingAssemblyPath + ".tmp");

                    // Move temp file to current assembly path.
                    File.Move(updateFileTempPath, executingAssemblyPath);

                    // Restart application with '--cleanup-update'
                    var processStartInfo = new ProcessStartInfo(executingAssemblyPath, "--allow-multiple-instances --cleanup-update")
                    {
                        WorkingDirectory = Path.GetDirectoryName(executingAssemblyPath)
                    };

                    Process.Start(processStartInfo);

                    Application.Exit();
                }
                else if (this.buttonUpdate.Text.Equals("Retry"))
                {
                    var asyncUpdateThread = new Thread(new ThreadStart(delegate { this.CheckForUpdates(); }))
                    {
                        Name = "checkForUpdatesThread"
                    };

                    asyncUpdateThread.Start();
                }
            }
            catch (Exception ex)
            {
                this.pictureBoxStatus.Image = global::uRADMonitorX.Properties.Resources.exclamation;

                this.UpdateStatus(string.Format("An error occurred while downloading application update: {0}.", TextStyleFormatter.LowercaseFirst(ex.Message).Trim().TrimEnd('.')));
                this.logger.Write(string.Format("An error occurred while downloading application update. Exception: {0}", ex.ToString()));

                Debug.WriteLine($"[{Program.ApplicationName}] [{nameof(FormUpdate)}] Exception: {ex.ToString()}");
            }
        }
    }
}
