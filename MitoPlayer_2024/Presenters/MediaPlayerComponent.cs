using AxWMPLib;
using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Views;
using System;
using System.Collections.Generic;
using System.Data;
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
        private int selectedRowIndex { get; set; }
        private int lastTrackIndex { get; set; }
        private double currentPlayPosition { get; set; }

        public MediaPlayerComponent(AxWindowsMediaPlayer mediaPLayer)
        {
            this.MediaPlayer = mediaPLayer;
            this.Reset();
        }
        public void Initialize(DataTable workingTable)
        {
            this.workingTable = workingTable;
            this.Reset();
        }
        public void Reset()
        {
            this.CurrentTrackIdInPlaylist = -1;
            this.selectedRowIndex = -1;
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

           // this.ValidateWorkingTable();

            if (this.workingTable == null || this.workingTable.Rows.Count == 0)
            {
                this.Reset();
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
                        this.currentPlayPosition = 0;
                        this.MediaPlayer.URL = path;
                        this.MediaPlayer.Ctlcontrols.currentPosition = 0;
                        this.MediaPlayer.Ctlcontrols.play();

                        result = MediaPlayerUpdateState.AfterPlay;
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
                            this.currentPlayPosition = 0;
                            this.MediaPlayer.URL = path;
                            this.MediaPlayer.Ctlcontrols.currentPosition = 0;
                            this.MediaPlayer.Ctlcontrols.play();

                            result = MediaPlayerUpdateState.AfterPlay;
                        }
                    }
                }
            }
            else if (this.MediaPlayer.playState == WMPLib.WMPPlayState.wmppsPaused)
            {
                this.MediaPlayer.Ctlcontrols.currentPosition = this.currentPlayPosition;
                this.MediaPlayer.Ctlcontrols.play();

                result = MediaPlayerUpdateState.AfterPlayAfterPause;
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
        /* nem szól semmi, üres a tábla: -
         * nem szól semmi, a táblába nem üres, nincs kijelölve semmi: az utoljára szólt szám indexéhez képest kell lejátszani a következőt/előzőt
         * nem szól semmi, a táblába nem üres, ki van jelölve egy vagy több sor: a kijelölések közül a legelső számot el kell indítani
         *
         * szól egy szám, üres a tábla: -
         * szól egy szám, a tábla nem üres, az éppen szóló szám nincs a táblában, nincs kijelölve semmi:  az utoljára szólt szám indexéhez képest kell lejátszani a következőt/előzőt
         * szól egy szám, a tábla nem üres, az éppen szóló szám nincs a táblában, ki van jelölve egy vagy több sor: a kijelölések közül a legelső számot el kell indítani
         * szól egy szám, a tábla nem üres, az éppen szóló szám a táblában van, nincs kijelölve semmi: az utoljára szólt szám indexéhez képest kell lejátszani a következőt/előzőt
         * szól egy szám, a tábla nem üres, az éppen szóló szám a táblában van, ki van jelölve valami: az utoljára szólt szám indexéhez képest kell lejátszani a következőt/előzőt
         *
         * szól valami/nem szól semmi: trackIdInPlaylist != -1 / trackIdInPlaylist == -1
         * a tábla üres/nem üres: dgvTrackList.Rows.Count == 0 / dgvTrackList.Rows.Count != 0
         * az éppen szóló szám a táblában van/nincs a táblában: dgvTrackList.Exsists(x => x.trackIdInPlaylist == trackIdInPlaylist)
         * van kijelölve valami/ninc skijelölve semmi: dgvTrackList.SelectedRows.Count == 0 / dgvTrackList.SelectedRows.Count != 0
         */
        private MediaPlayerUpdateState StepTrack(bool backward)
        {
            MediaPlayerUpdateState result = MediaPlayerUpdateState.Undefined;
        /* if (this.CurrentTrackIdInPlaylist == -1)
         {
             if (this.workingTable.Rows.Count > 0)
             {
                 if (this.selectedRowIndex != -1)
                 {
                     this.CurrentTrackIdInPlaylist = selectedRowId;
                     this.lastTrackIndex = selectedRowIndex;
                 }
             }
         }
         else
         {
             if (this.workingTable.Rows.Count > 0)
             {
                 for (int i = 0; i <= this.workingTable.Rows.Count - 1; i++)
                 {
                     int orderInList = Convert.ToInt32(this.workingTable.Rows[i]["OrderInList"]);
                     if (orderInList == this.CurrentTrackIdInPlaylist)
                     {
                         this.lastTrackIndex = orderInList;
                     }
                 }
             }
         }*/

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
                    if (!initialTrackIdInPlaylistList.Contains(trackIdInPlaylist))
                    {
                        initialTrackIdInPlaylistList.Add(trackIdInPlaylist);
                    }
                }
                for (int i = initialTrackIdInPlaylistList.Count - 1; i >= 0; i--)
                {
                    bool isDeleted = true;
                    for (int j = this.workingTable.Rows.Count - 1; j >= 0; j--)
                    {
                        if (initialTrackIdInPlaylistList[i] == Convert.ToInt32(this.workingTable.Rows[j]["TrackIdInPlaylist"]))
                        {
                            isDeleted = false;
                            break;
                        }
                    }
                    if (isDeleted)
                    {
                        int trackIdInPlaylist = initialTrackIdInPlaylistList[i];
                        initialTrackIdInPlaylistList.Remove(trackIdInPlaylist);
                        playedTrackIdInPlaylistList.Remove(trackIdInPlaylist);
                    }
                }
            }
            else
            {
                initialTrackIdInPlaylistList.Clear();
                playedTrackIdInPlaylistList.Clear();
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

                if(playedTrackIdInPlaylistList.Count > 0 && initialTrackIdInPlaylistList.Count > 0
                    && playedTrackIdInPlaylistList.Count == initialTrackIdInPlaylistList.Count)
                {
                    playedTrackIdInPlaylistList.Clear();
                }

                while (!nextIndexFound)
                {
                    int nextTrackIndex = rand.Next(0, initialTrackIdInPlaylistList.Count);
                    if (!playedTrackIdInPlaylistList.Contains(initialTrackIdInPlaylistList[nextTrackIndex]))
                    {
                        nextIndexFound = true;
                        playedTrackIdInPlaylistList.Add(initialTrackIdInPlaylistList[nextTrackIndex]);

                        DataRow nextRow = this.workingTable.Select("TrackIdInPlaylist = " + nextTrackIndex).First();
                        this.selectedRowIndex = this.workingTable.Rows.IndexOf(nextRow);

                        if (this.MediaPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
                            this.CurrentTrackIdInPlaylist = initialTrackIdInPlaylistList[nextTrackIndex];
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

    }
}
