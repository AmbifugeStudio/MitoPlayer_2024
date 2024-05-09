namespace MitoPlayer_2024
{
    partial class MainView
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripOpenFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripOpenDirectories = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStripCreatePlaylist = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripLoadPlaylist = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripRenamePlaylist = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripDeletePlaylist = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStripPreferences = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripExit = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStripRemoveMissingTracks = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripRemoveDuplicatedTracks = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripOrderByFileName = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripOrderByArtist = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripOrderByTitle = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStripReverse = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripShuffle = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStripClear = new System.Windows.Forms.ToolStripMenuItem();
            this.playbackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripStop = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripPause = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripPlay = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripPrev = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripNext = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripRandom = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnTagValues = new System.Windows.Forms.Button();
            this.btnPlaylist = new System.Windows.Forms.Button();
            this.btnTracks = new System.Windows.Forms.Button();
            this.btnRules = new System.Windows.Forms.Button();
            this.btnTemplates = new System.Windows.Forms.Button();
            this.btnHarmonizer = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.playbackToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1160, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuStripOpenFiles,
            this.menuStripOpenDirectories,
            this.toolStripSeparator2,
            this.menuStripCreatePlaylist,
            this.menuStripLoadPlaylist,
            this.menuStripRenamePlaylist,
            this.menuStripDeletePlaylist,
            this.toolStripSeparator3,
            this.menuStripPreferences,
            this.menuStripExit});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // menuStripOpenFiles
            // 
            this.menuStripOpenFiles.Name = "menuStripOpenFiles";
            this.menuStripOpenFiles.Size = new System.Drawing.Size(180, 22);
            this.menuStripOpenFiles.Text = "Add files...";
            this.menuStripOpenFiles.Click += new System.EventHandler(this.menuStripOpenFiles_Click);
            // 
            // menuStripOpenDirectories
            // 
            this.menuStripOpenDirectories.Name = "menuStripOpenDirectory";
            this.menuStripOpenDirectories.Size = new System.Drawing.Size(180, 22);
            this.menuStripOpenDirectories.Text = "Add folder...";
            this.menuStripOpenDirectories.Click += new System.EventHandler(this.menuStripOpenDirectory_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(177, 6);
            // 
            // menuStripCreatePlaylist
            // 
            this.menuStripCreatePlaylist.Name = "menuStripCreatePlaylist";
            this.menuStripCreatePlaylist.Size = new System.Drawing.Size(180, 22);
            this.menuStripCreatePlaylist.Text = "New playlist";
            this.menuStripCreatePlaylist.Click += new System.EventHandler(this.menuStripCreatePlaylist_Click);
            // 
            // menuStripLoadPlaylist
            // 
            this.menuStripLoadPlaylist.Name = "menuStripLoadPlaylist";
            this.menuStripLoadPlaylist.Size = new System.Drawing.Size(180, 22);
            this.menuStripLoadPlaylist.Text = "Load playlist...";
            this.menuStripLoadPlaylist.Click += new System.EventHandler(this.menuStripLoadPlaylist_Click);
            // 
            // menuStripRenamePlaylist
            // 
            this.menuStripRenamePlaylist.Name = "menuStripRenamePlaylist";
            this.menuStripRenamePlaylist.Size = new System.Drawing.Size(180, 22);
            this.menuStripRenamePlaylist.Text = "Rename playlist...";
            this.menuStripRenamePlaylist.Click += new System.EventHandler(this.menuStripRenamePlaylist_Click);
            // 
            // menuStripDeletePlaylist
            // 
            this.menuStripDeletePlaylist.Name = "menuStripDeletePlaylist";
            this.menuStripDeletePlaylist.Size = new System.Drawing.Size(180, 22);
            this.menuStripDeletePlaylist.Text = "Delete playlist...";
            this.menuStripDeletePlaylist.Click += new System.EventHandler(this.menuStripDeletePlaylist_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(177, 6);
            // 
            // menuStripPreferences
            // 
            this.menuStripPreferences.Name = "menuStripPreferences";
            this.menuStripPreferences.Size = new System.Drawing.Size(180, 22);
            this.menuStripPreferences.Text = "Preferences";
            this.menuStripPreferences.Click += new System.EventHandler(this.menuStripPreferences_Click);
            // 
            // menuStripExit
            // 
            this.menuStripExit.Name = "menuStripExit";
            this.menuStripExit.Size = new System.Drawing.Size(180, 22);
            this.menuStripExit.Text = "Exit";
            this.menuStripExit.Click += new System.EventHandler(this.menuStripExit_Click);
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.menuStripRemoveMissingTracks,
            this.menuStripRemoveDuplicatedTracks,
            this.toolStripSeparator6,
            this.toolStripMenuItem1,
            this.toolStripSeparator5,
            this.menuStripClear});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(192, 6);
            // 
            // menuStripRemoveMissingTracks
            // 
            this.menuStripRemoveMissingTracks.Name = "menuStripRemoveMissingTracks";
            this.menuStripRemoveMissingTracks.Size = new System.Drawing.Size(195, 22);
            this.menuStripRemoveMissingTracks.Text = "Remove missing tracks";
            this.menuStripRemoveMissingTracks.Click += new System.EventHandler(this.menuStripRemoveMissingTracks_Click);
            // 
            // menuStripRemoveDuplicatedTracks
            // 
            this.menuStripRemoveDuplicatedTracks.Name = "menuStripRemoveDuplicatedTracks";
            this.menuStripRemoveDuplicatedTracks.Size = new System.Drawing.Size(195, 22);
            this.menuStripRemoveDuplicatedTracks.Text = "Remove duplicates";
            this.menuStripRemoveDuplicatedTracks.Click += new System.EventHandler(this.menuStripRemoveDuplicatedTracks_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(192, 6);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuStripOrderByFileName,
            this.menuStripOrderByArtist,
            this.menuStripOrderByTitle,
            this.toolStripSeparator4,
            this.menuStripReverse,
            this.menuStripShuffle});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(195, 22);
            this.toolStripMenuItem1.Text = "Order";
            // 
            // menuStripOrderByFileName
            // 
            this.menuStripOrderByFileName.Name = "menuStripOrderByFileName";
            this.menuStripOrderByFileName.Size = new System.Drawing.Size(180, 22);
            this.menuStripOrderByFileName.Text = "Order by filename";
            this.menuStripOrderByFileName.Click += new System.EventHandler(this.menuStripOrderByFileName_Click);
            // 
            // menuStripOrderByArtist
            // 
            this.menuStripOrderByArtist.Name = "menuStripOrderByArtist";
            this.menuStripOrderByArtist.Size = new System.Drawing.Size(180, 22);
            this.menuStripOrderByArtist.Text = "Order by artists";
            this.menuStripOrderByArtist.Click += new System.EventHandler(this.menuStripOrderByArtist_Click);
            // 
            // menuStripOrderByTitle
            // 
            this.menuStripOrderByTitle.Name = "menuStripOrderByTitle";
            this.menuStripOrderByTitle.Size = new System.Drawing.Size(180, 22);
            this.menuStripOrderByTitle.Text = "Order by title";
            this.menuStripOrderByTitle.Click += new System.EventHandler(this.menuStripOrderByTitle_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(177, 6);
            // 
            // menuStripReverse
            // 
            this.menuStripReverse.Name = "menuStripReverse";
            this.menuStripReverse.Size = new System.Drawing.Size(180, 22);
            this.menuStripReverse.Text = "Reverse";
            this.menuStripReverse.Click += new System.EventHandler(this.menuStripReverse_Click);
            // 
            // menuStripShuffle
            // 
            this.menuStripShuffle.Name = "menuStripShuffle";
            this.menuStripShuffle.Size = new System.Drawing.Size(180, 22);
            this.menuStripShuffle.Text = "Shuffle";
            this.menuStripShuffle.Click += new System.EventHandler(this.menuStripShuffle_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(192, 6);
            // 
            // menuStripClear
            // 
            this.menuStripClear.Name = "menuStripClear";
            this.menuStripClear.Size = new System.Drawing.Size(195, 22);
            this.menuStripClear.Text = "Clear";
            this.menuStripClear.Click += new System.EventHandler(this.menuStripClear_Click);
            // 
            // playbackToolStripMenuItem
            // 
            this.playbackToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuStripStop,
            this.menuStripPause,
            this.menuStripPlay,
            this.menuStripPrev,
            this.menuStripNext,
            this.menuStripRandom});
            this.playbackToolStripMenuItem.Name = "playbackToolStripMenuItem";
            this.playbackToolStripMenuItem.Size = new System.Drawing.Size(66, 20);
            this.playbackToolStripMenuItem.Text = "Playback";
            // 
            // menuStripStop
            // 
            this.menuStripStop.Name = "menuStripStop";
            this.menuStripStop.Size = new System.Drawing.Size(180, 22);
            this.menuStripStop.Text = "Stop";
            this.menuStripStop.Click += new System.EventHandler(this.menuStripStop_Click);
            // 
            // menuStripPause
            // 
            this.menuStripPause.Name = "menuStripPause";
            this.menuStripPause.Size = new System.Drawing.Size(180, 22);
            this.menuStripPause.Text = "Pause";
            this.menuStripPause.Click += new System.EventHandler(this.menuStripPause_Click);
            // 
            // menuStripPlay
            // 
            this.menuStripPlay.Name = "menuStripPlay";
            this.menuStripPlay.Size = new System.Drawing.Size(180, 22);
            this.menuStripPlay.Text = "Play";
            this.menuStripPlay.Click += new System.EventHandler(this.menuStripPlay_Click);
            // 
            // menuStripPrev
            // 
            this.menuStripPrev.Name = "menuStripPrev";
            this.menuStripPrev.Size = new System.Drawing.Size(180, 22);
            this.menuStripPrev.Text = "Previous";
            this.menuStripPrev.Click += new System.EventHandler(this.menuStripPrev_Click);
            // 
            // menuStripNext
            // 
            this.menuStripNext.Name = "menuStripNext";
            this.menuStripNext.Size = new System.Drawing.Size(180, 22);
            this.menuStripNext.Text = "Next";
            this.menuStripNext.Click += new System.EventHandler(this.menuStripNext_Click);
            // 
            // menuStripRandom
            // 
            this.menuStripRandom.Name = "menuStripRandom";
            this.menuStripRandom.Size = new System.Drawing.Size(180, 22);
            this.menuStripRandom.Text = "Random";
            this.menuStripRandom.Click += new System.EventHandler(this.menuStripRandom_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuStripAbout});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // menuStripAbout
            // 
            this.menuStripAbout.Name = "menuStripAbout";
            this.menuStripAbout.Size = new System.Drawing.Size(180, 22);
            this.menuStripAbout.Text = "About";
            this.menuStripAbout.Click += new System.EventHandler(this.menuStripAbout_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnHarmonizer);
            this.panel2.Controls.Add(this.btnTemplates);
            this.panel2.Controls.Add(this.btnRules);
            this.panel2.Controls.Add(this.btnTracks);
            this.panel2.Controls.Add(this.btnTagValues);
            this.panel2.Controls.Add(this.btnPlaylist);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 24);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(91, 458);
            this.panel2.TabIndex = 2;
            // 
            // btnTagValues
            // 
            this.btnTagValues.Location = new System.Drawing.Point(0, 59);
            this.btnTagValues.Name = "btnTagValues";
            this.btnTagValues.Size = new System.Drawing.Size(91, 62);
            this.btnTagValues.TabIndex = 0;
            this.btnTagValues.Text = "Tag Values";
            this.btnTagValues.UseVisualStyleBackColor = true;
            this.btnTagValues.Click += new System.EventHandler(this.btnTagValues_Click);
            // 
            // btnPlaylist
            // 
            this.btnPlaylist.Location = new System.Drawing.Point(0, 0);
            this.btnPlaylist.Name = "btnPlaylist";
            this.btnPlaylist.Size = new System.Drawing.Size(91, 62);
            this.btnPlaylist.TabIndex = 0;
            this.btnPlaylist.Text = "Player";
            this.btnPlaylist.UseVisualStyleBackColor = true;
            this.btnPlaylist.Click += new System.EventHandler(this.btnPlaylist_Click);
            // 
            // btnTracks
            // 
            this.btnTracks.Location = new System.Drawing.Point(0, 118);
            this.btnTracks.Name = "btnTracks";
            this.btnTracks.Size = new System.Drawing.Size(91, 62);
            this.btnTracks.TabIndex = 0;
            this.btnTracks.Text = "Tracks";
            this.btnTracks.UseVisualStyleBackColor = true;
            this.btnTracks.Click += new System.EventHandler(this.btnTracks_Click);
            // 
            // btnRules
            // 
            this.btnRules.Location = new System.Drawing.Point(0, 176);
            this.btnRules.Name = "btnRules";
            this.btnRules.Size = new System.Drawing.Size(91, 62);
            this.btnRules.TabIndex = 0;
            this.btnRules.Text = "Rules";
            this.btnRules.UseVisualStyleBackColor = true;
            this.btnRules.Click += new System.EventHandler(this.btnRules_Click);
            // 
            // btnTemplates
            // 
            this.btnTemplates.Location = new System.Drawing.Point(0, 234);
            this.btnTemplates.Name = "btnTemplates";
            this.btnTemplates.Size = new System.Drawing.Size(91, 62);
            this.btnTemplates.TabIndex = 0;
            this.btnTemplates.Text = "Templates";
            this.btnTemplates.UseVisualStyleBackColor = true;
            this.btnTemplates.Click += new System.EventHandler(this.btnTemplates_Click);
            // 
            // btnHarmonizer
            // 
            this.btnHarmonizer.Location = new System.Drawing.Point(0, 292);
            this.btnHarmonizer.Name = "btnHarmonizer";
            this.btnHarmonizer.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnHarmonizer.Size = new System.Drawing.Size(91, 62);
            this.btnHarmonizer.TabIndex = 0;
            this.btnHarmonizer.Text = "HARMONIZER";
            this.btnHarmonizer.UseVisualStyleBackColor = true;
            this.btnHarmonizer.Click += new System.EventHandler(this.btnHarmonizer_Click);
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1160, 482);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MitoPlayer 2024 v0.1.3";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuStripOpenFiles;
        private System.Windows.Forms.ToolStripMenuItem menuStripOpenDirectories;
        private System.Windows.Forms.ToolStripMenuItem menuStripCreatePlaylist;
        private System.Windows.Forms.ToolStripMenuItem menuStripLoadPlaylist;
        private System.Windows.Forms.ToolStripMenuItem menuStripRenamePlaylist;
        private System.Windows.Forms.ToolStripMenuItem menuStripPreferences;
        private System.Windows.Forms.ToolStripMenuItem menuStripExit;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuStripRemoveDuplicatedTracks;
        private System.Windows.Forms.ToolStripMenuItem menuStripRemoveMissingTracks;
        private System.Windows.Forms.ToolStripMenuItem playbackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuStripStop;
        private System.Windows.Forms.ToolStripMenuItem menuStripPause;
        private System.Windows.Forms.ToolStripMenuItem menuStripPlay;
        private System.Windows.Forms.ToolStripMenuItem menuStripPrev;
        private System.Windows.Forms.ToolStripMenuItem menuStripNext;
        private System.Windows.Forms.ToolStripMenuItem menuStripRandom;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuStripAbout;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnPlaylist;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem menuStripOrderByFileName;
        private System.Windows.Forms.ToolStripMenuItem menuStripOrderByArtist;
        private System.Windows.Forms.ToolStripMenuItem menuStripOrderByTitle;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem menuStripReverse;
        private System.Windows.Forms.ToolStripMenuItem menuStripShuffle;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem menuStripClear;
        private System.Windows.Forms.Button btnTagValues;
        private System.Windows.Forms.ToolStripMenuItem menuStripDeletePlaylist;
        private System.Windows.Forms.Button btnTemplates;
        private System.Windows.Forms.Button btnRules;
        private System.Windows.Forms.Button btnTracks;
        private System.Windows.Forms.Button btnHarmonizer;
    }
}

