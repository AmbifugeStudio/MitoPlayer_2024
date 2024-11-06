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
            this.lblTrackCount = new System.Windows.Forms.Label();
            this.lblTrackSumLength = new System.Windows.Forms.Label();
            this.lblSelectedItemsCount = new System.Windows.Forms.Label();
            this.lblSelectedItemsLength = new System.Windows.Forms.Label();
            this.btnPlaylistListPanelToggle = new System.Windows.Forms.Button();
            this.btnDisplayTagComponentToggle = new System.Windows.Forms.Button();
            this.dgvPlaylistList = new System.Windows.Forms.DataGridView();
            this.dgvTrackList = new System.Windows.Forms.DataGridView();
            this.tagValueEditorPanel = new System.Windows.Forms.Panel();
            this.chbOnlyPlayingRowModeEnabled = new System.Windows.Forms.CheckBox();
            this.txtbFilter = new System.Windows.Forms.TextBox();
            this.lblFilter = new System.Windows.Forms.Label();
            this.btnClearTagValueFilter = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.pnlPlaylistList = new System.Windows.Forms.Panel();
            this.pnlTagComponent = new System.Windows.Forms.Panel();
            this.btnFilter = new System.Windows.Forms.Button();
            this.btnSetterModeToggle = new System.Windows.Forms.Button();
            this.btnFilterModeToggle = new System.Windows.Forms.Button();
            this.grbCovers = new System.Windows.Forms.GroupBox();
            this.lblMessage = new System.Windows.Forms.Label();
            this.btnCreateModel = new System.Windows.Forms.Button();
            this.btnDetectKey = new System.Windows.Forms.Button();
            this.btnAddTrackToModel = new System.Windows.Forms.Button();
            this.lblTracksInTheModel = new System.Windows.Forms.Label();
            this.lblLog = new System.Windows.Forms.Label();
            this.btnTrainModel = new System.Windows.Forms.Button();
            this.btnColumnVisibilityWithTagEditor = new System.Windows.Forms.Button();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlaylistList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrackList)).BeginInit();
            this.pnlPlaylistList.SuspendLayout();
            this.pnlTagComponent.SuspendLayout();
            this.SuspendLayout();
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
            this.renameToolStripMenuItem.Text = "Edit";
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
            this.btnScanKeyAndBpm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnScanKeyAndBpm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScanKeyAndBpm.Location = new System.Drawing.Point(1075, 14);
            this.btnScanKeyAndBpm.Name = "btnScanKeyAndBpm";
            this.btnScanKeyAndBpm.Size = new System.Drawing.Size(90, 23);
            this.btnScanKeyAndBpm.TabIndex = 31;
            this.btnScanKeyAndBpm.Text = "Scan Key/Bpm";
            this.btnScanKeyAndBpm.UseVisualStyleBackColor = true;
            this.btnScanKeyAndBpm.Click += new System.EventHandler(this.btnScanBpm_Click);
            // 
            // lblTrackCount
            // 
            this.lblTrackCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTrackCount.AutoSize = true;
            this.lblTrackCount.Location = new System.Drawing.Point(12, 643);
            this.lblTrackCount.Name = "lblTrackCount";
            this.lblTrackCount.Size = new System.Drawing.Size(116, 13);
            this.lblTrackCount.TabIndex = 3;
            this.lblTrackCount.Text = "1000 item(s) in [Playlist]";
            this.lblTrackCount.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTrackSumLength
            // 
            this.lblTrackSumLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTrackSumLength.AutoSize = true;
            this.lblTrackSumLength.Location = new System.Drawing.Point(12, 656);
            this.lblTrackSumLength.Name = "lblTrackSumLength";
            this.lblTrackSumLength.Size = new System.Drawing.Size(88, 13);
            this.lblTrackSumLength.TabIndex = 3;
            this.lblTrackSumLength.Text = "Length: 00:00:00";
            this.lblTrackSumLength.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSelectedItemsCount
            // 
            this.lblSelectedItemsCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSelectedItemsCount.AutoSize = true;
            this.lblSelectedItemsCount.Location = new System.Drawing.Point(206, 643);
            this.lblSelectedItemsCount.Name = "lblSelectedItemsCount";
            this.lblSelectedItemsCount.Size = new System.Drawing.Size(78, 13);
            this.lblSelectedItemsCount.TabIndex = 3;
            this.lblSelectedItemsCount.Text = "0 item selected";
            // 
            // lblSelectedItemsLength
            // 
            this.lblSelectedItemsLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSelectedItemsLength.AutoSize = true;
            this.lblSelectedItemsLength.Location = new System.Drawing.Point(206, 656);
            this.lblSelectedItemsLength.Name = "lblSelectedItemsLength";
            this.lblSelectedItemsLength.Size = new System.Drawing.Size(88, 13);
            this.lblSelectedItemsLength.TabIndex = 3;
            this.lblSelectedItemsLength.Text = "Length: 00:00:00";
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
            this.dgvPlaylistList.Location = new System.Drawing.Point(0, 1);
            this.dgvPlaylistList.MultiSelect = false;
            this.dgvPlaylistList.Name = "dgvPlaylistList";
            this.dgvPlaylistList.ReadOnly = true;
            this.dgvPlaylistList.RowHeadersVisible = false;
            this.dgvPlaylistList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPlaylistList.Size = new System.Drawing.Size(183, 597);
            this.dgvPlaylistList.TabIndex = 17;
            this.dgvPlaylistList.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvPlaylistList_DataBindingComplete);
            this.dgvPlaylistList.SelectionChanged += new System.EventHandler(this.dgvPlaylistList_SelectionChanged);
            this.dgvPlaylistList.DragDrop += new System.Windows.Forms.DragEventHandler(this.dgvPlaylistList_DragDrop);
            this.dgvPlaylistList.DragEnter += new System.Windows.Forms.DragEventHandler(this.dgvPlaylistList_DragEnter);
            this.dgvPlaylistList.DragOver += new System.Windows.Forms.DragEventHandler(this.dgvPlaylistList_DragOver);
            this.dgvPlaylistList.DragLeave += new System.EventHandler(this.dgvPlaylistList_DragLeave);
            this.dgvPlaylistList.Paint += new System.Windows.Forms.PaintEventHandler(this.dgvPlaylistList_Paint);
            this.dgvPlaylistList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvPlaylistList_KeyDown);
            this.dgvPlaylistList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvPlaylistList_MouseClick);
            this.dgvPlaylistList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dgvPlaylistList_MouseDoubleClick);
            this.dgvPlaylistList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvPlaylistList_MouseDown);
            this.dgvPlaylistList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dgvPlaylistList_MouseMove);
            this.dgvPlaylistList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvPlaylistList_MouseUp);
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
            this.dgvTrackList.Location = new System.Drawing.Point(199, 101);
            this.dgvTrackList.Name = "dgvTrackList";
            this.dgvTrackList.ReadOnly = true;
            this.dgvTrackList.RowHeadersVisible = false;
            this.dgvTrackList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTrackList.Size = new System.Drawing.Size(761, 539);
            this.dgvTrackList.TabIndex = 16;
            this.dgvTrackList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTrackList_CellClick);
            this.dgvTrackList.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvTrackList_ColumnHeaderMouseClick);
            this.dgvTrackList.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvTrackList_DataBindingComplete);
            this.dgvTrackList.SelectionChanged += new System.EventHandler(this.dgvTrackList_SelectionChanged);
            this.dgvTrackList.DragDrop += new System.Windows.Forms.DragEventHandler(this.dgvTrackList_DragDrop);
            this.dgvTrackList.DragEnter += new System.Windows.Forms.DragEventHandler(this.dgvTrackList_DragEnter);
            this.dgvTrackList.DragOver += new System.Windows.Forms.DragEventHandler(this.dgvTrackList_DragOver);
            this.dgvTrackList.DragLeave += new System.EventHandler(this.dgvTrackList_DragLeave);
            this.dgvTrackList.Paint += new System.Windows.Forms.PaintEventHandler(this.dgvTrackList_Paint); 
            this.dgvTrackList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvTrackList_KeyDown);
            this.dgvTrackList.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dgvTrackList_KeyUp);
            this.dgvTrackList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.dgvTrackList_MouseDoubleClick);
            this.dgvTrackList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvTrackList_MouseDown);
            this.dgvTrackList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dgvTrackList_MouseMove);
            this.dgvTrackList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvTrackList_MouseUp);
            // 
            // tagValueEditorPanel
            // 
            this.tagValueEditorPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tagValueEditorPanel.AutoScroll = true;
            this.tagValueEditorPanel.Location = new System.Drawing.Point(0, 58);
            this.tagValueEditorPanel.Name = "tagValueEditorPanel";
            this.tagValueEditorPanel.Size = new System.Drawing.Size(274, 505);
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
            this.txtbFilter.Size = new System.Drawing.Size(148, 20);
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
            // btnClearTagValueFilter
            // 
            this.btnClearTagValueFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearTagValueFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClearTagValueFilter.Location = new System.Drawing.Point(196, 569);
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
            this.btnSave.Location = new System.Drawing.Point(47, 13);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(32, 23);
            this.btnSave.TabIndex = 46;
            this.btnSave.Text = "💾";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // pnlPlaylistList
            // 
            this.pnlPlaylistList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlPlaylistList.Controls.Add(this.dgvPlaylistList);
            this.pnlPlaylistList.Location = new System.Drawing.Point(10, 42);
            this.pnlPlaylistList.Name = "pnlPlaylistList";
            this.pnlPlaylistList.Size = new System.Drawing.Size(183, 598);
            this.pnlPlaylistList.TabIndex = 48;
            // 
            // pnlTagComponent
            // 
            this.pnlTagComponent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTagComponent.Controls.Add(this.btnFilter);
            this.pnlTagComponent.Controls.Add(this.btnSetterModeToggle);
            this.pnlTagComponent.Controls.Add(this.btnFilterModeToggle);
            this.pnlTagComponent.Controls.Add(this.tagValueEditorPanel);
            this.pnlTagComponent.Controls.Add(this.btnClearTagValueFilter);
            this.pnlTagComponent.Controls.Add(this.txtbFilter);
            this.pnlTagComponent.Controls.Add(this.lblFilter);
            this.pnlTagComponent.Controls.Add(this.chbOnlyPlayingRowModeEnabled);
            this.pnlTagComponent.Location = new System.Drawing.Point(966, 43);
            this.pnlTagComponent.Name = "pnlTagComponent";
            this.pnlTagComponent.Size = new System.Drawing.Size(274, 597);
            this.pnlTagComponent.TabIndex = 49;
            // 
            // btnFilter
            // 
            this.btnFilter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFilter.Location = new System.Drawing.Point(196, 30);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(66, 23);
            this.btnFilter.TabIndex = 46;
            this.btnFilter.Text = "Filter";
            this.btnFilter.UseVisualStyleBackColor = true;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
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
            // grbCovers
            // 
            this.grbCovers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grbCovers.Location = new System.Drawing.Point(199, 7);
            this.grbCovers.Name = "grbCovers";
            this.grbCovers.Size = new System.Drawing.Size(761, 87);
            this.grbCovers.TabIndex = 50;
            this.grbCovers.TabStop = false;
            // 
            // lblMessage
            // 
            this.lblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMessage.Location = new System.Drawing.Point(747, 643);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(213, 26);
            this.lblMessage.TabIndex = 51;
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnCreateModel
            // 
            this.btnCreateModel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCreateModel.Location = new System.Drawing.Point(498, 646);
            this.btnCreateModel.Name = "btnCreateModel";
            this.btnCreateModel.Size = new System.Drawing.Size(85, 23);
            this.btnCreateModel.TabIndex = 52;
            this.btnCreateModel.Text = "Create Model";
            this.btnCreateModel.UseVisualStyleBackColor = true;
            this.btnCreateModel.Click += new System.EventHandler(this.btnCreateModel_Click);
            // 
            // btnDetectKey
            // 
            this.btnDetectKey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnDetectKey.Location = new System.Drawing.Point(679, 646);
            this.btnDetectKey.Name = "btnDetectKey";
            this.btnDetectKey.Size = new System.Drawing.Size(86, 23);
            this.btnDetectKey.TabIndex = 52;
            this.btnDetectKey.Text = "Detect Key";
            this.btnDetectKey.UseVisualStyleBackColor = true;
            this.btnDetectKey.Click += new System.EventHandler(this.btnDetectKey_Click);
            // 
            // btnAddTrackToModel
            // 
            this.btnAddTrackToModel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddTrackToModel.Location = new System.Drawing.Point(300, 646);
            this.btnAddTrackToModel.Name = "btnAddTrackToModel";
            this.btnAddTrackToModel.Size = new System.Drawing.Size(86, 23);
            this.btnAddTrackToModel.TabIndex = 52;
            this.btnAddTrackToModel.Text = "Add To Model";
            this.btnAddTrackToModel.UseVisualStyleBackColor = true;
            this.btnAddTrackToModel.Click += new System.EventHandler(this.btnAddTrackToModel_Click);
            // 
            // lblTracksInTheModel
            // 
            this.lblTracksInTheModel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTracksInTheModel.AutoSize = true;
            this.lblTracksInTheModel.Location = new System.Drawing.Point(392, 651);
            this.lblTracksInTheModel.Name = "lblTracksInTheModel";
            this.lblTracksInTheModel.Size = new System.Drawing.Size(100, 13);
            this.lblTracksInTheModel.TabIndex = 53;
            this.lblTracksInTheModel.Text = "0 track in the model";
            // 
            // lblLog
            // 
            this.lblLog.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLog.Location = new System.Drawing.Point(966, 638);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(274, 31);
            this.lblLog.TabIndex = 53;
            // 
            // btnTrainModel
            // 
            this.btnTrainModel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTrainModel.Location = new System.Drawing.Point(589, 646);
            this.btnTrainModel.Name = "btnTrainModel";
            this.btnTrainModel.Size = new System.Drawing.Size(84, 23);
            this.btnTrainModel.TabIndex = 54;
            this.btnTrainModel.Text = "Train Model";
            this.btnTrainModel.UseVisualStyleBackColor = true;
            this.btnTrainModel.Click += new System.EventHandler(this.btnTrainModel_Click);
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
            // PlaylistView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1252, 678);
            this.Controls.Add(this.btnTrainModel);
            this.Controls.Add(this.lblLog);
            this.Controls.Add(this.lblTracksInTheModel);
            this.Controls.Add(this.btnDetectKey);
            this.Controls.Add(this.btnAddTrackToModel);
            this.Controls.Add(this.btnCreateModel);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.grbCovers);
            this.Controls.Add(this.lblTrackSumLength);
            this.Controls.Add(this.lblTrackCount);
            this.Controls.Add(this.lblSelectedItemsLength);
            this.Controls.Add(this.lblSelectedItemsCount);
            this.Controls.Add(this.pnlTagComponent);
            this.Controls.Add(this.pnlPlaylistList);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnScanKeyAndBpm);
            this.Controls.Add(this.btnPlaylistListPanelToggle);
            this.Controls.Add(this.btnDisplayTagComponentToggle);
            this.Controls.Add(this.btnColumnVisibilityWithTagEditor);
            this.Controls.Add(this.dgvTrackList);
            this.Name = "PlaylistView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Player";
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlaylistList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrackList)).EndInit();
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
        private System.Windows.Forms.Label lblTrackCount;
        private System.Windows.Forms.Label lblTrackSumLength;
        private System.Windows.Forms.Label lblSelectedItemsCount;
        private System.Windows.Forms.Label lblSelectedItemsLength;
        private System.Windows.Forms.Button btnPlaylistListPanelToggle;
        private System.Windows.Forms.Button btnDisplayTagComponentToggle;
        private System.Windows.Forms.Button btnColumnVisibilityWithTagEditor;
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
        private System.Windows.Forms.Button btnClearTagValueFilter;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel pnlPlaylistList;
        private System.Windows.Forms.Panel pnlTagComponent;
        private System.Windows.Forms.Button btnSetterModeToggle;
        private System.Windows.Forms.Button btnFilterModeToggle;
        private System.Windows.Forms.Button btnFilter;
        private System.Windows.Forms.GroupBox grbCovers;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Button btnCreateModel;
        private System.Windows.Forms.Button btnDetectKey;
        private System.Windows.Forms.Button btnAddTrackToModel;
        private System.Windows.Forms.Label lblTracksInTheModel;
        private System.Windows.Forms.Label lblLog;
        private System.Windows.Forms.Button btnTrainModel;
    }
}