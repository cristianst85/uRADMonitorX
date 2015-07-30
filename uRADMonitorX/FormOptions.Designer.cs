namespace uRADMonitorX {
    partial class FormOptions {
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
            this.buttonApply = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.checkBoxShowInTaskbar = new System.Windows.Forms.CheckBox();
            this.checkBoxCloseToSystemTray = new System.Windows.Forms.CheckBox();
            this.checkBoxStartMinimized = new System.Windows.Forms.CheckBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.pictureBoxLoggingInfo = new System.Windows.Forms.PictureBox();
            this.labelLoggingInfo = new System.Windows.Forms.Label();
            this.textBoxLogDirectoryPath = new System.Windows.Forms.TextBox();
            this.buttonConfigureLogDirectoryPath = new System.Windows.Forms.Button();
            this.checkBoxLoggingEnable = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxStartWithWindows = new System.Windows.Forms.CheckBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageMisc = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.labelRadiationUnit = new System.Windows.Forms.Label();
            this.comboBoxRadiationUnit = new System.Windows.Forms.ComboBox();
            this.comboBoxPressureUnit = new System.Windows.Forms.ComboBox();
            this.comboBoxTemperatureUnit = new System.Windows.Forms.ComboBox();
            this.labelPressureUnit = new System.Windows.Forms.Label();
            this.labelTemperatureUnit = new System.Windows.Forms.Label();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLoggingInfo)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPageMisc.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonApply
            // 
            this.buttonApply.Location = new System.Drawing.Point(232, 226);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(75, 23);
            this.buttonApply.TabIndex = 1;
            this.buttonApply.Text = "Apply";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(151, 226);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 2;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            // 
            // buttonOK
            // 
            this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOK.Location = new System.Drawing.Point(70, 226);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 3;
            this.buttonOK.Text = "OK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(287, 182);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Display";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.checkBoxShowInTaskbar);
            this.groupBox2.Controls.Add(this.checkBoxCloseToSystemTray);
            this.groupBox2.Controls.Add(this.checkBoxStartMinimized);
            this.groupBox2.Location = new System.Drawing.Point(3, 1);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(280, 175);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Settings";
            // 
            // checkBoxShowInTaskbar
            // 
            this.checkBoxShowInTaskbar.AutoSize = true;
            this.checkBoxShowInTaskbar.Location = new System.Drawing.Point(19, 19);
            this.checkBoxShowInTaskbar.Name = "checkBoxShowInTaskbar";
            this.checkBoxShowInTaskbar.Size = new System.Drawing.Size(102, 17);
            this.checkBoxShowInTaskbar.TabIndex = 3;
            this.checkBoxShowInTaskbar.Text = "Show in taskbar";
            this.checkBoxShowInTaskbar.UseVisualStyleBackColor = true;
            // 
            // checkBoxCloseToSystemTray
            // 
            this.checkBoxCloseToSystemTray.AutoSize = true;
            this.checkBoxCloseToSystemTray.Location = new System.Drawing.Point(19, 65);
            this.checkBoxCloseToSystemTray.Name = "checkBoxCloseToSystemTray";
            this.checkBoxCloseToSystemTray.Size = new System.Drawing.Size(193, 17);
            this.checkBoxCloseToSystemTray.TabIndex = 2;
            this.checkBoxCloseToSystemTray.Text = "Close uRADMonitorX to system tray";
            this.checkBoxCloseToSystemTray.UseVisualStyleBackColor = true;
            // 
            // checkBoxStartMinimized
            // 
            this.checkBoxStartMinimized.AutoSize = true;
            this.checkBoxStartMinimized.Location = new System.Drawing.Point(19, 42);
            this.checkBoxStartMinimized.Name = "checkBoxStartMinimized";
            this.checkBoxStartMinimized.Size = new System.Drawing.Size(170, 17);
            this.checkBoxStartMinimized.TabIndex = 1;
            this.checkBoxStartMinimized.Text = "Start uRADMonitorX minimized";
            this.checkBoxStartMinimized.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox5);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(287, 182);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.pictureBoxLoggingInfo);
            this.groupBox5.Controls.Add(this.labelLoggingInfo);
            this.groupBox5.Controls.Add(this.textBoxLogDirectoryPath);
            this.groupBox5.Controls.Add(this.buttonConfigureLogDirectoryPath);
            this.groupBox5.Controls.Add(this.checkBoxLoggingEnable);
            this.groupBox5.Location = new System.Drawing.Point(3, 57);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(280, 119);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Logging";
            // 
            // pictureBoxLoggingInfo
            // 
            this.pictureBoxLoggingInfo.ErrorImage = null;
            this.pictureBoxLoggingInfo.Image = global::uRADMonitorX.Properties.Resources.information;
            this.pictureBoxLoggingInfo.Location = new System.Drawing.Point(19, 71);
            this.pictureBoxLoggingInfo.Name = "pictureBoxLoggingInfo";
            this.pictureBoxLoggingInfo.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxLoggingInfo.TabIndex = 6;
            this.pictureBoxLoggingInfo.TabStop = false;
            // 
            // labelLoggingInfo
            // 
            this.labelLoggingInfo.AutoSize = true;
            this.labelLoggingInfo.Location = new System.Drawing.Point(41, 72);
            this.labelLoggingInfo.Name = "labelLoggingInfo";
            this.labelLoggingInfo.Size = new System.Drawing.Size(150, 13);
            this.labelLoggingInfo.TabIndex = 5;
            this.labelLoggingInfo.Text = "Using default application path.";
            // 
            // textBoxLogDirectoryPath
            // 
            this.textBoxLogDirectoryPath.HideSelection = false;
            this.textBoxLogDirectoryPath.Location = new System.Drawing.Point(19, 41);
            this.textBoxLogDirectoryPath.Name = "textBoxLogDirectoryPath";
            this.textBoxLogDirectoryPath.Size = new System.Drawing.Size(255, 20);
            this.textBoxLogDirectoryPath.TabIndex = 4;
            // 
            // buttonConfigureLogDirectoryPath
            // 
            this.buttonConfigureLogDirectoryPath.Location = new System.Drawing.Point(201, 67);
            this.buttonConfigureLogDirectoryPath.Name = "buttonConfigureLogDirectoryPath";
            this.buttonConfigureLogDirectoryPath.Size = new System.Drawing.Size(73, 23);
            this.buttonConfigureLogDirectoryPath.TabIndex = 3;
            this.buttonConfigureLogDirectoryPath.Text = "Configure...";
            this.buttonConfigureLogDirectoryPath.UseVisualStyleBackColor = true;
            // 
            // checkBoxLoggingEnable
            // 
            this.checkBoxLoggingEnable.AutoSize = true;
            this.checkBoxLoggingEnable.Location = new System.Drawing.Point(19, 19);
            this.checkBoxLoggingEnable.Name = "checkBoxLoggingEnable";
            this.checkBoxLoggingEnable.Size = new System.Drawing.Size(59, 17);
            this.checkBoxLoggingEnable.TabIndex = 0;
            this.checkBoxLoggingEnable.Text = "Enable";
            this.checkBoxLoggingEnable.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBoxStartWithWindows);
            this.groupBox1.Location = new System.Drawing.Point(3, 1);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(280, 50);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // checkBoxStartWithWindows
            // 
            this.checkBoxStartWithWindows.AutoSize = true;
            this.checkBoxStartWithWindows.Location = new System.Drawing.Point(19, 19);
            this.checkBoxStartWithWindows.Name = "checkBoxStartWithWindows";
            this.checkBoxStartWithWindows.Size = new System.Drawing.Size(191, 17);
            this.checkBoxStartWithWindows.TabIndex = 0;
            this.checkBoxStartWithWindows.Text = "Start uRADMonitorX with Windows";
            this.checkBoxStartWithWindows.UseVisualStyleBackColor = true;
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Controls.Add(this.tabPageMisc);
            this.tabControl.Location = new System.Drawing.Point(12, 12);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(295, 208);
            this.tabControl.TabIndex = 0;
            // 
            // tabPageMisc
            // 
            this.tabPageMisc.Controls.Add(this.groupBox3);
            this.tabPageMisc.Location = new System.Drawing.Point(4, 22);
            this.tabPageMisc.Name = "tabPageMisc";
            this.tabPageMisc.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageMisc.Size = new System.Drawing.Size(287, 182);
            this.tabPageMisc.TabIndex = 2;
            this.tabPageMisc.Text = "Misc";
            this.tabPageMisc.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.labelRadiationUnit);
            this.groupBox3.Controls.Add(this.comboBoxRadiationUnit);
            this.groupBox3.Controls.Add(this.comboBoxPressureUnit);
            this.groupBox3.Controls.Add(this.comboBoxTemperatureUnit);
            this.groupBox3.Controls.Add(this.labelPressureUnit);
            this.groupBox3.Controls.Add(this.labelTemperatureUnit);
            this.groupBox3.Location = new System.Drawing.Point(3, 1);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(280, 175);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Settings";
            // 
            // labelRadiationUnit
            // 
            this.labelRadiationUnit.AutoSize = true;
            this.labelRadiationUnit.Location = new System.Drawing.Point(35, 73);
            this.labelRadiationUnit.Name = "labelRadiationUnit";
            this.labelRadiationUnit.Size = new System.Drawing.Size(88, 13);
            this.labelRadiationUnit.TabIndex = 5;
            this.labelRadiationUnit.Text = "Show radiation in";
            // 
            // comboBoxRadiationUnit
            // 
            this.comboBoxRadiationUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxRadiationUnit.FormattingEnabled = true;
            this.comboBoxRadiationUnit.Location = new System.Drawing.Point(129, 70);
            this.comboBoxRadiationUnit.Name = "comboBoxRadiationUnit";
            this.comboBoxRadiationUnit.Size = new System.Drawing.Size(93, 21);
            this.comboBoxRadiationUnit.TabIndex = 4;
            // 
            // comboBoxPressureUnit
            // 
            this.comboBoxPressureUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxPressureUnit.FormattingEnabled = true;
            this.comboBoxPressureUnit.Location = new System.Drawing.Point(129, 43);
            this.comboBoxPressureUnit.Name = "comboBoxPressureUnit";
            this.comboBoxPressureUnit.Size = new System.Drawing.Size(93, 21);
            this.comboBoxPressureUnit.TabIndex = 3;
            // 
            // comboBoxTemperatureUnit
            // 
            this.comboBoxTemperatureUnit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxTemperatureUnit.FormattingEnabled = true;
            this.comboBoxTemperatureUnit.Location = new System.Drawing.Point(129, 16);
            this.comboBoxTemperatureUnit.Name = "comboBoxTemperatureUnit";
            this.comboBoxTemperatureUnit.Size = new System.Drawing.Size(93, 21);
            this.comboBoxTemperatureUnit.TabIndex = 2;
            // 
            // labelPressureUnit
            // 
            this.labelPressureUnit.AutoSize = true;
            this.labelPressureUnit.Location = new System.Drawing.Point(35, 46);
            this.labelPressureUnit.Name = "labelPressureUnit";
            this.labelPressureUnit.Size = new System.Drawing.Size(88, 13);
            this.labelPressureUnit.TabIndex = 1;
            this.labelPressureUnit.Text = "Show pressure in";
            // 
            // labelTemperatureUnit
            // 
            this.labelTemperatureUnit.AutoSize = true;
            this.labelTemperatureUnit.Location = new System.Drawing.Point(19, 19);
            this.labelTemperatureUnit.Name = "labelTemperatureUnit";
            this.labelTemperatureUnit.Size = new System.Drawing.Size(104, 13);
            this.labelTemperatureUnit.TabIndex = 0;
            this.labelTemperatureUnit.Text = "Show temperature in";
            // 
            // FormOptions
            // 
            this.AcceptButton = this.buttonOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.ClientSize = new System.Drawing.Size(319, 261);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.tabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormOptions";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.tabPage2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLoggingInfo)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabPageMisc.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBoxShowInTaskbar;
        private System.Windows.Forms.CheckBox checkBoxCloseToSystemTray;
        private System.Windows.Forms.CheckBox checkBoxStartMinimized;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkBoxStartWithWindows;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageMisc;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label labelPressureUnit;
        private System.Windows.Forms.Label labelTemperatureUnit;
        private System.Windows.Forms.ComboBox comboBoxPressureUnit;
        private System.Windows.Forms.ComboBox comboBoxTemperatureUnit;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.PictureBox pictureBoxLoggingInfo;
        private System.Windows.Forms.Label labelLoggingInfo;
        private System.Windows.Forms.TextBox textBoxLogDirectoryPath;
        private System.Windows.Forms.Button buttonConfigureLogDirectoryPath;
        private System.Windows.Forms.CheckBox checkBoxLoggingEnable;
        private System.Windows.Forms.ComboBox comboBoxRadiationUnit;
        private System.Windows.Forms.Label labelRadiationUnit;
    }
}