namespace uRADMonitorX {
    partial class FormUpdate {
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
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonUpdate = new System.Windows.Forms.Button();
            this.buttonGoToDownloadPage = new System.Windows.Forms.Button();
            this.labelStatus = new System.Windows.Forms.Label();
            this.pictureBoxStatus = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.Location = new System.Drawing.Point(257, 76);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 0;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonUpdate
            // 
            this.buttonUpdate.Location = new System.Drawing.Point(33, 76);
            this.buttonUpdate.Name = "buttonUpdate";
            this.buttonUpdate.Size = new System.Drawing.Size(75, 23);
            this.buttonUpdate.TabIndex = 1;
            this.buttonUpdate.Text = "Update";
            this.buttonUpdate.UseVisualStyleBackColor = true;
            this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
            // 
            // buttonGoToDownloadPage
            // 
            this.buttonGoToDownloadPage.Location = new System.Drawing.Point(114, 76);
            this.buttonGoToDownloadPage.Name = "buttonGoToDownloadPage";
            this.buttonGoToDownloadPage.Size = new System.Drawing.Size(120, 23);
            this.buttonGoToDownloadPage.TabIndex = 2;
            this.buttonGoToDownloadPage.Text = "Go to download page";
            this.buttonGoToDownloadPage.UseVisualStyleBackColor = true;
            this.buttonGoToDownloadPage.Click += new System.EventHandler(this.buttonGoToDownloadPage_Click);
            // 
            // labelStatus
            // 
            this.labelStatus.Location = new System.Drawing.Point(30, 21);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(302, 52);
            this.labelStatus.TabIndex = 3;
            // 
            // pictureBoxStatus
            // 
            this.pictureBoxStatus.ErrorImage = null;
            this.pictureBoxStatus.Image = global::uRADMonitorX.Properties.Resources.information;
            this.pictureBoxStatus.Location = new System.Drawing.Point(13, 20);
            this.pictureBoxStatus.Name = "pictureBoxStatus";
            this.pictureBoxStatus.Size = new System.Drawing.Size(16, 16);
            this.pictureBoxStatus.TabIndex = 7;
            this.pictureBoxStatus.TabStop = false;
            // 
            // FormUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonClose;
            this.ClientSize = new System.Drawing.Size(344, 111);
            this.Controls.Add(this.pictureBoxStatus);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.buttonGoToDownloadPage);
            this.Controls.Add(this.buttonUpdate);
            this.Controls.Add(this.buttonClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormUpdate";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Check for update";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxStatus)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonUpdate;
        private System.Windows.Forms.Button buttonGoToDownloadPage;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.PictureBox pictureBoxStatus;
    }
}