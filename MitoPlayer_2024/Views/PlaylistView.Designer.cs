namespace MitoPlayer_2024.Views
{
    partial class PlaylistView
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlaylistView));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.exportToTxtToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.createPlaylistToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.setQuicklistGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setGroup1ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.setGroup2ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.setGroup3ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.setGroup4ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.exportToM3UToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToDirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripCreatePlaylist = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripLoadPlaylist = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripRenamePlaylist = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripDeletePlaylist = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToM3uToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStripSetQuickListGroup1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripSetQuickListGroup2 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripSetQuickListGroup3 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripSetQuickListGroup4 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripSetQuickListGroup = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnScanKeyAndBpm = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lblTrackCount = new System.Windows.Forms.Label();
            this.lblTrackSumLength = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblSelectedItemsCount = new System.Windows.Forms.Label();
            this.lblSelectedItemsLength = new System.Windows.Forms.Label();
            this.lblCurrentTrack = new System.Windows.Forms.Label();
            this.btnPlaylistListPanelToggle = new System.Windows.Forms.Button();
            this.btnDisplayTagComponentToggle = new System.Windows.Forms.Button();
            this.btnColumnVisibilityWithTagEditor = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.dgvPlaylistList = new System.Windows.Forms.DataGridView();
            this.dgvTrackList = new System.Windows.Forms.DataGridView();
            this.tagValueEditorPanel = new System.Windows.Forms.Panel();
            this.chbOnlyPlayingRowModeEnabled = new System.Windows.Forms.CheckBox();
            this.txtbFilter = new System.Windows.Forms.TextBox();
            this.lblFilter = new System.Windows.Forms.Label();
            this.rtxtbTagValueEditorParams = new System.Windows.Forms.RichTextBox();
            this.btnClearTagValueFilter = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.lblLog1 = new System.Windows.Forms.Label();
            this.lblLog2 = new System.Windows.Forms.Label();
            this.lblLog3 = new System.Windows.Forms.Label();
            this.lblLog4 = new System.Windows.Forms.Label();
            this.groupBoxPlaylist = new System.Windows.Forms.GroupBox();
            this.btnNewPlaylist = new System.Windows.Forms.Button();
            this.btnRenamePlaylist = new System.Windows.Forms.Button();
            this.btnLoadPlaylist = new System.Windows.Forms.Button();
            this.btnDeletePlaylist = new System.Windows.Forms.Button();
            this.pnlPlaylistList = new System.Windows.Forms.Panel();
            this.pnlTagComponent = new System.Windows.Forms.Panel();
            this.btnSetterModeToggle = new System.Windows.Forms.Button();
            this.btnFilterModeToggle = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlaylistList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrackList)).BeginInit();
            this.groupBoxPlaylist.SuspendLayout();
            this.pnlPlaylistList.SuspendLayout();
            this.pnlTagComponent.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // exportToTxtToolStripMenuItem
            // 
            this.exportToTxtToolStripMenuItem.Name = "exportToTxtToolStripMenuItem";
            this.exportToTxtToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.exportToTxtToolStripMenuItem.Text = "Export to txt";
            this.exportToTxtToolStripMenuItem.Click += new System.EventHandler(this.menuStripExportToTxtToolStripMenuItem_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.createPlaylistToolStripMenuItem,
            this.loadToolStripMenuItem,
            this.renameToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.toolStripSeparator3,
            this.setQuicklistGroupToolStripMenuItem,
            this.toolStripSeparator4,
            this.exportToM3UToolStripMenuItem1,
            this.exportToTxtToolStripMenuItem,
            this.exportToDirToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.ShowImageMargin = false;
            this.contextMenuStrip1.Size = new System.Drawing.Size(151, 192);
            // 
            // createPlaylistToolStripMenuItem
            // 
            this.createPlaylistToolStripMenuItem.Name = "createPlaylistToolStripMenuItem";
            this.createPlaylistToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.createPlaylistToolStripMenuItem.Text = "New";
            this.createPlaylistToolStripMenuItem.Click += new System.EventHandler(this.menuStripCreatePlaylist_Click);
            // 
            // loadToolStripMenuItem
            // 
            this.loadToolStripMenuItem.Name = "loadToolStripMenuItem";
            this.loadToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.loadToolStripMenuItem.Text = "Load";
            this.loadToolStripMenuItem.Click += new System.EventHandler(this.menuStripLoadPlaylist_Click);
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.renameToolStripMenuItem.Text = "Rename";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.menuStripRenamePlaylist_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.deleteToolStripMenuItem.Text = "Remove";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.menuStripDeletePlaylist_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(147, 6);
            // 
            // setQuicklistGroupToolStripMenuItem
            // 
            this.setQuicklistGroupToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setGroup1ToolStripMenuItem1,
            this.setGroup2ToolStripMenuItem1,
            this.setGroup3ToolStripMenuItem1,
            this.setGroup4ToolStripMenuItem1});
            this.setQuicklistGroupToolStripMenuItem.Name = "setQuicklistGroupToolStripMenuItem";
            this.setQuicklistGroupToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.setQuicklistGroupToolStripMenuItem.Text = "Set Quicklist Group";
            // 
            // setGroup1ToolStripMenuItem1
            // 
            this.setGroup1ToolStripMenuItem1.Name = "setGroup1ToolStripMenuItem1";
            this.setGroup1ToolStripMenuItem1.Size = new System.Drawing.Size(135, 22);
            this.setGroup1ToolStripMenuItem1.Text = "Set Group 1";
            this.setGroup1ToolStripMenuItem1.Click += new System.EventHandler(this.menuStripSetQuickListGroup1_Click);
            // 
            // setGroup2ToolStripMenuItem1
            // 
            this.setGroup2ToolStripMenuItem1.Name = "setGroup2ToolStripMenuItem1";
            this.setGroup2ToolStripMenuItem1.Size = new System.Drawing.Size(135, 22);
            this.setGroup2ToolStripMenuItem1.Text = "Set Group 2";
            this.setGroup2ToolStripMenuItem1.Click += new System.EventHandler(this.menuStripSetQuickListGroup2_Click);
            // 
            // setGroup3ToolStripMenuItem1
            // 
            this.setGroup3ToolStripMenuItem1.Name = "setGroup3ToolStripMenuItem1";
            this.setGroup3ToolStripMenuItem1.Size = new System.Drawing.Size(135, 22);
            this.setGroup3ToolStripMenuItem1.Text = "Set Group 3";
            this.setGroup3ToolStripMenuItem1.Click += new System.EventHandler(this.menuStripSetQuickListGroup3_Click);
            // 
            // setGroup4ToolStripMenuItem1
            // 
            this.setGroup4ToolStripMenuItem1.Name = "setGroup4ToolStripMenuItem1";
            this.setGroup4ToolStripMenuItem1.Size = new System.Drawing.Size(135, 22);
            this.setGroup4ToolStripMenuItem1.Text = "Set Group 4";
            this.setGroup4ToolStripMenuItem1.Click += new System.EventHandler(this.menuStripSetQuickListGroup4_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(147, 6);
            // 
            // exportToM3UToolStripMenuItem1
            // 
            this.exportToM3UToolStripMenuItem1.Name = "exportToM3UToolStripMenuItem1";
            this.exportToM3UToolStripMenuItem1.Size = new System.Drawing.Size(150, 22);
            this.exportToM3UToolStripMenuItem1.Text = "Export to m3u";
            this.exportToM3UToolStripMenuItem1.Click += new System.EventHandler(this.menuStripExportToM3uToolStripMenuItem_Click);
            // 
            // exportToDirToolStripMenuItem
            // 
            this.exportToDirToolStripMenuItem.Name = "exportToDirToolStripMenuItem";
            this.exportToDirToolStripMenuItem.Size = new System.Drawing.Size(150, 22);
            this.exportToDirToolStripMenuItem.Text = "Export to directory";
            this.exportToDirToolStripMenuItem.Click += new System.EventHandler(this.exportToDirectoryToolStripMenuItem_Click);
            // 
            // menuStripCreatePlaylist
            // 
            this.menuStripCreatePlaylist.Name = "menuStripCreatePlaylist";
            this.menuStripCreatePlaylist.Size = new System.Drawing.Size(139, 22);
            this.menuStripCreatePlaylist.Text = "New";
            this.menuStripCreatePlaylist.Click += new System.EventHandler(this.menuStripCreatePlaylist_Click);
            // 
            // menuStripLoadPlaylist
            // 
            this.menuStripLoadPlaylist.Name = "menuStripLoadPlaylist";
            this.menuStripLoadPlaylist.Size = new System.Drawing.Size(139, 22);
            this.menuStripLoadPlaylist.Text = "Load";
            this.menuStripLoadPlaylist.Click += new System.EventHandler(this.menuStripLoadPlaylist_Click);
            // 
            // menuStripRenamePlaylist
            // 
            this.menuStripRenamePlaylist.Name = "menuStripRenamePlaylist";
            this.menuStripRenamePlaylist.Size = new System.Drawing.Size(139, 22);
            this.menuStripRenamePlaylist.Text = "Rename";
            this.menuStripRenamePlaylist.Click += new System.EventHandler(this.menuStripRenamePlaylist_Click);
            // 
            // menuStripDeletePlaylist
            // 
            this.menuStripDeletePlaylist.Name = "menuStripDeletePlaylist";
            this.menuStripDeletePlaylist.Size = new System.Drawing.Size(139, 22);
            this.menuStripDeletePlaylist.Text = "Remove";
            this.menuStripDeletePlaylist.Click += new System.EventHandler(this.menuStripDeletePlaylist_Click);
            // 
            // exportToDirectoryToolStripMenuItem
            // 
            this.exportToDirectoryToolStripMenuItem.Name = "exportToDirectoryToolStripMenuItem";
            this.exportToDirectoryToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.exportToDirectoryToolStripMenuItem.Text = "Export to directory";
            this.exportToDirectoryToolStripMenuItem.Click += new System.EventHandler(this.exportToDirectoryToolStripMenuItem_Click);
            // 
            // exportToM3uToolStripMenuItem
            // 
            this.exportToM3uToolStripMenuItem.Name = "exportToM3uToolStripMenuItem";
            this.exportToM3uToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.exportToM3uToolStripMenuItem.Text = "Export to m3u";
            this.exportToM3uToolStripMenuItem.Click += new System.EventHandler(this.menuStripExportToM3uToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(169, 6);
            // 
            // menuStripSetQuickListGroup1
            // 
            this.menuStripSetQuickListGroup1.Name = "menuStripSetQuickListGroup1";
            this.menuStripSetQuickListGroup1.Size = new System.Drawing.Size(167, 22);
            this.menuStripSetQuickListGroup1.Text = "Quick list group 1";
            this.menuStripSetQuickListGroup1.Click += new System.EventHandler(this.menuStripSetQuickListGroup1_Click);
            // 
            // menuStripSetQuickListGroup2
            // 
            this.menuStripSetQuickListGroup2.Name = "menuStripSetQuickListGroup2";
            this.menuStripSetQuickListGroup2.Size = new System.Drawing.Size(167, 22);
            this.menuStripSetQuickListGroup2.Text = "Quick list group 2";
            this.menuStripSetQuickListGroup2.Click += new System.EventHandler(this.menuStripSetQuickListGroup2_Click);
            // 
            // menuStripSetQuickListGroup3
            // 
            this.menuStripSetQuickListGroup3.Name = "menuStripSetQuickListGroup3";
            this.menuStripSetQuickListGroup3.Size = new System.Drawing.Size(167, 22);
            this.menuStripSetQuickListGroup3.Text = "Quick list group 3";
            this.menuStripSetQuickListGroup3.Click += new System.EventHandler(this.menuStripSetQuickListGroup3_Click);
            // 
            // menuStripSetQuickListGroup4
            // 
            this.menuStripSetQuickListGroup4.Name = "menuStripSetQuickListGroup4";
            this.menuStripSetQuickListGroup4.Size = new System.Drawing.Size(167, 22);
            this.menuStripSetQuickListGroup4.Text = "Quick list group 4";
            this.menuStripSetQuickListGroup4.Click += new System.EventHandler(this.menuStripSetQuickListGroup4_Click);
            // 
            // menuStripSetQuickListGroup
            // 
            this.menuStripSetQuickListGroup.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuStripSetQuickListGroup1,
            this.menuStripSetQuickListGroup2,
            this.menuStripSetQuickListGroup3,
            this.menuStripSetQuickListGroup4});
            this.menuStripSetQuickListGroup.Name = "menuStripSetQuickListGroup";
            this.menuStripSetQuickListGroup.Size = new System.Drawing.Size(175, 22);
            this.menuStripSetQuickListGroup.Text = "Set quick list group";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(172, 6);
            // 
            // btnScanKeyAndBpm
            // 
            this.btnScanKeyAndBpm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnScanKeyAndBpm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScanKeyAndBpm.Location = new System.Drawing.Point(870, 583);
            this.btnScanKeyAndBpm.Name = "btnScanKeyAndBpm";
            this.btnScanKeyAndBpm.Size = new System.Drawing.Size(90, 23);
            this.btnScanKeyAndBpm.TabIndex = 31;
            this.btnScanKeyAndBpm.Text = "Scan Key/Bpm";
            this.btnScanKeyAndBpm.UseVisualStyleBackColor = true;
            this.btnScanKeyAndBpm.Click += new System.EventHandler(this.btnScanBpm_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox4.Controls.Add(this.lblTrackCount);
            this.groupBox4.Controls.Add(this.lblTrackSumLength);
            this.groupBox4.Location = new System.Drawing.Point(199, 583);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(183, 79);
            this.groupBox4.TabIndex = 30;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Playlist";
            // 
            // lblTrackCount
            // 
            this.lblTrackCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTrackCount.Location = new System.Drawing.Point(6, 22);
            this.lblTrackCount.Name = "lblTrackCount";
            this.lblTrackCount.Size = new System.Drawing.Size(171, 13);
            this.lblTrackCount.TabIndex = 3;
            this.lblTrackCount.Text = "1000 item(s) in [Playlist]";
            this.lblTrackCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTrackSumLength
            // 
            this.lblTrackSumLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTrackSumLength.Location = new System.Drawing.Point(6, 44);
            this.lblTrackSumLength.Name = "lblTrackSumLength";
            this.lblTrackSumLength.Size = new System.Drawing.Size(171, 13);
            this.lblTrackSumLength.TabIndex = 3;
            this.lblTrackSumLength.Text = "Length: 00:00:00";
            this.lblTrackSumLength.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.lblSelectedItemsCount);
            this.groupBox3.Controls.Add(this.lblSelectedItemsLength);
            this.groupBox3.Controls.Add(this.lblCurrentTrack);
            this.groupBox3.Location = new System.Drawing.Point(388, 583);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(375, 79);
            this.groupBox3.TabIndex = 29;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Player";
            // 
            // lblSelectedItemsCount
            // 
            this.lblSelectedItemsCount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSelectedItemsCount.AutoSize = true;
            this.lblSelectedItemsCount.Location = new System.Drawing.Point(6, 43);
            this.lblSelectedItemsCount.Name = "lblSelectedItemsCount";
            this.lblSelectedItemsCount.Size = new System.Drawing.Size(78, 13);
            this.lblSelectedItemsCount.TabIndex = 3;
            this.lblSelectedItemsCount.Text = "0 item selected";
            // 
            // lblSelectedItemsLength
            // 
            this.lblSelectedItemsLength.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSelectedItemsLength.Location = new System.Drawing.Point(6, 63);
            this.lblSelectedItemsLength.Name = "lblSelectedItemsLength";
            this.lblSelectedItemsLength.Size = new System.Drawing.Size(88, 13);
            this.lblSelectedItemsLength.TabIndex = 3;
            this.lblSelectedItemsLength.Text = "Length: 00:00:00";
            // 
            // lblCurrentTrack
            // 
            this.lblCurrentTrack.Location = new System.Drawing.Point(6, 23);
            this.lblCurrentTrack.Name = "lblCurrentTrack";
            this.lblCurrentTrack.Size = new System.Drawing.Size(363, 13);
            this.lblCurrentTrack.TabIndex = 2;
            this.lblCurrentTrack.Text = "Playing: -";
            // 
            // btnPlaylistListPanelToggle
            // 
            this.btnPlaylistListPanelToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlaylistListPanelToggle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPlaylistListPanelToggle.Location = new System.Drawing.Point(10, 13);
            this.btnPlaylistListPanelToggle.Name = "btnPlaylistListPanelToggle";
            this.btnPlaylistListPanelToggle.Size = new System.Drawing.Size(31, 23);
            this.btnPlaylistListPanelToggle.TabIndex = 22;
            this.btnPlaylistListPanelToggle.Text = "<";
            this.btnPlaylistListPanelToggle.UseVisualStyleBackColor = true;
            this.btnPlaylistListPanelToggle.Click += new System.EventHandler(this.btnDisplayPlaylistList_Click);
            // 
            // btnDisplayTagComponentToggle
            // 
            this.btnDisplayTagComponentToggle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDisplayTagComponentToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDisplayTagComponentToggle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDisplayTagComponentToggle.Location = new System.Drawing.Point(1209, 14);
            this.btnDisplayTagComponentToggle.Name = "btnDisplayTagComponentToggle";
            this.btnDisplayTagComponentToggle.Size = new System.Drawing.Size(31, 23);
            this.btnDisplayTagComponentToggle.TabIndex = 23;
            this.btnDisplayTagComponentToggle.Text = "<";
            this.btnDisplayTagComponentToggle.UseVisualStyleBackColor = true;
            this.btnDisplayTagComponentToggle.Click += new System.EventHandler(this.btnDisplayTagEditor_Click);
            // 
            // btnColumnVisibilityWithTagEditor
            // 
            this.btnColumnVisibilityWithTagEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnColumnVisibilityWithTagEditor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnColumnVisibilityWithTagEditor.Image = ((System.Drawing.Image)(resources.GetObject("btnColumnVisibilityWithTagEditor.Image")));
            this.btnColumnVisibilityWithTagEditor.Location = new System.Drawing.Point(1171, 14);
            this.btnColumnVisibilityWithTagEditor.Name = "btnColumnVisibilityWithTagEditor";
            this.btnColumnVisibilityWithTagEditor.Size = new System.Drawing.Size(31, 23);
            this.btnColumnVisibilityWithTagEditor.TabIndex = 24;
            this.btnColumnVisibilityWithTagEditor.UseVisualStyleBackColor = true;
            this.btnColumnVisibilityWithTagEditor.Click += new System.EventHandler(this.btnColumnVisibility_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMessage.Location = new System.Drawing.Point(606, 639);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(424, 13);
            this.lblMessage.TabIndex = 18;
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMessage.Visible = false;
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
            this.dgvPlaylistList.Location = new System.Drawing.Point(0, 0);
            this.dgvPlaylistList.MultiSelect = false;
            this.dgvPlaylistList.Name = "dgvPlaylistList";
            this.dgvPlaylistList.ReadOnly = true;
            this.dgvPlaylistList.RowHeadersVisible = false;
            this.dgvPlaylistList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPlaylistList.Size = new System.Drawing.Size(183, 536);
            this.dgvPlaylistList.TabIndex = 17;
            this.dgvPlaylistList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPlaylistList_CellDoubleClick);
            this.dgvPlaylistList.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvPlaylistList_CellPainting);
            this.dgvPlaylistList.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvPlaylistList_DataBindingComplete);
            this.dgvPlaylistList.DragDrop += new System.Windows.Forms.DragEventHandler(this.dgvPlaylistList_DragDrop);
            this.dgvPlaylistList.DragOver += new System.Windows.Forms.DragEventHandler(this.dgvPlaylistList_DragOver);
            this.dgvPlaylistList.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.dgvPlaylistList_QueryContinueDrag);
            this.dgvPlaylistList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvPlaylistList_KeyDown);
            this.dgvPlaylistList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvPlaylistList_MouseClick);
            this.dgvPlaylistList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvPlaylistList_MouseDown);
            this.dgvPlaylistList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dgvPlaylistList_MouseMove);
            // 
            // dgvTrackList
            // 
            this.dgvTrackList.AllowDrop = true;
            this.dgvTrackList.AllowUserToAddRows = false;
            this.dgvTrackList.AllowUserToDeleteRows = false;
            this.dgvTrackList.AllowUserToResizeRows = false;
            this.dgvTrackList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvTrackList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvTrackList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvTrackList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvTrackList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvTrackList.Location = new System.Drawing.Point(199, 41);
            this.dgvTrackList.Name = "dgvTrackList";
            this.dgvTrackList.ReadOnly = true;
            this.dgvTrackList.RowHeadersVisible = false;
            this.dgvTrackList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTrackList.Size = new System.Drawing.Size(761, 536);
            this.dgvTrackList.TabIndex = 16;
            this.dgvTrackList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTrackList_CellClick);
            this.dgvTrackList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTrackList_CellDoubleClick);
            this.dgvTrackList.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvTrackList_CellPainting);
            this.dgvTrackList.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvTrackList_ColumnHeaderMouseClick);
            this.dgvTrackList.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvTrackList_DataBindingComplete);
            this.dgvTrackList.SelectionChanged += new System.EventHandler(this.dgvTrackList_SelectionChanged);
            this.dgvTrackList.DragDrop += new System.Windows.Forms.DragEventHandler(this.dgvTrackList_DragDrop);
            this.dgvTrackList.DragOver += new System.Windows.Forms.DragEventHandler(this.dgvTrackList_DragOver);
            this.dgvTrackList.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.dgvTrackList_QueryContinueDrag);
            this.dgvTrackList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvTrackList_KeyDown);
            this.dgvTrackList.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dgvTrackList_KeyUp);
            this.dgvTrackList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvTrackList_MouseDown);
            this.dgvTrackList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dgvTrackList_MouseMove);
            // 
            // tagValueEditorPanel
            // 
            this.tagValueEditorPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tagValueEditorPanel.AutoScroll = true;
            this.tagValueEditorPanel.Location = new System.Drawing.Point(0, 58);
            this.tagValueEditorPanel.Name = "tagValueEditorPanel";
            this.tagValueEditorPanel.Size = new System.Drawing.Size(274, 386);
            this.tagValueEditorPanel.TabIndex = 36;
            this.tagValueEditorPanel.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.tagValueEditorPanel_PreviewKeyDown);
            // 
            // chbOnlyPlayingRowModeEnabled
            // 
            this.chbOnlyPlayingRowModeEnabled.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chbOnlyPlayingRowModeEnabled.AutoSize = true;
            this.chbOnlyPlayingRowModeEnabled.Location = new System.Drawing.Point(4, 34);
            this.chbOnlyPlayingRowModeEnabled.Name = "chbOnlyPlayingRowModeEnabled";
            this.chbOnlyPlayingRowModeEnabled.Size = new System.Drawing.Size(103, 17);
            this.chbOnlyPlayingRowModeEnabled.TabIndex = 40;
            this.chbOnlyPlayingRowModeEnabled.Text = "Set Only Playing";
            this.chbOnlyPlayingRowModeEnabled.UseVisualStyleBackColor = true;
            this.chbOnlyPlayingRowModeEnabled.CheckedChanged += new System.EventHandler(this.chbOnlyPlayingRowModeEnabled_CheckedChanged);
            // 
            // txtbFilter
            // 
            this.txtbFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtbFilter.Location = new System.Drawing.Point(42, 32);
            this.txtbFilter.Name = "txtbFilter";
            this.txtbFilter.Size = new System.Drawing.Size(229, 20);
            this.txtbFilter.TabIndex = 42;
            this.txtbFilter.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtbFilter_KeyUp);
            // 
            // lblFilter
            // 
            this.lblFilter.AutoSize = true;
            this.lblFilter.Location = new System.Drawing.Point(4, 35);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(32, 13);
            this.lblFilter.TabIndex = 43;
            this.lblFilter.Text = "Filter:";
            // 
            // rtxtbTagValueEditorParams
            // 
            this.rtxtbTagValueEditorParams.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.rtxtbTagValueEditorParams.Enabled = false;
            this.rtxtbTagValueEditorParams.Location = new System.Drawing.Point(0, 450);
            this.rtxtbTagValueEditorParams.Name = "rtxtbTagValueEditorParams";
            this.rtxtbTagValueEditorParams.Size = new System.Drawing.Size(274, 50);
            this.rtxtbTagValueEditorParams.TabIndex = 44;
            this.rtxtbTagValueEditorParams.Text = "";
            // 
            // btnClearTagValueFilter
            // 
            this.btnClearTagValueFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearTagValueFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearTagValueFilter.Location = new System.Drawing.Point(196, 506);
            this.btnClearTagValueFilter.Name = "btnClearTagValueFilter";
            this.btnClearTagValueFilter.Size = new System.Drawing.Size(75, 23);
            this.btnClearTagValueFilter.TabIndex = 45;
            this.btnClearTagValueFilter.Text = "Clear Filter";
            this.btnClearTagValueFilter.UseVisualStyleBackColor = true;
            this.btnClearTagValueFilter.Click += new System.EventHandler(this.btnClearTagValueFilter_Click);
            // 
            // btnSave
            // 
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Location = new System.Drawing.Point(236, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 46;
            this.btnSave.Text = "SAVE";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblLog1
            // 
            this.lblLog1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLog1.AutoSize = true;
            this.lblLog1.Location = new System.Drawing.Point(967, 583);
            this.lblLog1.Name = "lblLog1";
            this.lblLog1.Size = new System.Drawing.Size(35, 13);
            this.lblLog1.TabIndex = 47;
            this.lblLog1.Text = "label1";
            // 
            // lblLog2
            // 
            this.lblLog2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLog2.AutoSize = true;
            this.lblLog2.Location = new System.Drawing.Point(967, 602);
            this.lblLog2.Name = "lblLog2";
            this.lblLog2.Size = new System.Drawing.Size(35, 13);
            this.lblLog2.TabIndex = 47;
            this.lblLog2.Text = "label1";
            // 
            // lblLog3
            // 
            this.lblLog3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLog3.AutoSize = true;
            this.lblLog3.Location = new System.Drawing.Point(967, 626);
            this.lblLog3.Name = "lblLog3";
            this.lblLog3.Size = new System.Drawing.Size(35, 13);
            this.lblLog3.TabIndex = 47;
            this.lblLog3.Text = "label1";
            // 
            // lblLog4
            // 
            this.lblLog4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLog4.AutoSize = true;
            this.lblLog4.Location = new System.Drawing.Point(967, 646);
            this.lblLog4.Name = "lblLog4";
            this.lblLog4.Size = new System.Drawing.Size(35, 13);
            this.lblLog4.TabIndex = 47;
            this.lblLog4.Text = "label1";
            // 
            // groupBoxPlaylist
            // 
            this.groupBoxPlaylist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxPlaylist.Controls.Add(this.btnNewPlaylist);
            this.groupBoxPlaylist.Controls.Add(this.btnRenamePlaylist);
            this.groupBoxPlaylist.Controls.Add(this.btnLoadPlaylist);
            this.groupBoxPlaylist.Controls.Add(this.btnDeletePlaylist);
            this.groupBoxPlaylist.Location = new System.Drawing.Point(0, 542);
            this.groupBoxPlaylist.Name = "groupBoxPlaylist";
            this.groupBoxPlaylist.Size = new System.Drawing.Size(183, 79);
            this.groupBoxPlaylist.TabIndex = 19;
            this.groupBoxPlaylist.TabStop = false;
            this.groupBoxPlaylist.Text = "Playlist";
            // 
            // btnNewPlaylist
            // 
            this.btnNewPlaylist.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewPlaylist.Location = new System.Drawing.Point(6, 19);
            this.btnNewPlaylist.Name = "btnNewPlaylist";
            this.btnNewPlaylist.Size = new System.Drawing.Size(83, 23);
            this.btnNewPlaylist.TabIndex = 6;
            this.btnNewPlaylist.Text = "New";
            this.btnNewPlaylist.UseVisualStyleBackColor = true;
            this.btnNewPlaylist.Click += new System.EventHandler(this.btnNewPlaylist_Click);
            // 
            // btnRenamePlaylist
            // 
            this.btnRenamePlaylist.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRenamePlaylist.Location = new System.Drawing.Point(6, 48);
            this.btnRenamePlaylist.Name = "btnRenamePlaylist";
            this.btnRenamePlaylist.Size = new System.Drawing.Size(83, 23);
            this.btnRenamePlaylist.TabIndex = 6;
            this.btnRenamePlaylist.Text = "Rename";
            this.btnRenamePlaylist.UseVisualStyleBackColor = true;
            this.btnRenamePlaylist.Click += new System.EventHandler(this.btnRenamePlaylist_Click);
            // 
            // btnLoadPlaylist
            // 
            this.btnLoadPlaylist.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadPlaylist.Location = new System.Drawing.Point(95, 19);
            this.btnLoadPlaylist.Name = "btnLoadPlaylist";
            this.btnLoadPlaylist.Size = new System.Drawing.Size(82, 23);
            this.btnLoadPlaylist.TabIndex = 6;
            this.btnLoadPlaylist.Text = "Load";
            this.btnLoadPlaylist.UseVisualStyleBackColor = true;
            this.btnLoadPlaylist.Click += new System.EventHandler(this.btnLoadPlaylist_Click);
            // 
            // btnDeletePlaylist
            // 
            this.btnDeletePlaylist.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeletePlaylist.Location = new System.Drawing.Point(95, 48);
            this.btnDeletePlaylist.Name = "btnDeletePlaylist";
            this.btnDeletePlaylist.Size = new System.Drawing.Size(82, 23);
            this.btnDeletePlaylist.TabIndex = 6;
            this.btnDeletePlaylist.Text = "Delete";
            this.btnDeletePlaylist.UseVisualStyleBackColor = true;
            this.btnDeletePlaylist.Click += new System.EventHandler(this.btnDeletePlaylist_Click);
            // 
            // pnlPlaylistList
            // 
            this.pnlPlaylistList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlPlaylistList.Controls.Add(this.groupBoxPlaylist);
            this.pnlPlaylistList.Controls.Add(this.dgvPlaylistList);
            this.pnlPlaylistList.Location = new System.Drawing.Point(10, 41);
            this.pnlPlaylistList.Name = "pnlPlaylistList";
            this.pnlPlaylistList.Size = new System.Drawing.Size(183, 621);
            this.pnlPlaylistList.TabIndex = 48;
            // 
            // pnlTagComponent
            // 
            this.pnlTagComponent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTagComponent.Controls.Add(this.btnSetterModeToggle);
            this.pnlTagComponent.Controls.Add(this.btnFilterModeToggle);
            this.pnlTagComponent.Controls.Add(this.tagValueEditorPanel);
            this.pnlTagComponent.Controls.Add(this.btnClearTagValueFilter);
            this.pnlTagComponent.Controls.Add(this.rtxtbTagValueEditorParams);
            this.pnlTagComponent.Controls.Add(this.txtbFilter);
            this.pnlTagComponent.Controls.Add(this.lblFilter);
            this.pnlTagComponent.Controls.Add(this.chbOnlyPlayingRowModeEnabled);
            this.pnlTagComponent.Location = new System.Drawing.Point(966, 43);
            this.pnlTagComponent.Name = "pnlTagComponent";
            this.pnlTagComponent.Size = new System.Drawing.Size(274, 534);
            this.pnlTagComponent.TabIndex = 49;
            // 
            // btnSetterModeToggle
            // 
            this.btnSetterModeToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSetterModeToggle.Location = new System.Drawing.Point(4, 3);
            this.btnSetterModeToggle.Name = "btnSetterModeToggle";
            this.btnSetterModeToggle.Size = new System.Drawing.Size(75, 23);
            this.btnSetterModeToggle.TabIndex = 46;
            this.btnSetterModeToggle.Text = "Setter";
            this.btnSetterModeToggle.UseVisualStyleBackColor = true;
            this.btnSetterModeToggle.Click += new System.EventHandler(this.btnSetterModeToggle_Click);
            // 
            // btnFilterModeToggle
            // 
            this.btnFilterModeToggle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFilterModeToggle.Location = new System.Drawing.Point(85, 3);
            this.btnFilterModeToggle.Name = "btnFilterModeToggle";
            this.btnFilterModeToggle.Size = new System.Drawing.Size(75, 23);
            this.btnFilterModeToggle.TabIndex = 46;
            this.btnFilterModeToggle.Text = "Filter";
            this.btnFilterModeToggle.UseVisualStyleBackColor = true;
            this.btnFilterModeToggle.Click += new System.EventHandler(this.btnFilterModeToggle_Click);
            // 
            // PlaylistView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1252, 678);
            this.Controls.Add(this.pnlTagComponent);
            this.Controls.Add(this.pnlPlaylistList);
            this.Controls.Add(this.lblLog4);
            this.Controls.Add(this.lblLog3);
            this.Controls.Add(this.lblLog2);
            this.Controls.Add(this.lblLog1);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnScanKeyAndBpm);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnPlaylistListPanelToggle);
            this.Controls.Add(this.btnDisplayTagComponentToggle);
            this.Controls.Add(this.btnColumnVisibilityWithTagEditor);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.dgvTrackList);
            this.Name = "PlaylistView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Player";
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlaylistList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrackList)).EndInit();
            this.groupBoxPlaylist.ResumeLayout(false);
            this.pnlPlaylistList.ResumeLayout(false);
            this.pnlTagComponent.ResumeLayout(false);
            this.pnlTagComponent.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripMenuItem exportToTxtToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuStripCreatePlaylist;
        private System.Windows.Forms.ToolStripMenuItem menuStripLoadPlaylist;
        private System.Windows.Forms.ToolStripMenuItem menuStripRenamePlaylist;
        private System.Windows.Forms.ToolStripMenuItem menuStripDeletePlaylist;
        private System.Windows.Forms.ToolStripMenuItem exportToDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToM3uToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuStripSetQuickListGroup1;
        private System.Windows.Forms.ToolStripMenuItem menuStripSetQuickListGroup2;
        private System.Windows.Forms.ToolStripMenuItem menuStripSetQuickListGroup3;
        private System.Windows.Forms.ToolStripMenuItem menuStripSetQuickListGroup4;
        private System.Windows.Forms.ToolStripMenuItem menuStripSetQuickListGroup;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Button btnScanKeyAndBpm;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Label lblTrackCount;
        private System.Windows.Forms.Label lblTrackSumLength;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lblSelectedItemsCount;
        private System.Windows.Forms.Label lblSelectedItemsLength;
        private System.Windows.Forms.Label lblCurrentTrack;
        private System.Windows.Forms.Button btnPlaylistListPanelToggle;
        private System.Windows.Forms.Button btnDisplayTagComponentToggle;
        private System.Windows.Forms.Button btnColumnVisibilityWithTagEditor;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.DataGridView dgvPlaylistList;
        private System.Windows.Forms.DataGridView dgvTrackList;
        private System.Windows.Forms.ToolStripMenuItem createPlaylistToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem exportToM3UToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem setQuicklistGroupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem setGroup1ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem setGroup2ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem setGroup3ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem setGroup4ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exportToDirToolStripMenuItem;
        private System.Windows.Forms.Panel tagValueEditorPanel;
        private System.Windows.Forms.CheckBox chbOnlyPlayingRowModeEnabled;
        private System.Windows.Forms.TextBox txtbFilter;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.RichTextBox rtxtbTagValueEditorParams;
        private System.Windows.Forms.Button btnClearTagValueFilter;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblLog1;
        private System.Windows.Forms.Label lblLog2;
        private System.Windows.Forms.Label lblLog3;
        private System.Windows.Forms.Label lblLog4;
        private System.Windows.Forms.GroupBox groupBoxPlaylist;
        private System.Windows.Forms.Button btnNewPlaylist;
        private System.Windows.Forms.Button btnRenamePlaylist;
        private System.Windows.Forms.Button btnLoadPlaylist;
        private System.Windows.Forms.Button btnDeletePlaylist;
        private System.Windows.Forms.Panel pnlPlaylistList;
        private System.Windows.Forms.Panel pnlTagComponent;
        private System.Windows.Forms.Button btnSetterModeToggle;
        private System.Windows.Forms.Button btnFilterModeToggle;
    }
}