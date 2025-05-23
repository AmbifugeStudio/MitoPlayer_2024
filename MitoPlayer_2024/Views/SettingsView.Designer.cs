﻿namespace MitoPlayer_2024.Views
{
    partial class SettingsView
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
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chbShortTrackColouring = new System.Windows.Forms.CheckBox();
            this.grbVirtualDjImport = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rdbImportKeyFromMixedInKey = new System.Windows.Forms.RadioButton();
            this.chbAutomaticKeyImport = new System.Windows.Forms.CheckBox();
            this.rdbImportKeyFromVirtualDj = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chbAutomaticBpmImport = new System.Windows.Forms.CheckBox();
            this.rdbImportBpmFromVirtualDj = new System.Windows.Forms.RadioButton();
            this.grbPlayer = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.nmdPreviewPercentage = new System.Windows.Forms.NumericUpDown();
            this.chbPlayTrackAfterOpenFiles = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtbShortTrackColouringThreshold = new System.Windows.Forms.TextBox();
            this.nmdLogMessageDisplayTime = new System.Windows.Forms.NumericUpDown();
            this.chbLogMessageEnabled = new System.Windows.Forms.CheckBox();
            this.grbLog = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.grbTracklist = new System.Windows.Forms.GroupBox();
            this.grbVirtualDjImport.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.grbPlayer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmdPreviewPercentage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmdLogMessageDisplayTime)).BeginInit();
            this.grbLog.SuspendLayout();
            this.grbTracklist.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Location = new System.Drawing.Point(722, 533);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(803, 533);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chbShortTrackColouring
            // 
            this.chbShortTrackColouring.AutoSize = true;
            this.chbShortTrackColouring.Location = new System.Drawing.Point(6, 19);
            this.chbShortTrackColouring.Name = "chbShortTrackColouring";
            this.chbShortTrackColouring.Size = new System.Drawing.Size(124, 17);
            this.chbShortTrackColouring.TabIndex = 3;
            this.chbShortTrackColouring.Text = "Short track colouring";
            this.chbShortTrackColouring.UseVisualStyleBackColor = true;
            this.chbShortTrackColouring.CheckedChanged += new System.EventHandler(this.chbShortTrackColouring_CheckedChanged);
            // 
            // grbVirtualDjImport
            // 
            this.grbVirtualDjImport.Controls.Add(this.panel2);
            this.grbVirtualDjImport.Controls.Add(this.panel1);
            this.grbVirtualDjImport.Location = new System.Drawing.Point(12, 12);
            this.grbVirtualDjImport.Name = "grbVirtualDjImport";
            this.grbVirtualDjImport.Size = new System.Drawing.Size(416, 78);
            this.grbVirtualDjImport.TabIndex = 1;
            this.grbVirtualDjImport.TabStop = false;
            this.grbVirtualDjImport.Text = "Metadata Import";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rdbImportBpmFromVirtualDj);
            this.panel2.Controls.Add(this.chbAutomaticBpmImport);
            this.panel2.Location = new System.Drawing.Point(4, 45);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(406, 25);
            this.panel2.TabIndex = 7;
            // 
            // rdbImportKeyFromMixedInKey
            // 
            this.rdbImportKeyFromMixedInKey.AutoSize = true;
            this.rdbImportKeyFromMixedInKey.Location = new System.Drawing.Point(266, 5);
            this.rdbImportKeyFromMixedInKey.Name = "rdbImportKeyFromMixedInKey";
            this.rdbImportKeyFromMixedInKey.Size = new System.Drawing.Size(138, 17);
            this.rdbImportKeyFromMixedInKey.TabIndex = 1;
            this.rdbImportKeyFromMixedInKey.TabStop = true;
            this.rdbImportKeyFromMixedInKey.Text = "Import From MixedInKey";
            this.rdbImportKeyFromMixedInKey.UseVisualStyleBackColor = true;
            this.rdbImportKeyFromMixedInKey.CheckedChanged += new System.EventHandler(this.rdbImportKeyFromMixedInKey_CheckedChanged);
            // 
            // chbAutomaticKeyImport
            // 
            this.chbAutomaticKeyImport.AutoSize = true;
            this.chbAutomaticKeyImport.Location = new System.Drawing.Point(3, 6);
            this.chbAutomaticKeyImport.Name = "chbAutomaticKeyImport";
            this.chbAutomaticKeyImport.Size = new System.Drawing.Size(126, 17);
            this.chbAutomaticKeyImport.TabIndex = 0;
            this.chbAutomaticKeyImport.Text = "Automatic Key Import";
            this.chbAutomaticKeyImport.UseVisualStyleBackColor = true;
            this.chbAutomaticKeyImport.CheckedChanged += new System.EventHandler(this.chbAutomaticKeyImport_CheckedChanged);
            // 
            // rdbImportKeyFromVirtualDj
            // 
            this.rdbImportKeyFromVirtualDj.AutoSize = true;
            this.rdbImportKeyFromVirtualDj.Location = new System.Drawing.Point(135, 5);
            this.rdbImportKeyFromVirtualDj.Name = "rdbImportKeyFromVirtualDj";
            this.rdbImportKeyFromVirtualDj.Size = new System.Drawing.Size(122, 17);
            this.rdbImportKeyFromVirtualDj.TabIndex = 1;
            this.rdbImportKeyFromVirtualDj.TabStop = true;
            this.rdbImportKeyFromVirtualDj.Text = "Import From VirtualDj";
            this.rdbImportKeyFromVirtualDj.UseVisualStyleBackColor = true;
            this.rdbImportKeyFromVirtualDj.CheckedChanged += new System.EventHandler(this.rdbImportKeyFromVirtualDj_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.rdbImportKeyFromMixedInKey);
            this.panel1.Controls.Add(this.chbAutomaticKeyImport);
            this.panel1.Controls.Add(this.rdbImportKeyFromVirtualDj);
            this.panel1.Location = new System.Drawing.Point(4, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(406, 25);
            this.panel1.TabIndex = 6;
            // 
            // chbAutomaticBpmImport
            // 
            this.chbAutomaticBpmImport.AutoSize = true;
            this.chbAutomaticBpmImport.Location = new System.Drawing.Point(3, 5);
            this.chbAutomaticBpmImport.Name = "chbAutomaticBpmImport";
            this.chbAutomaticBpmImport.Size = new System.Drawing.Size(129, 17);
            this.chbAutomaticBpmImport.TabIndex = 0;
            this.chbAutomaticBpmImport.Text = "Automatic Bpm Import";
            this.chbAutomaticBpmImport.UseVisualStyleBackColor = true;
            this.chbAutomaticBpmImport.CheckedChanged += new System.EventHandler(this.chbAutomaticBpmImport_CheckedChanged);
            // 
            // rdbImportBpmFromVirtualDj
            // 
            this.rdbImportBpmFromVirtualDj.AutoSize = true;
            this.rdbImportBpmFromVirtualDj.Location = new System.Drawing.Point(135, 4);
            this.rdbImportBpmFromVirtualDj.Name = "rdbImportBpmFromVirtualDj";
            this.rdbImportBpmFromVirtualDj.Size = new System.Drawing.Size(122, 17);
            this.rdbImportBpmFromVirtualDj.TabIndex = 1;
            this.rdbImportBpmFromVirtualDj.TabStop = true;
            this.rdbImportBpmFromVirtualDj.Text = "Import From VirtualDj";
            this.rdbImportBpmFromVirtualDj.UseVisualStyleBackColor = true;
            this.rdbImportBpmFromVirtualDj.CheckedChanged += new System.EventHandler(this.rdbImportBpmFromVirtualDj_CheckedChanged);
            // 
            // grbPlayer
            // 
            this.grbPlayer.Controls.Add(this.label2);
            this.grbPlayer.Controls.Add(this.nmdPreviewPercentage);
            this.grbPlayer.Controls.Add(this.chbPlayTrackAfterOpenFiles);
            this.grbPlayer.Location = new System.Drawing.Point(12, 96);
            this.grbPlayer.Name = "grbPlayer";
            this.grbPlayer.Size = new System.Drawing.Size(416, 75);
            this.grbPlayer.TabIndex = 4;
            this.grbPlayer.TabStop = false;
            this.grbPlayer.Text = "Player";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(62, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Preview (%)";
            // 
            // nmdPreviewPercentage
            // 
            this.nmdPreviewPercentage.Location = new System.Drawing.Point(75, 42);
            this.nmdPreviewPercentage.Name = "nmdPreviewPercentage";
            this.nmdPreviewPercentage.Size = new System.Drawing.Size(76, 20);
            this.nmdPreviewPercentage.TabIndex = 1;
            this.nmdPreviewPercentage.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmdPreviewPercentage.ValueChanged += new System.EventHandler(this.nmdPreviewPercentage_ValueChanged);
            // 
            // chbPlayTrackAfterOpenFiles
            // 
            this.chbPlayTrackAfterOpenFiles.AutoSize = true;
            this.chbPlayTrackAfterOpenFiles.Location = new System.Drawing.Point(6, 19);
            this.chbPlayTrackAfterOpenFiles.Name = "chbPlayTrackAfterOpenFiles";
            this.chbPlayTrackAfterOpenFiles.Size = new System.Drawing.Size(145, 17);
            this.chbPlayTrackAfterOpenFiles.TabIndex = 0;
            this.chbPlayTrackAfterOpenFiles.Text = "Play track after open files";
            this.chbPlayTrackAfterOpenFiles.UseVisualStyleBackColor = true;
            this.chbPlayTrackAfterOpenFiles.CheckedChanged += new System.EventHandler(this.chbPlayTrackAfterOpenFiles_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(194, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Short track colouring threshold (minute):";
            // 
            // txtbShortTrackColouringThreshold
            // 
            this.txtbShortTrackColouringThreshold.Location = new System.Drawing.Point(203, 36);
            this.txtbShortTrackColouringThreshold.Name = "txtbShortTrackColouringThreshold";
            this.txtbShortTrackColouringThreshold.Size = new System.Drawing.Size(54, 20);
            this.txtbShortTrackColouringThreshold.TabIndex = 4;
            this.txtbShortTrackColouringThreshold.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtbShortTrackColouringThreshold_KeyPress);
            // 
            // nmdLogMessageDisplayTime
            // 
            this.nmdLogMessageDisplayTime.Location = new System.Drawing.Point(203, 37);
            this.nmdLogMessageDisplayTime.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nmdLogMessageDisplayTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmdLogMessageDisplayTime.Name = "nmdLogMessageDisplayTime";
            this.nmdLogMessageDisplayTime.Size = new System.Drawing.Size(54, 20);
            this.nmdLogMessageDisplayTime.TabIndex = 6;
            this.nmdLogMessageDisplayTime.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmdLogMessageDisplayTime.ValueChanged += new System.EventHandler(this.nmdLogMessageDisplayTime_ValueChanged);
            // 
            // chbLogMessageEnabled
            // 
            this.chbLogMessageEnabled.AutoSize = true;
            this.chbLogMessageEnabled.Location = new System.Drawing.Point(8, 19);
            this.chbLogMessageEnabled.Name = "chbLogMessageEnabled";
            this.chbLogMessageEnabled.Size = new System.Drawing.Size(130, 17);
            this.chbLogMessageEnabled.TabIndex = 7;
            this.chbLogMessageEnabled.Text = "Log message enabled";
            this.chbLogMessageEnabled.UseVisualStyleBackColor = true;
            this.chbLogMessageEnabled.CheckedChanged += new System.EventHandler(this.chbLogMessageEnabled_CheckedChanged);
            // 
            // grbLog
            // 
            this.grbLog.Controls.Add(this.label1);
            this.grbLog.Controls.Add(this.chbLogMessageEnabled);
            this.grbLog.Controls.Add(this.nmdLogMessageDisplayTime);
            this.grbLog.Location = new System.Drawing.Point(12, 248);
            this.grbLog.Name = "grbLog";
            this.grbLog.Size = new System.Drawing.Size(416, 68);
            this.grbLog.TabIndex = 8;
            this.grbLog.TabStop = false;
            this.grbLog.Text = "Log";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(156, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Log message display time (sec):";
            // 
            // grbTracklist
            // 
            this.grbTracklist.Controls.Add(this.chbShortTrackColouring);
            this.grbTracklist.Controls.Add(this.label3);
            this.grbTracklist.Controls.Add(this.txtbShortTrackColouringThreshold);
            this.grbTracklist.Location = new System.Drawing.Point(12, 177);
            this.grbTracklist.Name = "grbTracklist";
            this.grbTracklist.Size = new System.Drawing.Size(416, 65);
            this.grbTracklist.TabIndex = 9;
            this.grbTracklist.TabStop = false;
            this.grbTracklist.Text = "Tracklist";
            // 
            // SettingsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(890, 568);
            this.Controls.Add(this.grbTracklist);
            this.Controls.Add(this.grbLog);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.grbPlayer);
            this.Controls.Add(this.grbVirtualDjImport);
            this.Name = "SettingsView";
            this.Text = "Settings";
            this.grbVirtualDjImport.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.grbPlayer.ResumeLayout(false);
            this.grbPlayer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmdPreviewPercentage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmdLogMessageDisplayTime)).EndInit();
            this.grbLog.ResumeLayout(false);
            this.grbLog.PerformLayout();
            this.grbTracklist.ResumeLayout(false);
            this.grbTracklist.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chbShortTrackColouring;
        private System.Windows.Forms.GroupBox grbVirtualDjImport;
        private System.Windows.Forms.CheckBox chbAutomaticKeyImport;
        private System.Windows.Forms.CheckBox chbAutomaticBpmImport;
        private System.Windows.Forms.GroupBox grbPlayer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nmdPreviewPercentage;
        private System.Windows.Forms.CheckBox chbPlayTrackAfterOpenFiles;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtbShortTrackColouringThreshold;
        private System.Windows.Forms.RadioButton rdbImportKeyFromMixedInKey;
        private System.Windows.Forms.RadioButton rdbImportKeyFromVirtualDj;
        private System.Windows.Forms.RadioButton rdbImportBpmFromVirtualDj;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown nmdLogMessageDisplayTime;
        private System.Windows.Forms.CheckBox chbLogMessageEnabled;
        private System.Windows.Forms.GroupBox grbLog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox grbTracklist;
    }
}