namespace MitoPlayer_2024.Views
{
    partial class ExportToDirectoryView
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBoxPath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.dgvTrackList = new System.Windows.Forms.DataGridView();
            this.grbFileNamePrefix = new System.Windows.Forms.GroupBox();
            this.chbTrunkBpm = new System.Windows.Forms.CheckBox();
            this.chbBpmNumber = new System.Windows.Forms.CheckBox();
            this.chbKeyCode = new System.Windows.Forms.CheckBox();
            this.chbRowNumber = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.grbFileName = new System.Windows.Forms.GroupBox();
            this.numTitle = new System.Windows.Forms.NumericUpDown();
            this.numArtist = new System.Windows.Forms.NumericUpDown();
            this.chbTrunkTitle = new System.Windows.Forms.CheckBox();
            this.chbTrunkArtist = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrackList)).BeginInit();
            this.grbFileNamePrefix.SuspendLayout();
            this.grbFileName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTitle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numArtist)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(604, 372);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Location = new System.Drawing.Point(523, 372);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 5;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Path";
            // 
            // txtBoxPath
            // 
            this.txtBoxPath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBoxPath.Location = new System.Drawing.Point(47, 12);
            this.txtBoxPath.Name = "txtBoxPath";
            this.txtBoxPath.Size = new System.Drawing.Size(551, 20);
            this.txtBoxPath.TabIndex = 7;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBrowse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowse.Location = new System.Drawing.Point(604, 10);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 8;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // dgvTrackList
            // 
            this.dgvTrackList.AllowUserToAddRows = false;
            this.dgvTrackList.AllowUserToDeleteRows = false;
            this.dgvTrackList.AllowUserToResizeColumns = false;
            this.dgvTrackList.AllowUserToResizeRows = false;
            this.dgvTrackList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgvTrackList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTrackList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTrackList.Location = new System.Drawing.Point(15, 60);
            this.dgvTrackList.MultiSelect = false;
            this.dgvTrackList.Name = "dgvTrackList";
            this.dgvTrackList.ReadOnly = true;
            this.dgvTrackList.RowHeadersVisible = false;
            this.dgvTrackList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvTrackList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTrackList.Size = new System.Drawing.Size(474, 306);
            this.dgvTrackList.TabIndex = 9;
            this.dgvTrackList.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvTrackList_DataBindingComplete);
            // 
            // grbFilenamePrefix
            // 
            this.grbFileNamePrefix.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grbFileNamePrefix.Controls.Add(this.chbTrunkBpm);
            this.grbFileNamePrefix.Controls.Add(this.chbBpmNumber);
            this.grbFileNamePrefix.Controls.Add(this.chbKeyCode);
            this.grbFileNamePrefix.Controls.Add(this.chbRowNumber);
            this.grbFileNamePrefix.Location = new System.Drawing.Point(495, 60);
            this.grbFileNamePrefix.Name = "grbFilenamePrefix";
            this.grbFileNamePrefix.Size = new System.Drawing.Size(184, 90);
            this.grbFileNamePrefix.TabIndex = 10;
            this.grbFileNamePrefix.TabStop = false;
            this.grbFileNamePrefix.Text = "Filename Prefix";
            // 
            // chbTrunkBpm
            // 
            this.chbTrunkBpm.AutoSize = true;
            this.chbTrunkBpm.Location = new System.Drawing.Point(99, 65);
            this.chbTrunkBpm.Name = "chbTrunkBpm";
            this.chbTrunkBpm.Size = new System.Drawing.Size(78, 17);
            this.chbTrunkBpm.TabIndex = 0;
            this.chbTrunkBpm.Text = "Trunk Bpm";
            this.chbTrunkBpm.UseVisualStyleBackColor = true;
            this.chbTrunkBpm.CheckedChanged += new System.EventHandler(this.chbTrunkBpm_CheckedChanged);
            // 
            // chbBpmNumber
            // 
            this.chbBpmNumber.AutoSize = true;
            this.chbBpmNumber.Location = new System.Drawing.Point(6, 65);
            this.chbBpmNumber.Name = "chbBpmNumber";
            this.chbBpmNumber.Size = new System.Drawing.Size(87, 17);
            this.chbBpmNumber.TabIndex = 0;
            this.chbBpmNumber.Text = "Bpm Number";
            this.chbBpmNumber.UseVisualStyleBackColor = true;
            this.chbBpmNumber.CheckedChanged += new System.EventHandler(this.chbBpmNumber_CheckedChanged);
            // 
            // chbKeyCode
            // 
            this.chbKeyCode.AutoSize = true;
            this.chbKeyCode.Location = new System.Drawing.Point(6, 42);
            this.chbKeyCode.Name = "chbKeyCode";
            this.chbKeyCode.Size = new System.Drawing.Size(72, 17);
            this.chbKeyCode.TabIndex = 0;
            this.chbKeyCode.Text = "Key Code";
            this.chbKeyCode.UseVisualStyleBackColor = true;
            this.chbKeyCode.CheckedChanged += new System.EventHandler(this.chbKeyCode_CheckedChanged);
            // 
            // chbRowNumber
            // 
            this.chbRowNumber.AutoSize = true;
            this.chbRowNumber.Location = new System.Drawing.Point(6, 19);
            this.chbRowNumber.Name = "chbRowNumber";
            this.chbRowNumber.Size = new System.Drawing.Size(88, 17);
            this.chbRowNumber.TabIndex = 0;
            this.chbRowNumber.Text = "Row Number";
            this.chbRowNumber.UseVisualStyleBackColor = true;
            this.chbRowNumber.CheckedChanged += new System.EventHandler(this.chbRowNumber_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Tracklist";
            // 
            // grbFileName
            // 
            this.grbFileName.Controls.Add(this.numTitle);
            this.grbFileName.Controls.Add(this.numArtist);
            this.grbFileName.Controls.Add(this.chbTrunkTitle);
            this.grbFileName.Controls.Add(this.chbTrunkArtist);
            this.grbFileName.Location = new System.Drawing.Point(495, 156);
            this.grbFileName.Name = "grbFileName";
            this.grbFileName.Size = new System.Drawing.Size(184, 100);
            this.grbFileName.TabIndex = 11;
            this.grbFileName.TabStop = false;
            this.grbFileName.Text = "Filename";
            // 
            // numTitle
            // 
            this.numTitle.Location = new System.Drawing.Point(99, 40);
            this.numTitle.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numTitle.Name = "numTitle";
            this.numTitle.Size = new System.Drawing.Size(78, 20);
            this.numTitle.TabIndex = 1;
            this.numTitle.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numTitle.ValueChanged += new System.EventHandler(this.numTitle_ValueChanged);
            // 
            // numArtist
            // 
            this.numArtist.Location = new System.Drawing.Point(99, 17);
            this.numArtist.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numArtist.Name = "numArtist";
            this.numArtist.Size = new System.Drawing.Size(78, 20);
            this.numArtist.TabIndex = 1;
            this.numArtist.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numArtist.ValueChanged += new System.EventHandler(this.numArtist_ValueChanged);
            // 
            // chbTrunkTitle
            // 
            this.chbTrunkTitle.AutoSize = true;
            this.chbTrunkTitle.Location = new System.Drawing.Point(6, 43);
            this.chbTrunkTitle.Name = "chbTrunkTitle";
            this.chbTrunkTitle.Size = new System.Drawing.Size(77, 17);
            this.chbTrunkTitle.TabIndex = 0;
            this.chbTrunkTitle.Text = "Trunk Title";
            this.chbTrunkTitle.UseVisualStyleBackColor = true;
            this.chbTrunkTitle.CheckedChanged += new System.EventHandler(this.chbTrunkTitle_CheckedChanged);
            // 
            // chbTrunkArtist
            // 
            this.chbTrunkArtist.AutoSize = true;
            this.chbTrunkArtist.Location = new System.Drawing.Point(6, 20);
            this.chbTrunkArtist.Name = "chbTrunkArtist";
            this.chbTrunkArtist.Size = new System.Drawing.Size(80, 17);
            this.chbTrunkArtist.TabIndex = 0;
            this.chbTrunkArtist.Text = "Trunk Artist";
            this.chbTrunkArtist.UseVisualStyleBackColor = true;
            this.chbTrunkArtist.CheckedChanged += new System.EventHandler(this.chbTrunkArtist_CheckedChanged);
            // 
            // ExportToDirectoryView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(691, 407);
            this.Controls.Add(this.grbFileName);
            this.Controls.Add(this.grbFileNamePrefix);
            this.Controls.Add(this.dgvTrackList);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtBoxPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ExportToDirectoryView";
            this.Text = "Export To Directory";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ExportToDirectoryView_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrackList)).EndInit();
            this.grbFileNamePrefix.ResumeLayout(false);
            this.grbFileNamePrefix.PerformLayout();
            this.grbFileName.ResumeLayout(false);
            this.grbFileName.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTitle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numArtist)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBoxPath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.DataGridView dgvTrackList;
        private System.Windows.Forms.GroupBox grbFileNamePrefix;
        private System.Windows.Forms.CheckBox chbBpmNumber;
        private System.Windows.Forms.CheckBox chbKeyCode;
        private System.Windows.Forms.CheckBox chbRowNumber;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chbTrunkBpm;
        private System.Windows.Forms.GroupBox grbFileName;
        private System.Windows.Forms.NumericUpDown numArtist;
        private System.Windows.Forms.CheckBox chbTrunkArtist;
        private System.Windows.Forms.NumericUpDown numTitle;
        private System.Windows.Forms.CheckBox chbTrunkTitle;
    }
}