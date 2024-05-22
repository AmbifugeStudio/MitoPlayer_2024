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
            this.tabPagePlaylistDetail = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSetQuickListGroup1 = new System.Windows.Forms.Button();
            this.btnSetQuickListGroup2 = new System.Windows.Forms.Button();
            this.btnSetQuickListGroup4 = new System.Windows.Forms.Button();
            this.btnSetQuickListGroup3 = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnNewPlaylist = new System.Windows.Forms.Button();
            this.btnRenamePlaylist = new System.Windows.Forms.Button();
            this.btnLoadPlaylist = new System.Windows.Forms.Button();
            this.btnDeletePlaylist = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.lblSelectedItemsLength = new System.Windows.Forms.Label();
            this.lblSelectedItemsCount = new System.Windows.Forms.Label();
            this.lblCurrentTrack = new System.Windows.Forms.Label();
            this.dgvPlaylistList = new System.Windows.Forms.DataGridView();
            this.dgvTrackList = new System.Windows.Forms.DataGridView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuStripCreatePlaylist = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripLoadPlaylist = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripRenamePlaylist = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripDeletePlaylist = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStripSetQuickListGroup = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripSetQuickListGroup1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripSetQuickListGroup2 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripSetQuickListGroup3 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripSetQuickListGroup4 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exportToM3uToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToTxtToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblTrackCount = new System.Windows.Forms.Label();
            this.lblTrackSumLength = new System.Windows.Forms.Label();
            this.tabPagePlaylistDetail.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlaylistList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrackList)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPagePlaylistDetail
            // 
            this.tabPagePlaylistDetail.Controls.Add(this.groupBox2);
            this.tabPagePlaylistDetail.Controls.Add(this.groupBox1);
            this.tabPagePlaylistDetail.Controls.Add(this.lblTrackSumLength);
            this.tabPagePlaylistDetail.Controls.Add(this.lblTrackCount);
            this.tabPagePlaylistDetail.Controls.Add(this.lblMessage);
            this.tabPagePlaylistDetail.Controls.Add(this.lblSelectedItemsLength);
            this.tabPagePlaylistDetail.Controls.Add(this.lblSelectedItemsCount);
            this.tabPagePlaylistDetail.Controls.Add(this.lblCurrentTrack);
            this.tabPagePlaylistDetail.Controls.Add(this.dgvPlaylistList);
            this.tabPagePlaylistDetail.Controls.Add(this.dgvTrackList);
            this.tabPagePlaylistDetail.Location = new System.Drawing.Point(4, 22);
            this.tabPagePlaylistDetail.Name = "tabPagePlaylistDetail";
            this.tabPagePlaylistDetail.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePlaylistDetail.Size = new System.Drawing.Size(1121, 550);
            this.tabPagePlaylistDetail.TabIndex = 1;
            this.tabPagePlaylistDetail.Text = "Player";
            this.tabPagePlaylistDetail.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.btnSetQuickListGroup1);
            this.groupBox2.Controls.Add(this.btnSetQuickListGroup2);
            this.groupBox2.Controls.Add(this.btnSetQuickListGroup4);
            this.groupBox2.Controls.Add(this.btnSetQuickListGroup3);
            this.groupBox2.Location = new System.Drawing.Point(3, 495);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(164, 49);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "QuickList";
            // 
            // btnSetQuickListGroup1
            // 
            this.btnSetQuickListGroup1.Location = new System.Drawing.Point(6, 19);
            this.btnSetQuickListGroup1.Name = "btnSetQuickListGroup1";
            this.btnSetQuickListGroup1.Size = new System.Drawing.Size(32, 23);
            this.btnSetQuickListGroup1.TabIndex = 6;
            this.btnSetQuickListGroup1.Text = "1";
            this.btnSetQuickListGroup1.UseVisualStyleBackColor = true;
            this.btnSetQuickListGroup1.Click += new System.EventHandler(this.btnSetQuickListGroup1_Click);
            // 
            // btnSetQuickListGroup2
            // 
            this.btnSetQuickListGroup2.Location = new System.Drawing.Point(44, 19);
            this.btnSetQuickListGroup2.Name = "btnSetQuickListGroup2";
            this.btnSetQuickListGroup2.Size = new System.Drawing.Size(32, 23);
            this.btnSetQuickListGroup2.TabIndex = 6;
            this.btnSetQuickListGroup2.Text = "2";
            this.btnSetQuickListGroup2.UseVisualStyleBackColor = true;
            this.btnSetQuickListGroup2.Click += new System.EventHandler(this.btnSetQuickListGroup2_Click);
            // 
            // btnSetQuickListGroup4
            // 
            this.btnSetQuickListGroup4.Location = new System.Drawing.Point(120, 19);
            this.btnSetQuickListGroup4.Name = "btnSetQuickListGroup4";
            this.btnSetQuickListGroup4.Size = new System.Drawing.Size(35, 23);
            this.btnSetQuickListGroup4.TabIndex = 6;
            this.btnSetQuickListGroup4.Text = "4";
            this.btnSetQuickListGroup4.UseVisualStyleBackColor = true;
            this.btnSetQuickListGroup4.Click += new System.EventHandler(this.btnSetQuickListGroup4_Click);
            // 
            // btnSetQuickListGroup3
            // 
            this.btnSetQuickListGroup3.Location = new System.Drawing.Point(82, 19);
            this.btnSetQuickListGroup3.Name = "btnSetQuickListGroup3";
            this.btnSetQuickListGroup3.Size = new System.Drawing.Size(32, 23);
            this.btnSetQuickListGroup3.TabIndex = 6;
            this.btnSetQuickListGroup3.Text = "3";
            this.btnSetQuickListGroup3.UseVisualStyleBackColor = true;
            this.btnSetQuickListGroup3.Click += new System.EventHandler(this.btnSetQuickListGroup3_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.btnNewPlaylist);
            this.groupBox1.Controls.Add(this.btnRenamePlaylist);
            this.groupBox1.Controls.Add(this.btnLoadPlaylist);
            this.groupBox1.Controls.Add(this.btnDeletePlaylist);
            this.groupBox1.Location = new System.Drawing.Point(3, 410);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(164, 79);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Playlist";
            // 
            // btnNewPlaylist
            // 
            this.btnNewPlaylist.Location = new System.Drawing.Point(6, 19);
            this.btnNewPlaylist.Name = "btnNewPlaylist";
            this.btnNewPlaylist.Size = new System.Drawing.Size(75, 23);
            this.btnNewPlaylist.TabIndex = 6;
            this.btnNewPlaylist.Text = "New";
            this.btnNewPlaylist.UseVisualStyleBackColor = true;
            this.btnNewPlaylist.Click += new System.EventHandler(this.btnNewPlaylist_Click);
            // 
            // btnRenamePlaylist
            // 
            this.btnRenamePlaylist.Location = new System.Drawing.Point(6, 48);
            this.btnRenamePlaylist.Name = "btnRenamePlaylist";
            this.btnRenamePlaylist.Size = new System.Drawing.Size(75, 23);
            this.btnRenamePlaylist.TabIndex = 6;
            this.btnRenamePlaylist.Text = "Rename";
            this.btnRenamePlaylist.UseVisualStyleBackColor = true;
            this.btnRenamePlaylist.Click += new System.EventHandler(this.btnRenamePlaylist_Click);
            // 
            // btnLoadPlaylist
            // 
            this.btnLoadPlaylist.Location = new System.Drawing.Point(83, 19);
            this.btnLoadPlaylist.Name = "btnLoadPlaylist";
            this.btnLoadPlaylist.Size = new System.Drawing.Size(75, 23);
            this.btnLoadPlaylist.TabIndex = 6;
            this.btnLoadPlaylist.Text = "Load";
            this.btnLoadPlaylist.UseVisualStyleBackColor = true;
            this.btnLoadPlaylist.Click += new System.EventHandler(this.btnLoadPlaylist_Click);
            // 
            // btnDeletePlaylist
            // 
            this.btnDeletePlaylist.Location = new System.Drawing.Point(83, 48);
            this.btnDeletePlaylist.Name = "btnDeletePlaylist";
            this.btnDeletePlaylist.Size = new System.Drawing.Size(75, 23);
            this.btnDeletePlaylist.TabIndex = 6;
            this.btnDeletePlaylist.Text = "Delete";
            this.btnDeletePlaylist.UseVisualStyleBackColor = true;
            this.btnDeletePlaylist.Click += new System.EventHandler(this.btnDeletePlaylist_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMessage.Location = new System.Drawing.Point(768, 531);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(220, 13);
            this.lblMessage.TabIndex = 3;
            this.lblMessage.Text = "Temporary messages";
            this.lblMessage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblMessage.Visible = false;
            // 
            // lblSelectedItemsLength
            // 
            this.lblSelectedItemsLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSelectedItemsLength.AutoSize = true;
            this.lblSelectedItemsLength.Location = new System.Drawing.Point(173, 531);
            this.lblSelectedItemsLength.Name = "lblSelectedItemsLength";
            this.lblSelectedItemsLength.Size = new System.Drawing.Size(88, 13);
            this.lblSelectedItemsLength.TabIndex = 3;
            this.lblSelectedItemsLength.Text = "Length: 00:00:00";
            // 
            // lblSelectedItemsCount
            // 
            this.lblSelectedItemsCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSelectedItemsCount.AutoSize = true;
            this.lblSelectedItemsCount.Location = new System.Drawing.Point(173, 515);
            this.lblSelectedItemsCount.Name = "lblSelectedItemsCount";
            this.lblSelectedItemsCount.Size = new System.Drawing.Size(101, 13);
            this.lblSelectedItemsCount.TabIndex = 3;
            this.lblSelectedItemsCount.Text = "1000 items selected";
            // 
            // lblCurrentTrack
            // 
            this.lblCurrentTrack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCurrentTrack.AutoSize = true;
            this.lblCurrentTrack.Location = new System.Drawing.Point(280, 515);
            this.lblCurrentTrack.Name = "lblCurrentTrack";
            this.lblCurrentTrack.Size = new System.Drawing.Size(50, 13);
            this.lblCurrentTrack.TabIndex = 2;
            this.lblCurrentTrack.Text = "Playing: -";
            // 
            // dgvPlaylistList
            // 
            this.dgvPlaylistList.AllowUserToAddRows = false;
            this.dgvPlaylistList.AllowUserToDeleteRows = false;
            this.dgvPlaylistList.AllowUserToResizeRows = false;
            this.dgvPlaylistList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgvPlaylistList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPlaylistList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvPlaylistList.Location = new System.Drawing.Point(3, 6);
            this.dgvPlaylistList.MultiSelect = false;
            this.dgvPlaylistList.Name = "dgvPlaylistList";
            this.dgvPlaylistList.ReadOnly = true;
            this.dgvPlaylistList.RowHeadersVisible = false;
            this.dgvPlaylistList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPlaylistList.Size = new System.Drawing.Size(164, 401);
            this.dgvPlaylistList.TabIndex = 1;
            this.dgvPlaylistList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPlaylistList_CellDoubleClick);
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
            this.dgvTrackList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvTrackList.Location = new System.Drawing.Point(173, 6);
            this.dgvTrackList.Name = "dgvTrackList";
            this.dgvTrackList.ReadOnly = true;
            this.dgvTrackList.RowHeadersVisible = false;
            this.dgvTrackList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTrackList.Size = new System.Drawing.Size(940, 506);
            this.dgvTrackList.TabIndex = 0;
            this.dgvTrackList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTrackList_CellClick);
            this.dgvTrackList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTrackList_CellDoubleClick);
            this.dgvTrackList.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvTrackList_CellPainting);
            this.dgvTrackList.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvTrackList_ColumnHeaderMouseClick);
            this.dgvTrackList.SelectionChanged += new System.EventHandler(this.dgvTrackList_SelectionChanged);
            this.dgvTrackList.DragDrop += new System.Windows.Forms.DragEventHandler(this.dgvTrackList_DragDrop);
            this.dgvTrackList.DragOver += new System.Windows.Forms.DragEventHandler(this.dgvTrackList_DragOver);
            this.dgvTrackList.QueryContinueDrag += new System.Windows.Forms.QueryContinueDragEventHandler(this.dgvTrackList_QueryContinueDrag);
            this.dgvTrackList.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvTrackList_KeyDown);
            this.dgvTrackList.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dgvTrackList_KeyUp);
            this.dgvTrackList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvTrackList_MouseDown);
            this.dgvTrackList.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dgvTrackList_MouseMove);
            this.dgvTrackList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dgvTrackList_MouseUp);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPagePlaylistDetail);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1129, 576);
            this.tabControl1.TabIndex = 4;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuStripCreatePlaylist,
            this.menuStripLoadPlaylist,
            this.menuStripRenamePlaylist,
            this.menuStripDeletePlaylist,
            this.toolStripSeparator2,
            this.menuStripSetQuickListGroup,
            this.toolStripSeparator1,
            this.exportToM3uToolStripMenuItem,
            this.exportToTxtToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(176, 170);
            // 
            // menuStripCreatePlaylist
            // 
            this.menuStripCreatePlaylist.Name = "menuStripCreatePlaylist";
            this.menuStripCreatePlaylist.Size = new System.Drawing.Size(175, 22);
            this.menuStripCreatePlaylist.Text = "New";
            this.menuStripCreatePlaylist.Click += new System.EventHandler(this.menuStripCreatePlaylist_Click);
            // 
            // menuStripLoadPlaylist
            // 
            this.menuStripLoadPlaylist.Name = "menuStripLoadPlaylist";
            this.menuStripLoadPlaylist.Size = new System.Drawing.Size(175, 22);
            this.menuStripLoadPlaylist.Text = "Load";
            this.menuStripLoadPlaylist.Click += new System.EventHandler(this.menuStripLoadPlaylist_Click);
            // 
            // menuStripRenamePlaylist
            // 
            this.menuStripRenamePlaylist.Name = "menuStripRenamePlaylist";
            this.menuStripRenamePlaylist.Size = new System.Drawing.Size(175, 22);
            this.menuStripRenamePlaylist.Text = "Rename";
            this.menuStripRenamePlaylist.Click += new System.EventHandler(this.menuStripRenamePlaylist_Click);
            // 
            // menuStripDeletePlaylist
            // 
            this.menuStripDeletePlaylist.Name = "menuStripDeletePlaylist";
            this.menuStripDeletePlaylist.Size = new System.Drawing.Size(175, 22);
            this.menuStripDeletePlaylist.Text = "Remove";
            this.menuStripDeletePlaylist.Click += new System.EventHandler(this.menuStripDeletePlaylist_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(172, 6);
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
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(172, 6);
            // 
            // exportToM3uToolStripMenuItem
            // 
            this.exportToM3uToolStripMenuItem.Name = "exportToM3uToolStripMenuItem";
            this.exportToM3uToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.exportToM3uToolStripMenuItem.Text = "Export to m3u";
            // 
            // exportToTxtToolStripMenuItem
            // 
            this.exportToTxtToolStripMenuItem.Name = "exportToTxtToolStripMenuItem";
            this.exportToTxtToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.exportToTxtToolStripMenuItem.Text = "Export to txt";
            // 
            // lblTrackCount
            // 
            this.lblTrackCount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTrackCount.Location = new System.Drawing.Point(997, 515);
            this.lblTrackCount.Name = "lblTrackCount";
            this.lblTrackCount.Size = new System.Drawing.Size(116, 13);
            this.lblTrackCount.TabIndex = 3;
            this.lblTrackCount.Text = "1000 item(s) in [Playlist]";
            this.lblTrackCount.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblTrackSumLength
            // 
            this.lblTrackSumLength.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTrackSumLength.Location = new System.Drawing.Point(994, 531);
            this.lblTrackSumLength.Name = "lblTrackSumLength";
            this.lblTrackSumLength.Size = new System.Drawing.Size(119, 13);
            this.lblTrackSumLength.TabIndex = 3;
            this.lblTrackSumLength.Text = "Length: 00:00:00";
            this.lblTrackSumLength.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PlaylistView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1129, 576);
            this.Controls.Add(this.tabControl1);
            this.Name = "PlaylistView";
            this.Text = "PlayerView";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.tabPagePlaylistDetail.ResumeLayout(false);
            this.tabPagePlaylistDetail.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlaylistList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrackList)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabPage tabPagePlaylistDetail;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.DataGridView dgvTrackList;
        private System.Windows.Forms.DataGridView dgvPlaylistList;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblCurrentTrack;
        private System.Windows.Forms.Label lblSelectedItemsLength;
        private System.Windows.Forms.Label lblSelectedItemsCount;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuStripLoadPlaylist;
        private System.Windows.Forms.ToolStripMenuItem menuStripRenamePlaylist;
        private System.Windows.Forms.ToolStripMenuItem menuStripDeletePlaylist;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuStripCreatePlaylist;
        private System.Windows.Forms.ToolStripMenuItem exportToM3uToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToTxtToolStripMenuItem;
        private System.Windows.Forms.Button btnSetQuickListGroup4;
        private System.Windows.Forms.Button btnSetQuickListGroup3;
        private System.Windows.Forms.Button btnSetQuickListGroup2;
        private System.Windows.Forms.Button btnRenamePlaylist;
        private System.Windows.Forms.Button btnLoadPlaylist;
        private System.Windows.Forms.Button btnNewPlaylist;
        private System.Windows.Forms.Button btnSetQuickListGroup1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnDeletePlaylist;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menuStripSetQuickListGroup;
        private System.Windows.Forms.ToolStripMenuItem menuStripSetQuickListGroup1;
        private System.Windows.Forms.ToolStripMenuItem menuStripSetQuickListGroup2;
        private System.Windows.Forms.ToolStripMenuItem menuStripSetQuickListGroup3;
        private System.Windows.Forms.ToolStripMenuItem menuStripSetQuickListGroup4;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label lblTrackCount;
        private System.Windows.Forms.Label lblTrackSumLength;
    }
}