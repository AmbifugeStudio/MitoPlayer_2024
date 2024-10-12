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
            this.lblTagColor = new System.Windows.Forms.Label();
            this.cmbColor = new System.Windows.Forms.ComboBox();
            this.btnHidePlaylistList = new System.Windows.Forms.Button();
            this.btnDisplayTagEditor = new System.Windows.Forms.Button();
            this.btnColumnVisibilityWithTagEditor = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdbPlaylist = new System.Windows.Forms.RadioButton();
            this.rdbTagValue = new System.Windows.Forms.RadioButton();
            this.groupBoxTagValueHotkeys = new System.Windows.Forms.GroupBox();
            this.tgvHotkeyName3 = new System.Windows.Forms.Label();
            this.tgvHotkeyName4 = new System.Windows.Forms.Label();
            this.tgvHotkeyName2 = new System.Windows.Forms.Label();
            this.tgvHotkeyName1 = new System.Windows.Forms.Label();
            this.groupBoxPlaylist = new System.Windows.Forms.GroupBox();
            this.btnNewPlaylist = new System.Windows.Forms.Button();
            this.btnRenamePlaylist = new System.Windows.Forms.Button();
            this.btnLoadPlaylist = new System.Windows.Forms.Button();
            this.btnDeletePlaylist = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.dgvPlaylistList = new System.Windows.Forms.DataGridView();
            this.dgvTrackList = new System.Windows.Forms.DataGridView();
            this.btnDisplayPlaylistList = new System.Windows.Forms.Button();
            this.btnHideTagEditor = new System.Windows.Forms.Button();
            this.btnColumnVisibility = new System.Windows.Forms.Button();
            this.tagValueEditorPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.rdbtnSelectedRow = new System.Windows.Forms.RadioButton();
            this.rdbtnPlayingRow = new System.Windows.Forms.RadioButton();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBoxTagValueHotkeys.SuspendLayout();
            this.groupBoxPlaylist.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlaylistList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrackList)).BeginInit();
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
            this.btnScanKeyAndBpm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnScanKeyAndBpm.Location = new System.Drawing.Point(400, 12);
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
            // lblTagColor
            // 
            this.lblTagColor.AutoSize = true;
            this.lblTagColor.Location = new System.Drawing.Point(236, 17);
            this.lblTagColor.Name = "lblTagColor";
            this.lblTagColor.Size = new System.Drawing.Size(31, 13);
            this.lblTagColor.TabIndex = 28;
            this.lblTagColor.Text = "Color";
            // 
            // cmbColor
            // 
            this.cmbColor.FormattingEnabled = true;
            this.cmbColor.Location = new System.Drawing.Point(273, 14);
            this.cmbColor.Name = "cmbColor";
            this.cmbColor.Size = new System.Drawing.Size(121, 21);
            this.cmbColor.TabIndex = 27;
            this.cmbColor.SelectedIndexChanged += new System.EventHandler(this.cmbColor_SelectedIndexChanged);
            // 
            // btnHidePlaylistList
            // 
            this.btnHidePlaylistList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHidePlaylistList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHidePlaylistList.Location = new System.Drawing.Point(199, 12);
            this.btnHidePlaylistList.Name = "btnHidePlaylistList";
            this.btnHidePlaylistList.Size = new System.Drawing.Size(31, 23);
            this.btnHidePlaylistList.TabIndex = 22;
            this.btnHidePlaylistList.Text = "<";
            this.btnHidePlaylistList.UseVisualStyleBackColor = true;
            this.btnHidePlaylistList.Click += new System.EventHandler(this.btnDisplayPlaylistList_Click);
            // 
            // btnDisplayTagEditor
            // 
            this.btnDisplayTagEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDisplayTagEditor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDisplayTagEditor.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDisplayTagEditor.Location = new System.Drawing.Point(1212, 14);
            this.btnDisplayTagEditor.Name = "btnDisplayTagEditor";
            this.btnDisplayTagEditor.Size = new System.Drawing.Size(31, 23);
            this.btnDisplayTagEditor.TabIndex = 23;
            this.btnDisplayTagEditor.Text = "<";
            this.btnDisplayTagEditor.UseVisualStyleBackColor = true;
            this.btnDisplayTagEditor.Click += new System.EventHandler(this.btnDisplayTagEditor_Click);
            // 
            // btnColumnVisibilityWithTagEditor
            // 
            this.btnColumnVisibilityWithTagEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnColumnVisibilityWithTagEditor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnColumnVisibilityWithTagEditor.Image = ((System.Drawing.Image)(resources.GetObject("btnColumnVisibilityWithTagEditor.Image")));
            this.btnColumnVisibilityWithTagEditor.Location = new System.Drawing.Point(1175, 14);
            this.btnColumnVisibilityWithTagEditor.Name = "btnColumnVisibilityWithTagEditor";
            this.btnColumnVisibilityWithTagEditor.Size = new System.Drawing.Size(31, 23);
            this.btnColumnVisibilityWithTagEditor.TabIndex = 24;
            this.btnColumnVisibilityWithTagEditor.UseVisualStyleBackColor = true;
            this.btnColumnVisibilityWithTagEditor.Click += new System.EventHandler(this.btnColumnVisibility_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.rdbPlaylist);
            this.groupBox2.Controls.Add(this.rdbTagValue);
            this.groupBox2.Location = new System.Drawing.Point(769, 583);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(191, 79);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Hotkey mode";
            // 
            // rdbPlaylist
            // 
            this.rdbPlaylist.AutoSize = true;
            this.rdbPlaylist.Location = new System.Drawing.Point(11, 18);
            this.rdbPlaylist.Name = "rdbPlaylist";
            this.rdbPlaylist.Size = new System.Drawing.Size(57, 17);
            this.rdbPlaylist.TabIndex = 0;
            this.rdbPlaylist.Text = "Playlist";
            this.rdbPlaylist.UseVisualStyleBackColor = true;
            // 
            // rdbTagValue
            // 
            this.rdbTagValue.AutoSize = true;
            this.rdbTagValue.Checked = true;
            this.rdbTagValue.Location = new System.Drawing.Point(74, 18);
            this.rdbTagValue.Name = "rdbTagValue";
            this.rdbTagValue.Size = new System.Drawing.Size(71, 17);
            this.rdbTagValue.TabIndex = 0;
            this.rdbTagValue.TabStop = true;
            this.rdbTagValue.Text = "TagValue";
            this.rdbTagValue.UseVisualStyleBackColor = true;
            // 
            // groupBoxTagValueHotkeys
            // 
            this.groupBoxTagValueHotkeys.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxTagValueHotkeys.Controls.Add(this.tgvHotkeyName3);
            this.groupBoxTagValueHotkeys.Controls.Add(this.tgvHotkeyName4);
            this.groupBoxTagValueHotkeys.Controls.Add(this.tgvHotkeyName2);
            this.groupBoxTagValueHotkeys.Controls.Add(this.tgvHotkeyName1);
            this.groupBoxTagValueHotkeys.Location = new System.Drawing.Point(989, 590);
            this.groupBoxTagValueHotkeys.Name = "groupBoxTagValueHotkeys";
            this.groupBoxTagValueHotkeys.Size = new System.Drawing.Size(254, 72);
            this.groupBoxTagValueHotkeys.TabIndex = 21;
            this.groupBoxTagValueHotkeys.TabStop = false;
            this.groupBoxTagValueHotkeys.Text = "Hotkey";
            // 
            // tgvHotkeyName3
            // 
            this.tgvHotkeyName3.AutoSize = true;
            this.tgvHotkeyName3.Location = new System.Drawing.Point(6, 39);
            this.tgvHotkeyName3.Name = "tgvHotkeyName3";
            this.tgvHotkeyName3.Size = new System.Drawing.Size(25, 13);
            this.tgvHotkeyName3.TabIndex = 1;
            this.tgvHotkeyName3.Text = "(3) -";
            // 
            // tgvHotkeyName4
            // 
            this.tgvHotkeyName4.AutoSize = true;
            this.tgvHotkeyName4.Location = new System.Drawing.Point(124, 39);
            this.tgvHotkeyName4.Name = "tgvHotkeyName4";
            this.tgvHotkeyName4.Size = new System.Drawing.Size(25, 13);
            this.tgvHotkeyName4.TabIndex = 1;
            this.tgvHotkeyName4.Text = "(4) -";
            // 
            // tgvHotkeyName2
            // 
            this.tgvHotkeyName2.AutoSize = true;
            this.tgvHotkeyName2.Location = new System.Drawing.Point(124, 16);
            this.tgvHotkeyName2.Name = "tgvHotkeyName2";
            this.tgvHotkeyName2.Size = new System.Drawing.Size(25, 13);
            this.tgvHotkeyName2.TabIndex = 1;
            this.tgvHotkeyName2.Text = "(2) -";
            // 
            // tgvHotkeyName1
            // 
            this.tgvHotkeyName1.AutoSize = true;
            this.tgvHotkeyName1.Location = new System.Drawing.Point(6, 16);
            this.tgvHotkeyName1.Name = "tgvHotkeyName1";
            this.tgvHotkeyName1.Size = new System.Drawing.Size(25, 13);
            this.tgvHotkeyName1.TabIndex = 1;
            this.tgvHotkeyName1.Text = "(1) -";
            // 
            // groupBoxPlaylist
            // 
            this.groupBoxPlaylist.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBoxPlaylist.Controls.Add(this.btnNewPlaylist);
            this.groupBoxPlaylist.Controls.Add(this.btnRenamePlaylist);
            this.groupBoxPlaylist.Controls.Add(this.btnLoadPlaylist);
            this.groupBoxPlaylist.Controls.Add(this.btnDeletePlaylist);
            this.groupBoxPlaylist.Location = new System.Drawing.Point(10, 583);
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
            this.dgvPlaylistList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.dgvPlaylistList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvPlaylistList.Location = new System.Drawing.Point(10, 12);
            this.dgvPlaylistList.MultiSelect = false;
            this.dgvPlaylistList.Name = "dgvPlaylistList";
            this.dgvPlaylistList.ReadOnly = true;
            this.dgvPlaylistList.RowHeadersVisible = false;
            this.dgvPlaylistList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPlaylistList.Size = new System.Drawing.Size(183, 565);
            this.dgvPlaylistList.TabIndex = 17;
            this.dgvPlaylistList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPlaylistList_CellDoubleClick);
            this.dgvPlaylistList.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvPlaylistList_DataBindingComplete);
            this.dgvPlaylistList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvPlaylistList_KeyDown);
            this.dgvPlaylistList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dgvPlaylistList_MouseClick);
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
            this.dgvTrackList.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvTrackList_ColumnHeaderMouseClick);
            this.dgvTrackList.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvTrackList_DataBindingComplete);
            this.dgvTrackList.SelectionChanged += new System.EventHandler(this.dgvTrackList_SelectionChanged);
            this.dgvTrackList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvTrackList_KeyDown);
            // 
            // btnDisplayPlaylistList
            // 
            this.btnDisplayPlaylistList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDisplayPlaylistList.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDisplayPlaylistList.Location = new System.Drawing.Point(10, 12);
            this.btnDisplayPlaylistList.Name = "btnDisplayPlaylistList";
            this.btnDisplayPlaylistList.Size = new System.Drawing.Size(31, 23);
            this.btnDisplayPlaylistList.TabIndex = 32;
            this.btnDisplayPlaylistList.Text = "<";
            this.btnDisplayPlaylistList.UseVisualStyleBackColor = true;
            this.btnDisplayPlaylistList.Click += new System.EventHandler(this.btnDisplayPlaylistList2_Click);
            // 
            // btnHideTagEditor
            // 
            this.btnHideTagEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHideTagEditor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHideTagEditor.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHideTagEditor.Location = new System.Drawing.Point(929, 12);
            this.btnHideTagEditor.Name = "btnHideTagEditor";
            this.btnHideTagEditor.Size = new System.Drawing.Size(31, 23);
            this.btnHideTagEditor.TabIndex = 33;
            this.btnHideTagEditor.Text = "<";
            this.btnHideTagEditor.UseVisualStyleBackColor = true;
            this.btnHideTagEditor.Click += new System.EventHandler(this.btnDisplayTagEditor2_Click);
            // 
            // btnColumnVisibility
            // 
            this.btnColumnVisibility.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnColumnVisibility.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnColumnVisibility.Image = ((System.Drawing.Image)(resources.GetObject("btnColumnVisibility.Image")));
            this.btnColumnVisibility.Location = new System.Drawing.Point(892, 12);
            this.btnColumnVisibility.Name = "btnColumnVisibility";
            this.btnColumnVisibility.Size = new System.Drawing.Size(31, 23);
            this.btnColumnVisibility.TabIndex = 34;
            this.btnColumnVisibility.UseVisualStyleBackColor = true;
            this.btnColumnVisibility.Click += new System.EventHandler(this.btnColumnVisibility2_Click);
            // 
            // tagValueEditorPanel
            // 
            this.tagValueEditorPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tagValueEditorPanel.AutoScroll = true;
            this.tagValueEditorPanel.Location = new System.Drawing.Point(966, 41);
            this.tagValueEditorPanel.Name = "tagValueEditorPanel";
            this.tagValueEditorPanel.Size = new System.Drawing.Size(274, 536);
            this.tagValueEditorPanel.TabIndex = 36;
            this.tagValueEditorPanel.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.tagValueEditorPanel_PreviewKeyDown);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(967, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 37;
            this.label1.Text = "Set mode:";
            // 
            // rdbtnSelectedRow
            // 
            this.rdbtnSelectedRow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdbtnSelectedRow.AutoSize = true;
            this.rdbtnSelectedRow.Location = new System.Drawing.Point(1028, 15);
            this.rdbtnSelectedRow.Name = "rdbtnSelectedRow";
            this.rdbtnSelectedRow.Size = new System.Drawing.Size(67, 17);
            this.rdbtnSelectedRow.TabIndex = 39;
            this.rdbtnSelectedRow.TabStop = true;
            this.rdbtnSelectedRow.Text = "Selected";
            this.rdbtnSelectedRow.UseVisualStyleBackColor = true;
            this.rdbtnSelectedRow.CheckedChanged += new System.EventHandler(this.rdbtnSelectedRow_CheckedChanged);
            // 
            // rdbtnPlayingRow
            // 
            this.rdbtnPlayingRow.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rdbtnPlayingRow.AutoSize = true;
            this.rdbtnPlayingRow.Location = new System.Drawing.Point(1101, 15);
            this.rdbtnPlayingRow.Name = "rdbtnPlayingRow";
            this.rdbtnPlayingRow.Size = new System.Drawing.Size(59, 17);
            this.rdbtnPlayingRow.TabIndex = 39;
            this.rdbtnPlayingRow.TabStop = true;
            this.rdbtnPlayingRow.Text = "Playing";
            this.rdbtnPlayingRow.UseVisualStyleBackColor = true;
            this.rdbtnPlayingRow.CheckedChanged += new System.EventHandler(this.rdbtnPlayingRow_CheckedChanged);
            // 
            // PlaylistView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1252, 678);
            this.Controls.Add(this.rdbtnPlayingRow);
            this.Controls.Add(this.rdbtnSelectedRow);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tagValueEditorPanel);
            this.Controls.Add(this.btnColumnVisibility);
            this.Controls.Add(this.btnHideTagEditor);
            this.Controls.Add(this.btnDisplayPlaylistList);
            this.Controls.Add(this.btnScanKeyAndBpm);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.lblTagColor);
            this.Controls.Add(this.cmbColor);
            this.Controls.Add(this.btnHidePlaylistList);
            this.Controls.Add(this.btnDisplayTagEditor);
            this.Controls.Add(this.btnColumnVisibilityWithTagEditor);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBoxTagValueHotkeys);
            this.Controls.Add(this.groupBoxPlaylist);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.dgvPlaylistList);
            this.Controls.Add(this.dgvTrackList);
            this.Name = "PlaylistView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Player";
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBoxTagValueHotkeys.ResumeLayout(false);
            this.groupBoxTagValueHotkeys.PerformLayout();
            this.groupBoxPlaylist.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlaylistList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrackList)).EndInit();
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
        private System.Windows.Forms.Label lblTagColor;
        private System.Windows.Forms.ComboBox cmbColor;
        private System.Windows.Forms.Button btnHidePlaylistList;
        private System.Windows.Forms.Button btnDisplayTagEditor;
        private System.Windows.Forms.Button btnColumnVisibilityWithTagEditor;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdbPlaylist;
        private System.Windows.Forms.RadioButton rdbTagValue;
        private System.Windows.Forms.GroupBox groupBoxTagValueHotkeys;
        private System.Windows.Forms.Label tgvHotkeyName3;
        private System.Windows.Forms.Label tgvHotkeyName4;
        private System.Windows.Forms.Label tgvHotkeyName2;
        private System.Windows.Forms.Label tgvHotkeyName1;
        private System.Windows.Forms.GroupBox groupBoxPlaylist;
        private System.Windows.Forms.Button btnNewPlaylist;
        private System.Windows.Forms.Button btnRenamePlaylist;
        private System.Windows.Forms.Button btnLoadPlaylist;
        private System.Windows.Forms.Button btnDeletePlaylist;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.DataGridView dgvPlaylistList;
        private System.Windows.Forms.DataGridView dgvTrackList;
        private System.Windows.Forms.Button btnDisplayPlaylistList;
        private System.Windows.Forms.Button btnHideTagEditor;
        private System.Windows.Forms.Button btnColumnVisibility;
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rdbtnSelectedRow;
        private System.Windows.Forms.RadioButton rdbtnPlayingRow;
    }
}