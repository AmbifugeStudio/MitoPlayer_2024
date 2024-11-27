using AxWMPLib;
using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Views;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MitoPlayer_2024.Presenters
{
    public class MediaPlayerComponent
    {
        public AxWindowsMediaPlayer MediaPlayer { get; set; }
        private DataTable workingTable { get; set; }
        public int CurrentTrackIdInPlaylist { get; set; }
        public int selectedRowIndex { get; set; }
        private double currentPlayPosition { get; set; }
        public bool IsShuffleEnabled { get; set; }
        public bool IsPreviewEnabled { get; set; }
        public int PreviewPercent { get; set; }
        private ISettingDao settingDao { get; set; }

        public MediaPlayerComponent(AxWindowsMediaPlayer mediaPLayer, ISettingDao settingDao)
        {
            this.MediaPlayer = mediaPLayer;
            this.settingDao = settingDao;

            this.CurrentTrackIdInPlaylist = -1;
            this.selectedRowIndex = -1;
            this.currentPlayPosition = 0;
            this.PreviewPercent = 0;

            this.MediaPlayer.PlayStateChange += new AxWMPLib._WMPOCXEvents_PlayStateChangeEventHandler(MediaPlayer_PlayStateChange);

        }
        private void MediaPlayer_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (e.newState == (int)WMPLib.WMPPlayState.wmppsPlaying && currentState == MediaPlayerUpdateState.AfterPlay)
            {
                if (this.IsPreviewEnabled)
                {
                    this.PreviewPercent = this.settingDao.GetIntegerSetting(Settings.PreviewPercentage.ToString());
                    this.currentPlayPosition = this.MediaPlayer.currentMedia.duration / 100 * this.PreviewPercent;
                    this.MediaPlayer.Ctlcontrols.currentPosition = this.currentPlayPosition;
                }
            }
        }
        public void Initialize(DataTable workingTable)
        {
            this.workingTable = workingTable;

            this.CurrentTrackIdInPlaylist = -1;
            this.selectedRowIndex = -1;
            this.currentPlayPosition = 0;
        }
        public void LoadPlaylist(DataTable workingTable)
        {
            this.workingTable = workingTable;

           // this.selectedRowIndex = -1;
            this.currentPlayPosition = 0;
        }
        public void ClearPlaylist(DataTable workingTable)
        {
            this.workingTable = workingTable;
            this.currentPlayPosition = 0;
        }

        public void SetCurrentTrackIndex(int index)
        {
            this.selectedRowIndex = index;
        }
        public int GetCurrentTrackIndex()
        {
            return this.selectedRowIndex;
        }
        public void SetWorkingTable(DataTable workingTable)
        {
            this.workingTable = workingTable;

            this.ValidateWorkingTable();

            if (this.workingTable == null || this.workingTable.Rows.Count == 0)
            {
                this.selectedRowIndex = -1;
                this.currentPlayPosition = 0;
            }
            else
            {
                this.UpdateRowIndex();
            }
        }
        private void UpdateRowIndex()
        {
            for (int i = 0; i <= this.workingTable.Rows.Count - 1; i++)
            {
                int trackIdInPlaylist = Convert.ToInt32(this.workingTable.Rows[i]["TrackIdInPlaylist"]);
                if (trackIdInPlaylist == this.CurrentTrackIdInPlaylist)
                {
                    this.selectedRowIndex = i;
                    break;
                }
            }
            if (this.selectedRowIndex >= this.workingTable.Rows.Count)
            {
                this.selectedRowIndex = this.workingTable.Rows.Count - 1;
            }
        }
        public DataTable GetWorkingTable()
        {
            return this.workingTable;
        }

        private MediaPlayerUpdateState currentState { get; set; }

        public MediaPlayerUpdateState PlayTrack()
        {
            MediaPlayerUpdateState result = MediaPlayerUpdateState.Undefined;

            if (this.MediaPlayer.playState == WMPLib.WMPPlayState.wmppsUndefined ||
                this.MediaPlayer.playState == WMPLib.WMPPlayState.wmppsStopped)
            {
                if (this.workingTable.Rows.Count > 0)
                {
                    if(this.selectedRowIndex == -1)
                    {
                        this.selectedRowIndex = 0;
                    }

                    this.CurrentTrackIdInPlaylist = Convert.ToInt32(this.workingTable.Rows[this.selectedRowIndex]["TrackIdInPlaylist"]);
                    
                    String path = this.workingTable.Rows[this.selectedRowIndex]["Path"].ToString();

                    if (System.IO.File.Exists(path))
                    {
                        currentState = MediaPlayerUpdateState.AfterPlay;

                        this.MediaPlayer.URL = path;
                        this.currentPlayPosition = 0;
                        this.MediaPlayer.Ctlcontrols.play();

                        result = currentState;
                    }
                }
            }
            else if (this.MediaPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                if (this.workingTable.Rows.Count > 0)
                {
                    if (this.selectedRowIndex != -1)
                    {
                        this.CurrentTrackIdInPlaylist = Convert.ToInt32(this.workingTable.Rows[this.selectedRowIndex]["TrackIdInPlaylist"]);

                        String path = this.workingTable.Rows[this.selectedRowIndex]["Path"].ToString();

                        if (System.IO.File.Exists(path))
                        {
                            currentState = MediaPlayerUpdateState.AfterPlay;

                            this.MediaPlayer.URL = path;
                            this.currentPlayPosition = 0;
                            this.MediaPlayer.Ctlcontrols.play();

                            result = currentState;
                        }
                    }
                }
            }
            else if (this.MediaPlayer.playState == WMPLib.WMPPlayState.wmppsPaused)
            {
                currentState = MediaPlayerUpdateState.AfterPlayAfterPause;

                this.MediaPlayer.Ctlcontrols.currentPosition = this.currentPlayPosition;
                this.MediaPlayer.Ctlcontrols.play();

                result = currentState;
            }

            return result;

         
        }
        public void PauseTrack()
        {
            if (this.MediaPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                this.currentPlayPosition = this.MediaPlayer.Ctlcontrols.currentPosition;
                this.MediaPlayer.Ctlcontrols.pause();
            }
        }
        public void StopTrack()
        {
            if (this.MediaPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying ||
               this.MediaPlayer.playState == WMPLib.WMPPlayState.wmppsPaused)
            {
                this.currentPlayPosition = 0;
                this.CurrentTrackIdInPlaylist = -1;

                this.MediaPlayer.Ctlcontrols.stop();
                this.MediaPlayer.Ctlcontrols.currentPosition = 0;
            }
        }
        public MediaPlayerUpdateState PrevTrack()
        {
            MediaPlayerUpdateState result = MediaPlayerUpdateState.Undefined;
            result = this.StepTrack(true);
            return result;
        }
        public MediaPlayerUpdateState NextTrack()
        {
            MediaPlayerUpdateState result = MediaPlayerUpdateState.Undefined;
            result = this.StepTrack(false);
            return result;
        }

        private MediaPlayerUpdateState StepTrack(bool backward)
        {
            MediaPlayerUpdateState result = MediaPlayerUpdateState.Undefined;

            if (this.workingTable.Rows.Count > 0)
            {
                this.UpdateRowIndex();

                if (!backward)
                {
                    if (this.selectedRowIndex < this.workingTable.Rows.Count - 1)
                    {
                        this.selectedRowIndex = this.selectedRowIndex + 1;
                    }
                }
                else
                {
                    if (this.selectedRowIndex > 0)
                    {
                        this.selectedRowIndex = this.selectedRowIndex - 1;
                    }
                }

                this.CurrentTrackIdInPlaylist = Convert.ToInt32(this.workingTable.Rows[this.selectedRowIndex]["TrackIdInPlaylist"]);

                result = this.PlayTrack();
            }
            return result;
        }
        
        private List<int> initialTrackIdInPlaylistList = new List<int>();
        private List<int> playedTrackIdInPlaylistList = new List<int>();
        private void ValidateWorkingTable()
        {
            if (this.workingTable.Rows.Count > 1)
            {
                for (int i = 0; i <= this.workingTable.Rows.Count - 1; i++)
                {
                    int trackIdInPlaylist = Convert.ToInt32(this.workingTable.Rows[i]["TrackIdInPlaylist"]);
                    if (!this.initialTrackIdInPlaylistList.Contains(trackIdInPlaylist))
                    {
                        this.initialTrackIdInPlaylistList.Add(trackIdInPlaylist);
                    }
                }
                for (int i = this.initialTrackIdInPlaylistList.Count - 1; i >= 0; i--)
                {
                    bool isDeleted = true;
                    for (int j = this.workingTable.Rows.Count - 1; j >= 0; j--)
                    {
                        if (this.initialTrackIdInPlaylistList[i] == Convert.ToInt32(this.workingTable.Rows[j]["TrackIdInPlaylist"]))
                        {
                            isDeleted = false;
                            break;
                        }
                    }
                    if (isDeleted)
                    {
                        int trackIdInPlaylist = initialTrackIdInPlaylistList[i];
                        this.initialTrackIdInPlaylistList.Remove(trackIdInPlaylist);
                        this.playedTrackIdInPlaylistList.Remove(trackIdInPlaylist);
                    }
                }
            }
            else
            {
                this.initialTrackIdInPlaylistList.Clear();
                this.playedTrackIdInPlaylistList.Clear();
            }
        }
        public MediaPlayerUpdateState RandomTrack()
        {
            MediaPlayerUpdateState result = MediaPlayerUpdateState.AfterPlay;

            if (this.workingTable.Rows.Count > 1)
            {
                this.ValidateWorkingTable();

                Random rand = new Random();

                bool nextIndexFound = false;

                if(this.playedTrackIdInPlaylistList.Count > 0 && this.initialTrackIdInPlaylistList.Count > 0
                    && this.playedTrackIdInPlaylistList.Count == this.initialTrackIdInPlaylistList.Count)
                {
                    this.playedTrackIdInPlaylistList.Clear();
                }

                while (!nextIndexFound)
                {
                    int nextTrackIndex = rand.Next(0, this.initialTrackIdInPlaylistList.Count);
                    if (!this.playedTrackIdInPlaylistList.Contains(this.initialTrackIdInPlaylistList[nextTrackIndex]))
                    {
                        nextIndexFound = true;
                        this.playedTrackIdInPlaylistList.Add(this.initialTrackIdInPlaylistList[nextTrackIndex]);

                        int trackIdInPlaylist = this.initialTrackIdInPlaylistList[nextTrackIndex];
                        for(int i = 0; i <= this.workingTable.Rows.Count -1; i++)
                        {
                            if (trackIdInPlaylist == Convert.ToInt32(this.workingTable.Rows[i]["TrackIdInPlaylist"]))
                            {
                                this.selectedRowIndex = i;
                                break;
                            }
                        }

                        this.CurrentTrackIdInPlaylist = this.initialTrackIdInPlaylistList[nextTrackIndex];

                    }
                }

                this.StopTrack();
                result = this.PlayTrack();
            }
            return result;
        }

        public void ChangeVolume(int volume)
        {
            this.MediaPlayer.settings.volume = volume;
        }

        public void ChangeShuffle(bool isShuffleEnabled)
        {
            this.IsShuffleEnabled = isShuffleEnabled;

            this.initialTrackIdInPlaylistList = new List<int>();
            this.playedTrackIdInPlaylistList = new List<int>();
        }

        public void ChangePreview(bool isPreviewEnabled)
        {
            this.IsPreviewEnabled = isPreviewEnabled;
        }


        public void ChangeProgress(int position, int length)
        {
            if (this.MediaPlayer != null && this.MediaPlayer.Ctlcontrols != null && this.MediaPlayer.Ctlcontrols.currentItem != null)
            {
                this.currentPlayPosition = this.MediaPlayer.currentMedia.duration * position / length;
                this.MediaPlayer.Ctlcontrols.currentPosition = this.currentPlayPosition;
            }
           
        }
        public int GetDuration()
        {
            int result = 0;
            if (this.MediaPlayer != null && this.MediaPlayer.Ctlcontrols != null && this.MediaPlayer.Ctlcontrols.currentItem != null)
            {
                if (this.MediaPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
                {
                    if (this.MediaPlayer != null && this.MediaPlayer.Ctlcontrols != null && this.MediaPlayer.Ctlcontrols.currentItem != null)
                        result = (int)this.MediaPlayer.Ctlcontrols.currentItem.duration;
                }
            }
            return result;
        }
        public int GetCurrentPosition()
        {
            int result = 0;
            if (this.MediaPlayer != null && this.MediaPlayer.Ctlcontrols != null && this.MediaPlayer.Ctlcontrols.currentItem != null)
            {
                if (this.MediaPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
                {
                    if (this.MediaPlayer != null && this.MediaPlayer.Ctlcontrols != null && this.MediaPlayer.Ctlcontrols.currentItem != null)
                        result = (int)this.MediaPlayer.Ctlcontrols.currentPosition;
                }
            }
            return result;
        }
        public String GetDurationString()
        {
            String result = String.Empty;
            if (this.MediaPlayer != null && this.MediaPlayer.Ctlcontrols != null && this.MediaPlayer.Ctlcontrols.currentItem != null)
            {
                if (this.MediaPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
                {
                    if (this.MediaPlayer != null && this.MediaPlayer.Ctlcontrols != null && this.MediaPlayer.Ctlcontrols.currentItem != null)
                        result = this.MediaPlayer.Ctlcontrols.currentItem.durationString;
                }
            }
            if (result.Length > 0 && result.Length <= 5)
            {
                result = "00:" + result;
            }
            return result;
        }
        
        public String GetCurrentPositionString()
        {
            String result = String.Empty;
            if (this.MediaPlayer != null && this.MediaPlayer.Ctlcontrols != null && this.MediaPlayer.Ctlcontrols.currentItem != null)
            {
                if (this.MediaPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
                {
                    if (this.MediaPlayer != null && this.MediaPlayer.Ctlcontrols != null && this.MediaPlayer.Ctlcontrols.currentItem != null)
                        result = this.MediaPlayer.Ctlcontrols.currentPositionString;
                }
            }
            if(result.Length > 0 && result.Length <= 5)
            {
                result = "00:" + result;
            }
            return result;
        }

        public void JumpForward()
        {
            double cp = this.MediaPlayer.Ctlcontrols.currentPosition;

            if (cp + 5 > this.MediaPlayer.currentMedia.duration)
                cp = this.MediaPlayer.currentMedia.duration;
            else
                cp += 5;

            this.currentPlayPosition = cp;
            this.MediaPlayer.Ctlcontrols.currentPosition = cp;
        }
        public void JumpBackward()
        {
            double cp = this.MediaPlayer.Ctlcontrols.currentPosition;

            if (cp - 5 <= 0)
                cp = 0;
            else
                cp -= 5;

            this.currentPlayPosition = cp;
            this.MediaPlayer.Ctlcontrols.currentPosition = cp;
        }
    }
}
