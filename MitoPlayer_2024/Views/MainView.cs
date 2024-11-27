using Google.Protobuf.WellKnownTypes;
using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Views;
using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using TagLib.Riff;

namespace MitoPlayer_2024
{
    public partial class MainView : Form, IMainView
    {
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
        public event EventHandler ExportToDirectory;
        public event EventHandler Settings;
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
        public event EventHandler<Messenger> ChangeProgress;
        public event EventHandler<Messenger> ChangeVolume;
        public event EventHandler<Messenger> ChangeShuffle;
        public event EventHandler<Messenger> ChangeMute;
        public event EventHandler<Messenger> ChangePreview;

        public event EventHandler About;

        public event EventHandler<Messenger> ScanFiles;

        public event EventHandler GetMediaPlayerProgressStatusEvent;

        public event EventHandler OpenChartEvent;

        private MMDevice mmDevice;

        public MainView()
        {
            this.InitializeComponent();
            this.SetControlColors();

            this.prbTrackProgress.Hide();
            this.lblTrackStart.Hide();
            this.lblTrackEnd.Hide();

            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            MMDeviceCollection devices = enumerator.EnumerateAudioEndPoints(DataFlow.All, DeviceState.Active);
            if(devices != null && devices.Count > 0)
            {
                mmDevice = devices[0];
            }

            this.tmrPeak.Start();
        }

        Color BackgroundColor = System.Drawing.ColorTranslator.FromHtml("#363639");
        Color FontColor = System.Drawing.ColorTranslator.FromHtml("#c6c6c6");
        Color ButtonColor = System.Drawing.ColorTranslator.FromHtml("#292a2d");
        Color ButtonBorderColor = System.Drawing.ColorTranslator.FromHtml("#1b1b1b");
        Color WarningColor = System.Drawing.ColorTranslator.FromHtml("#ff8088");
        private void SetControlColors()
        {
            this.strMenu.BackColor = this.BackgroundColor;
            this.strMenu.ForeColor = this.FontColor;

            this.pnlMainMenu.BackColor = this.BackgroundColor;
            this.pnlMainMenu.ForeColor = this.FontColor;

            this.btnPlaylist.BackColor = this.ButtonColor;
            this.btnPlaylist.ForeColor = this.FontColor;
            this.btnPlaylist.FlatAppearance.BorderColor = this.ButtonBorderColor;
            this.btnTagValues.BackColor = this.ButtonColor;
            this.btnTagValues.ForeColor = this.FontColor;
            this.btnTagValues.FlatAppearance.BorderColor = this.ButtonBorderColor;
            this.btnTracks.BackColor = this.ButtonColor;
            this.btnTracks.ForeColor = this.FontColor;
            this.btnTracks.FlatAppearance.BorderColor = this.ButtonBorderColor;
            this.btnRules.BackColor = this.ButtonColor;
            this.btnRules.ForeColor = this.FontColor;
            this.btnRules.FlatAppearance.BorderColor = this.ButtonBorderColor;
            this.btnTemplates.BackColor = this.ButtonColor;
            this.btnTemplates.ForeColor = this.FontColor;
            this.btnTemplates.FlatAppearance.BorderColor = this.ButtonBorderColor;
            this.btnHarmonizer.BackColor = this.ButtonColor;
            this.btnHarmonizer.ForeColor = this.FontColor;
            this.btnHarmonizer.FlatAppearance.BorderColor = this.ButtonBorderColor;

            this.pnlMediaPlayer.BackColor = this.BackgroundColor;
            this.pnlMediaPlayer.ForeColor = this.FontColor;

            this.btnStop.BackColor = this.ButtonColor;
            this.btnStop.ForeColor = this.FontColor;
            this.btnStop.FlatAppearance.BorderColor = this.ButtonBorderColor;
            this.btnPause.BackColor = this.ButtonColor;
            this.btnPause.ForeColor = this.FontColor;
            this.btnPause.FlatAppearance.BorderColor = this.ButtonBorderColor;
            this.btnPlay.BackColor = this.ButtonColor;
            this.btnPlay.ForeColor = this.FontColor;
            this.btnPlay.FlatAppearance.BorderColor = this.ButtonBorderColor;
            this.btnPrev.BackColor = this.ButtonColor;
            this.btnPrev.ForeColor = this.FontColor;
            this.btnPrev.FlatAppearance.BorderColor = this.ButtonBorderColor;
            this.btnNext.BackColor = this.ButtonColor;
            this.btnNext.ForeColor = this.FontColor;
            this.btnNext.FlatAppearance.BorderColor = this.ButtonBorderColor;
            this.btnOpen.BackColor = this.ButtonColor;
            this.btnOpen.ForeColor = this.FontColor;
            this.btnOpen.FlatAppearance.BorderColor = this.ButtonBorderColor;
            this.btnOpenDirectory.BackColor = this.ButtonColor;
            this.btnOpenDirectory.ForeColor = this.FontColor;
            this.btnOpenDirectory.FlatAppearance.BorderColor = this.ButtonBorderColor;

            this.prbTrackProgress.BackColor = this.ButtonColor;
            this.prbTrackProgress.ForeColor = this.BackgroundColor;
            this.prbVolume.BackColor = this.ButtonColor;
            this.prbVolume.ForeColor = this.BackgroundColor;

            this.pnlMarkerBackground.BackColor = this.BackgroundColor;

            this.btnPlot.BackColor = this.ButtonColor;
            this.btnPlot.ForeColor = this.FontColor;
            this.btnPlot.FlatAppearance.BorderColor = this.ButtonBorderColor;



        }

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
            this.ExportToM3U?.Invoke(this, Messenger.Empty);
        }

        private void menuStripExportToM3U_Click(object sender, EventArgs e)
        {
            this.ExportToTXT?.Invoke(this, Messenger.Empty);
        }
        private void menuStripExportToDirectory_Click(object sender, EventArgs e)
        {
            this.ExportToDirectory?.Invoke(this, Messenger.Empty);
        }
        private void menuStripSettings_Click(object sender, EventArgs e)
        {
            this.Settings?.Invoke(this, EventArgs.Empty);
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
            this.prbTrackProgress.Value = 0;
            this.prbTrackProgress.Hide();
            this.lblTrackStart.Hide();
            this.lblTrackEnd.Hide();
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
            this.prbTrackProgress.Value = 0;
            this.prbTrackProgress.Hide();
            this.lblTrackStart.Hide();
            this.lblTrackEnd.Hide();
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
            this.ChangeProgress?.Invoke(this, new Messenger() { IntegerField1 = e.X, IntegerField2 = prbTrackProgress.Width });
        }

        private void chbMute_CheckedChanged(object sender, EventArgs e)
        {
            this.ChangeMute?.Invoke(this, new Messenger() { BooleanField1 = this.chbMute.Checked }); 
        }

        private void chbShuffle_CheckedChanged(object sender, EventArgs e)
        {
            this.ChangeShuffle?.Invoke(this, new Messenger() { BooleanField1 = this.chbShuffle.Checked });
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
            this.prbVolume.Value = volume;
        }
        public void SetMuted(bool isMuted)
        {
            this.chbMute.Checked = isMuted;
        }



        //CALL FROM PLAYLIST VIEW 
        //UPDATE ELAPSED DATE, DURATION AND PROGRESS BAR VIA TIMER

        public void UpdateAfterPlayTrack(String artist)
        {
            this.tmrPlayer.Stop();
            this.tmrPlayer.Start();
            this.lblCurrentTrack.Text = artist;
        }
        public void UpdateAfterPlayTrackAfterPause()
        {
            this.tmrPlayer.Stop();
            this.tmrPlayer.Start();
            this.lblCurrentTrack.Text = this.lblCurrentTrack.Text.Replace("Paused: ", "Playing: ");
        }
        public void UpdateAfterStopTrack()
        {
            this.tmrPlayer.Stop();
            this.lblCurrentTrack.Text = "Playing: -";

            leftReferenceHeight = this.pcbMasterPeakLeftColoured.Height;
            rightReferenceHeight = this.pcbMasterPeakLeftColoured.Height;
            this.pcbMasterPeakLeftBackground.Height = leftReferenceHeight;
            this.pcbMasterPeakRightBackground.Height = rightReferenceHeight;
        }
        public void UpdateAfterPauseTrack()
        {
            this.tmrPlayer.Stop();
            this.lblCurrentTrack.Text = this.lblCurrentTrack.Text.Replace("Playing: ", "Paused: ");

            leftReferenceHeight = this.pcbMasterPeakLeftColoured.Height;
            rightReferenceHeight = this.pcbMasterPeakLeftColoured.Height;
            this.pcbMasterPeakLeftBackground.Height = leftReferenceHeight;
            this.pcbMasterPeakRightBackground.Height = rightReferenceHeight;
        }
        //private int leftReferenceHeight = 0;
        // private int rightReferenceHeight = 0;


        public void InitializeMediaPlayerProgressStatus(double duration, String durationString, double currentPosition, String currentPositionString)
        {
            this.prbTrackProgress.Show();
            this.lblTrackStart.Show();
            this.lblTrackEnd.Show();
            this.prbTrackProgress.Maximum = (int)duration;
            this.prbTrackProgress.Value = (int)currentPosition;
            this.lblTrackEnd.Text = durationString;
            this.lblTrackStart.Text = currentPositionString;
        }

        public void ResetMediaPlayerProgressStatus()
        {
            this.prbTrackProgress.Hide();
            this.lblTrackStart.Hide();
            this.lblTrackEnd.Hide();

            this.prbTrackProgress.Value = 0;
            this.lblTrackEnd.Text = "";
            this.lblTrackStart.Text = "";
        }

        public void UpdateMediaPlayerProgressStatus(double duration, String durationString, double currentPosition, String currentPositionString)
        {
            if((int)Math.Ceiling(currentPosition) > 0 && (int)Math.Ceiling(currentPosition) < (int)Math.Ceiling(duration))
            {
                if (!this.prbTrackProgress.Visible)
                {
                    this.prbTrackProgress.Show();
                    this.lblTrackStart.Show();
                    this.lblTrackEnd.Show();
                }
               
                this.prbTrackProgress.Maximum = (int)Math.Ceiling(duration);

                if (this.prbTrackProgress.Maximum >= (int)Math.Ceiling(currentPosition))
                {
                    this.prbTrackProgress.Value = (int)Math.Ceiling(currentPosition);
                }

                this.lblTrackEnd.Text = durationString;
                this.lblTrackStart.Text = currentPositionString;
            }
            else
            {
                if (this.prbTrackProgress.Visible)
                {
                    this.prbTrackProgress.Hide();
                    this.lblTrackStart.Hide();
                    this.lblTrackEnd.Hide();
                }
              
                this.prbTrackProgress.Maximum = 0;
                this.prbTrackProgress.Value = 0;
                this.lblTrackEnd.Text = "";
                this.lblTrackStart.Text = "";
            }

            
        }

        private bool masterPeakLeftGreenEnabled = false;
        private bool masterPeakLeftOrangeEnabled = false;
        private bool masterPeakLeftRedEnabled = false;
        
        private bool masterPeakRightGreenEnabled = false;
        private bool masterPeakRightOrangeEnabled = false;
        private bool masterPeakRightRedEnabled = false;

        
        private int medPeakThreshold = 60;
        private int highPeakThreshold = 80;

        private void tmrPlayer_Tick(object sender, EventArgs e)
        {
            this.GetMediaPlayerProgressStatusEvent?.Invoke(this, EventArgs.Empty);
        }




        private DateTime? lastCurrentTime = null;
        private DateTime? lastCurrentTimeForPeak = null;
        private List<float> peakDecibelList = new List<float>();


        int peakLineY;

        private void prbVolume_MouseDown(object sender, MouseEventArgs e)
        {
            this.chbMute.Checked = false;

            if (e.X > 100)
            {
                this.prbVolume.Value = 100;
            }
            else
            {
                this.prbVolume.Value = e.X;
            }

            this.ChangeVolume?.Invoke(this, new Messenger() { IntegerField1 = e.X });
        }

        private int leftReferenceHeight;
        private int rightReferenceHeight;
        private int leftPeak;
        private int rightPeak;
        private int masterPeak;
        private float masterPeakInDecibel;
        private DateTime currentTime;

        private void tmrPeak_Tick(object sender, EventArgs e)
        {
            if (this.mmDevice == null || this.mmDevice.AudioMeterInformation.PeakValues.Count < 1) return;

            leftReferenceHeight = this.pcbMasterPeakLeftColoured.Height;
            rightReferenceHeight = this.pcbMasterPeakLeftColoured.Height;

            if(this.mmDevice.AudioMeterInformation.PeakValues != null && this.mmDevice.AudioMeterInformation.PeakValues.Count > 0)
            {
                try
                {
                    leftPeak = (int)(Math.Round(this.mmDevice.AudioMeterInformation.PeakValues[0] * 100));
                    rightPeak = (int)(Math.Round(this.mmDevice.AudioMeterInformation.PeakValues[1] * 100));
                }
                catch (Exception ex)
                {
                    leftPeak = 0;
                    rightPeak = 0;
                }
            }
            else
            {
                leftPeak = 0; 
                rightPeak = 0;
            }
            
            masterPeak = (int)(Math.Round(this.mmDevice.AudioMeterInformation.MasterPeakValue * 100));

            masterPeakInDecibel = this.ConvertToDecibels(this.mmDevice.AudioMeterInformation.MasterPeakValue);

            this.pcbMasterPeakLeftBackground.Height = leftReferenceHeight - leftPeak * (leftReferenceHeight / 100);
            this.pcbMasterPeakRightBackground.Height = rightReferenceHeight - rightPeak * (leftReferenceHeight / 100);

            currentTime = DateTime.Now;

            if (!lastCurrentTime.HasValue || lastCurrentTime.Value.AddMilliseconds(100) < currentTime)
            {
                lastCurrentTime = currentTime;
                peakDecibelList.Insert(0, masterPeakInDecibel);
                if (peakDecibelList.Count > 10)
                {
                    peakDecibelList.RemoveAt(peakDecibelList.Count - 1);
                }
            }

            if (!lastCurrentTimeForPeak.HasValue || lastCurrentTimeForPeak.Value.AddMilliseconds(1000) < currentTime)
            {
                lastCurrentTimeForPeak = currentTime;

                if (peakDecibelList.Count > 0)
                {
                    float averagePeak = peakDecibelList.Max();
                    this.lblPeak.Text = averagePeak != float.NegativeInfinity ? $"{averagePeak:N3} dB" : string.Empty;

                    int peakLineY = rightReferenceHeight - masterPeak * (leftReferenceHeight / 100);
                    this.pcbMarkerGrey.Location = new Point(0, peakLineY);
                    this.pcbMarkerRed.Location = new Point(0, peakLineY);

                    if (averagePeak > -1)
                    {
                        this.lblPeak.ForeColor = this.WarningColor;
                        this.pcbMarkerGrey.Hide();
                        this.pcbMarkerRed.Show();
                    }
                    else if (averagePeak <= -1 && averagePeak > -3)
                    {
                        this.lblPeak.ForeColor = this.FontColor;
                        this.pcbMarkerGrey.Show();
                        this.pcbMarkerRed.Hide();
                    }
                    else
                    {
                        this.lblPeak.ForeColor = this.FontColor;
                        this.pcbMarkerGrey.Show();
                        this.pcbMarkerRed.Hide();
                    }
                }
            }
        }
        public float ConvertToDecibels(float value)
        {
            if (value <= 0)
            {
                return float.NegativeInfinity; // Handle log(0) case
            }
            return 20 * (float)Math.Log10(value);
        }

        private void chbPreview_CheckedChanged(object sender, EventArgs e)
        {
            this.ChangePreview?.Invoke(this, new Messenger() { BooleanField1 = this.chbPreview.Checked });
        }

        private void btnPlot_Click(object sender, EventArgs e)
        {
            this.OpenChartEvent?.Invoke(this, new EventArgs());
        }
    }

}
