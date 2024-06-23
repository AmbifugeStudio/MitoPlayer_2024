using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Views;
using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace MitoPlayer_2024
{
    public partial class MainView : Form, IMainView
    {
        public MainView()
        {
            InitializeComponent();
        }

        public event EventHandler ShowProfileEditorView;
        public event EventHandler ShowPlaylistView;
        public event EventHandler ShowTagValueEditorView;
        public event EventHandler ShowRuleEditorView;
        public event EventHandler ShowTrackEditorView;
        public event EventHandler ShowTemplateEditorView;
        public event EventHandler ShowHarmonizerView;
        public event EventHandler ShowPreferencesView;
        public event EventHandler ShowAboutView;

        public event EventHandler OpenFiles;
        public event EventHandler OpenDirectory;
        public event EventHandler CreatePlaylist;
        public event EventHandler LoadPlaylist;
        public event EventHandler RenamePlaylist;
        public event EventHandler DeletePlaylist;
        public event EventHandler ExportToM3U;
        public event EventHandler ExportToTXT;
        public event EventHandler Preferences;
        public event EventHandler Exit;

        public event EventHandler RemoveMissingTracks;
        public event EventHandler RemoveDuplicatedTracks;
        public event EventHandler OrderByTitle;
        public event EventHandler OrderByArtist;
        public event EventHandler OrderByFileName;
        public event EventHandler Reverse;
        public event EventHandler Shuffle;
        public event EventHandler Clear;

        public event EventHandler PlayTrack;
        public event EventHandler PauseTrack;
        public event EventHandler StopTrack;
        public event EventHandler PrevTrack;
        public event EventHandler NextTrack;
        public event EventHandler RandomTrack;
        public event EventHandler<ListEventArgs> ChangeProgress;
        public event EventHandler<ListEventArgs> ChangeVolume;

        public event EventHandler About;

        public event EventHandler<ListEventArgs> ScanFiles;

        private void MainView_Load(object sender, EventArgs e)
        {
            String version = "";
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
                version = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            this.Text = "MitoPlayer 2024 v" + version;
        }

        //MENU BUTTONS
        private void menuStripProfile_Click(object sender, EventArgs e)
        {
            this.ShowProfileEditorView?.Invoke(this, EventArgs.Empty);
        }
        private void btnPlaylist_Click(object sender, EventArgs e)
        {
            this.ShowPlaylistView?.Invoke(this, EventArgs.Empty);
        }
        private void btnTagValues_Click(object sender, EventArgs e)
        {
            this.ShowTagValueEditorView?.Invoke(this, EventArgs.Empty);
        }
        private void btnTracks_Click(object sender, EventArgs e)
        {
            this.ShowTrackEditorView?.Invoke(this, EventArgs.Empty);
        }
        private void btnRules_Click(object sender, EventArgs e)
        {
            this.ShowRuleEditorView?.Invoke(this, EventArgs.Empty);
        }
        private void btnTemplates_Click(object sender, EventArgs e)
        {

            this.ShowTemplateEditorView?.Invoke(this, EventArgs.Empty);
        }
        private void btnHarmonizer_Click(object sender, EventArgs e)
        {
            this.ShowHarmonizerView?.Invoke(this, EventArgs.Empty);
        }

        //MENU STRIP
        //FILE
        private void menuStripOpenFiles_Click(object sender, EventArgs e)
        {
            this.OpenFiles?.Invoke(this, EventArgs.Empty);
        }
        private void menuStripOpenDirectory_Click(object sender, EventArgs e)
        {
            this.OpenDirectory?.Invoke(this, EventArgs.Empty);
        }
        private void menuStripCreatePlaylist_Click(object sender, EventArgs e)
        {
            this.CreatePlaylist?.Invoke(this, EventArgs.Empty);
        }
        private void menuStripLoadPlaylist_Click(object sender, EventArgs e)
        {
            this.LoadPlaylist?.Invoke(this, EventArgs.Empty);
        }
        private void menuStripRenamePlaylist_Click(object sender, EventArgs e)
        {
            this.RenamePlaylist?.Invoke(this, EventArgs.Empty);
        }
        private void menuStripDeletePlaylist_Click(object sender, EventArgs e)
        {
            this.DeletePlaylist?.Invoke(this, EventArgs.Empty);
        }
        private void menuStripExportToTXT_Click(object sender, EventArgs e)
        {
            this.ExportToM3U?.Invoke(this, ListEventArgs.Empty);
        }

        private void menuStripExportToM3U_Click(object sender, EventArgs e)
        {
            this.ExportToTXT?.Invoke(this, ListEventArgs.Empty);
        }
        private void menuStripPreferences_Click(object sender, EventArgs e)
        {
            this.Preferences?.Invoke(this, EventArgs.Empty);
        }
        private void menuStripExit_Click(object sender, EventArgs e)
        {
            this.Exit?.Invoke(this, EventArgs.Empty);
        }

        //EDIT
        private void menuStripRemoveMissingTracks_Click(object sender, EventArgs e)
        {
            this.RemoveMissingTracks?.Invoke(this, EventArgs.Empty);
        }
        private void menuStripRemoveDuplicatedTracks_Click(object sender, EventArgs e)
        {
            this.RemoveDuplicatedTracks?.Invoke(this, EventArgs.Empty);
        }
        private void menuStripOrderByTitle_Click(object sender, EventArgs e)
        {
            this.OrderByTitle?.Invoke(this, EventArgs.Empty);
        }
        private void menuStripOrderByArtist_Click(object sender, EventArgs e)
        {
            this.OrderByArtist?.Invoke(this, EventArgs.Empty);
        }
        private void menuStripOrderByFileName_Click(object sender, EventArgs e)
        {
            this.OrderByFileName?.Invoke(this, EventArgs.Empty);
        }
        private void menuStripReverse_Click(object sender, EventArgs e)
        {
            this.Reverse?.Invoke(this, EventArgs.Empty);
        }
        private void menuStripShuffle_Click(object sender, EventArgs e)
        {
            this.Shuffle?.Invoke(this, EventArgs.Empty);
        }
        private void menuStripClear_Click(object sender, EventArgs e)
        {
            this.Clear?.Invoke(this, EventArgs.Empty);
        }

        //PLAYBACK
        private void menuStripPlay_Click(object sender, EventArgs e)
        {
            this.PlayTrack?.Invoke(this, EventArgs.Empty);
        }
        private void menuStripStop_Click(object sender, EventArgs e)
        {
            this.StopTrack?.Invoke(this, EventArgs.Empty);
            this.lblTrackStart.Text = "";
            this.lblTrackEnd.Text = "";
            this.pBar.Value = 0;
        }
        private void menuStripPause_Click(object sender, EventArgs e)
        {
            this.PauseTrack?.Invoke(this, EventArgs.Empty);
        }
        private void menuStripPrev_Click(object sender, EventArgs e)
        {
            this.PrevTrack?.Invoke(this, EventArgs.Empty);
        }
        private void menuStripNext_Click(object sender, EventArgs e)
        {
            this.NextTrack?.Invoke(this, EventArgs.Empty);
        }
        private void menuStripRandom_Click(object sender, EventArgs e)
        {
            this.RandomTrack?.Invoke(this, EventArgs.Empty);
        }
        
        //HELP
        private void menuStripAbout_Click(object sender, EventArgs e)
        {
            this.About?.Invoke(this, EventArgs.Empty);
        }

        //MENU STRIP ACCESSIBILITY
        public void SetMenuStripAccessibility(object view)
        {
            this.menuStripOpenFiles.Enabled = true;
            this.menuStripOpenDirectories.Enabled = true;
            this.menuStripCreatePlaylist.Enabled = true;
            this.menuStripRenamePlaylist.Enabled = true;
            this.menuStripLoadPlaylist.Enabled = true;
            this.menuStripDeletePlaylist.Enabled = true;
            this.menuStripPreferences.Enabled = true;
            this.menuStripExit.Enabled = true;
            this.menuStripRemoveMissingTracks.Enabled = true;
            this.menuStripRemoveDuplicatedTracks.Enabled = true;
            this.menuStripOrderByArtist.Enabled = true;
            this.menuStripOrderByTitle.Enabled = true;
            this.menuStripOrderByFileName.Enabled = true;
            this.menuStripShuffle.Enabled = true;
            this.menuStripReverse.Enabled = true;
            this.menuStripClear.Enabled = true;
            this.menuStripPlay.Enabled = true;
            this.menuStripStop.Enabled = true;
            this.menuStripPause.Enabled = true;
            this.menuStripPrev.Enabled = true;
            this.menuStripNext.Enabled = true;
            this.menuStripRandom.Enabled = true;
            this.menuStripAbout.Enabled = true;
            this.btnPrev.Enabled = true;
            this.btnNext.Enabled = true;
            this.btnOpen.Enabled = true;
            this.btnOpenDirectory.Enabled = true;

            if (view.GetType() == typeof(PlaylistView))
            {
                return;
            }
            if (view.GetType() == typeof(TrackEditorView))
            {
                this.menuStripRemoveMissingTracks.Enabled = false;
                return;
            }
            if (view.GetType() == typeof(HarmonizerView))
            {
                this.menuStripRemoveMissingTracks.Enabled = false;
                return;
            }

            this.menuStripOpenFiles.Enabled = false;
            this.menuStripOpenDirectories.Enabled = false;
            this.menuStripCreatePlaylist.Enabled = false;
            this.menuStripRenamePlaylist.Enabled = false;
            this.menuStripLoadPlaylist.Enabled = false;
            this.menuStripDeletePlaylist.Enabled = false;
            this.menuStripPreferences.Enabled = true;
            this.menuStripExit.Enabled = true;
            this.menuStripRemoveMissingTracks.Enabled = false;
            this.menuStripRemoveDuplicatedTracks.Enabled = false;
            this.menuStripOrderByArtist.Enabled = false;
            this.menuStripOrderByTitle.Enabled = false;
            this.menuStripOrderByFileName.Enabled = false;
            this.menuStripShuffle.Enabled = false;
            this.menuStripReverse.Enabled = false;
            this.menuStripClear.Enabled = false;
            this.menuStripPlay.Enabled = true;
            this.menuStripStop.Enabled = true;
            this.menuStripPause.Enabled = true;
            this.menuStripPrev.Enabled = false;
            this.menuStripNext.Enabled = false;
            this.menuStripRandom.Enabled = false;
            this.menuStripAbout.Enabled = true;
            this.btnPrev.Enabled = false;
            this.btnNext.Enabled = false;
            this.btnOpen.Enabled = false;
            this.btnOpenDirectory.Enabled = false;

        }

        //PLAYER BUTTONS
        private void btnPlay_Click(object sender, EventArgs e)
        {
            this.PlayTrack?.Invoke(this, EventArgs.Empty);
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            this.StopTrack?.Invoke(this, EventArgs.Empty);
            this.lblTrackStart.Text = "";
            this.lblTrackEnd.Text = "";
            this.pBar.Value = 0;
        }
        private void btnPause_Click(object sender, EventArgs e)
        {
            this.PauseTrack?.Invoke(this, EventArgs.Empty);
        }
        private void btnPrev_Click(object sender, EventArgs e)
        {
            this.PrevTrack?.Invoke(this, EventArgs.Empty);
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            this.NextTrack?.Invoke(this, EventArgs.Empty);
        }
        private void pBar_MouseDown(object sender, MouseEventArgs e)
        {
            this.ChangeProgress?.Invoke(this, new ListEventArgs() { IntegerField1 = e.X, IntegerField2 = pBar.Width });
        }
        private void trackVolume_Scroll(object sender, EventArgs e)
        {
            this.ChangeVolume?.Invoke(this, new ListEventArgs() { IntegerField1 = this.trackVolume.Value });
            this.lblVolume.Text = this.trackVolume.Value.ToString() + "%";
        }
        private void btnOpen_Click(object sender, EventArgs e)
        {
            this.OpenFiles?.Invoke(this, EventArgs.Empty);
        }
        private void btnOpenDirectory_Click(object sender, EventArgs e)
        {
            this.OpenDirectory?.Invoke(this, EventArgs.Empty);
        }

        //CALL FROM PLAYLIST VIEW 
        //SETVOLUME - INIT VOLUME FROM SETTING
        public void SetVolume(int volume)
        {
            trackVolume.Value = volume;
            lblVolume.Text = volume.ToString() + "%";
        }
        //CALL FROM PLAYLIST VIEW 
        //UPDATE ELAPSED DATE, DURATION AND PROGRESS BAR VIA TIMER
        public void UpdateMediaPlayerProgressStatus(double duration, String durationString, double currentPosition, String currentPositionString)
        {
            this.pBar.Maximum = (int)duration;
            this.pBar.Value = (int)currentPosition;
            this.lblTrackEnd.Text = durationString;
            this.lblTrackStart.Text = currentPositionString;
        }


    }
}
