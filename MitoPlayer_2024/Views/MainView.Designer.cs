using MitoPlayer_2024.Helpers;

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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainView));
            this.strMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripProfile = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStripOpenFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripOpenDirectories = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStripCreatePlaylist = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripLoadPlaylist = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripRenamePlaylist = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripDeletePlaylist = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStripExportToTXT = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripExportToM3U = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStripExportToDirectory = new System.Windows.Forms.ToolStripMenuItem();
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
            this.pnlMainMenu = new System.Windows.Forms.Panel();
            this.lblPeak = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.pcbMasterPeakRightBackground = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pcbMasterPeakRightColoured = new System.Windows.Forms.PictureBox();
            this.pcbMasterPeakLeftBackground = new System.Windows.Forms.PictureBox();
            this.pcbMasterPeakLeftColoured = new System.Windows.Forms.PictureBox();
            this.btnSelector = new System.Windows.Forms.Button();
            this.btnTagValues = new System.Windows.Forms.Button();
            this.btnPlaylist = new System.Windows.Forms.Button();
            this.mediaPlayer = new AxWMPLib.AxWindowsMediaPlayer();
            this.pnlMediaPlayer = new System.Windows.Forms.Panel();
            this.pnlFrequency = new System.Windows.Forms.Panel();
            this.btnPlot = new System.Windows.Forms.Button();
            this.lblCurrentTrack = new System.Windows.Forms.Label();
            this.lblTrackEnd = new System.Windows.Forms.Label();
            this.btnOpenDirectory = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.chbMute = new System.Windows.Forms.CheckBox();
            this.chbPreview = new System.Windows.Forms.CheckBox();
            this.chbShuffle = new System.Windows.Forms.CheckBox();
            this.lblTrackStart = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnPrev = new System.Windows.Forms.Button();
            this.btnPlay = new System.Windows.Forms.Button();
            this.tmrPlayer = new System.Windows.Forms.Timer(this.components);
            this.tmrPeak = new System.Windows.Forms.Timer(this.components);
            this.pnlMarkerBackground = new System.Windows.Forms.Panel();
            this.pcbMarkerGrey = new System.Windows.Forms.PictureBox();
            this.pcbMarkerRed = new System.Windows.Forms.PictureBox();
            this.prbVolume = new MitoPlayer_2024.Helpers.CustomProgressBar();
            this.prbTrackProgress = new MitoPlayer_2024.Helpers.CustomProgressBar();
            this.strMenu.SuspendLayout();
            this.pnlMainMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcbMasterPeakRightBackground)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbMasterPeakRightColoured)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbMasterPeakLeftBackground)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbMasterPeakLeftColoured)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.mediaPlayer)).BeginInit();
            this.pnlMediaPlayer.SuspendLayout();
            this.pnlMarkerBackground.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcbMarkerGrey)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbMarkerRed)).BeginInit();
            this.SuspendLayout();
            // 
            // strMenu
            // 
            this.strMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.playbackToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.strMenu.Location = new System.Drawing.Point(0, 0);
            this.strMenu.Name = "strMenu";
            this.strMenu.Size = new System.Drawing.Size(1904, 24);
            this.strMenu.TabIndex = 0;
            this.strMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuStripProfile,
            this.toolStripSeparator7,
            this.menuStripOpenFiles,
            this.menuStripOpenDirectories,
            this.toolStripSeparator2,
            this.menuStripCreatePlaylist,
            this.menuStripLoadPlaylist,
            this.menuStripRenamePlaylist,
            this.menuStripDeletePlaylist,
            this.toolStripSeparator8,
            this.menuStripExportToTXT,
            this.menuStripExportToM3U,
            this.menuStripExportToDirectory,
            this.toolStripSeparator3,
            this.menuStripPreferences,
            this.menuStripExit});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // menuStripProfile
            // 
            this.menuStripProfile.Name = "menuStripProfile";
            this.menuStripProfile.Size = new System.Drawing.Size(172, 22);
            this.menuStripProfile.Text = "Select profile";
            this.menuStripProfile.Visible = false;
            this.menuStripProfile.Click += new System.EventHandler(this.menuStripProfile_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(169, 6);
            // 
            // menuStripOpenFiles
            // 
            this.menuStripOpenFiles.Name = "menuStripOpenFiles";
            this.menuStripOpenFiles.Size = new System.Drawing.Size(172, 22);
            this.menuStripOpenFiles.Text = "Add files...";
            this.menuStripOpenFiles.Click += new System.EventHandler(this.menuStripOpenFiles_Click);
            // 
            // menuStripOpenDirectories
            // 
            this.menuStripOpenDirectories.Name = "menuStripOpenDirectories";
            this.menuStripOpenDirectories.Size = new System.Drawing.Size(172, 22);
            this.menuStripOpenDirectories.Text = "Add folder...";
            this.menuStripOpenDirectories.Click += new System.EventHandler(this.menuStripOpenDirectory_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(169, 6);
            // 
            // menuStripCreatePlaylist
            // 
            this.menuStripCreatePlaylist.Name = "menuStripCreatePlaylist";
            this.menuStripCreatePlaylist.Size = new System.Drawing.Size(172, 22);
            this.menuStripCreatePlaylist.Text = "New playlist";
            this.menuStripCreatePlaylist.Click += new System.EventHandler(this.menuStripCreatePlaylist_Click);
            // 
            // menuStripLoadPlaylist
            // 
            this.menuStripLoadPlaylist.Name = "menuStripLoadPlaylist";
            this.menuStripLoadPlaylist.Size = new System.Drawing.Size(172, 22);
            this.menuStripLoadPlaylist.Text = "Load playlist...";
            this.menuStripLoadPlaylist.Click += new System.EventHandler(this.menuStripLoadPlaylist_Click);
            // 
            // menuStripRenamePlaylist
            // 
            this.menuStripRenamePlaylist.Name = "menuStripRenamePlaylist";
            this.menuStripRenamePlaylist.Size = new System.Drawing.Size(172, 22);
            this.menuStripRenamePlaylist.Text = "Rename playlist...";
            this.menuStripRenamePlaylist.Click += new System.EventHandler(this.menuStripRenamePlaylist_Click);
            // 
            // menuStripDeletePlaylist
            // 
            this.menuStripDeletePlaylist.Name = "menuStripDeletePlaylist";
            this.menuStripDeletePlaylist.Size = new System.Drawing.Size(172, 22);
            this.menuStripDeletePlaylist.Text = "Delete playlist...";
            this.menuStripDeletePlaylist.Click += new System.EventHandler(this.menuStripDeletePlaylist_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(169, 6);
            // 
            // menuStripExportToTXT
            // 
            this.menuStripExportToTXT.Name = "menuStripExportToTXT";
            this.menuStripExportToTXT.Size = new System.Drawing.Size(172, 22);
            this.menuStripExportToTXT.Text = "Export to *.txt";
            this.menuStripExportToTXT.Click += new System.EventHandler(this.menuStripExportToTXT_Click);
            // 
            // menuStripExportToM3U
            // 
            this.menuStripExportToM3U.Name = "menuStripExportToM3U";
            this.menuStripExportToM3U.Size = new System.Drawing.Size(172, 22);
            this.menuStripExportToM3U.Text = "Export to *.m3u";
            this.menuStripExportToM3U.Click += new System.EventHandler(this.menuStripExportToM3U_Click);
            // 
            // menuStripExportToDirectory
            // 
            this.menuStripExportToDirectory.Name = "menuStripExportToDirectory";
            this.menuStripExportToDirectory.Size = new System.Drawing.Size(172, 22);
            this.menuStripExportToDirectory.Text = "Export to directory";
            this.menuStripExportToDirectory.Click += new System.EventHandler(this.menuStripExportToDirectory_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(169, 6);
            // 
            // menuStripPreferences
            // 
            this.menuStripPreferences.Name = "menuStripPreferences";
            this.menuStripPreferences.Size = new System.Drawing.Size(172, 22);
            this.menuStripPreferences.Text = "Settings";
            this.menuStripPreferences.Click += new System.EventHandler(this.menuStripSettings_Click);
            // 
            // menuStripExit
            // 
            this.menuStripExit.Name = "menuStripExit";
            this.menuStripExit.Size = new System.Drawing.Size(172, 22);
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
            this.menuStripOrderByFileName.Size = new System.Drawing.Size(169, 22);
            this.menuStripOrderByFileName.Text = "Order by filename";
            this.menuStripOrderByFileName.Click += new System.EventHandler(this.menuStripOrderByFileName_Click);
            // 
            // menuStripOrderByArtist
            // 
            this.menuStripOrderByArtist.Name = "menuStripOrderByArtist";
            this.menuStripOrderByArtist.Size = new System.Drawing.Size(169, 22);
            this.menuStripOrderByArtist.Text = "Order by artists";
            this.menuStripOrderByArtist.Click += new System.EventHandler(this.menuStripOrderByArtist_Click);
            // 
            // menuStripOrderByTitle
            // 
            this.menuStripOrderByTitle.Name = "menuStripOrderByTitle";
            this.menuStripOrderByTitle.Size = new System.Drawing.Size(169, 22);
            this.menuStripOrderByTitle.Text = "Order by title";
            this.menuStripOrderByTitle.Click += new System.EventHandler(this.menuStripOrderByTitle_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(166, 6);
            // 
            // menuStripReverse
            // 
            this.menuStripReverse.Name = "menuStripReverse";
            this.menuStripReverse.Size = new System.Drawing.Size(169, 22);
            this.menuStripReverse.Text = "Reverse";
            this.menuStripReverse.Click += new System.EventHandler(this.menuStripReverse_Click);
            // 
            // menuStripShuffle
            // 
            this.menuStripShuffle.Name = "menuStripShuffle";
            this.menuStripShuffle.Size = new System.Drawing.Size(169, 22);
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
            this.menuStripStop.Size = new System.Drawing.Size(137, 22);
            this.menuStripStop.Text = "Stop";
            this.menuStripStop.Click += new System.EventHandler(this.menuStripStop_Click);
            // 
            // menuStripPause
            // 
            this.menuStripPause.Name = "menuStripPause";
            this.menuStripPause.Size = new System.Drawing.Size(137, 22);
            this.menuStripPause.Text = "Pause";
            this.menuStripPause.Click += new System.EventHandler(this.menuStripPause_Click);
            // 
            // menuStripPlay
            // 
            this.menuStripPlay.Name = "menuStripPlay";
            this.menuStripPlay.Size = new System.Drawing.Size(137, 22);
            this.menuStripPlay.Text = "Play";
            this.menuStripPlay.Click += new System.EventHandler(this.menuStripPlay_Click);
            // 
            // menuStripPrev
            // 
            this.menuStripPrev.Name = "menuStripPrev";
            this.menuStripPrev.Size = new System.Drawing.Size(137, 22);
            this.menuStripPrev.Text = "Previous";
            this.menuStripPrev.Click += new System.EventHandler(this.menuStripPrev_Click);
            // 
            // menuStripNext
            // 
            this.menuStripNext.Name = "menuStripNext";
            this.menuStripNext.Size = new System.Drawing.Size(137, 22);
            this.menuStripNext.Text = "Next (B)";
            this.menuStripNext.Click += new System.EventHandler(this.menuStripNext_Click);
            // 
            // menuStripRandom
            // 
            this.menuStripRandom.Enabled = false;
            this.menuStripRandom.Name = "menuStripRandom";
            this.menuStripRandom.Size = new System.Drawing.Size(137, 22);
            this.menuStripRandom.Text = "Random (R)";
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
            this.menuStripAbout.Size = new System.Drawing.Size(107, 22);
            this.menuStripAbout.Text = "About";
            this.menuStripAbout.Click += new System.EventHandler(this.menuStripAbout_Click);
            // 
            // pnlMainMenu
            // 
            this.pnlMainMenu.Controls.Add(this.lblPeak);
            this.pnlMainMenu.Controls.Add(this.label2);
            this.pnlMainMenu.Controls.Add(this.pcbMasterPeakRightBackground);
            this.pnlMainMenu.Controls.Add(this.label1);
            this.pnlMainMenu.Controls.Add(this.pcbMasterPeakRightColoured);
            this.pnlMainMenu.Controls.Add(this.pcbMasterPeakLeftBackground);
            this.pnlMainMenu.Controls.Add(this.pcbMasterPeakLeftColoured);
            this.pnlMainMenu.Controls.Add(this.btnSelector);
            this.pnlMainMenu.Controls.Add(this.btnTagValues);
            this.pnlMainMenu.Controls.Add(this.btnPlaylist);
            this.pnlMainMenu.Controls.Add(this.mediaPlayer);
            this.pnlMainMenu.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlMainMenu.Location = new System.Drawing.Point(0, 24);
            this.pnlMainMenu.Name = "pnlMainMenu";
            this.pnlMainMenu.Size = new System.Drawing.Size(91, 1017);
            this.pnlMainMenu.TabIndex = 2;
            // 
            // lblPeak
            // 
            this.lblPeak.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lblPeak.Location = new System.Drawing.Point(3, 374);
            this.lblPeak.Name = "lblPeak";
            this.lblPeak.Size = new System.Drawing.Size(85, 37);
            this.lblPeak.TabIndex = 9;
            this.lblPeak.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(42, 995);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(15, 13);
            this.label2.TabIndex = 49;
            this.label2.Text = "R";
            // 
            // pcbMasterPeakRightBackground
            // 
            this.pcbMasterPeakRightBackground.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pcbMasterPeakRightBackground.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pcbMasterPeakRightBackground.Image = global::MitoPlayer_2024.Properties.Resources.MasterPeakBackground;
            this.pcbMasterPeakRightBackground.Location = new System.Drawing.Point(37, 414);
            this.pcbMasterPeakRightBackground.Name = "pcbMasterPeakRightBackground";
            this.pcbMasterPeakRightBackground.Size = new System.Drawing.Size(25, 573);
            this.pcbMasterPeakRightBackground.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pcbMasterPeakRightBackground.TabIndex = 7;
            this.pcbMasterPeakRightBackground.TabStop = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 995);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 13);
            this.label1.TabIndex = 48;
            this.label1.Text = "L";
            // 
            // pcbMasterPeakRightColoured
            // 
            this.pcbMasterPeakRightColoured.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pcbMasterPeakRightColoured.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pcbMasterPeakRightColoured.Image = global::MitoPlayer_2024.Properties.Resources.MasterPeakColoured1;
            this.pcbMasterPeakRightColoured.Location = new System.Drawing.Point(37, 414);
            this.pcbMasterPeakRightColoured.Name = "pcbMasterPeakRightColoured";
            this.pcbMasterPeakRightColoured.Size = new System.Drawing.Size(25, 573);
            this.pcbMasterPeakRightColoured.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pcbMasterPeakRightColoured.TabIndex = 8;
            this.pcbMasterPeakRightColoured.TabStop = false;
            // 
            // pcbMasterPeakLeftBackground
            // 
            this.pcbMasterPeakLeftBackground.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pcbMasterPeakLeftBackground.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pcbMasterPeakLeftBackground.Image = global::MitoPlayer_2024.Properties.Resources.MasterPeakBackground;
            this.pcbMasterPeakLeftBackground.Location = new System.Drawing.Point(6, 414);
            this.pcbMasterPeakLeftBackground.Name = "pcbMasterPeakLeftBackground";
            this.pcbMasterPeakLeftBackground.Size = new System.Drawing.Size(25, 573);
            this.pcbMasterPeakLeftBackground.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pcbMasterPeakLeftBackground.TabIndex = 7;
            this.pcbMasterPeakLeftBackground.TabStop = false;
            // 
            // pcbMasterPeakLeftColoured
            // 
            this.pcbMasterPeakLeftColoured.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pcbMasterPeakLeftColoured.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pcbMasterPeakLeftColoured.Image = global::MitoPlayer_2024.Properties.Resources.MasterPeakColoured1;
            this.pcbMasterPeakLeftColoured.Location = new System.Drawing.Point(6, 414);
            this.pcbMasterPeakLeftColoured.Name = "pcbMasterPeakLeftColoured";
            this.pcbMasterPeakLeftColoured.Size = new System.Drawing.Size(25, 573);
            this.pcbMasterPeakLeftColoured.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pcbMasterPeakLeftColoured.TabIndex = 8;
            this.pcbMasterPeakLeftColoured.TabStop = false;
            // 
            // btnSelector
            // 
            this.btnSelector.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelector.Location = new System.Drawing.Point(0, 124);
            this.btnSelector.Name = "btnSelector";
            this.btnSelector.Size = new System.Drawing.Size(91, 62);
            this.btnSelector.TabIndex = 0;
            this.btnSelector.Text = "Selector";
            this.btnSelector.UseVisualStyleBackColor = false;
            this.btnSelector.Click += new System.EventHandler(this.btnSelector_Click);
            // 
            // btnTagValues
            // 
            this.btnTagValues.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTagValues.Location = new System.Drawing.Point(0, 62);
            this.btnTagValues.Name = "btnTagValues";
            this.btnTagValues.Size = new System.Drawing.Size(91, 62);
            this.btnTagValues.TabIndex = 0;
            this.btnTagValues.Text = "Tag Values";
            this.btnTagValues.UseVisualStyleBackColor = false;
            this.btnTagValues.Click += new System.EventHandler(this.btnTagValues_Click);
            // 
            // btnPlaylist
            // 
            this.btnPlaylist.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlaylist.Location = new System.Drawing.Point(0, 0);
            this.btnPlaylist.Name = "btnPlaylist";
            this.btnPlaylist.Size = new System.Drawing.Size(91, 62);
            this.btnPlaylist.TabIndex = 0;
            this.btnPlaylist.Text = "Player";
            this.btnPlaylist.UseVisualStyleBackColor = false;
            this.btnPlaylist.Click += new System.EventHandler(this.btnPlaylist_Click);
            // 
            // mediaPlayer
            // 
            this.mediaPlayer.Enabled = true;
            this.mediaPlayer.Location = new System.Drawing.Point(65, 21);
            this.mediaPlayer.Name = "mediaPlayer";
            this.mediaPlayer.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("mediaPlayer.OcxState")));
            this.mediaPlayer.Size = new System.Drawing.Size(10, 10);
            this.mediaPlayer.TabIndex = 1;
            // 
            // pnlMediaPlayer
            // 
            this.pnlMediaPlayer.Controls.Add(this.pnlFrequency);
            this.pnlMediaPlayer.Controls.Add(this.btnPlot);
            this.pnlMediaPlayer.Controls.Add(this.prbVolume);
            this.pnlMediaPlayer.Controls.Add(this.lblCurrentTrack);
            this.pnlMediaPlayer.Controls.Add(this.lblTrackEnd);
            this.pnlMediaPlayer.Controls.Add(this.btnOpenDirectory);
            this.pnlMediaPlayer.Controls.Add(this.btnOpen);
            this.pnlMediaPlayer.Controls.Add(this.prbTrackProgress);
            this.pnlMediaPlayer.Controls.Add(this.chbMute);
            this.pnlMediaPlayer.Controls.Add(this.chbPreview);
            this.pnlMediaPlayer.Controls.Add(this.chbShuffle);
            this.pnlMediaPlayer.Controls.Add(this.lblTrackStart);
            this.pnlMediaPlayer.Controls.Add(this.btnNext);
            this.pnlMediaPlayer.Controls.Add(this.btnStop);
            this.pnlMediaPlayer.Controls.Add(this.btnPause);
            this.pnlMediaPlayer.Controls.Add(this.btnPrev);
            this.pnlMediaPlayer.Controls.Add(this.btnPlay);
            this.pnlMediaPlayer.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlMediaPlayer.Location = new System.Drawing.Point(91, 24);
            this.pnlMediaPlayer.Name = "pnlMediaPlayer";
            this.pnlMediaPlayer.Size = new System.Drawing.Size(1813, 40);
            this.pnlMediaPlayer.TabIndex = 4;
            // 
            // pnlFrequency
            // 
            this.pnlFrequency.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlFrequency.Location = new System.Drawing.Point(753, 28);
            this.pnlFrequency.Name = "pnlFrequency";
            this.pnlFrequency.Size = new System.Drawing.Size(917, 31);
            this.pnlFrequency.TabIndex = 52;
            this.pnlFrequency.Visible = false;
            // 
            // btnPlot
            // 
            this.btnPlot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPlot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlot.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnPlot.Location = new System.Drawing.Point(1753, 4);
            this.btnPlot.Name = "btnPlot";
            this.btnPlot.Size = new System.Drawing.Size(48, 34);
            this.btnPlot.TabIndex = 51;
            this.btnPlot.Text = "📊";
            this.btnPlot.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnPlot.UseVisualStyleBackColor = true;
            this.btnPlot.Visible = false;
            this.btnPlot.Click += new System.EventHandler(this.btnPlot_Click);
            // 
            // lblCurrentTrack
            // 
            this.lblCurrentTrack.Location = new System.Drawing.Point(463, 0);
            this.lblCurrentTrack.Name = "lblCurrentTrack";
            this.lblCurrentTrack.Size = new System.Drawing.Size(207, 38);
            this.lblCurrentTrack.TabIndex = 50;
            this.lblCurrentTrack.Text = "Playing: - ";
            this.lblCurrentTrack.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTrackEnd
            // 
            this.lblTrackEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTrackEnd.AutoSize = true;
            this.lblTrackEnd.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblTrackEnd.Location = new System.Drawing.Point(1730, 9);
            this.lblTrackEnd.Name = "lblTrackEnd";
            this.lblTrackEnd.Size = new System.Drawing.Size(71, 20);
            this.lblTrackEnd.TabIndex = 43;
            this.lblTrackEnd.Text = "00:00:00";
            // 
            // btnOpenDirectory
            // 
            this.btnOpenDirectory.FlatAppearance.BorderSize = 0;
            this.btnOpenDirectory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnOpenDirectory.Location = new System.Drawing.Point(183, 3);
            this.btnOpenDirectory.Margin = new System.Windows.Forms.Padding(0);
            this.btnOpenDirectory.Name = "btnOpenDirectory";
            this.btnOpenDirectory.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnOpenDirectory.Size = new System.Drawing.Size(30, 30);
            this.btnOpenDirectory.TabIndex = 35;
            this.btnOpenDirectory.Text = "🗁";
            this.btnOpenDirectory.UseVisualStyleBackColor = true;
            this.btnOpenDirectory.Click += new System.EventHandler(this.btnOpenDirectory_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.FlatAppearance.BorderSize = 0;
            this.btnOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpen.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnOpen.Location = new System.Drawing.Point(153, 3);
            this.btnOpen.Margin = new System.Windows.Forms.Padding(0);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnOpen.Size = new System.Drawing.Size(30, 30);
            this.btnOpen.TabIndex = 36;
            this.btnOpen.Text = "⏏";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // chbMute
            // 
            this.chbMute.AutoSize = true;
            this.chbMute.Location = new System.Drawing.Point(216, 12);
            this.chbMute.Name = "chbMute";
            this.chbMute.Size = new System.Drawing.Size(50, 17);
            this.chbMute.TabIndex = 47;
            this.chbMute.Text = "Mute";
            this.chbMute.UseVisualStyleBackColor = true;
            this.chbMute.CheckedChanged += new System.EventHandler(this.chbMute_CheckedChanged);
            // 
            // chbPreview
            // 
            this.chbPreview.AutoSize = true;
            this.chbPreview.Location = new System.Drawing.Point(281, 12);
            this.chbPreview.Name = "chbPreview";
            this.chbPreview.Size = new System.Drawing.Size(64, 17);
            this.chbPreview.TabIndex = 47;
            this.chbPreview.Text = "Preview";
            this.chbPreview.UseVisualStyleBackColor = true;
            this.chbPreview.CheckedChanged += new System.EventHandler(this.chbPreview_CheckedChanged);
            // 
            // chbShuffle
            // 
            this.chbShuffle.AutoSize = true;
            this.chbShuffle.Enabled = false;
            this.chbShuffle.Location = new System.Drawing.Point(216, 3);
            this.chbShuffle.Name = "chbShuffle";
            this.chbShuffle.Size = new System.Drawing.Size(59, 17);
            this.chbShuffle.TabIndex = 47;
            this.chbShuffle.Text = "Shuffle";
            this.chbShuffle.UseVisualStyleBackColor = true;
            this.chbShuffle.Visible = false;
            this.chbShuffle.CheckedChanged += new System.EventHandler(this.chbShuffle_CheckedChanged);
            // 
            // lblTrackStart
            // 
            this.lblTrackStart.AutoSize = true;
            this.lblTrackStart.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lblTrackStart.Location = new System.Drawing.Point(676, 9);
            this.lblTrackStart.Name = "lblTrackStart";
            this.lblTrackStart.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblTrackStart.Size = new System.Drawing.Size(71, 20);
            this.lblTrackStart.TabIndex = 44;
            this.lblTrackStart.Text = "00:00:00";
            this.lblTrackStart.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.lblTrackStart.Click += new System.EventHandler(this.lblTrackStart_Click);
            // 
            // btnNext
            // 
            this.btnNext.FlatAppearance.BorderSize = 0;
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNext.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnNext.Location = new System.Drawing.Point(123, 3);
            this.btnNext.Margin = new System.Windows.Forms.Padding(0);
            this.btnNext.Name = "btnNext";
            this.btnNext.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnNext.Size = new System.Drawing.Size(30, 30);
            this.btnNext.TabIndex = 37;
            this.btnNext.Text = "⏯️";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnStop
            // 
            this.btnStop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnStop.FlatAppearance.BorderSize = 0;
            this.btnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnStop.Location = new System.Drawing.Point(3, 4);
            this.btnStop.Margin = new System.Windows.Forms.Padding(0);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(30, 30);
            this.btnStop.TabIndex = 38;
            this.btnStop.Text = "⏹️";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnPause
            // 
            this.btnPause.FlatAppearance.BorderSize = 0;
            this.btnPause.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPause.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnPause.Location = new System.Drawing.Point(33, 3);
            this.btnPause.Margin = new System.Windows.Forms.Padding(0);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(30, 30);
            this.btnPause.TabIndex = 39;
            this.btnPause.Text = "⏸";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnPrev
            // 
            this.btnPrev.FlatAppearance.BorderSize = 0;
            this.btnPrev.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrev.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnPrev.Location = new System.Drawing.Point(93, 3);
            this.btnPrev.Margin = new System.Windows.Forms.Padding(0);
            this.btnPrev.Name = "btnPrev";
            this.btnPrev.Size = new System.Drawing.Size(30, 30);
            this.btnPrev.TabIndex = 40;
            this.btnPrev.Text = "⏮";
            this.btnPrev.UseVisualStyleBackColor = true;
            this.btnPrev.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnPlay
            // 
            this.btnPlay.FlatAppearance.BorderSize = 0;
            this.btnPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlay.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnPlay.Location = new System.Drawing.Point(63, 3);
            this.btnPlay.Margin = new System.Windows.Forms.Padding(0);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(30, 30);
            this.btnPlay.TabIndex = 41;
            this.btnPlay.Text = "▷";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // tmrPlayer
            // 
            this.tmrPlayer.Tick += new System.EventHandler(this.tmrPlayer_Tick);
            // 
            // tmrPeak
            // 
            this.tmrPeak.Tick += new System.EventHandler(this.tmrPeak_Tick);
            // 
            // pnlMarkerBackground
            // 
            this.pnlMarkerBackground.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.pnlMarkerBackground.Controls.Add(this.pcbMarkerGrey);
            this.pnlMarkerBackground.Controls.Add(this.pcbMarkerRed);
            this.pnlMarkerBackground.Location = new System.Drawing.Point(65, 438);
            this.pnlMarkerBackground.Name = "pnlMarkerBackground";
            this.pnlMarkerBackground.Size = new System.Drawing.Size(20, 573);
            this.pnlMarkerBackground.TabIndex = 11;
            // 
            // pcbMarkerGrey
            // 
            this.pcbMarkerGrey.Image = global::MitoPlayer_2024.Properties.Resources.MarkerGrey;
            this.pcbMarkerGrey.Location = new System.Drawing.Point(0, 300);
            this.pcbMarkerGrey.Name = "pcbMarkerGrey";
            this.pcbMarkerGrey.Size = new System.Drawing.Size(20, 6);
            this.pcbMarkerGrey.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pcbMarkerGrey.TabIndex = 12;
            this.pcbMarkerGrey.TabStop = false;
            // 
            // pcbMarkerRed
            // 
            this.pcbMarkerRed.Image = global::MitoPlayer_2024.Properties.Resources.MarkerRed;
            this.pcbMarkerRed.Location = new System.Drawing.Point(0, 300);
            this.pcbMarkerRed.Name = "pcbMarkerRed";
            this.pcbMarkerRed.Size = new System.Drawing.Size(25, 6);
            this.pcbMarkerRed.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pcbMarkerRed.TabIndex = 14;
            this.pcbMarkerRed.TabStop = false;
            // 
            // prbVolume
            // 
            this.prbVolume.Location = new System.Drawing.Point(351, 10);
            this.prbVolume.Name = "prbVolume";
            this.prbVolume.ProgressBarBackgroundColor = System.Drawing.Color.Empty;
            this.prbVolume.ProgressBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(191)))), ((int)(((byte)(128)))));
            this.prbVolume.Size = new System.Drawing.Size(107, 23);
            this.prbVolume.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.prbVolume.TabIndex = 12;
            this.prbVolume.MouseDown += new System.Windows.Forms.MouseEventHandler(this.prbVolume_MouseDown);
            // 
            // prbTrackProgress
            // 
            this.prbTrackProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.prbTrackProgress.Location = new System.Drawing.Point(753, 8);
            this.prbTrackProgress.Name = "prbTrackProgress";
            this.prbTrackProgress.ProgressBarBackgroundColor = System.Drawing.Color.Empty;
            this.prbTrackProgress.ProgressBarColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(191)))), ((int)(((byte)(128)))));
            this.prbTrackProgress.Size = new System.Drawing.Size(971, 21);
            this.prbTrackProgress.Step = 1;
            this.prbTrackProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.prbTrackProgress.TabIndex = 45;
            this.prbTrackProgress.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pBar_MouseDown);
            // 
            // MainView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1904, 1041);
            this.Controls.Add(this.pnlMarkerBackground);
            this.Controls.Add(this.pnlMediaPlayer);
            this.Controls.Add(this.pnlMainMenu);
            this.Controls.Add(this.strMenu);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.strMenu;
            this.MinimumSize = new System.Drawing.Size(1280, 720);
            this.Name = "MainView";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MitoPlayer 2024 v0.16.0.8";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainView_Load);
            this.strMenu.ResumeLayout(false);
            this.strMenu.PerformLayout();
            this.pnlMainMenu.ResumeLayout(false);
            this.pnlMainMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcbMasterPeakRightBackground)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbMasterPeakRightColoured)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbMasterPeakLeftBackground)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbMasterPeakLeftColoured)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.mediaPlayer)).EndInit();
            this.pnlMediaPlayer.ResumeLayout(false);
            this.pnlMediaPlayer.PerformLayout();
            this.pnlMarkerBackground.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pcbMarkerGrey)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcbMarkerRed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip strMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
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
        private System.Windows.Forms.Panel pnlMainMenu;
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
        private System.Windows.Forms.Button btnSelector;
        public AxWMPLib.AxWindowsMediaPlayer mediaPlayer;
        private System.Windows.Forms.Panel pnlMediaPlayer;
        private System.Windows.Forms.Label lblTrackEnd;
        private System.Windows.Forms.Label lblTrackStart;
        private System.Windows.Forms.Button btnOpenDirectory;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnPrev;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.ToolStripMenuItem menuStripProfile;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem menuStripOpenFiles;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem menuStripExportToTXT;
        private System.Windows.Forms.ToolStripMenuItem menuStripExportToM3U;
        private System.Windows.Forms.ToolStripMenuItem menuStripExportToDirectory;
        private CustomProgressBar prbTrackProgress;
        private System.Windows.Forms.CheckBox chbShuffle;
        private System.Windows.Forms.CheckBox chbMute;
        private System.Windows.Forms.Timer tmrPlayer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pcbMasterPeakLeftBackground;
        private System.Windows.Forms.PictureBox pcbMasterPeakLeftColoured;
        private System.Windows.Forms.PictureBox pcbMasterPeakRightBackground;
        private System.Windows.Forms.PictureBox pcbMasterPeakRightColoured;
        private System.Windows.Forms.Label lblCurrentTrack;
        private System.Windows.Forms.Label lblPeak;
        private System.Windows.Forms.Timer tmrPeak;
        private System.Windows.Forms.PictureBox pcbMarkerGrey;
        private System.Windows.Forms.Panel pnlMarkerBackground;
        private System.Windows.Forms.PictureBox pcbMarkerRed;
        private CustomProgressBar prbVolume;
        private System.Windows.Forms.CheckBox chbPreview;
        private System.Windows.Forms.Button btnPlot;
        private System.Windows.Forms.Panel pnlFrequency;
    }
}

