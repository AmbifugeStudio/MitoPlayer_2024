using MitoPlayer_2024.Helpers;

namespace MitoPlayer_2024.Views
{
    partial class ModelTrainerView
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
            this.cmbPlaylists = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbTags = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbTemplates = new System.Windows.Forms.ComboBox();
            this.dgvInputTrackList = new System.Windows.Forms.DataGridView();
            this.gpbFeatures = new System.Windows.Forms.GroupBox();
            this.btnCancelGeneration = new System.Windows.Forms.Button();
            this.lblEstimatedSize = new System.Windows.Forms.Label();
            this.lblGeneratingIsInProgress = new System.Windows.Forms.Label();
            this.lblEstimatedSizeLabel = new System.Windows.Forms.Label();
            this.lblRemainingTime = new System.Windows.Forms.Label();
            this.nmpBatchedProcess = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblProcessedTracks = new System.Windows.Forms.Label();
            this.chbPitch = new System.Windows.Forms.CheckBox();
            this.chbRms = new System.Windows.Forms.CheckBox();
            this.chbZcr = new System.Windows.Forms.CheckBox();
            this.chbTonnetz = new System.Windows.Forms.CheckBox();
            this.chbSpectralBandwidth = new System.Windows.Forms.CheckBox();
            this.chbSpectralCentroid = new System.Windows.Forms.CheckBox();
            this.chbHps = new System.Windows.Forms.CheckBox();
            this.chbHpcp = new System.Windows.Forms.CheckBox();
            this.chbSpectralContrast = new System.Windows.Forms.CheckBox();
            this.chbMfccs = new System.Windows.Forms.CheckBox();
            this.chbChroma = new System.Windows.Forms.CheckBox();
            this.lblLog = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.dgvTrainingDataList = new System.Windows.Forms.DataGridView();
            this.btnDeleteTrainingData = new System.Windows.Forms.Button();
            this.grbResult = new System.Windows.Forms.GroupBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.chbIsTracklistDetailsDisplayed = new System.Windows.Forms.CheckBox();
            this.btnResult = new System.Windows.Forms.Button();
            this.prbProcessedTracks = new MitoPlayer_2024.Helpers.CustomProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInputTrackList)).BeginInit();
            this.gpbFeatures.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmpBatchedProcess)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrainingDataList)).BeginInit();
            this.grbResult.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbPlaylists
            // 
            this.cmbPlaylists.FormattingEnabled = true;
            this.cmbPlaylists.Location = new System.Drawing.Point(68, 6);
            this.cmbPlaylists.Name = "cmbPlaylists";
            this.cmbPlaylists.Size = new System.Drawing.Size(129, 21);
            this.cmbPlaylists.TabIndex = 0;
            this.cmbPlaylists.SelectedIndexChanged += new System.EventHandler(this.cmbPlaylists_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Playlist:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Tag:";
            // 
            // cmbTags
            // 
            this.cmbTags.FormattingEnabled = true;
            this.cmbTags.Location = new System.Drawing.Point(68, 33);
            this.cmbTags.Name = "cmbTags";
            this.cmbTags.Size = new System.Drawing.Size(129, 21);
            this.cmbTags.TabIndex = 0;
            this.cmbTags.SelectedIndexChanged += new System.EventHandler(this.cmbTags_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Template:";
            // 
            // cmbTemplates
            // 
            this.cmbTemplates.FormattingEnabled = true;
            this.cmbTemplates.Location = new System.Drawing.Point(66, 19);
            this.cmbTemplates.Name = "cmbTemplates";
            this.cmbTemplates.Size = new System.Drawing.Size(119, 21);
            this.cmbTemplates.TabIndex = 0;
            this.cmbTemplates.SelectedIndexChanged += new System.EventHandler(this.cmbTemplates_SelectedIndexChanged);
            // 
            // dgvInputTrackList
            // 
            this.dgvInputTrackList.AllowUserToAddRows = false;
            this.dgvInputTrackList.AllowUserToDeleteRows = false;
            this.dgvInputTrackList.AllowUserToResizeRows = false;
            this.dgvInputTrackList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvInputTrackList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvInputTrackList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvInputTrackList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInputTrackList.Location = new System.Drawing.Point(12, 80);
            this.dgvInputTrackList.MultiSelect = false;
            this.dgvInputTrackList.Name = "dgvInputTrackList";
            this.dgvInputTrackList.ReadOnly = true;
            this.dgvInputTrackList.RowHeadersVisible = false;
            this.dgvInputTrackList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvInputTrackList.Size = new System.Drawing.Size(475, 236);
            this.dgvInputTrackList.TabIndex = 2;
            this.dgvInputTrackList.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvInputTrackList_DataBindingComplete);
            // 
            // gpbFeatures
            // 
            this.gpbFeatures.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.gpbFeatures.Controls.Add(this.btnCancelGeneration);
            this.gpbFeatures.Controls.Add(this.lblEstimatedSize);
            this.gpbFeatures.Controls.Add(this.lblGeneratingIsInProgress);
            this.gpbFeatures.Controls.Add(this.lblEstimatedSizeLabel);
            this.gpbFeatures.Controls.Add(this.lblRemainingTime);
            this.gpbFeatures.Controls.Add(this.nmpBatchedProcess);
            this.gpbFeatures.Controls.Add(this.label4);
            this.gpbFeatures.Controls.Add(this.btnGenerate);
            this.gpbFeatures.Controls.Add(this.label5);
            this.gpbFeatures.Controls.Add(this.label6);
            this.gpbFeatures.Controls.Add(this.lblProcessedTracks);
            this.gpbFeatures.Controls.Add(this.prbProcessedTracks);
            this.gpbFeatures.Controls.Add(this.chbPitch);
            this.gpbFeatures.Controls.Add(this.chbRms);
            this.gpbFeatures.Controls.Add(this.chbZcr);
            this.gpbFeatures.Controls.Add(this.chbTonnetz);
            this.gpbFeatures.Controls.Add(this.chbSpectralBandwidth);
            this.gpbFeatures.Controls.Add(this.chbSpectralCentroid);
            this.gpbFeatures.Controls.Add(this.chbHps);
            this.gpbFeatures.Controls.Add(this.chbHpcp);
            this.gpbFeatures.Controls.Add(this.chbSpectralContrast);
            this.gpbFeatures.Controls.Add(this.chbMfccs);
            this.gpbFeatures.Controls.Add(this.chbChroma);
            this.gpbFeatures.Controls.Add(this.label3);
            this.gpbFeatures.Controls.Add(this.cmbTemplates);
            this.gpbFeatures.Location = new System.Drawing.Point(12, 322);
            this.gpbFeatures.Name = "gpbFeatures";
            this.gpbFeatures.Size = new System.Drawing.Size(475, 241);
            this.gpbFeatures.TabIndex = 3;
            this.gpbFeatures.TabStop = false;
            this.gpbFeatures.Text = "Features";
            // 
            // btnCancelGeneration
            // 
            this.btnCancelGeneration.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelGeneration.Location = new System.Drawing.Point(309, 138);
            this.btnCancelGeneration.Name = "btnCancelGeneration";
            this.btnCancelGeneration.Size = new System.Drawing.Size(75, 23);
            this.btnCancelGeneration.TabIndex = 4;
            this.btnCancelGeneration.Text = "Cancel";
            this.btnCancelGeneration.UseVisualStyleBackColor = true;
            this.btnCancelGeneration.Click += new System.EventHandler(this.btnCancelGeneration_Click);
            // 
            // lblEstimatedSize
            // 
            this.lblEstimatedSize.Location = new System.Drawing.Point(84, 219);
            this.lblEstimatedSize.Name = "lblEstimatedSize";
            this.lblEstimatedSize.Size = new System.Drawing.Size(75, 13);
            this.lblEstimatedSize.TabIndex = 5;
            this.lblEstimatedSize.Text = "-";
            this.lblEstimatedSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblGeneratingIsInProgress
            // 
            this.lblGeneratingIsInProgress.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblGeneratingIsInProgress.Location = new System.Drawing.Point(306, 193);
            this.lblGeneratingIsInProgress.Name = "lblGeneratingIsInProgress";
            this.lblGeneratingIsInProgress.Size = new System.Drawing.Size(163, 45);
            this.lblGeneratingIsInProgress.TabIndex = 7;
            this.lblGeneratingIsInProgress.Text = "Generating Is In Progress...";
            this.lblGeneratingIsInProgress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblEstimatedSizeLabel
            // 
            this.lblEstimatedSizeLabel.AutoSize = true;
            this.lblEstimatedSizeLabel.Location = new System.Drawing.Point(7, 219);
            this.lblEstimatedSizeLabel.Name = "lblEstimatedSizeLabel";
            this.lblEstimatedSizeLabel.Size = new System.Drawing.Size(51, 13);
            this.lblEstimatedSizeLabel.TabIndex = 7;
            this.lblEstimatedSizeLabel.Text = "Est. Size:";
            // 
            // lblRemainingTime
            // 
            this.lblRemainingTime.Location = new System.Drawing.Point(84, 206);
            this.lblRemainingTime.Name = "lblRemainingTime";
            this.lblRemainingTime.Size = new System.Drawing.Size(75, 13);
            this.lblRemainingTime.TabIndex = 5;
            this.lblRemainingTime.Text = "-";
            this.lblRemainingTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nmpBatchedProcess
            // 
            this.nmpBatchedProcess.Location = new System.Drawing.Point(374, 19);
            this.nmpBatchedProcess.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nmpBatchedProcess.Name = "nmpBatchedProcess";
            this.nmpBatchedProcess.Size = new System.Drawing.Size(91, 20);
            this.nmpBatchedProcess.TabIndex = 8;
            this.nmpBatchedProcess.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nmpBatchedProcess.ValueChanged += new System.EventHandler(this.nmpBatchedProcess_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 206);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Est. Rem.Time:";
            // 
            // btnGenerate
            // 
            this.btnGenerate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerate.Location = new System.Drawing.Point(390, 138);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnGenerate.TabIndex = 4;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 193);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Processed:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(306, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Batch Size:";
            // 
            // lblProcessedTracks
            // 
            this.lblProcessedTracks.Location = new System.Drawing.Point(84, 193);
            this.lblProcessedTracks.Name = "lblProcessedTracks";
            this.lblProcessedTracks.Size = new System.Drawing.Size(75, 13);
            this.lblProcessedTracks.TabIndex = 5;
            this.lblProcessedTracks.Text = "0/0";
            this.lblProcessedTracks.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chbPitch
            // 
            this.chbPitch.AutoSize = true;
            this.chbPitch.Location = new System.Drawing.Point(237, 95);
            this.chbPitch.Name = "chbPitch";
            this.chbPitch.Size = new System.Drawing.Size(50, 17);
            this.chbPitch.TabIndex = 3;
            this.chbPitch.Text = "Pitch";
            this.chbPitch.UseVisualStyleBackColor = true;
            this.chbPitch.CheckedChanged += new System.EventHandler(this.chbPitch_CheckedChanged);
            // 
            // chbRms
            // 
            this.chbRms.AutoSize = true;
            this.chbRms.Location = new System.Drawing.Point(237, 72);
            this.chbRms.Name = "chbRms";
            this.chbRms.Size = new System.Drawing.Size(50, 17);
            this.chbRms.TabIndex = 3;
            this.chbRms.Text = "RMS";
            this.chbRms.UseVisualStyleBackColor = true;
            this.chbRms.CheckedChanged += new System.EventHandler(this.chbRms_CheckedChanged);
            // 
            // chbZcr
            // 
            this.chbZcr.AutoSize = true;
            this.chbZcr.Location = new System.Drawing.Point(237, 49);
            this.chbZcr.Name = "chbZcr";
            this.chbZcr.Size = new System.Drawing.Size(48, 17);
            this.chbZcr.TabIndex = 3;
            this.chbZcr.Text = "ZCR";
            this.chbZcr.UseVisualStyleBackColor = true;
            this.chbZcr.CheckedChanged += new System.EventHandler(this.chbZcr_CheckedChanged);
            // 
            // chbTonnetz
            // 
            this.chbTonnetz.AutoSize = true;
            this.chbTonnetz.Location = new System.Drawing.Point(122, 118);
            this.chbTonnetz.Name = "chbTonnetz";
            this.chbTonnetz.Size = new System.Drawing.Size(109, 17);
            this.chbTonnetz.TabIndex = 3;
            this.chbTonnetz.Text = "Tonnetz Features";
            this.chbTonnetz.UseVisualStyleBackColor = true;
            this.chbTonnetz.CheckedChanged += new System.EventHandler(this.chbTonnetz_CheckedChanged);
            // 
            // chbSpectralBandwidth
            // 
            this.chbSpectralBandwidth.AutoSize = true;
            this.chbSpectralBandwidth.Location = new System.Drawing.Point(122, 95);
            this.chbSpectralBandwidth.Name = "chbSpectralBandwidth";
            this.chbSpectralBandwidth.Size = new System.Drawing.Size(118, 17);
            this.chbSpectralBandwidth.TabIndex = 3;
            this.chbSpectralBandwidth.Text = "Spectral Bandwidth";
            this.chbSpectralBandwidth.UseVisualStyleBackColor = true;
            this.chbSpectralBandwidth.CheckedChanged += new System.EventHandler(this.chbSpectralBandwidth_CheckedChanged);
            // 
            // chbSpectralCentroid
            // 
            this.chbSpectralCentroid.AutoSize = true;
            this.chbSpectralCentroid.Location = new System.Drawing.Point(122, 72);
            this.chbSpectralCentroid.Name = "chbSpectralCentroid";
            this.chbSpectralCentroid.Size = new System.Drawing.Size(107, 17);
            this.chbSpectralCentroid.TabIndex = 3;
            this.chbSpectralCentroid.Text = "Spectral Centroid";
            this.chbSpectralCentroid.UseVisualStyleBackColor = true;
            this.chbSpectralCentroid.CheckedChanged += new System.EventHandler(this.chbSpectralCentroid_CheckedChanged);
            // 
            // chbHps
            // 
            this.chbHps.AutoSize = true;
            this.chbHps.Location = new System.Drawing.Point(10, 118);
            this.chbHps.Name = "chbHps";
            this.chbHps.Size = new System.Drawing.Size(48, 17);
            this.chbHps.TabIndex = 3;
            this.chbHps.Text = "HPS";
            this.chbHps.UseVisualStyleBackColor = true;
            this.chbHps.CheckedChanged += new System.EventHandler(this.chbHps_CheckedChanged);
            // 
            // chbHpcp
            // 
            this.chbHpcp.AutoSize = true;
            this.chbHpcp.Location = new System.Drawing.Point(10, 95);
            this.chbHpcp.Name = "chbHpcp";
            this.chbHpcp.Size = new System.Drawing.Size(55, 17);
            this.chbHpcp.TabIndex = 3;
            this.chbHpcp.Text = "HPCP";
            this.chbHpcp.UseVisualStyleBackColor = true;
            this.chbHpcp.CheckedChanged += new System.EventHandler(this.chbHpcp_CheckedChanged);
            // 
            // chbSpectralContrast
            // 
            this.chbSpectralContrast.AutoSize = true;
            this.chbSpectralContrast.Location = new System.Drawing.Point(122, 49);
            this.chbSpectralContrast.Name = "chbSpectralContrast";
            this.chbSpectralContrast.Size = new System.Drawing.Size(107, 17);
            this.chbSpectralContrast.TabIndex = 3;
            this.chbSpectralContrast.Text = "Spectral Contrast";
            this.chbSpectralContrast.UseVisualStyleBackColor = true;
            this.chbSpectralContrast.CheckedChanged += new System.EventHandler(this.chbSpectralContrast_CheckedChanged);
            // 
            // chbMfccs
            // 
            this.chbMfccs.AutoSize = true;
            this.chbMfccs.Location = new System.Drawing.Point(10, 72);
            this.chbMfccs.Name = "chbMfccs";
            this.chbMfccs.Size = new System.Drawing.Size(60, 17);
            this.chbMfccs.TabIndex = 3;
            this.chbMfccs.Text = "MFCCs";
            this.chbMfccs.UseVisualStyleBackColor = true;
            this.chbMfccs.CheckedChanged += new System.EventHandler(this.chbMfccs_CheckedChanged);
            // 
            // chbChroma
            // 
            this.chbChroma.AutoSize = true;
            this.chbChroma.Location = new System.Drawing.Point(10, 49);
            this.chbChroma.Name = "chbChroma";
            this.chbChroma.Size = new System.Drawing.Size(106, 17);
            this.chbChroma.TabIndex = 3;
            this.chbChroma.Text = "Chroma Features";
            this.chbChroma.UseVisualStyleBackColor = true;
            this.chbChroma.CheckedChanged += new System.EventHandler(this.chbChroma_CheckedChanged);
            // 
            // lblLog
            // 
            this.lblLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLog.Location = new System.Drawing.Point(6, 16);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(463, 215);
            this.lblLog.TabIndex = 5;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOk.Location = new System.Drawing.Point(896, 569);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "Close";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // dgvTrainingDataList
            // 
            this.dgvTrainingDataList.AllowUserToAddRows = false;
            this.dgvTrainingDataList.AllowUserToDeleteRows = false;
            this.dgvTrainingDataList.AllowUserToResizeRows = false;
            this.dgvTrainingDataList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTrainingDataList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTrainingDataList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvTrainingDataList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvTrainingDataList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTrainingDataList.Location = new System.Drawing.Point(496, 80);
            this.dgvTrainingDataList.MultiSelect = false;
            this.dgvTrainingDataList.Name = "dgvTrainingDataList";
            this.dgvTrainingDataList.ReadOnly = true;
            this.dgvTrainingDataList.RowHeadersVisible = false;
            this.dgvTrainingDataList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTrainingDataList.Size = new System.Drawing.Size(475, 236);
            this.dgvTrainingDataList.TabIndex = 2;
            this.dgvTrainingDataList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTrainingDataList_CellDoubleClick);
            this.dgvTrainingDataList.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvTrainingDataList_DataBindingComplete);
            // 
            // btnDeleteTrainingData
            // 
            this.btnDeleteTrainingData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeleteTrainingData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteTrainingData.Location = new System.Drawing.Point(896, 51);
            this.btnDeleteTrainingData.Name = "btnDeleteTrainingData";
            this.btnDeleteTrainingData.Size = new System.Drawing.Size(75, 23);
            this.btnDeleteTrainingData.TabIndex = 5;
            this.btnDeleteTrainingData.Text = "Delete";
            this.btnDeleteTrainingData.UseVisualStyleBackColor = true;
            this.btnDeleteTrainingData.Click += new System.EventHandler(this.btnDeleteTrainingData_Click);
            // 
            // grbResult
            // 
            this.grbResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grbResult.Controls.Add(this.lblLog);
            this.grbResult.Location = new System.Drawing.Point(496, 323);
            this.grbResult.Name = "grbResult";
            this.grbResult.Size = new System.Drawing.Size(475, 240);
            this.grbResult.TabIndex = 6;
            this.grbResult.TabStop = false;
            this.grbResult.Text = "Result";
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(493, 61);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(93, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Training Datasets:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 64);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(50, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "Tracklist:";
            // 
            // chbIsTracklistDetailsDisplayed
            // 
            this.chbIsTracklistDetailsDisplayed.AutoSize = true;
            this.chbIsTracklistDetailsDisplayed.Location = new System.Drawing.Point(326, 8);
            this.chbIsTracklistDetailsDisplayed.Name = "chbIsTracklistDetailsDisplayed";
            this.chbIsTracklistDetailsDisplayed.Size = new System.Drawing.Size(155, 17);
            this.chbIsTracklistDetailsDisplayed.TabIndex = 7;
            this.chbIsTracklistDetailsDisplayed.Text = "Display Training Set Details";
            this.chbIsTracklistDetailsDisplayed.UseVisualStyleBackColor = true;
            this.chbIsTracklistDetailsDisplayed.CheckedChanged += new System.EventHandler(this.chbIsTracklistDetailsDisplayed_CheckedChanged);
            // 
            // btnResult
            // 
            this.btnResult.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResult.Location = new System.Drawing.Point(412, 51);
            this.btnResult.Name = "btnResult";
            this.btnResult.Size = new System.Drawing.Size(75, 23);
            this.btnResult.TabIndex = 4;
            this.btnResult.Text = "Result";
            this.btnResult.UseVisualStyleBackColor = true;
            this.btnResult.Click += new System.EventHandler(this.btnResult_Click);
            // 
            // prbProcessedTracks
            // 
            this.prbProcessedTracks.Location = new System.Drawing.Point(6, 167);
            this.prbProcessedTracks.Name = "prbProcessedTracks";
            this.prbProcessedTracks.ProgressBarBackgroundColor = System.Drawing.Color.Empty;
            this.prbProcessedTracks.ProgressBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(191)))), ((int)(((byte)(128)))));
            this.prbProcessedTracks.Size = new System.Drawing.Size(463, 23);
            this.prbProcessedTracks.TabIndex = 6;
            // 
            // ModelTrainerView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(982, 603);
            this.Controls.Add(this.btnResult);
            this.Controls.Add(this.chbIsTracklistDetailsDisplayed);
            this.Controls.Add(this.grbResult);
            this.Controls.Add(this.dgvInputTrackList);
            this.Controls.Add(this.btnDeleteTrainingData);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.gpbFeatures);
            this.Controls.Add(this.cmbTags);
            this.Controls.Add(this.dgvTrainingDataList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbPlaylists);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ModelTrainerView";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ModelTrainer";
            ((System.ComponentModel.ISupportInitialize)(this.dgvInputTrackList)).EndInit();
            this.gpbFeatures.ResumeLayout(false);
            this.gpbFeatures.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nmpBatchedProcess)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrainingDataList)).EndInit();
            this.grbResult.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbPlaylists;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbTags;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbTemplates;
        private System.Windows.Forms.DataGridView dgvInputTrackList;
        private System.Windows.Forms.GroupBox gpbFeatures;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.CheckBox chbChroma;
        private System.Windows.Forms.CheckBox chbMfccs;
        private System.Windows.Forms.CheckBox chbSpectralContrast;
        private System.Windows.Forms.CheckBox chbHpcp;
        private System.Windows.Forms.CheckBox chbTonnetz;
        private System.Windows.Forms.CheckBox chbSpectralCentroid;
        private System.Windows.Forms.CheckBox chbHps;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Label lblProcessedTracks;
        private System.Windows.Forms.DataGridView dgvTrainingDataList;
        private System.Windows.Forms.CheckBox chbSpectralBandwidth;
        private System.Windows.Forms.CheckBox chbZcr;
        private System.Windows.Forms.CheckBox chbRms;
        private System.Windows.Forms.CheckBox chbPitch;
        private CustomProgressBar prbProcessedTracks;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nmpBatchedProcess;
        private System.Windows.Forms.Label lblRemainingTime;
        private System.Windows.Forms.Label lblEstimatedSizeLabel;
        private System.Windows.Forms.Label lblEstimatedSize;
        private System.Windows.Forms.Button btnCancelGeneration;
        private System.Windows.Forms.Label lblGeneratingIsInProgress;
        private System.Windows.Forms.Label lblLog;
        private System.Windows.Forms.Button btnDeleteTrainingData;
        private System.Windows.Forms.GroupBox grbResult;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chbIsTracklistDetailsDisplayed;
        private System.Windows.Forms.Button btnResult;
    }
}