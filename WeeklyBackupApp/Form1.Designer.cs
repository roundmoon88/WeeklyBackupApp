namespace WeeklyBackupApp
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblMessage = new System.Windows.Forms.Label();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnBackup = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lblTabPage2StateBar = new System.Windows.Forms.Label();
            this.btnGeneralCopy = new System.Windows.Forms.Button();
            this.lblSourcePath = new System.Windows.Forms.Label();
            this.txtSourcePath = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btnSourceBrowse = new System.Windows.Forms.Button();
            this.btnTargetPath = new System.Windows.Forms.Button();
            this.txtTargetPath = new System.Windows.Forms.TextBox();
            this.lalTargetPath = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(25, 78);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(165, 13);
            this.lblMessage.TabIndex = 0;
            this.lblMessage.Text = "Welcome to Weekly Backup App";
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(48, 186);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(100, 23);
            this.progressBar2.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 167);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(48, 226);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "label2";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(0, 4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(800, 456);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnBackup);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.lblMessage);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.progressBar2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(792, 430);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Weekly Backup";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnBackup
            // 
            this.btnBackup.Location = new System.Drawing.Point(140, 304);
            this.btnBackup.Name = "btnBackup";
            this.btnBackup.Size = new System.Drawing.Size(75, 23);
            this.btnBackup.TabIndex = 2;
            this.btnBackup.Text = "Backup";
            this.btnBackup.UseVisualStyleBackColor = true;
            this.btnBackup.Click += new System.EventHandler(this.btnBackup_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnTargetPath);
            this.tabPage2.Controls.Add(this.txtTargetPath);
            this.tabPage2.Controls.Add(this.lalTargetPath);
            this.tabPage2.Controls.Add(this.btnSourceBrowse);
            this.tabPage2.Controls.Add(this.txtSourcePath);
            this.tabPage2.Controls.Add(this.lblSourcePath);
            this.tabPage2.Controls.Add(this.lblTabPage2StateBar);
            this.tabPage2.Controls.Add(this.btnGeneralCopy);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(792, 430);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "General Copy Tool";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lblTabPage2StateBar
            // 
            this.lblTabPage2StateBar.AutoSize = true;
            this.lblTabPage2StateBar.Location = new System.Drawing.Point(22, 325);
            this.lblTabPage2StateBar.Name = "lblTabPage2StateBar";
            this.lblTabPage2StateBar.Size = new System.Drawing.Size(108, 13);
            this.lblTabPage2StateBar.TabIndex = 3;
            this.lblTabPage2StateBar.Text = "lblTabPage2StateBar";
            // 
            // btnGeneralCopy
            // 
            this.btnGeneralCopy.Location = new System.Drawing.Point(313, 207);
            this.btnGeneralCopy.Name = "btnGeneralCopy";
            this.btnGeneralCopy.Size = new System.Drawing.Size(75, 23);
            this.btnGeneralCopy.TabIndex = 2;
            this.btnGeneralCopy.Text = "Copy";
            this.btnGeneralCopy.UseVisualStyleBackColor = true;
            this.btnGeneralCopy.Click += new System.EventHandler(this.btnGeneralCopy_Click);
            // 
            // lblSourcePath
            // 
            this.lblSourcePath.AutoSize = true;
            this.lblSourcePath.Location = new System.Drawing.Point(34, 45);
            this.lblSourcePath.Name = "lblSourcePath";
            this.lblSourcePath.Size = new System.Drawing.Size(69, 13);
            this.lblSourcePath.TabIndex = 4;
            this.lblSourcePath.Text = "Source Path:";
            // 
            // txtSourcePath
            // 
            this.txtSourcePath.Location = new System.Drawing.Point(109, 45);
            this.txtSourcePath.Name = "txtSourcePath";
            this.txtSourcePath.Size = new System.Drawing.Size(256, 20);
            this.txtSourcePath.TabIndex = 5;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnSourceBrowse
            // 
            this.btnSourceBrowse.Location = new System.Drawing.Point(408, 41);
            this.btnSourceBrowse.Name = "btnSourceBrowse";
            this.btnSourceBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnSourceBrowse.TabIndex = 6;
            this.btnSourceBrowse.Text = "Browse";
            this.btnSourceBrowse.UseVisualStyleBackColor = true;
            this.btnSourceBrowse.Click += new System.EventHandler(this.btnSourceBrowse_Click);
            // 
            // btnTargetPath
            // 
            this.btnTargetPath.Location = new System.Drawing.Point(408, 74);
            this.btnTargetPath.Name = "btnTargetPath";
            this.btnTargetPath.Size = new System.Drawing.Size(75, 23);
            this.btnTargetPath.TabIndex = 9;
            this.btnTargetPath.Text = "Browse";
            this.btnTargetPath.UseVisualStyleBackColor = true;
            this.btnTargetPath.Click += new System.EventHandler(this.btnTargetPath_Click);
            // 
            // txtTargetPath
            // 
            this.txtTargetPath.Location = new System.Drawing.Point(109, 78);
            this.txtTargetPath.Name = "txtTargetPath";
            this.txtTargetPath.Size = new System.Drawing.Size(256, 20);
            this.txtTargetPath.TabIndex = 8;
            // 
            // lalTargetPath
            // 
            this.lalTargetPath.AutoSize = true;
            this.lalTargetPath.Location = new System.Drawing.Point(34, 78);
            this.lalTargetPath.Name = "lalTargetPath";
            this.lalTargetPath.Size = new System.Drawing.Size(66, 13);
            this.lalTargetPath.TabIndex = 7;
            this.lalTargetPath.Text = "Target Path:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.tabControl1);
            this.Name = "Form1";
            this.Text = "Weekly Backup App";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnGeneralCopy;
        private System.Windows.Forms.Label lblTabPage2StateBar;
        private System.Windows.Forms.Button btnBackup;
        private System.Windows.Forms.Button btnSourceBrowse;
        private System.Windows.Forms.TextBox txtSourcePath;
        private System.Windows.Forms.Label lblSourcePath;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btnTargetPath;
        private System.Windows.Forms.TextBox txtTargetPath;
        private System.Windows.Forms.Label lalTargetPath;
    }
}

