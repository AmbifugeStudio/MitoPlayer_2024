using MitoPlayer_2024.Helpers;

namespace MitoPlayer_2024.Views
{
    partial class ChartView
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
            this.grbFeatures = new System.Windows.Forms.GroupBox();
            this.lblInProgress = new System.Windows.Forms.Label();
            this.prbAnalyseProgress = new MitoPlayer_2024.Helpers.CustomProgressBar();
            this.rdbPitch = new System.Windows.Forms.RadioButton();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnAnalyse = new System.Windows.Forms.Button();
            this.rdbHPSS = new System.Windows.Forms.RadioButton();
            this.rdbHPCP = new System.Windows.Forms.RadioButton();
            this.rdbRMS = new System.Windows.Forms.RadioButton();
            this.rdbMFCCs = new System.Windows.Forms.RadioButton();
            this.rdbChroma = new System.Windows.Forms.RadioButton();
            this.rdbZCR = new System.Windows.Forms.RadioButton();
            this.rdbSpectralContrast = new System.Windows.Forms.RadioButton();
            this.rdbSpectralCentroid = new System.Windows.Forms.RadioButton();
            this.rdbTonnetz = new System.Windows.Forms.RadioButton();
            this.rdbSpectralBandwidth = new System.Windows.Forms.RadioButton();
            this.btnClose = new System.Windows.Forms.Button();
            this.pnlPlot = new System.Windows.Forms.Panel();
            this.pnlFrequency = new System.Windows.Forms.Panel();
            this.dgvTracklist = new System.Windows.Forms.DataGridView();
            this.grbFeatures.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTracklist)).BeginInit();
            this.SuspendLayout();
            // 
            // grbFeatures
            // 
            this.grbFeatures.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grbFeatures.Controls.Add(this.lblInProgress);
            this.grbFeatures.Controls.Add(this.prbAnalyseProgress);
            this.grbFeatures.Controls.Add(this.rdbPitch);
            this.grbFeatures.Controls.Add(this.btnCancel);
            this.grbFeatures.Controls.Add(this.btnAnalyse);
            this.grbFeatures.Controls.Add(this.rdbHPSS);
            this.grbFeatures.Controls.Add(this.rdbHPCP);
            this.grbFeatures.Controls.Add(this.rdbRMS);
            this.grbFeatures.Controls.Add(this.rdbMFCCs);
            this.grbFeatures.Controls.Add(this.rdbChroma);
            this.grbFeatures.Controls.Add(this.rdbZCR);
            this.grbFeatures.Controls.Add(this.rdbSpectralContrast);
            this.grbFeatures.Controls.Add(this.rdbSpectralCentroid);
            this.grbFeatures.Controls.Add(this.rdbTonnetz);
            this.grbFeatures.Controls.Add(this.rdbSpectralBandwidth);
            this.grbFeatures.Location = new System.Drawing.Point(12, 63);
            this.grbFeatures.Name = "grbFeatures";
            this.grbFeatures.Size = new System.Drawing.Size(984, 72);
            this.grbFeatures.TabIndex = 9;
            this.grbFeatures.TabStop = false;
            this.grbFeatures.Text = "Features";
            // 
            // lblInProgress
            // 
            this.lblInProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInProgress.AutoSize = true;
            this.lblInProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.lblInProgress.Location = new System.Drawing.Point(714, 45);
            this.lblInProgress.Name = "lblInProgress";
            this.lblInProgress.Size = new System.Drawing.Size(183, 17);
            this.lblInProgress.TabIndex = 6;
            this.lblInProgress.Text = "Analysation Is In Progress...";
            // 
            // prbAnalyseProgress
            // 
            this.prbAnalyseProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.prbAnalyseProgress.Location = new System.Drawing.Point(6, 42);
            this.prbAnalyseProgress.Name = "prbAnalyseProgress";
            this.prbAnalyseProgress.ProgressBarBackgroundColor = System.Drawing.Color.Empty;
            this.prbAnalyseProgress.ProgressBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(191)))), ((int)(((byte)(128)))));
            this.prbAnalyseProgress.Size = new System.Drawing.Size(702, 23);
            this.prbAnalyseProgress.Step = 1;
            this.prbAnalyseProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.prbAnalyseProgress.TabIndex = 5;
            // 
            // rdbPitch
            // 
            this.rdbPitch.AutoSize = true;
            this.rdbPitch.Location = new System.Drawing.Point(784, 19);
            this.rdbPitch.Name = "rdbPitch";
            this.rdbPitch.Size = new System.Drawing.Size(49, 17);
            this.rdbPitch.TabIndex = 0;
            this.rdbPitch.TabStop = true;
            this.rdbPitch.Text = "Pitch";
            this.rdbPitch.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(903, 42);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.bnCancel_Click);
            // 
            // btnAnalyse
            // 
            this.btnAnalyse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAnalyse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAnalyse.Location = new System.Drawing.Point(903, 16);
            this.btnAnalyse.Name = "btnAnalyse";
            this.btnAnalyse.Size = new System.Drawing.Size(75, 23);
            this.btnAnalyse.TabIndex = 4;
            this.btnAnalyse.Text = "Analyse";
            this.btnAnalyse.UseVisualStyleBackColor = true;
            this.btnAnalyse.Click += new System.EventHandler(this.btnAnalyse_Click);
            // 
            // rdbHPSS
            // 
            this.rdbHPSS.AutoSize = true;
            this.rdbHPSS.Location = new System.Drawing.Point(198, 19);
            this.rdbHPSS.Name = "rdbHPSS";
            this.rdbHPSS.Size = new System.Drawing.Size(54, 17);
            this.rdbHPSS.TabIndex = 0;
            this.rdbHPSS.TabStop = true;
            this.rdbHPSS.Text = "HPSS";
            this.rdbHPSS.UseVisualStyleBackColor = true;
            // 
            // rdbHPCP
            // 
            this.rdbHPCP.AutoSize = true;
            this.rdbHPCP.Location = new System.Drawing.Point(138, 19);
            this.rdbHPCP.Name = "rdbHPCP";
            this.rdbHPCP.Size = new System.Drawing.Size(54, 17);
            this.rdbHPCP.TabIndex = 0;
            this.rdbHPCP.TabStop = true;
            this.rdbHPCP.Text = "HPCP";
            this.rdbHPCP.UseVisualStyleBackColor = true;
            // 
            // rdbRMS
            // 
            this.rdbRMS.AutoSize = true;
            this.rdbRMS.Location = new System.Drawing.Point(729, 19);
            this.rdbRMS.Name = "rdbRMS";
            this.rdbRMS.Size = new System.Drawing.Size(49, 17);
            this.rdbRMS.TabIndex = 0;
            this.rdbRMS.TabStop = true;
            this.rdbRMS.Text = "RMS";
            this.rdbRMS.UseVisualStyleBackColor = true;
            // 
            // rdbMFCCs
            // 
            this.rdbMFCCs.AutoSize = true;
            this.rdbMFCCs.Location = new System.Drawing.Point(73, 19);
            this.rdbMFCCs.Name = "rdbMFCCs";
            this.rdbMFCCs.Size = new System.Drawing.Size(59, 17);
            this.rdbMFCCs.TabIndex = 0;
            this.rdbMFCCs.TabStop = true;
            this.rdbMFCCs.Text = "MFCCs";
            this.rdbMFCCs.UseVisualStyleBackColor = true;
            // 
            // rdbChroma
            // 
            this.rdbChroma.AutoSize = true;
            this.rdbChroma.Checked = true;
            this.rdbChroma.Location = new System.Drawing.Point(6, 19);
            this.rdbChroma.Name = "rdbChroma";
            this.rdbChroma.Size = new System.Drawing.Size(61, 17);
            this.rdbChroma.TabIndex = 0;
            this.rdbChroma.TabStop = true;
            this.rdbChroma.Text = "Chroma";
            this.rdbChroma.UseVisualStyleBackColor = true;
            // 
            // rdbZCR
            // 
            this.rdbZCR.AutoSize = true;
            this.rdbZCR.Location = new System.Drawing.Point(676, 19);
            this.rdbZCR.Name = "rdbZCR";
            this.rdbZCR.Size = new System.Drawing.Size(47, 17);
            this.rdbZCR.TabIndex = 0;
            this.rdbZCR.TabStop = true;
            this.rdbZCR.Text = "ZCR";
            this.rdbZCR.UseVisualStyleBackColor = true;
            this.rdbZCR.CheckedChanged += new System.EventHandler(this.rdbZCR_CheckedChanged);
            // 
            // rdbSpectralContrast
            // 
            this.rdbSpectralContrast.AutoSize = true;
            this.rdbSpectralContrast.Location = new System.Drawing.Point(258, 19);
            this.rdbSpectralContrast.Name = "rdbSpectralContrast";
            this.rdbSpectralContrast.Size = new System.Drawing.Size(106, 17);
            this.rdbSpectralContrast.TabIndex = 0;
            this.rdbSpectralContrast.TabStop = true;
            this.rdbSpectralContrast.Text = "Spectral Contrast";
            this.rdbSpectralContrast.UseVisualStyleBackColor = true;
            // 
            // rdbSpectralCentroid
            // 
            this.rdbSpectralCentroid.AutoSize = true;
            this.rdbSpectralCentroid.Location = new System.Drawing.Point(370, 19);
            this.rdbSpectralCentroid.Name = "rdbSpectralCentroid";
            this.rdbSpectralCentroid.Size = new System.Drawing.Size(106, 17);
            this.rdbSpectralCentroid.TabIndex = 0;
            this.rdbSpectralCentroid.TabStop = true;
            this.rdbSpectralCentroid.Text = "Spectral Centroid";
            this.rdbSpectralCentroid.UseVisualStyleBackColor = true;
            // 
            // rdbTonnetz
            // 
            this.rdbTonnetz.AutoSize = true;
            this.rdbTonnetz.Location = new System.Drawing.Point(606, 19);
            this.rdbTonnetz.Name = "rdbTonnetz";
            this.rdbTonnetz.Size = new System.Drawing.Size(64, 17);
            this.rdbTonnetz.TabIndex = 0;
            this.rdbTonnetz.TabStop = true;
            this.rdbTonnetz.Text = "Tonnetz";
            this.rdbTonnetz.UseVisualStyleBackColor = true;
            // 
            // rdbSpectralBandwidth
            // 
            this.rdbSpectralBandwidth.AutoSize = true;
            this.rdbSpectralBandwidth.Location = new System.Drawing.Point(483, 19);
            this.rdbSpectralBandwidth.Name = "rdbSpectralBandwidth";
            this.rdbSpectralBandwidth.Size = new System.Drawing.Size(117, 17);
            this.rdbSpectralBandwidth.TabIndex = 0;
            this.rdbSpectralBandwidth.TabStop = true;
            this.rdbSpectralBandwidth.Text = "Spectral Bandwidth";
            this.rdbSpectralBandwidth.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Location = new System.Drawing.Point(921, 694);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pnlPlot
            // 
            this.pnlPlot.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlPlot.Location = new System.Drawing.Point(12, 141);
            this.pnlPlot.Name = "pnlPlot";
            this.pnlPlot.Size = new System.Drawing.Size(984, 340);
            this.pnlPlot.TabIndex = 10;
            // 
            // pnlFrequency
            // 
            this.pnlFrequency.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlFrequency.Location = new System.Drawing.Point(12, 487);
            this.pnlFrequency.Name = "pnlFrequency";
            this.pnlFrequency.Size = new System.Drawing.Size(984, 201);
            this.pnlFrequency.TabIndex = 10;
            // 
            // dgvTracklist
            // 
            this.dgvTracklist.AllowUserToAddRows = false;
            this.dgvTracklist.AllowUserToDeleteRows = false;
            this.dgvTracklist.AllowUserToOrderColumns = true;
            this.dgvTracklist.AllowUserToResizeRows = false;
            this.dgvTracklist.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTracklist.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvTracklist.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvTracklist.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTracklist.Enabled = false;
            this.dgvTracklist.Location = new System.Drawing.Point(12, 12);
            this.dgvTracklist.MultiSelect = false;
            this.dgvTracklist.Name = "dgvTracklist";
            this.dgvTracklist.ReadOnly = true;
            this.dgvTracklist.RowHeadersVisible = false;
            this.dgvTracklist.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvTracklist.Size = new System.Drawing.Size(984, 45);
            this.dgvTracklist.TabIndex = 0;
            this.dgvTracklist.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvTrackList_DataBindingComplete);
            // 
            // ChartView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.pnlFrequency);
            this.Controls.Add(this.grbFeatures);
            this.Controls.Add(this.dgvTracklist);
            this.Controls.Add(this.pnlPlot);
            this.Controls.Add(this.btnClose);
            this.Name = "ChartView";
            this.Text = "ChartView";
            this.TopMost = true;
            this.grbFeatures.ResumeLayout(false);
            this.grbFeatures.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTracklist)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox grbFeatures;
        private System.Windows.Forms.RadioButton rdbPitch;
        private System.Windows.Forms.Button btnAnalyse;
        private System.Windows.Forms.RadioButton rdbRMS;
        private System.Windows.Forms.RadioButton rdbZCR;
        private System.Windows.Forms.RadioButton rdbTonnetz;
        private System.Windows.Forms.RadioButton rdbSpectralBandwidth;
        private System.Windows.Forms.RadioButton rdbSpectralCentroid;
        private System.Windows.Forms.RadioButton rdbSpectralContrast;
        private System.Windows.Forms.RadioButton rdbHPSS;
        private System.Windows.Forms.RadioButton rdbHPCP;
        private System.Windows.Forms.RadioButton rdbMFCCs;
        private System.Windows.Forms.RadioButton rdbChroma;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel pnlPlot;
        private System.Windows.Forms.Panel pnlFrequency;
        private System.Windows.Forms.Label lblInProgress;
        private CustomProgressBar prbAnalyseProgress;
        private System.Windows.Forms.DataGridView dgvTracklist;
        private System.Windows.Forms.Button btnCancel;
    }
}