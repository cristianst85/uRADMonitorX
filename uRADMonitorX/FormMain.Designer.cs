using System.Windows.Forms;
using uRADMonitorX.Commons.Controls;

namespace uRADMonitorX {

    partial class FormMain {
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showHideToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enablePollingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.viewDeviceWebpageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewDeviceOnlineDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dashboardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
            this.temperatureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pressureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cpmToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.configurationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uRADMonitorWebsiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uRADMonitorForumToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
            this.checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelDeviceStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelUptime = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelDeviceUptime = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.labelId = new System.Windows.Forms.Label();
            this.labelDetector = new System.Windows.Forms.Label();
            this.labelModel = new System.Windows.Forms.Label();
            this.labelFirmware = new System.Windows.Forms.Label();
            this.labelHardware = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.labelWDT = new System.Windows.Forms.Label();
            this.labelVoltage = new System.Windows.Forms.Label();
            this.labelPressure = new System.Windows.Forms.Label();
            this.labelTemperature = new System.Windows.Forms.Label();
            this.labelRadiationAverage = new System.Windows.Forms.Label();
            this.labelRadiation = new System.Windows.Forms.Label();
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.viewDeviceWebpageToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
            this.showHideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.viewOnlyTextBoxWDT = new uRADMonitorX.Commons.Controls.ViewOnlyTextBox();
            this.viewOnlyTextBoxVoltage = new uRADMonitorX.Commons.Controls.ViewOnlyTextBox();
            this.viewOnlyTextBoxPressure = new uRADMonitorX.Commons.Controls.ViewOnlyTextBox();
            this.viewOnlyTextBoxTemperature = new uRADMonitorX.Commons.Controls.ViewOnlyTextBox();
            this.viewOnlyTextBoxRadiationAverage = new uRADMonitorX.Commons.Controls.ViewOnlyTextBox();
            this.viewOnlyTextBoxRadiation = new uRADMonitorX.Commons.Controls.ViewOnlyTextBox();
            this.viewOnlyTextBoxId = new uRADMonitorX.Commons.Controls.ViewOnlyTextBox();
            this.viewOnlyTextBoxDetector = new uRADMonitorX.Commons.Controls.ViewOnlyTextBox();
            this.viewOnlyTextBoxFirmware = new uRADMonitorX.Commons.Controls.ViewOnlyTextBox();
            this.viewOnlyTextBoxHardware = new uRADMonitorX.Commons.Controls.ViewOnlyTextBox();
            this.viewOnlyTextBoxModel = new uRADMonitorX.Commons.Controls.ViewOnlyTextBox();
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(354, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showHideToolStripMenuItem1,
            this.toolStripMenuItem5,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // showHideToolStripMenuItem1
            // 
            this.showHideToolStripMenuItem1.Image = global::uRADMonitorX.Properties.Resources.Radiation;
            this.showHideToolStripMenuItem1.Name = "showHideToolStripMenuItem1";
            this.showHideToolStripMenuItem1.ShortcutKeyDisplayString = "";
            this.showHideToolStripMenuItem1.ShortcutKeys = System.Windows.Forms.Keys.F9;
            this.showHideToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.showHideToolStripMenuItem1.Text = "Show/Hide";
            this.showHideToolStripMenuItem1.Click += new System.EventHandler(this.ShowHideToolStripMenuItem1_Click);
            // 
            // toolStripMenuItem5
            // 
            this.toolStripMenuItem5.Name = "toolStripMenuItem5";
            this.toolStripMenuItem5.Size = new System.Drawing.Size(149, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Image = global::uRADMonitorX.Properties.Resources.door_out;
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.CloseApplication);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.enablePollingToolStripMenuItem,
            this.toolStripMenuItem2,
            this.viewDeviceWebpageToolStripMenuItem,
            this.viewDeviceOnlineDataToolStripMenuItem,
            this.toolStripMenuItem3,
            this.configurationToolStripMenuItem,
            this.toolStripMenuItem1,
            this.optionsToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // enablePollingToolStripMenuItem
            // 
            this.enablePollingToolStripMenuItem.Name = "enablePollingToolStripMenuItem";
            this.enablePollingToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.enablePollingToolStripMenuItem.Text = "Enable polling";
            this.enablePollingToolStripMenuItem.Click += new System.EventHandler(this.EnablePollingToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(195, 6);
            // 
            // viewDeviceWebpageToolStripMenuItem
            // 
            this.viewDeviceWebpageToolStripMenuItem.Image = global::uRADMonitorX.Properties.Resources.page_white_go;
            this.viewDeviceWebpageToolStripMenuItem.Name = "viewDeviceWebpageToolStripMenuItem";
            this.viewDeviceWebpageToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.viewDeviceWebpageToolStripMenuItem.Text = "View device &webpage";
            this.viewDeviceWebpageToolStripMenuItem.Click += new System.EventHandler(this.ViewDeviceWebpageToolStripMenuItem_Click);
            // 
            // viewDeviceOnlineDataToolStripMenuItem
            // 
            this.viewDeviceOnlineDataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dashboardToolStripMenuItem,
            this.toolStripMenuItem7,
            this.temperatureToolStripMenuItem,
            this.pressureToolStripMenuItem,
            this.cpmToolStripMenuItem});
            this.viewDeviceOnlineDataToolStripMenuItem.Image = global::uRADMonitorX.Properties.Resources.chart_curve_go;
            this.viewDeviceOnlineDataToolStripMenuItem.Name = "viewDeviceOnlineDataToolStripMenuItem";
            this.viewDeviceOnlineDataToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.viewDeviceOnlineDataToolStripMenuItem.Text = "View device online data";
            // 
            // dashboardToolStripMenuItem
            // 
            this.dashboardToolStripMenuItem.Name = "dashboardToolStripMenuItem";
            this.dashboardToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.dashboardToolStripMenuItem.Text = "Dashboard";
            this.dashboardToolStripMenuItem.Click += new System.EventHandler(this.ViewDeviceDashboardToolStripMenuItem_Click);
            // 
            // toolStripMenuItem7
            // 
            this.toolStripMenuItem7.Name = "toolStripMenuItem7";
            this.toolStripMenuItem7.Size = new System.Drawing.Size(177, 6);
            // 
            // temperatureToolStripMenuItem
            // 
            this.temperatureToolStripMenuItem.Name = "temperatureToolStripMenuItem";
            this.temperatureToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.temperatureToolStripMenuItem.Name = "Temperature";
            this.temperatureToolStripMenuItem.Text = "Temperature";
            this.temperatureToolStripMenuItem.Click += new System.EventHandler(this.ViewDeviceGraphDataToolStripMenuItem_Click);
            // 
            // pressureToolStripMenuItem
            // 
            this.pressureToolStripMenuItem.Name = "pressureToolStripMenuItem";
            this.pressureToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.pressureToolStripMenuItem.Name = "Pressure";
            this.pressureToolStripMenuItem.Text = "Pressure";
            this.pressureToolStripMenuItem.Click += new System.EventHandler(this.ViewDeviceGraphDataToolStripMenuItem_Click);
            // 
            // cpmToolStripMenuItem
            // 
            this.cpmToolStripMenuItem.Name = "cpmToolStripMenuItem";
            this.cpmToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.cpmToolStripMenuItem.Name = "Cpm";
            this.cpmToolStripMenuItem.Text = "Radiation";
            this.cpmToolStripMenuItem.Click += new System.EventHandler(this.ViewDeviceGraphDataToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(195, 6);
            // 
            // configurationToolStripMenuItem
            // 
            this.configurationToolStripMenuItem.Image = global::uRADMonitorX.Properties.Resources.cog;
            this.configurationToolStripMenuItem.Name = "configurationToolStripMenuItem";
            this.configurationToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.configurationToolStripMenuItem.Text = "Device &configuration...";
            this.configurationToolStripMenuItem.Click += new System.EventHandler(this.ConfigurationToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(195, 6);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Image = global::uRADMonitorX.Properties.Resources.wrench;
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.optionsToolStripMenuItem.Text = "&Options...";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.OptionsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uRADMonitorWebsiteToolStripMenuItem,
            this.uRADMonitorForumToolStripMenuItem,
            this.toolStripMenuItem8,
            this.checkForUpdatesToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // uRADMonitorWebsiteToolStripMenuItem
            // 
            this.uRADMonitorWebsiteToolStripMenuItem.Image = global::uRADMonitorX.Properties.Resources.world_go;
            this.uRADMonitorWebsiteToolStripMenuItem.Name = "uRADMonitorWebsiteToolStripMenuItem";
            this.uRADMonitorWebsiteToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.uRADMonitorWebsiteToolStripMenuItem.Text = "uRADMonitor Website";
            this.uRADMonitorWebsiteToolStripMenuItem.Click += new System.EventHandler(this.uRADMonitorWebsiteToolStripMenuItem_Click);
            // 
            // uRADMonitorForumToolStripMenuItem
            // 
            this.uRADMonitorForumToolStripMenuItem.Image = global::uRADMonitorX.Properties.Resources.group_go;
            this.uRADMonitorForumToolStripMenuItem.Name = "uRADMonitorForumToolStripMenuItem";
            this.uRADMonitorForumToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.uRADMonitorForumToolStripMenuItem.Text = "uRADMonitor Forum";
            this.uRADMonitorForumToolStripMenuItem.Click += new System.EventHandler(this.uRADMonitorForumToolStripMenuItem_Click);
            // 
            // toolStripMenuItem8
            // 
            this.toolStripMenuItem8.Name = "toolStripMenuItem8";
            this.toolStripMenuItem8.Size = new System.Drawing.Size(189, 6);
            // 
            // checkForUpdatesToolStripMenuItem
            // 
            this.checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
            this.checkForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.checkForUpdatesToolStripMenuItem.Text = "Check for Updates";
            this.checkForUpdatesToolStripMenuItem.Click += new System.EventHandler(this.CheckForUpdatesToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Image = global::uRADMonitorX.Properties.Resources.information;
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(192, 22);
            this.aboutToolStripMenuItem.Text = "&About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.AboutToolStripMenuItem_Click);
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelDeviceStatus,
            this.toolStripStatusLabelUptime,
            this.toolStripStatusLabelDeviceUptime});
            this.statusStrip.Location = new System.Drawing.Point(0, 240);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.statusStrip.Size = new System.Drawing.Size(354, 22);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // toolStripStatusLabelDeviceStatus
            // 
            this.toolStripStatusLabelDeviceStatus.Name = "toolStripStatusLabelDeviceStatus";
            this.toolStripStatusLabelDeviceStatus.Size = new System.Drawing.Size(237, 17);
            this.toolStripStatusLabelDeviceStatus.Spring = true;
            this.toolStripStatusLabelDeviceStatus.Text = "{status}";
            this.toolStripStatusLabelDeviceStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // toolStripStatusLabelUptime
            // 
            this.toolStripStatusLabelUptime.Name = "toolStripStatusLabelUptime";
            this.toolStripStatusLabelUptime.Size = new System.Drawing.Size(49, 17);
            this.toolStripStatusLabelUptime.Text = "Uptime:";
            // 
            // toolStripStatusLabelDeviceUptime
            // 
            this.toolStripStatusLabelDeviceUptime.Name = "toolStripStatusLabelDeviceUptime";
            this.toolStripStatusLabelDeviceUptime.Size = new System.Drawing.Size(53, 17);
            this.toolStripStatusLabelDeviceUptime.Text = "{uptime}";
            this.toolStripStatusLabelDeviceUptime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.viewOnlyTextBoxId);
            this.groupBox1.Controls.Add(this.labelId);
            this.groupBox1.Controls.Add(this.viewOnlyTextBoxDetector);
            this.groupBox1.Controls.Add(this.labelDetector);
            this.groupBox1.Controls.Add(this.viewOnlyTextBoxFirmware);
            this.groupBox1.Controls.Add(this.viewOnlyTextBoxHardware);
            this.groupBox1.Controls.Add(this.labelModel);
            this.groupBox1.Controls.Add(this.labelFirmware);
            this.groupBox1.Controls.Add(this.labelHardware);
            this.groupBox1.Controls.Add(this.viewOnlyTextBoxModel);
            this.groupBox1.Location = new System.Drawing.Point(12, 27);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(332, 100);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Device Information";
            // 
            // labelId
            // 
            this.labelId.AutoSize = true;
            this.labelId.Location = new System.Drawing.Point(65, 20);
            this.labelId.Name = "labelId";
            this.labelId.Size = new System.Drawing.Size(21, 13);
            this.labelId.TabIndex = 1;
            this.labelId.Text = "ID:";
            // 
            // labelDetector
            // 
            this.labelDetector.AutoSize = true;
            this.labelDetector.Location = new System.Drawing.Point(181, 46);
            this.labelDetector.Name = "labelDetector";
            this.labelDetector.Size = new System.Drawing.Size(51, 13);
            this.labelDetector.TabIndex = 5;
            this.labelDetector.Text = "Detector:";
            // 
            // labelModel
            // 
            this.labelModel.AutoSize = true;
            this.labelModel.Location = new System.Drawing.Point(47, 46);
            this.labelModel.Name = "labelModel";
            this.labelModel.Size = new System.Drawing.Size(39, 13);
            this.labelModel.TabIndex = 3;
            this.labelModel.Text = "Model:";
            // 
            // labelFirmware
            // 
            this.labelFirmware.AutoSize = true;
            this.labelFirmware.Location = new System.Drawing.Point(180, 72);
            this.labelFirmware.Name = "labelFirmware";
            this.labelFirmware.Size = new System.Drawing.Size(52, 13);
            this.labelFirmware.TabIndex = 9;
            this.labelFirmware.Text = "Firmware:";
            // 
            // labelHardware
            // 
            this.labelHardware.AutoSize = true;
            this.labelHardware.Location = new System.Drawing.Point(30, 72);
            this.labelHardware.Name = "labelHardware";
            this.labelHardware.Size = new System.Drawing.Size(56, 13);
            this.labelHardware.TabIndex = 7;
            this.labelHardware.Text = "Hardware:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.viewOnlyTextBoxWDT);
            this.groupBox2.Controls.Add(this.labelWDT);
            this.groupBox2.Controls.Add(this.viewOnlyTextBoxVoltage);
            this.groupBox2.Controls.Add(this.labelVoltage);
            this.groupBox2.Controls.Add(this.viewOnlyTextBoxPressure);
            this.groupBox2.Controls.Add(this.labelPressure);
            this.groupBox2.Controls.Add(this.labelTemperature);
            this.groupBox2.Controls.Add(this.viewOnlyTextBoxTemperature);
            this.groupBox2.Controls.Add(this.viewOnlyTextBoxRadiationAverage);
            this.groupBox2.Controls.Add(this.labelRadiationAverage);
            this.groupBox2.Controls.Add(this.labelRadiation);
            this.groupBox2.Controls.Add(this.viewOnlyTextBoxRadiation);
            this.groupBox2.Location = new System.Drawing.Point(12, 133);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(332, 100);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Device Readings";
            // 
            // labelWDT
            // 
            this.labelWDT.AutoSize = true;
            this.labelWDT.Location = new System.Drawing.Point(196, 73);
            this.labelWDT.Name = "labelWDT";
            this.labelWDT.Size = new System.Drawing.Size(36, 13);
            this.labelWDT.TabIndex = 21;
            this.labelWDT.Text = "WDT:";
            // 
            // labelVoltage
            // 
            this.labelVoltage.AutoSize = true;
            this.labelVoltage.Location = new System.Drawing.Point(40, 73);
            this.labelVoltage.Name = "labelVoltage";
            this.labelVoltage.Size = new System.Drawing.Size(46, 13);
            this.labelVoltage.TabIndex = 19;
            this.labelVoltage.Text = "Voltage:";
            // 
            // labelPressure
            // 
            this.labelPressure.AutoSize = true;
            this.labelPressure.Location = new System.Drawing.Point(181, 47);
            this.labelPressure.Name = "labelPressure";
            this.labelPressure.Size = new System.Drawing.Size(51, 13);
            this.labelPressure.TabIndex = 17;
            this.labelPressure.Text = "Pressure:";
            // 
            // labelTemperature
            // 
            this.labelTemperature.AutoSize = true;
            this.labelTemperature.Location = new System.Drawing.Point(16, 47);
            this.labelTemperature.Name = "labelTemperature";
            this.labelTemperature.Size = new System.Drawing.Size(70, 13);
            this.labelTemperature.TabIndex = 15;
            this.labelTemperature.Text = "Temperature:";
            // 
            // labelRadiationAverage
            // 
            this.labelRadiationAverage.AutoSize = true;
            this.labelRadiationAverage.Location = new System.Drawing.Point(182, 21);
            this.labelRadiationAverage.Name = "labelRadiationAverage";
            this.labelRadiationAverage.Size = new System.Drawing.Size(50, 13);
            this.labelRadiationAverage.TabIndex = 13;
            this.labelRadiationAverage.Text = "Average:";
            // 
            // labelRadiation
            // 
            this.labelRadiation.AutoSize = true;
            this.labelRadiation.Location = new System.Drawing.Point(31, 21);
            this.labelRadiation.Name = "labelRadiation";
            this.labelRadiation.Size = new System.Drawing.Size(55, 13);
            this.labelRadiation.TabIndex = 11;
            this.labelRadiation.Text = "Radiation:";
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewDeviceWebpageToolStripMenuItem1,
            this.toolStripMenuItem6,
            this.showHideToolStripMenuItem,
            this.toolStripMenuItem4,
            this.exitToolStripMenuItem1});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(188, 82);
            this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.ContextMenuStrip_Opening);
            // 
            // viewDeviceWebpageToolStripMenuItem1
            // 
            this.viewDeviceWebpageToolStripMenuItem1.Image = global::uRADMonitorX.Properties.Resources.page_white_go;
            this.viewDeviceWebpageToolStripMenuItem1.Name = "viewDeviceWebpageToolStripMenuItem1";
            this.viewDeviceWebpageToolStripMenuItem1.Size = new System.Drawing.Size(187, 22);
            this.viewDeviceWebpageToolStripMenuItem1.Text = "View device webpage";
            this.viewDeviceWebpageToolStripMenuItem1.Click += new System.EventHandler(this.ViewDeviceWebpageToolStripMenuItem1_Click);
            // 
            // toolStripMenuItem6
            // 
            this.toolStripMenuItem6.Name = "toolStripMenuItem6";
            this.toolStripMenuItem6.Size = new System.Drawing.Size(184, 6);
            // 
            // showHideToolStripMenuItem
            // 
            this.showHideToolStripMenuItem.Image = global::uRADMonitorX.Properties.Resources.Radiation;
            this.showHideToolStripMenuItem.Name = "showHideToolStripMenuItem";
            this.showHideToolStripMenuItem.ShortcutKeyDisplayString = "";
            this.showHideToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F9;
            this.showHideToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.showHideToolStripMenuItem.Text = "Show/Hide";
            this.showHideToolStripMenuItem.Click += new System.EventHandler(this.ShowHideToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(184, 6);
            // 
            // exitToolStripMenuItem1
            // 
            this.exitToolStripMenuItem1.Image = global::uRADMonitorX.Properties.Resources.door_out;
            this.exitToolStripMenuItem1.Name = "exitToolStripMenuItem1";
            this.exitToolStripMenuItem1.Size = new System.Drawing.Size(187, 22);
            this.exitToolStripMenuItem1.Text = "Exit";
            this.exitToolStripMenuItem1.Click += new System.EventHandler(this.CloseApplication);
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
            this.notifyIcon.Text = "uRADMonitorX";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.NotifyIcon_MouseClick);
            // 
            // viewOnlyTextBoxWDT
            // 
            this.viewOnlyTextBoxWDT.Location = new System.Drawing.Point(237, 70);
            this.viewOnlyTextBoxWDT.Name = "viewOnlyTextBoxWDT";
            this.viewOnlyTextBoxWDT.ReadOnly = true;
            this.viewOnlyTextBoxWDT.ShortcutsEnabled = false;
            this.viewOnlyTextBoxWDT.Size = new System.Drawing.Size(84, 20);
            this.viewOnlyTextBoxWDT.TabIndex = 22;
            // 
            // viewOnlyTextBoxVoltage
            // 
            this.viewOnlyTextBoxVoltage.Location = new System.Drawing.Point(91, 70);
            this.viewOnlyTextBoxVoltage.Name = "viewOnlyTextBoxVoltage";
            this.viewOnlyTextBoxVoltage.ReadOnly = true;
            this.viewOnlyTextBoxVoltage.ShortcutsEnabled = false;
            this.viewOnlyTextBoxVoltage.Size = new System.Drawing.Size(84, 20);
            this.viewOnlyTextBoxVoltage.TabIndex = 20;
            // 
            // viewOnlyTextBoxPressure
            // 
            this.viewOnlyTextBoxPressure.Location = new System.Drawing.Point(237, 44);
            this.viewOnlyTextBoxPressure.Name = "viewOnlyTextBoxPressure";
            this.viewOnlyTextBoxPressure.ReadOnly = true;
            this.viewOnlyTextBoxPressure.ShortcutsEnabled = false;
            this.viewOnlyTextBoxPressure.Size = new System.Drawing.Size(84, 20);
            this.viewOnlyTextBoxPressure.TabIndex = 18;
            // 
            // viewOnlyTextBoxTemperature
            // 
            this.viewOnlyTextBoxTemperature.Location = new System.Drawing.Point(91, 44);
            this.viewOnlyTextBoxTemperature.Name = "viewOnlyTextBoxTemperature";
            this.viewOnlyTextBoxTemperature.ReadOnly = true;
            this.viewOnlyTextBoxTemperature.ShortcutsEnabled = false;
            this.viewOnlyTextBoxTemperature.Size = new System.Drawing.Size(84, 20);
            this.viewOnlyTextBoxTemperature.TabIndex = 16;
            // 
            // viewOnlyTextBoxRadiationAverage
            // 
            this.viewOnlyTextBoxRadiationAverage.Location = new System.Drawing.Point(237, 18);
            this.viewOnlyTextBoxRadiationAverage.Name = "viewOnlyTextBoxRadiationAverage";
            this.viewOnlyTextBoxRadiationAverage.ReadOnly = true;
            this.viewOnlyTextBoxRadiationAverage.ShortcutsEnabled = false;
            this.viewOnlyTextBoxRadiationAverage.Size = new System.Drawing.Size(85, 20);
            this.viewOnlyTextBoxRadiationAverage.TabIndex = 14;
            // 
            // viewOnlyTextBoxRadiation
            // 
            this.viewOnlyTextBoxRadiation.Location = new System.Drawing.Point(91, 18);
            this.viewOnlyTextBoxRadiation.Name = "viewOnlyTextBoxRadiation";
            this.viewOnlyTextBoxRadiation.ReadOnly = true;
            this.viewOnlyTextBoxRadiation.ShortcutsEnabled = false;
            this.viewOnlyTextBoxRadiation.Size = new System.Drawing.Size(84, 20);
            this.viewOnlyTextBoxRadiation.TabIndex = 12;
            // 
            // viewOnlyTextBoxId
            // 
            this.viewOnlyTextBoxId.Location = new System.Drawing.Point(91, 17);
            this.viewOnlyTextBoxId.Name = "viewOnlyTextBoxId";
            this.viewOnlyTextBoxId.ReadOnly = true;
            this.viewOnlyTextBoxId.ShortcutsEnabled = false;
            this.viewOnlyTextBoxId.Size = new System.Drawing.Size(84, 20);
            this.viewOnlyTextBoxId.TabIndex = 2;
            // 
            // viewOnlyTextBoxDetector
            // 
            this.viewOnlyTextBoxDetector.Location = new System.Drawing.Point(237, 43);
            this.viewOnlyTextBoxDetector.Name = "viewOnlyTextBoxDetector";
            this.viewOnlyTextBoxDetector.ReadOnly = true;
            this.viewOnlyTextBoxDetector.ShortcutsEnabled = false;
            this.viewOnlyTextBoxDetector.Size = new System.Drawing.Size(84, 20);
            this.viewOnlyTextBoxDetector.TabIndex = 6;
            // 
            // viewOnlyTextBoxFirmware
            // 
            this.viewOnlyTextBoxFirmware.Location = new System.Drawing.Point(237, 69);
            this.viewOnlyTextBoxFirmware.Name = "viewOnlyTextBoxFirmware";
            this.viewOnlyTextBoxFirmware.ReadOnly = true;
            this.viewOnlyTextBoxFirmware.ShortcutsEnabled = false;
            this.viewOnlyTextBoxFirmware.Size = new System.Drawing.Size(84, 20);
            this.viewOnlyTextBoxFirmware.TabIndex = 10;
            // 
            // viewOnlyTextBoxHardware
            // 
            this.viewOnlyTextBoxHardware.Location = new System.Drawing.Point(91, 69);
            this.viewOnlyTextBoxHardware.Name = "viewOnlyTextBoxHardware";
            this.viewOnlyTextBoxHardware.ReadOnly = true;
            this.viewOnlyTextBoxHardware.ShortcutsEnabled = false;
            this.viewOnlyTextBoxHardware.Size = new System.Drawing.Size(84, 20);
            this.viewOnlyTextBoxHardware.TabIndex = 8;
            // 
            // viewOnlyTextBoxModel
            // 
            this.viewOnlyTextBoxModel.Location = new System.Drawing.Point(91, 43);
            this.viewOnlyTextBoxModel.Name = "viewOnlyTextBoxModel";
            this.viewOnlyTextBoxModel.ReadOnly = true;
            this.viewOnlyTextBoxModel.ShortcutsEnabled = false;
            this.viewOnlyTextBoxModel.Size = new System.Drawing.Size(84, 20);
            this.viewOnlyTextBoxModel.TabIndex = 4;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(354, 262);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.MaximizeBox = false;
            this.Name = "FormMain";
            this.Text = "uRADMonitorX {version}";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configurationToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private ViewOnlyTextBox viewOnlyTextBoxRadiation;
        private ViewOnlyTextBox viewOnlyTextBoxRadiationAverage;
        private Label labelRadiationAverage;
        private Label labelRadiation;
        private Label labelTemperature;
        private ViewOnlyTextBox viewOnlyTextBoxTemperature;
        private ViewOnlyTextBox viewOnlyTextBoxModel;
        private ToolStripStatusLabel toolStripStatusLabelDeviceStatus;
        private ToolStripStatusLabel toolStripStatusLabelUptime;
        private ToolStripStatusLabel toolStripStatusLabelDeviceUptime;
        private ViewOnlyTextBox viewOnlyTextBoxDetector;
        private Label labelDetector;
        private ViewOnlyTextBox viewOnlyTextBoxFirmware;
        private ViewOnlyTextBox viewOnlyTextBoxHardware;
        private Label labelModel;
        private Label labelFirmware;
        private Label labelHardware;
        private ViewOnlyTextBox viewOnlyTextBoxVoltage;
        private Label labelVoltage;
        private ViewOnlyTextBox viewOnlyTextBoxPressure;
        private Label labelPressure;
        private ViewOnlyTextBox viewOnlyTextBoxId;
        private Label labelId;
        private ViewOnlyTextBox viewOnlyTextBoxWDT;
        private Label labelWDT;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem enablePollingToolStripMenuItem;
        private ToolStripMenuItem viewDeviceWebpageToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem3;
        private ContextMenuStrip contextMenuStrip;
        private ToolStripMenuItem showHideToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem4;
        private ToolStripMenuItem exitToolStripMenuItem1;
        private NotifyIcon notifyIcon;
        private ToolStripMenuItem showHideToolStripMenuItem1;
        private ToolStripSeparator toolStripMenuItem5;
        private ToolStripMenuItem viewDeviceWebpageToolStripMenuItem1;
        private ToolStripSeparator toolStripMenuItem6;
        private ToolStripMenuItem uRADMonitorWebsiteToolStripMenuItem;
        private ToolStripMenuItem uRADMonitorForumToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem8;
        private ToolStripMenuItem viewDeviceOnlineDataToolStripMenuItem;
        private ToolStripMenuItem temperatureToolStripMenuItem;
        private ToolStripMenuItem pressureToolStripMenuItem;
        private ToolStripMenuItem cpmToolStripMenuItem;
        private ToolStripMenuItem dashboardToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem7;
        private ToolStripMenuItem checkForUpdatesToolStripMenuItem;
    }
}

