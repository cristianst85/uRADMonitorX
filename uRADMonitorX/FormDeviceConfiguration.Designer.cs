namespace uRADMonitorX {
    partial class FormDeviceConfiguration {
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
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.labelPollingIntervalUnits = new System.Windows.Forms.Label();
            this.textBoxPollingInterval = new System.Windows.Forms.TextBox();
            this.radioButtonPollingTypeInterval = new System.Windows.Forms.RadioButton();
            this.radioButtonPollingTypeWDTSync = new System.Windows.Forms.RadioButton();
            this.textBoxIPAddress = new System.Windows.Forms.TextBox();
            this.labelPollingType = new System.Windows.Forms.Label();
            this.labelIPAddress = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.groupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox
            // 
            this.groupBox.Controls.Add(this.labelPollingIntervalUnits);
            this.groupBox.Controls.Add(this.textBoxPollingInterval);
            this.groupBox.Controls.Add(this.radioButtonPollingTypeInterval);
            this.groupBox.Controls.Add(this.radioButtonPollingTypeWDTSync);
            this.groupBox.Controls.Add(this.textBoxIPAddress);
            this.groupBox.Controls.Add(this.labelPollingType);
            this.groupBox.Controls.Add(this.labelIPAddress);
            this.groupBox.Location = new System.Drawing.Point(10, 12);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(320, 90);
            this.groupBox.TabIndex = 0;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Configuration";
            // 
            // labelPollingIntervalUnits
            // 
            this.labelPollingIntervalUnits.AutoSize = true;
            this.labelPollingIntervalUnits.Location = new System.Drawing.Point(263, 62);
            this.labelPollingIntervalUnits.Name = "labelPollingIntervalUnits";
            this.labelPollingIntervalUnits.Size = new System.Drawing.Size(53, 13);
            this.labelPollingIntervalUnits.TabIndex = 7;
            this.labelPollingIntervalUnits.Text = "second(s)";
            this.labelPollingIntervalUnits.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxPollingInterval
            // 
            this.textBoxPollingInterval.Location = new System.Drawing.Point(230, 59);
            this.textBoxPollingInterval.MaxLength = 3;
            this.textBoxPollingInterval.Name = "textBoxPollingInterval";
            this.textBoxPollingInterval.Size = new System.Drawing.Size(27, 20);
            this.textBoxPollingInterval.TabIndex = 4;
            this.textBoxPollingInterval.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // radioButtonPollingTypeInterval
            // 
            this.radioButtonPollingTypeInterval.AutoSize = true;
            this.radioButtonPollingTypeInterval.Location = new System.Drawing.Point(164, 59);
            this.radioButtonPollingTypeInterval.Name = "radioButtonPollingTypeInterval";
            this.radioButtonPollingTypeInterval.Size = new System.Drawing.Size(60, 17);
            this.radioButtonPollingTypeInterval.TabIndex = 3;
            this.radioButtonPollingTypeInterval.TabStop = true;
            this.radioButtonPollingTypeInterval.Text = "Interval";
            this.radioButtonPollingTypeInterval.UseVisualStyleBackColor = true;
            // 
            // radioButtonPollingTypeWDTSync
            // 
            this.radioButtonPollingTypeWDTSync.AutoSize = true;
            this.radioButtonPollingTypeWDTSync.Location = new System.Drawing.Point(80, 59);
            this.radioButtonPollingTypeWDTSync.Name = "radioButtonPollingTypeWDTSync";
            this.radioButtonPollingTypeWDTSync.Size = new System.Drawing.Size(78, 17);
            this.radioButtonPollingTypeWDTSync.TabIndex = 2;
            this.radioButtonPollingTypeWDTSync.TabStop = true;
            this.radioButtonPollingTypeWDTSync.Text = "WDT Sync";
            this.radioButtonPollingTypeWDTSync.UseVisualStyleBackColor = true;
            // 
            // textBoxIPAddress
            // 
            this.textBoxIPAddress.Location = new System.Drawing.Point(80, 23);
            this.textBoxIPAddress.MaxLength = 21;
            this.textBoxIPAddress.Name = "textBoxIPAddress";
            this.textBoxIPAddress.Size = new System.Drawing.Size(177, 20);
            this.textBoxIPAddress.TabIndex = 1;
            // 
            // labelPollingType
            // 
            this.labelPollingType.AutoSize = true;
            this.labelPollingType.Location = new System.Drawing.Point(10, 61);
            this.labelPollingType.Name = "labelPollingType";
            this.labelPollingType.Size = new System.Drawing.Size(64, 13);
            this.labelPollingType.TabIndex = 0;
            this.labelPollingType.Text = "Polling type:";
            // 
            // labelIPAddress
            // 
            this.labelIPAddress.AutoSize = true;
            this.labelIPAddress.Location = new System.Drawing.Point(13, 26);
            this.labelIPAddress.Name = "labelIPAddress";
            this.labelIPAddress.Size = new System.Drawing.Size(61, 13);
            this.labelIPAddress.TabIndex = 0;
            this.labelIPAddress.Text = "IP Address:";
            // 
            // buttonSave
            // 
            this.buttonSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonSave.Location = new System.Drawing.Point(90, 111);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 23);
            this.buttonSave.TabIndex = 5;
            this.buttonSave.Text = "Save";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(180, 111);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 6;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // FormDeviceConfiguration
            // 
            this.AcceptButton = this.buttonSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(344, 146);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.groupBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormDeviceConfiguration";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Device Configuration";
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Label labelPollingType;
        private System.Windows.Forms.Label labelIPAddress;
        private System.Windows.Forms.TextBox textBoxIPAddress;
        private System.Windows.Forms.Label labelPollingIntervalUnits;
        private System.Windows.Forms.TextBox textBoxPollingInterval;
        private System.Windows.Forms.RadioButton radioButtonPollingTypeInterval;
        private System.Windows.Forms.RadioButton radioButtonPollingTypeWDTSync;
    }
}