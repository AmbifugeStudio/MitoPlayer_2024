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
            this.tabPagePlaylistDetail = new System.Windows.Forms.TabPage();
            this.debug = new System.Windows.Forms.RichTextBox();
            this.lblSelectedItemsLength = new System.Windows.Forms.Label();
            this.lblSelectedItemsCount = new System.Windows.Forms.Label();
            this.lblCurrentTrack = new System.Windows.Forms.Label();
            this.dgvPlaylistList = new System.Windows.Forms.DataGridView();
            this.dgvTrackList = new System.Windows.Forms.DataGridView();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.panel1 = new System.Windows.Forms.Panel();
            this.trackVolume = new System.Windows.Forms.TrackBar();
            this.pBar = new System.Windows.Forms.ProgressBar();
            this.lblVolume = new System.Windows.Forms.Label();
            this.lblTrackEnd = new System.Windows.Forms.Label();
            this.lblTrackStart = new System.Windows.Forms.Label();
            this.btnOpenDirectory = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnPlay = new System.Windows.Forms.Button();
            this.mediaPlayer = new AxWMPLib.AxWindowsMediaPlayer();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.playToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToM3uToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportToTxtToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabPagePlaylistDetail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlaylistList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrackList)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mediaPlayer)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPagePlaylistDetail
            // 
            this.tabPagePlaylistDetail.Controls.Add(this.debug);
            this.tabPagePlaylistDetail.Controls.Add(this.lblSelectedItemsLength);
            this.tabPagePlaylistDetail.Controls.Add(this.lblSelectedItemsCount);
            this.tabPagePlaylistDetail.Controls.Add(this.lblCurrentTrack);
            this.tabPagePlaylistDetail.Controls.Add(this.dgvPlaylistList);
            this.tabPagePlaylistDetail.Controls.Add(this.dgvTrackList);
            this.tabPagePlaylistDetail.Location = new System.Drawing.Point(4, 22);
            this.tabPagePlaylistDetail.Name = "tabPagePlaylistDetail";
            this.tabPagePlaylistDetail.Padding = new System.Windows.Forms.Padding(3);
            this.tabPagePlaylistDetail.Size = new System.Drawing.Size(1121, 487);
            this.tabPagePlaylistDetail.TabIndex = 1;
            this.tabPagePlaylistDetail.Text = "Player";
            this.tabPagePlaylistDetail.UseVisualStyleBackColor = true;
            // 
            // debug
            // 
            this.debug.Location = new System.Drawing.Point(961, 6);
            this.debug.Name = "debug";
            this.debug.Size = new System.Drawing.Size(152, 460);
            this.debug.TabIndex = 4;
            this.debug.Text = "";
            // 
            // lblSelectedItemsLength
            // 
            this.lblSelectedItemsLength.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSelectedItemsLength.AutoSize = true;
            this.lblSelectedItemsLength.Location = new System.Drawing.Point(6, 469);
            this.lblSelectedItemsLength.Name = "lblSelectedItemsLength";
            this.lblSelectedItemsLength.Size = new System.Drawing.Size(43, 13);
            this.lblSelectedItemsLength.TabIndex = 3;
            this.lblSelectedItemsLength.Text = "Length:";
            // 
            // lblSelectedItemsCount
            // 
            this.lblSelectedItemsCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSelectedItemsCount.AutoSize = true;
            this.lblSelectedItemsCount.Location = new System.Drawing.Point(6, 453);
            this.lblSelectedItemsCount.Name = "lblSelectedItemsCount";
            this.lblSelectedItemsCount.Size = new System.Drawing.Size(80, 13);
            this.lblSelectedItemsCount.TabIndex = 3;
            this.lblSelectedItemsCount.Text = "- items selected";
            // 
            // lblCurrentTrack
            // 
            this.lblCurrentTrack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCurrentTrack.AutoSize = true;
            this.lblCurrentTrack.Location = new System.Drawing.Point(170, 469);
            this.lblCurrentTrack.Name = "lblCurrentTrack";
            this.lblCurrentTrack.Size = new System.Drawing.Size(50, 13);
            this.lblCurrentTrack.TabIndex = 2;
            this.lblCurrentTrack.Text = "Playing: -";
            // 
            // dgvPlaylistList
            // 
            this.dgvPlaylistList.AllowUserToAddRows = false;
            this.dgvPlaylistList.AllowUserToDeleteRows = false;
            this.dgvPlaylistList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgvPlaylistList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPlaylistList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPlaylistList.Location = new System.Drawing.Point(3, 3);
            this.dgvPlaylistList.MultiSelect = false;
            this.dgvPlaylistList.Name = "dgvPlaylistList";
            this.dgvPlaylistList.ReadOnly = true;
            this.dgvPlaylistList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPlaylistList.Size = new System.Drawing.Size(164, 447);
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
            this.dgvTrackList.Location = new System.Drawing.Point(173, 6);
            this.dgvTrackList.Name = "dgvTrackList";
            this.dgvTrackList.ReadOnly = true;
            this.dgvTrackList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTrackList.Size = new System.Drawing.Size(782, 460);
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
            this.tabControl1.Location = new System.Drawing.Point(0, 63);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1129, 513);
            this.tabControl1.TabIndex = 4;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Controls.Add(this.trackVolume);
            this.panel1.Controls.Add(this.pBar);
            this.panel1.Controls.Add(this.lblVolume);
            this.panel1.Controls.Add(this.lblTrackEnd);
            this.panel1.Controls.Add(this.lblTrackStart);
            this.panel1.Controls.Add(this.btnOpenDirectory);
            this.panel1.Controls.Add(this.btnOpen);
            this.panel1.Controls.Add(this.btnNext);
            this.panel1.Controls.Add(this.btnStop);
            this.panel1.Controls.Add(this.btnPause);
            this.panel1.Controls.Add(this.btnPrev);
            this.panel1.Controls.Add(this.btnPlay);
            this.panel1.Controls.Add(this.mediaPlayer);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1129, 63);
            this.panel1.TabIndex = 3;
            // 
            // trackVolume
            // 
            this.trackVolume.Location = new System.Drawing.Point(872, 10);
            this.trackVolume.Maximum = 100;
            this.trackVolume.Name = "trackVolume";
            this.trackVolume.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.trackVolume.Size = new System.Drawing.Size(139, 45);
            this.trackVolume.TabIndex = 34;
            this.trackVolume.TickFrequency = 10;
            this.trackVolume.Scroll += new System.EventHandler(this.trackVolume_Scroll);
            // 
            // pBar
            // 
            this.pBar.Location = new System.Drawing.Point(415, 19);
            this.pBar.Name = "pBar";
            this.pBar.Size = new System.Drawing.Size(365, 23);
            this.pBar.Step = 1;
            this.pBar.TabIndex = 33;
            this.pBar.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pBar_MouseDown);
            // 
            // lblVolume
            // 
            this.lblVolume.AutoSize = true;
            this.lblVolume.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblVolume.Location = new System.Drawing.Point(1017, 22);
            this.lblVolume.Name = "lblVolume";
            this.lblVolume.Size = new System.Drawing.Size(50, 20);
            this.lblVolume.TabIndex = 30;
            this.lblVolume.Text = "100%";
            // 
            // lblTrackEnd
            // 
            this.lblTrackEnd.AutoSize = true;
            this.lblTrackEnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblTrackEnd.Location = new System.Drawing.Point(786, 19);
            this.lblTrackEnd.Name = "lblTrackEnd";
            this.lblTrackEnd.Size = new System.Drawing.Size(80, 24);
            this.lblTrackEnd.TabIndex = 31;
            this.lblTrackEnd.Text = "00:00:00";
            // 
            // lblTrackStart
            // 
            this.lblTrackStart.AutoSize = true;
            this.lblTrackStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblTrackStart.Location = new System.Drawing.Point(329, 19);
            this.lblTrackStart.Name = "lblTrackStart";
            this.lblTrackStart.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblTrackStart.Size = new System.Drawing.Size(80, 24);
            this.lblTrackStart.TabIndex = 32;
            this.lblTrackStart.Text = "00:00:00";
            this.lblTrackStart.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblTrackStart.Click += new System.EventHandler(this.lblTrackStart_Click);
            // 
            // btnOpenDirectory
            // 
            this.btnOpenDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnOpenDirectory.Location = new System.Drawing.Point(283, 12);
            this.btnOpenDirectory.Name = "btnOpenDirectory";
            this.btnOpenDirectory.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnOpenDirectory.Size = new System.Drawing.Size(40, 40);
            this.btnOpenDirectory.TabIndex = 23;
            this.btnOpenDirectory.Text = "🗁";
            this.btnOpenDirectory.UseVisualStyleBackColor = true;
            this.btnOpenDirectory.Click += new System.EventHandler(this.btnOpenDirectory_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnOpen.Location = new System.Drawing.Point(237, 12);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnOpen.Size = new System.Drawing.Size(40, 40);
            this.btnOpen.TabIndex = 24;
            this.btnOpen.Text = "⏏";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnNext
            // 
            this.btnNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnNext.Location = new System.Drawing.Point(191, 12);
            this.btnNext.Name = "btnNext";
            this.btnNext.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnNext.Size = new System.Drawing.Size(40, 40);
            this.btnNext.TabIndex = 25;
            this.btnNext.Text = "⏯️";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnStop
            // 
            this.btnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnStop.Location = new System.Drawing.Point(7, 12);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(40, 40);
            this.btnStop.TabIndex = 26;
            this.btnStop.Text = "⏹️";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnPause
            // 
            this.btnPause.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnPause.Location = new System.Drawing.Point(53, 12);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(40, 40);
            this.btnPause.TabIndex = 27;
            this.btnPause.Text = "⏸";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnPrev.Location = new System.Drawing.Point(145, 12);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(40, 40);
            this.btnPrev.TabIndex = 28;
            this.btnPrev.Text = "⏮";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnPlay
            // 
            this.btnPlay.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnPlay.Location = new System.Drawing.Point(99, 12);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(40, 40);
            this.btnPlay.TabIndex = 29;
            this.btnPlay.Text = "▶";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // mediaPlayer
            // 
            this.mediaPlayer.Enabled = true;
            this.mediaPlayer.Location = new System.Drawing.Point(299, 32);
            this.mediaPlayer.Name = "mediaPlayer";
            this.mediaPlayer.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("mediaPlayer.OcxState")));
            this.mediaPlayer.Size = new System.Drawing.Size(10, 10);
            this.mediaPlayer.TabIndex = 35;
            this.mediaPlayer.Enter += new System.EventHandler(this.mediaPlayer_Enter);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.playToolStripMenuItem,
            this.renameToolStripMenuItem,
            this.removeStripMenuItem,
            this.toolStripSeparator1,
            this.newToolStripMenuItem,
            this.exportToM3uToolStripMenuItem,
            this.exportToTxtToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(150, 142);
            // 
            // playToolStripMenuItem
            // 
            this.playToolStripMenuItem.Name = "playToolStripMenuItem";
            this.playToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.playToolStripMenuItem.Text = "Play";
            // 
            // renameToolStripMenuItem
            // 
            this.renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            this.renameToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.renameToolStripMenuItem.Text = "Rename";
            this.renameToolStripMenuItem.Click += new System.EventHandler(this.renameToolStripMenuItem_Click);
            // 
            // removeStripMenuItem
            // 
            this.removeStripMenuItem.Name = "removeStripMenuItem";
            this.removeStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.removeStripMenuItem.Text = "Remove";
            this.removeStripMenuItem.Click += new System.EventHandler(this.removeStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(146, 6);
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // exportToM3uToolStripMenuItem
            // 
            this.exportToM3uToolStripMenuItem.Name = "exportToM3uToolStripMenuItem";
            this.exportToM3uToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.exportToM3uToolStripMenuItem.Text = "Export to m3u";
            // 
            // exportToTxtToolStripMenuItem
            // 
            this.exportToTxtToolStripMenuItem.Name = "exportToTxtToolStripMenuItem";
            this.exportToTxtToolStripMenuItem.Size = new System.Drawing.Size(149, 22);
            this.exportToTxtToolStripMenuItem.Text = "Export to txt";
            // 
            // PlaylistView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1129, 576);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.Name = "PlaylistView";
            this.Text = "PlayerView";
            this.tabPagePlaylistDetail.ResumeLayout(false);
            this.tabPagePlaylistDetail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlaylistList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTrackList)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mediaPlayer)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabPage tabPagePlaylistDetail;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dgvTrackList;
        private System.Windows.Forms.DataGridView dgvPlaylistList;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblCurrentTrack;
        private System.Windows.Forms.Label lblSelectedItemsLength;
        private System.Windows.Forms.Label lblSelectedItemsCount;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem playToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem renameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToM3uToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToTxtToolStripMenuItem;
        private System.Windows.Forms.TrackBar trackVolume;
        private System.Windows.Forms.ProgressBar pBar;
        private System.Windows.Forms.Label lblVolume;
        private System.Windows.Forms.Label lblTrackEnd;
        private System.Windows.Forms.Label lblTrackStart;
        private System.Windows.Forms.Button btnOpenDirectory;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnPlay;
        private AxWMPLib.AxWindowsMediaPlayer mediaPlayer;
        private System.Windows.Forms.RichTextBox debug;
    }
}