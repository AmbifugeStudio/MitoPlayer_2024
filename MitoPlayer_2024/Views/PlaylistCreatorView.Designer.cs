namespace MitoPlayer_2024.Views
{
    partial class PlaylistCreatorView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlaylistCreatorView));
            this.btnSave = new System.Windows.Forms.Button();
            this.btnPlaylistListPanelToggle = new System.Windows.Forms.Button();
            this.dgvPlaylistList = new System.Windows.Forms.DataGridView();
            this.lblTrackSumLength = new System.Windows.Forms.Label();
            this.lblTrackCount = new System.Windows.Forms.Label();
            this.lblSelectedItemsLength = new System.Windows.Forms.Label();
            this.lblSelectedItemsCount = new System.Windows.Forms.Label();
            this.grbCoverImageComponent = new System.Windows.Forms.GroupBox();
            this.btnDisplayCoverImageToggle = new System.Windows.Forms.Button();
            this.btnDisplayTagComponentToggle = new System.Windows.Forms.Button();
            this.pnlTagComponent = new System.Windows.Forms.Panel();
            this.lblFilter = new System.Windows.Forms.Label();
            this.btnFilter = new System.Windows.Forms.Button();
            this.txtbFilter = new System.Windows.Forms.TextBox();
            this.tagValueEditorPanel = new System.Windows.Forms.Panel();
            this.btnClearTagValueFilter = new System.Windows.Forms.Button();
            this.dgvTracklist = new MitoPlayer_2024.Helpers.CustomDataGridView();
            this.dgvSelectorTracklist = new MitoPlayer_2024.Helpers.CustomDataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlaylistList)).BeginInit();
            this.pnlTagComponent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTracklist)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectorTracklist)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSave
            // 
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(49, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(32, 23);
            this.btnSave.TabIndex = 51;
            this.btnSave.Text = "💾";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnPlaylistListPanelToggle
            // 
            this.btnPlaylistListPanelToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlaylistListPanelToggle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPlaylistListPanelToggle.Image = global::MitoPlayer_2024.Properties.Resources.Arrow_Left_20_20;
            this.btnPlaylistListPanelToggle.Location = new System.Drawing.Point(12, 12);
            this.btnPlaylistListPanelToggle.Name = "btnPlaylistListPanelToggle";
            this.btnPlaylistListPanelToggle.Size = new System.Drawing.Size(31, 23);
            this.btnPlaylistListPanelToggle.TabIndex = 50;
            this.btnPlaylistListPanelToggle.UseVisualStyleBackColor = true;
            // 
            // dgvPlaylistList
            // 
            this.dgvPlaylistList.AllowDrop = true;
            this.dgvPlaylistList.AllowUserToAddRows = false;
            this.dgvPlaylistList.AllowUserToDeleteRows = false;
            this.dgvPlaylistList.AllowUserToResizeRows = false;
            this.dgvPlaylistList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgvPlaylistList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPlaylistList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvPlaylistList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvPlaylistList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvPlaylistList.Location = new System.Drawing.Point(12, 41);
            this.dgvPlaylistList.MultiSelect = false;
            this.dgvPlaylistList.Name = "dgvPlaylistList";
            this.dgvPlaylistList.ReadOnly = true;
            this.dgvPlaylistList.RowHeadersVisible = false;
            this.dgvPlaylistList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPlaylistList.Size = new System.Drawing.Size(183, 601);
            this.dgvPlaylistList.TabIndex = 49;
            this.dgvPlaylistList.SelectionChanged += new System.EventHandler(this.dgvPlaylistList_SelectionChanged);
            // 
            // lblTrackSumLength
            // 
            this.lblTrackSumLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTrackSumLength.Location = new System.Drawing.Point(12, 659);
            this.lblTrackSumLength.Name = "lblTrackSumLength";
            this.lblTrackSumLength.Size = new System.Drawing.Size(181, 13);
            this.lblTrackSumLength.TabIndex = 47;
            this.lblTrackSumLength.Text = "Length: 00:00:00";
            this.lblTrackSumLength.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTrackCount
            // 
            this.lblTrackCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTrackCount.AutoSize = true;
            this.lblTrackCount.Location = new System.Drawing.Point(12, 645);
            this.lblTrackCount.Name = "lblTrackCount";
            this.lblTrackCount.Size = new System.Drawing.Size(116, 13);
            this.lblTrackCount.TabIndex = 48;
            this.lblTrackCount.Text = "1000 item(s) in [Playlist]";
            this.lblTrackCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSelectedItemsLength
            // 
            this.lblSelectedItemsLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSelectedItemsLength.Location = new System.Drawing.Point(199, 659);
            this.lblSelectedItemsLength.Name = "lblSelectedItemsLength";
            this.lblSelectedItemsLength.Size = new System.Drawing.Size(106, 13);
            this.lblSelectedItemsLength.TabIndex = 52;
            this.lblSelectedItemsLength.Text = "Length: 00:00:00";
            // 
            // lblSelectedItemsCount
            // 
            this.lblSelectedItemsCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSelectedItemsCount.AutoSize = true;
            this.lblSelectedItemsCount.Location = new System.Drawing.Point(198, 645);
            this.lblSelectedItemsCount.Name = "lblSelectedItemsCount";
            this.lblSelectedItemsCount.Size = new System.Drawing.Size(78, 13);
            this.lblSelectedItemsCount.TabIndex = 53;
            this.lblSelectedItemsCount.Text = "0 item selected";
            // 
            // grbCoverImageComponent
            // 
            this.grbCoverImageComponent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grbCoverImageComponent.Location = new System.Drawing.Point(201, 7);
            this.grbCoverImageComponent.Name = "grbCoverImageComponent";
            this.grbCoverImageComponent.Size = new System.Drawing.Size(773, 87);
            this.grbCoverImageComponent.TabIndex = 55;
            this.grbCoverImageComponent.TabStop = false;
            // 
            // btnDisplayCoverImageToggle
            // 
            this.btnDisplayCoverImageToggle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDisplayCoverImageToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDisplayCoverImageToggle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDisplayCoverImageToggle.Image = global::MitoPlayer_2024.Properties.Resources.Arrow_Up_20_20;
            this.btnDisplayCoverImageToggle.Location = new System.Drawing.Point(980, 12);
            this.btnDisplayCoverImageToggle.Name = "btnDisplayCoverImageToggle";
            this.btnDisplayCoverImageToggle.Size = new System.Drawing.Size(31, 23);
            this.btnDisplayCoverImageToggle.TabIndex = 56;
            this.btnDisplayCoverImageToggle.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnDisplayCoverImageToggle.UseVisualStyleBackColor = true;
            // 
            // btnDisplayTagComponentToggle
            // 
            this.btnDisplayTagComponentToggle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDisplayTagComponentToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDisplayTagComponentToggle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDisplayTagComponentToggle.Image = global::MitoPlayer_2024.Properties.Resources.Arrow_Right_20_20;
            this.btnDisplayTagComponentToggle.Location = new System.Drawing.Point(1221, 12);
            this.btnDisplayTagComponentToggle.Name = "btnDisplayTagComponentToggle";
            this.btnDisplayTagComponentToggle.Size = new System.Drawing.Size(31, 23);
            this.btnDisplayTagComponentToggle.TabIndex = 57;
            this.btnDisplayTagComponentToggle.UseVisualStyleBackColor = true;
            // 
            // pnlTagComponent
            // 
            this.pnlTagComponent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTagComponent.Controls.Add(this.lblFilter);
            this.pnlTagComponent.Controls.Add(this.btnFilter);
            this.pnlTagComponent.Controls.Add(this.txtbFilter);
            this.pnlTagComponent.Controls.Add(this.tagValueEditorPanel);
            this.pnlTagComponent.Controls.Add(this.btnClearTagValueFilter);
            this.pnlTagComponent.Location = new System.Drawing.Point(980, 42);
            this.pnlTagComponent.Name = "pnlTagComponent";
            this.pnlTagComponent.Size = new System.Drawing.Size(274, 600);
            this.pnlTagComponent.TabIndex = 58;
            // 
            // lblFilter
            // 
            this.lblFilter.AutoSize = true;
            this.lblFilter.Location = new System.Drawing.Point(13, 8);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(32, 13);
            this.lblFilter.TabIndex = 43;
            this.lblFilter.Text = "Filter:";
            // 
            // btnFilter
            // 
            this.btnFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFilter.Location = new System.Drawing.Point(205, 3);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(66, 23);
            this.btnFilter.TabIndex = 46;
            this.btnFilter.Text = "Filter";
            this.btnFilter.UseVisualStyleBackColor = true;
            // 
            // txtbFilter
            // 
            this.txtbFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtbFilter.Location = new System.Drawing.Point(51, 5);
            this.txtbFilter.Name = "txtbFilter";
            this.txtbFilter.Size = new System.Drawing.Size(148, 20);
            this.txtbFilter.TabIndex = 42;
            // 
            // tagValueEditorPanel
            // 
            this.tagValueEditorPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tagValueEditorPanel.AutoScroll = true;
            this.tagValueEditorPanel.Location = new System.Drawing.Point(0, 32);
            this.tagValueEditorPanel.Name = "tagValueEditorPanel";
            this.tagValueEditorPanel.Size = new System.Drawing.Size(274, 536);
            this.tagValueEditorPanel.TabIndex = 36;
            // 
            // btnClearTagValueFilter
            // 
            this.btnClearTagValueFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearTagValueFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearTagValueFilter.Location = new System.Drawing.Point(196, 574);
            this.btnClearTagValueFilter.Name = "btnClearTagValueFilter";
            this.btnClearTagValueFilter.Size = new System.Drawing.Size(75, 23);
            this.btnClearTagValueFilter.TabIndex = 45;
            this.btnClearTagValueFilter.Text = "Clear Filter";
            this.btnClearTagValueFilter.UseVisualStyleBackColor = true;
            // 
            // dgvTracklist
            // 
            this.dgvTracklist.AllowDrop = true;
            this.dgvTracklist.AllowUserToAddRows = false;
            this.dgvTracklist.AllowUserToDeleteRows = false;
            this.dgvTracklist.AllowUserToResizeRows = false;
            this.dgvTracklist.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTracklist.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTracklist.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvTracklist.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvTracklist.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvTracklist.Location = new System.Drawing.Point(201, 100);
            this.dgvTracklist.Name = "dgvTracklist";
            this.dgvTracklist.ReadOnly = true;
            this.dgvTracklist.RowHeadersVisible = false;
            this.dgvTracklist.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTracklist.Size = new System.Drawing.Size(773, 337);
            this.dgvTracklist.TabIndex = 54;
            this.dgvTracklist.SelectionChanged += new System.EventHandler(this.dgvTracklist_SelectionChanged);
            // 
            // dgvSelectorTracklist
            // 
            this.dgvSelectorTracklist.AllowDrop = true;
            this.dgvSelectorTracklist.AllowUserToAddRows = false;
            this.dgvSelectorTracklist.AllowUserToDeleteRows = false;
            this.dgvSelectorTracklist.AllowUserToResizeRows = false;
            this.dgvSelectorTracklist.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSelectorTracklist.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSelectorTracklist.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvSelectorTracklist.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvSelectorTracklist.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvSelectorTracklist.Location = new System.Drawing.Point(201, 443);
            this.dgvSelectorTracklist.Name = "dgvSelectorTracklist";
            this.dgvSelectorTracklist.ReadOnly = true;
            this.dgvSelectorTracklist.RowHeadersVisible = false;
            this.dgvSelectorTracklist.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSelectorTracklist.Size = new System.Drawing.Size(773, 196);
            this.dgvSelectorTracklist.TabIndex = 54;
            this.dgvSelectorTracklist.SelectionChanged += new System.EventHandler(this.dgvSelectorTracklist_SelectionChanged);
            // 
            // PlaylistCreatorView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.pnlTagComponent);
            this.Controls.Add(this.btnDisplayCoverImageToggle);
            this.Controls.Add(this.btnDisplayTagComponentToggle);
            this.Controls.Add(this.grbCoverImageComponent);
            this.Controls.Add(this.lblSelectedItemsLength);
            this.Controls.Add(this.lblSelectedItemsCount);
            this.Controls.Add(this.dgvSelectorTracklist);
            this.Controls.Add(this.dgvTracklist);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnPlaylistListPanelToggle);
            this.Controls.Add(this.dgvPlaylistList);
            this.Controls.Add(this.lblTrackSumLength);
            this.Controls.Add(this.lblTrackCount);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PlaylistCreatorView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PlaylistCreatorView";
            this.Shown += new System.EventHandler(this.PlaylistCreatorView_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlaylistList)).EndInit();
            this.pnlTagComponent.ResumeLayout(false);
            this.pnlTagComponent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTracklist)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelectorTracklist)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnPlaylistListPanelToggle;
        private System.Windows.Forms.DataGridView dgvPlaylistList;
        private System.Windows.Forms.Label lblTrackSumLength;
        private System.Windows.Forms.Label lblTrackCount;
        private System.Windows.Forms.Label lblSelectedItemsLength;
        private System.Windows.Forms.Label lblSelectedItemsCount;
        private Helpers.CustomDataGridView dgvTracklist;
        private System.Windows.Forms.GroupBox grbCoverImageComponent;
        private System.Windows.Forms.Button btnDisplayCoverImageToggle;
        private System.Windows.Forms.Button btnDisplayTagComponentToggle;
        private System.Windows.Forms.Panel pnlTagComponent;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.TextBox txtbFilter;
        private System.Windows.Forms.Panel tagValueEditorPanel;
        private System.Windows.Forms.Button btnClearTagValueFilter;
        private Helpers.CustomDataGridView dgvSelectorTracklist;
    }
}