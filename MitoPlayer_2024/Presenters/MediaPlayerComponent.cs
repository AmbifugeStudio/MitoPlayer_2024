using AxWMPLib;
using MitoPlayer_2024.Views;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MitoPlayer_2024.Presenters
{
    public class MediaPlayerComponent
    {
        public AxWindowsMediaPlayer MediaPlayer { get; set; }
        private DataTable workingTable { get; set; }
        private int trackIdInPlaylist { get; set; }
        private int currentTrackIndex { get; set; }
        private int lastTrackIndex { get; set; }
        private double currentPlayPosition { get; set; }

        public MediaPlayerComponent(AxWindowsMediaPlayer mediaPLayer)
        {
            this.MediaPlayer = mediaPLayer;
            this.Reset();
            this.currentPlayPosition = 0;
        }
        public void Initialize(DataTable workingTable)
        {
            this.workingTable = workingTable;
            this.Reset();
        }
        public void Reset()
        {
            this.trackIdInPlaylist = -1;
            this.currentTrackIndex = -1;
            this.lastTrackIndex = -1;
        }
        public void SetCurrentTrackIndex(int index)
        {
            this.currentTrackIndex = index;
        }
        public int GetCurrentTrackIndex()
        {
            return this.currentTrackIndex;
        }
        public void SetWorkingTable(DataTable workingTable)
        {
            this.workingTable = workingTable;
            if(this.workingTable == null || this.workingTable.Rows.Count == 0)
            {
                this.Reset();
            }
        }
        public DataTable GetWorkingTable()
        {
            return this.workingTable;
        }
        public bool PlayTrack()
        {
            bool result = true;
            if (this.trackIdInPlaylist == -1)
            {
                if (this.currentTrackIndex != -1 && this.workingTable.Rows.Count > 0)
                {
                    this.trackIdInPlaylist = Convert.ToInt32(this.workingTable.Rows[this.currentTrackIndex]["OrderInList"]);
                    this.lastTrackIndex = this.currentTrackIndex;

                    String path = (string)workingTable.Rows[this.currentTrackIndex]["Path"];

                    if (System.IO.File.Exists(path))
                    {
                        this.MediaPlayer.URL = path;
                        this.MediaPlayer.Ctlcontrols.currentPosition = 0;
                        this.MediaPlayer.Ctlcontrols.play();
                    }
                }
            }
            else
            {
                if (this.MediaPlayer.playState != WMPLib.WMPPlayState.wmppsPlaying)
                {
                    this.MediaPlayer.Ctlcontrols.currentPosition = this.currentPlayPosition;
                    this.MediaPlayer.Ctlcontrols.play();
                    result = false;
                }
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
                this.trackIdInPlaylist = -1;

                this.MediaPlayer.Ctlcontrols.stop();
                this.MediaPlayer.Ctlcontrols.currentPosition = 0;
            }
        }
        public void PrevTrack(int selectedRowId, int selectedRowIndex)
        {
            this.StepTrack(true, selectedRowId, selectedRowIndex);
        }
        public void NextTrack(int selectedRowId, int selectedRowIndex)
        {
            this.StepTrack(false, selectedRowId, selectedRowIndex);
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
        private void StepTrack(bool backward,int selectedRowId, int selectedRowIndex)
        {
            if (this.trackIdInPlaylist == -1)
            {
                if (this.workingTable.Rows.Count > 0)
                {
                    if (selectedRowId != -1)
                    {
                        this.trackIdInPlaylist = selectedRowId;
                        this.lastTrackIndex = selectedRowIndex;
                    }
                }
            }
            else
            {
                if (this.workingTable.Rows.Count > 0)
                {
                    if (selectedRowId != -1)
                    {
                        if (this.workingTable.Rows.Cast<DataGridViewRow>().ToList().Exists(x => Convert.ToInt32(x.Cells["OrderInList"].Value) == this.trackIdInPlaylist))
                        {
                            this.lastTrackIndex = this.workingTable.Rows.Cast<DataGridViewRow>().Where(x => Convert.ToInt32(x.Cells["OrderInList"].Value) == this.trackIdInPlaylist).First().Index;
                        }
                    }
                    else
                    {
                        if (this.workingTable.Rows.Cast<DataGridViewRow>().ToList().Exists(x => Convert.ToInt32(x.Cells["OrderInList"].Value) == this.trackIdInPlaylist))
                        {
                            this.lastTrackIndex = this.workingTable.Rows.Cast<DataGridViewRow>().Where(x => Convert.ToInt32(x.Cells["OrderInList"].Value) == this.trackIdInPlaylist).First().Index;
                        }
                    }
                }
            }

            if (!backward)
            {
                if (this.lastTrackIndex < this.workingTable.Rows.Count - 1)
                {
                    this.lastTrackIndex = this.lastTrackIndex + 1;
                }
            }
            else
            {
                if (this.lastTrackIndex > 0)
                {
                    this.lastTrackIndex = this.lastTrackIndex - 1;
                }
            }

            this.currentTrackIndex = this.lastTrackIndex;

            this.StopTrack();
            this.PlayTrack();
        }
        public void RandomTrack()
        {
            if (this.workingTable.Rows.Count > 1)
            {
                Random rand = new Random();
                this.currentTrackIndex = rand.Next(0, this.workingTable.Rows.Count - 1);
                this.StopTrack();
                this.PlayTrack();
            }
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
