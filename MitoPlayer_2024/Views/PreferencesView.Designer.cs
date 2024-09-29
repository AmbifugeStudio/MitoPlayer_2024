namespace MitoPlayer_2024.Views
{
    partial class PreferencesView
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtBoxVirtualDjDatabasePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chbAutomaticKeyImport = new System.Windows.Forms.CheckBox();
            this.chbAutomaticBpmImport = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.chbPlayTrackAfterOpenFiles = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabGeneral);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(742, 359);
            this.tabControl1.TabIndex = 0;
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.groupBox3);
            this.tabGeneral.Controls.Add(this.groupBox2);
            this.tabGeneral.Controls.Add(this.groupBox1);
            this.tabGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabGeneral.Size = new System.Drawing.Size(734, 333);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "General";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtBoxVirtualDjDatabasePath);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.chbAutomaticKeyImport);
            this.groupBox2.Controls.Add(this.chbAutomaticBpmImport);
            this.groupBox2.Location = new System.Drawing.Point(6, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(437, 100);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "VirtualDJ Import";
            // 
            // txtBoxVirtualDjDatabasePath
            // 
            this.txtBoxVirtualDjDatabasePath.Enabled = false;
            this.txtBoxVirtualDjDatabasePath.Location = new System.Drawing.Point(94, 62);
            this.txtBoxVirtualDjDatabasePath.Name = "txtBoxVirtualDjDatabasePath";
            this.txtBoxVirtualDjDatabasePath.Size = new System.Drawing.Size(337, 20);
            this.txtBoxVirtualDjDatabasePath.TabIndex = 3;
            this.txtBoxVirtualDjDatabasePath.TextChanged += new System.EventHandler(this.txtBoxVirtualDjDatabasePath_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Database Path:";
            // 
            // chbAutomaticKeyImport
            // 
            this.chbAutomaticKeyImport.AutoSize = true;
            this.chbAutomaticKeyImport.Location = new System.Drawing.Point(6, 42);
            this.chbAutomaticKeyImport.Name = "chbAutomaticKeyImport";
            this.chbAutomaticKeyImport.Size = new System.Drawing.Size(126, 17);
            this.chbAutomaticKeyImport.TabIndex = 0;
            this.chbAutomaticKeyImport.Text = "Automatic Key Import";
            this.chbAutomaticKeyImport.UseVisualStyleBackColor = true;
            this.chbAutomaticKeyImport.CheckedChanged += new System.EventHandler(this.chbAutomaticKeyImport_CheckedChanged);
            // 
            // chbAutomaticBpmImport
            // 
            this.chbAutomaticBpmImport.AutoSize = true;
            this.chbAutomaticBpmImport.Location = new System.Drawing.Point(6, 19);
            this.chbAutomaticBpmImport.Name = "chbAutomaticBpmImport";
            this.chbAutomaticBpmImport.Size = new System.Drawing.Size(129, 17);
            this.chbAutomaticBpmImport.TabIndex = 0;
            this.chbAutomaticBpmImport.Text = "Automatic Bpm Import";
            this.chbAutomaticBpmImport.UseVisualStyleBackColor = true;
            this.chbAutomaticBpmImport.CheckedChanged += new System.EventHandler(this.chbAutomaticBpmImport_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnClear);
            this.groupBox1.Location = new System.Drawing.Point(528, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Database";
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(119, 71);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 0;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(598, 377);
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
            this.btnCancel.Location = new System.Drawing.Point(679, 377);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.chbPlayTrackAfterOpenFiles);
            this.groupBox3.Location = new System.Drawing.Point(6, 112);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(437, 47);
            this.groupBox3.TabIndex = 4;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Player";
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
            // PreferencesView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(766, 412);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.tabControl1);
            this.Name = "PreferencesView";
            this.Text = "PreferencesView";
            this.tabControl1.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chbAutomaticBpmImport;
        private System.Windows.Forms.CheckBox chbAutomaticKeyImport;
        private System.Windows.Forms.TextBox txtBoxVirtualDjDatabasePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chbPlayTrackAfterOpenFiles;
    }
}