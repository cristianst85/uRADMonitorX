namespace uRADMonitorX {
    partial class FormNetwork {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormNetwork));
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBoxDeviceInformation = new System.Windows.Forms.GroupBox();
            this.richTextBoxRaw = new System.Windows.Forms.RichTextBox();
            this.textBoxStatus = new uRADMonitorX.Commons.Controls.ViewOnlyTextBox();
            this.labelStatus = new System.Windows.Forms.Label();
            this.textBoxPlacement = new uRADMonitorX.Commons.Controls.ViewOnlyTextBox();
            this.labelPlacement = new System.Windows.Forms.Label();
            this.labelLongitude = new System.Windows.Forms.Label();
            this.textBoxLongitude = new uRADMonitorX.Commons.Controls.ViewOnlyTextBox();
            this.textBoxLatitude = new uRADMonitorX.Commons.Controls.ViewOnlyTextBox();
            this.labelLatitude = new System.Windows.Forms.Label();
            this.groupBoxDevices = new System.Windows.Forms.GroupBox();
            this.checkBoxShowOnlineDevicesOnly = new System.Windows.Forms.CheckBox();
            this.listViewDevices = new System.Windows.Forms.ListView();
            this.id = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.location = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.configToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.authenticationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBoxDeviceInformation.SuspendLayout();
            this.groupBoxDevices.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(184, 402);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 8;
            this.buttonCancel.Text = "Close";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // groupBoxDeviceInformation
            // 
            this.groupBoxDeviceInformation.Controls.Add(this.richTextBoxRaw);
            this.groupBoxDeviceInformation.Controls.Add(this.textBoxStatus);
            this.groupBoxDeviceInformation.Controls.Add(this.labelStatus);
            this.groupBoxDeviceInformation.Controls.Add(this.textBoxPlacement);
            this.groupBoxDeviceInformation.Controls.Add(this.labelPlacement);
            this.groupBoxDeviceInformation.Controls.Add(this.labelLongitude);
            this.groupBoxDeviceInformation.Controls.Add(this.textBoxLongitude);
            this.groupBoxDeviceInformation.Controls.Add(this.textBoxLatitude);
            this.groupBoxDeviceInformation.Controls.Add(this.labelLatitude);
            this.groupBoxDeviceInformation.Location = new System.Drawing.Point(13, 209);
            this.groupBoxDeviceInformation.Name = "groupBoxDeviceInformation";
            this.groupBoxDeviceInformation.Size = new System.Drawing.Size(414, 187);
            this.groupBoxDeviceInformation.TabIndex = 10;
            this.groupBoxDeviceInformation.TabStop = false;
            this.groupBoxDeviceInformation.Text = "Device Information";
            // 
            // richTextBoxRaw
            // 
            this.richTextBoxRaw.DetectUrls = false;
            this.richTextBoxRaw.Location = new System.Drawing.Point(5, 78);
            this.richTextBoxRaw.Name = "richTextBoxRaw";
            this.richTextBoxRaw.ReadOnly = true;
            this.richTextBoxRaw.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.richTextBoxRaw.Size = new System.Drawing.Size(401, 96);
            this.richTextBoxRaw.TabIndex = 8;
            this.richTextBoxRaw.Text = "";
            // 
            // textBoxStatus
            // 
            this.textBoxStatus.Location = new System.Drawing.Point(251, 48);
            this.textBoxStatus.Name = "textBoxStatus";
            this.textBoxStatus.ReadOnly = true;
            this.textBoxStatus.ShortcutsEnabled = false;
            this.textBoxStatus.Size = new System.Drawing.Size(155, 20);
            this.textBoxStatus.TabIndex = 7;
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(206, 51);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(40, 13);
            this.labelStatus.TabIndex = 6;
            this.labelStatus.Text = "Status:";
            // 
            // textBoxPlacement
            // 
            this.textBoxPlacement.Location = new System.Drawing.Point(251, 19);
            this.textBoxPlacement.Name = "textBoxPlacement";
            this.textBoxPlacement.ReadOnly = true;
            this.textBoxPlacement.ShortcutsEnabled = false;
            this.textBoxPlacement.Size = new System.Drawing.Size(155, 20);
            this.textBoxPlacement.TabIndex = 5;
            // 
            // labelPlacement
            // 
            this.labelPlacement.AutoSize = true;
            this.labelPlacement.Location = new System.Drawing.Point(186, 22);
            this.labelPlacement.Name = "labelPlacement";
            this.labelPlacement.Size = new System.Drawing.Size(60, 13);
            this.labelPlacement.TabIndex = 4;
            this.labelPlacement.Text = "Placement:";
            // 
            // labelLongitude
            // 
            this.labelLongitude.AutoSize = true;
            this.labelLongitude.Location = new System.Drawing.Point(16, 51);
            this.labelLongitude.Name = "labelLongitude";
            this.labelLongitude.Size = new System.Drawing.Size(57, 13);
            this.labelLongitude.TabIndex = 3;
            this.labelLongitude.Text = "Longitude:";
            // 
            // textBoxLongitude
            // 
            this.textBoxLongitude.Location = new System.Drawing.Point(79, 48);
            this.textBoxLongitude.Name = "textBoxLongitude";
            this.textBoxLongitude.ReadOnly = true;
            this.textBoxLongitude.ShortcutsEnabled = false;
            this.textBoxLongitude.Size = new System.Drawing.Size(100, 20);
            this.textBoxLongitude.TabIndex = 2;
            // 
            // textBoxLatitude
            // 
            this.textBoxLatitude.Location = new System.Drawing.Point(79, 19);
            this.textBoxLatitude.Name = "textBoxLatitude";
            this.textBoxLatitude.ReadOnly = true;
            this.textBoxLatitude.ShortcutsEnabled = false;
            this.textBoxLatitude.Size = new System.Drawing.Size(100, 20);
            this.textBoxLatitude.TabIndex = 1;
            // 
            // labelLatitude
            // 
            this.labelLatitude.AutoSize = true;
            this.labelLatitude.Location = new System.Drawing.Point(25, 22);
            this.labelLatitude.Name = "labelLatitude";
            this.labelLatitude.Size = new System.Drawing.Size(48, 13);
            this.labelLatitude.TabIndex = 0;
            this.labelLatitude.Text = "Latitude:";
            // 
            // groupBoxDevices
            // 
            this.groupBoxDevices.Controls.Add(this.checkBoxShowOnlineDevicesOnly);
            this.groupBoxDevices.Controls.Add(this.listViewDevices);
            this.groupBoxDevices.Controls.Add(this.buttonRefresh);
            this.groupBoxDevices.Location = new System.Drawing.Point(12, 27);
            this.groupBoxDevices.Name = "groupBoxDevices";
            this.groupBoxDevices.Size = new System.Drawing.Size(414, 176);
            this.groupBoxDevices.TabIndex = 11;
            this.groupBoxDevices.TabStop = false;
            this.groupBoxDevices.Text = "Devices";
            // 
            // checkBoxShowOnlineDevicesOnly
            // 
            this.checkBoxShowOnlineDevicesOnly.AutoSize = true;
            this.checkBoxShowOnlineDevicesOnly.Location = new System.Drawing.Point(20, 147);
            this.checkBoxShowOnlineDevicesOnly.Name = "checkBoxShowOnlineDevicesOnly";
            this.checkBoxShowOnlineDevicesOnly.Size = new System.Drawing.Size(152, 17);
            this.checkBoxShowOnlineDevicesOnly.TabIndex = 10;
            this.checkBoxShowOnlineDevicesOnly.Text = "Show Online Devices Only";
            this.checkBoxShowOnlineDevicesOnly.UseVisualStyleBackColor = true;
            // 
            // listViewDevices
            // 
            this.listViewDevices.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.id,
            this.location,
            this.status});
            this.listViewDevices.FullRowSelect = true;
            this.listViewDevices.HideSelection = false;
            this.listViewDevices.Location = new System.Drawing.Point(6, 19);
            this.listViewDevices.Name = "listViewDevices";
            this.listViewDevices.Size = new System.Drawing.Size(402, 118);
            this.listViewDevices.TabIndex = 8;
            this.listViewDevices.UseCompatibleStateImageBehavior = false;
            this.listViewDevices.View = System.Windows.Forms.View.Details;
            // 
            // id
            // 
            this.id.Text = "ID";
            this.id.Width = 109;
            // 
            // location
            // 
            this.location.Text = "Location";
            this.location.Width = 160;
            // 
            // status
            // 
            this.status.Text = "Status";
            this.status.Width = 109;
            // 
            // buttonRefresh
            // 
            this.buttonRefresh.Location = new System.Drawing.Point(333, 143);
            this.buttonRefresh.Name = "buttonRefresh";
            this.buttonRefresh.Size = new System.Drawing.Size(75, 23);
            this.buttonRefresh.TabIndex = 9;
            this.buttonRefresh.Text = "Refresh";
            this.buttonRefresh.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(439, 24);
            this.menuStrip1.TabIndex = 12;
            this.menuStrip1.Text = "menuStrip";
            // 
            // configToolStripMenuItem
            // 
            this.configToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.authenticationToolStripMenuItem});
            this.configToolStripMenuItem.Name = "configToolStripMenuItem";
            this.configToolStripMenuItem.Size = new System.Drawing.Size(93, 20);
            this.configToolStripMenuItem.Text = "Configuration";
            // 
            // authenticationToolStripMenuItem
            // 
            this.authenticationToolStripMenuItem.Name = "authenticationToolStripMenuItem";
            this.authenticationToolStripMenuItem.Size = new System.Drawing.Size(162, 22);
            this.authenticationToolStripMenuItem.Text = "Authentication...";
            // 
            // FormNetwork
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(439, 437);
            this.Controls.Add(this.groupBoxDevices);
            this.Controls.Add(this.groupBoxDeviceInformation);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormNetwork";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "uRADMonitor Network";
            this.groupBoxDeviceInformation.ResumeLayout(false);
            this.groupBoxDeviceInformation.PerformLayout();
            this.groupBoxDevices.ResumeLayout(false);
            this.groupBoxDevices.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.GroupBox groupBoxDeviceInformation;
        private System.Windows.Forms.GroupBox groupBoxDevices;
        private System.Windows.Forms.ListView listViewDevices;
        private System.Windows.Forms.Button buttonRefresh;
        private System.Windows.Forms.ColumnHeader id;
        private System.Windows.Forms.ColumnHeader location;
        private Commons.Controls.ViewOnlyTextBox textBoxLatitude;
        private Commons.Controls.ViewOnlyTextBox textBoxStatus;
        private System.Windows.Forms.Label labelStatus;
        private Commons.Controls.ViewOnlyTextBox textBoxPlacement;
        private System.Windows.Forms.Label labelPlacement;
        private System.Windows.Forms.Label labelLongitude;
        private Commons.Controls.ViewOnlyTextBox textBoxLongitude;
        private System.Windows.Forms.ColumnHeader status;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem configToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem authenticationToolStripMenuItem;
        private System.Windows.Forms.Label labelLatitude;
        private System.Windows.Forms.RichTextBox richTextBoxRaw;
        private System.Windows.Forms.CheckBox checkBoxShowOnlineDevicesOnly;
    }
}