using Google.Protobuf.WellKnownTypes;
using K4os.Compression.LZ4.Internal;
using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Model;
using MitoPlayer_2024.Models;
using Org.BouncyCastle.Tls.Crypto;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using static System.Windows.Forms.DataGridView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace MitoPlayer_2024.Views
{
    public partial class PlaylistView : Form, IPlaylistView
    {
        public List<Playlist> PlaylistList { get; set; }
        public List<Track> TrackList { get; set; }
        private int lastTrackIndex { get; set; }
        private double currentPlayPosition { get; set; }

        private BindingSource playlistListBindingSource;
        private BindingSource selectedTrackListBindingSource;
        private BindingSource trackListBindingSource;

        public event EventHandler OpenFiles;
        public event EventHandler OpenDirectory;
        public event EventHandler<ListEventArgs> ScanFiles;
        public event EventHandler<ListEventArgs> OrderTableByColumn;
        public event EventHandler OrderByArtist;
        public event EventHandler OrderByTitle;
        public event EventHandler OrderByFileName;
        public event EventHandler Shuffle;
        public event EventHandler Reverse;
        public event EventHandler Clear;
        public event EventHandler RemoveMissingTracks;
        public event EventHandler RemoveDuplicatedTracks;
        public event EventHandler<ListEventArgs> DeleteTracks;
        
        public event EventHandler<ListEventArgs> TrackDragAndDrop;

        public event EventHandler<ListEventArgs> ChangeVolume;

        public event EventHandler ReorderPlaylist;
        public event EventHandler AddTrackToPlaylist;
        public event EventHandler RemoveTrackFromPlaylist;

        public event EventHandler ClearPlaylist;


        public event EventHandler<ListEventArgs> ShowPlaylistEditorView;
        public event EventHandler<ListEventArgs> LoadPlaylist;
        public event EventHandler<ListEventArgs> DeletePlaylist;
        
        public PlaylistView()
        {
            InitializeComponent();
            AssociateAndRaiseViewEvents();
        }
        private void AssociateAndRaiseViewEvents()
        {
            this.currentPlayPosition = 0;
            this.currentTrackIndex = 0;
        }

        #region SINGLETON

        private static PlaylistView instance;

        //MDI nélkül kiveszed a containert a pm-ből
        public static PlaylistView GetInstance(Form parentContainer)
        {
            if (instance == null || instance.IsDisposed)
            {
                instance = new PlaylistView();
                instance.MdiParent = parentContainer;
                instance.FormBorderStyle = FormBorderStyle.None;
                instance.Dock = DockStyle.Fill;
            }
            else
            {
                if (instance.WindowState == FormWindowState.Minimized)
                    instance.WindowState = FormWindowState.Normal;
                instance.BringToFront();
            }
            return instance;
        }
        #endregion

        #region TABLE BINDINGS AND INIT
        public void SetPlaylistListBindingSource(BindingSource playlistList, bool[] columnVisibility, int currentPlaylistId)
        {
            playlistListBindingSource = new BindingSource();
            playlistListBindingSource.DataSource = playlistList;
            dgvPlaylistList.DataSource = playlistListBindingSource.DataSource;
            for (int i = 0; i <= dgvPlaylistList.Columns.Count - 1; i++)
            {
                dgvPlaylistList.Columns[i].Visible = columnVisibility[i];
            }
            if (dgvPlaylistList.Rows.Count > 0)
            {
                for (int i = 0; i < dgvPlaylistList.Rows.Count; i++)
                {
                    if (Convert.ToInt32(dgvPlaylistList.Rows[i].Cells["Id"].Value) == currentPlaylistId)
                    {
                        dgvPlaylistList.Rows[i].Selected = true;
                        dgvPlaylistList.CurrentCell = dgvPlaylistList.Rows[i].Cells["Name"];
                        break;
                    }
                }
            }
            
        }
        public void SetTrackListBindingSource(BindingSource trackList, bool[] columnVisibility)
        {
            trackListBindingSource = new BindingSource();
            trackListBindingSource.DataSource = trackList;
            dgvTrackList.DataSource = trackListBindingSource.DataSource;
            for (int i = 0; i <= dgvTrackList.Columns.Count - 1; i++)
            {
                dgvTrackList.Columns[i].Visible = columnVisibility[i];
            }

            int isMissingColumnIndex = dgvTrackList.Columns["IsMissing"].Index;
            for (int i = 0; i < dgvTrackList.Rows.Count; i++)
            {
                if ((bool)dgvTrackList.Rows[i].Cells[isMissingColumnIndex].Value)
                {
                    dgvTrackList.Rows[i].DefaultCellStyle.BackColor = Color.Salmon;
                }
            }
        }
        public void SetSelectedTrackListBindingSource(BindingSource selectedTrackList)
        {
            selectedTrackListBindingSource = new BindingSource();
            selectedTrackListBindingSource.DataSource = selectedTrackList;
        }
        public void SetVolume(int volume)
        {
            trackVolume.Value = volume;
            lblVolume.Text = volume.ToString() + "%";
        }
        #endregion

        #region EVENT CALLS FROM MAINVIEW
        public void OpenFilesExt()
        {
            this.OpenFiles?.Invoke(this, EventArgs.Empty);
            if (mediaPlayer.playState != WMPLib.WMPPlayState.wmppsPlaying)
            {
                this.PlayTrack();
            }
        }
        public void OpenDirectoryExt()
        {
            OpenDirectory?.Invoke(this, EventArgs.Empty);
            if (mediaPlayer.playState != WMPLib.WMPPlayState.wmppsPlaying)
            {
                PlayTrack();
            }
        }
        public void Play()
        {
            PlayTrack();
        }
        public void Pause()
        {
            PauseTrack();
        }
        public void Stop()
        {
            StopTrack();
        }
        public void Prev()
        {
            PrevTrack();
        }
        public void Next()
        {
            NextTrack();
        }
        public void OrderByTitleExt()
        {
            OrderByTitle?.Invoke(this, EventArgs.Empty);
        }
        public void OrderByArtistExt()
        {
            OrderByArtist?.Invoke(this, EventArgs.Empty);
        }
        public void OrderByFileNameExt()
        {
            OrderByFileName?.Invoke(this, EventArgs.Empty);
        }
        public void ShuffleExt()
        {
            Shuffle?.Invoke(this, EventArgs.Empty);
        }
        public void ReverseExt()
        {
            Reverse?.Invoke(this, EventArgs.Empty);
        }
        public void Random()
        {
            RandomTrack();
        }
        public void ClearExt()
        {
            Clear?.Invoke(this, EventArgs.Empty);
            if (dgvTrackList.Rows == null || dgvTrackList.Rows.Count == 0)
            {
                trackIdInPlaylist = -1;
                currentTrackIndex = -1;
                lastTrackIndex = -1;
            }
        }
        public void RemoveMissingTracksExt()
        {
            this.RemoveMissingTracks?.Invoke(this, EventArgs.Empty);
            if (dgvTrackList.Rows == null || dgvTrackList.Rows.Count == 0)
            {
                trackIdInPlaylist = -1;
                currentTrackIndex = -1;
                lastTrackIndex = -1;
            }
        }
        public void RemoveDuplicatedTracksExt()
        {
            this.RemoveDuplicatedTracks?.Invoke(this, EventArgs.Empty);
            if (dgvTrackList.Rows == null || dgvTrackList.Rows.Count == 0)
            {
                trackIdInPlaylist = -1;
                currentTrackIndex = -1;
                lastTrackIndex = -1;
            }
        }

        public void CreatePlaylist()
        {
            CreatePlaylistExt();
        }
        public void RenamePlaylist()
        {
            RenamePlaylistExt();
        }
        
        #endregion

        #region MEDIA PLAYER

        //OPEN FILES
        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFiles?.Invoke(this, EventArgs.Empty);
            if (mediaPlayer.playState != WMPLib.WMPPlayState.wmppsPlaying)
            {
                PlayTrack();
            }
        }
        
        //OPEN DIRECTORY
        private void btnOpenDirectory_Click(object sender, EventArgs e)
        {
            OpenDirectory?.Invoke(this, EventArgs.Empty);
            if (mediaPlayer.playState != WMPLib.WMPPlayState.wmppsPlaying)
            {
                PlayTrack();
            }
        }
        
        //MEDIA PLAYER
        private void btnPlay_Click(object sender, EventArgs e)
        {
            this.PlayTrack();
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            this.StopTrack();

        }
        private void btnPause_Click(object sender, EventArgs e)
        {
            PauseTrack();
        }
        private void btnPrev_Click(object sender, EventArgs e)
        {
            PrevTrack();
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            NextTrack();
        }
        
        //CELL CLICK
        private int currentTrackIndex = -1;
        private int trackIdInPlaylist = -1;
        /*
         * a jelenleg kijelölt szám indexe
         */
        private void dgvTrackList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.currentTrackIndex = (int)e.RowIndex;
        }
        //CELL DOUBLE CLICK
        /*
         * szám lejátszása
         */
        private void dgvTrackList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            this.StopTrack();
            this.PlayTrack();
        }
        //PLAY TRACK
        private void PlayTrack()
        {
            if (trackIdInPlaylist == -1)
            {
                if (currentTrackIndex != -1 && dgvTrackList.Rows.Count > 0)
                {
                    trackIdInPlaylist = Convert.ToInt32(dgvTrackList.Rows[currentTrackIndex].Cells["OrderInList"].Value);
                    lastTrackIndex = currentTrackIndex;

                    String path = (string)dgvTrackList.Rows[currentTrackIndex].Cells["Path"].Value;
                    if (File.Exists(path))
                    {
                        mediaPlayer.URL = path;
                        mediaPlayer.Ctlcontrols.currentPosition = 0;
                        mediaPlayer.Ctlcontrols.play();

                        String title = "Playing: " + (string)dgvTrackList.Rows[currentTrackIndex].Cells["Artist"].Value;
                        if (!String.IsNullOrEmpty((string)dgvTrackList.Rows[currentTrackIndex].Cells["Title"].Value))
                        {
                            title += " - " + (string)dgvTrackList.Rows[currentTrackIndex].Cells["Title"].Value;
                        }
                        lblCurrentTrack.Text = title;

                        SetCurrentTrackColor();
                    }
                }

            }
            else
            {
                if (mediaPlayer.playState != WMPLib.WMPPlayState.wmppsPlaying)
                {
                    lblCurrentTrack.Text = lblCurrentTrack.Text.Replace("Paused: ", "Playing: ");

                    mediaPlayer.Ctlcontrols.currentPosition = currentPlayPosition;
                    mediaPlayer.Ctlcontrols.play();
                }

            }
        }
        private void SetCurrentTrackColor(int rowIndex = -1)
        {
            for (int i = 0; i < dgvTrackList.Rows.Count; i++)
            {
                if (dgvTrackList.Rows[i].DefaultCellStyle.BackColor == Color.LightSeaGreen)
                {
                    dgvTrackList.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    break;
                }
            }
            if (trackIdInPlaylist != -1)
            {
                int idInPlaylist = -1;
                for (int i = 0; i < dgvTrackList.Rows.Count; i++)
                {
                    idInPlaylist = Convert.ToInt32(dgvTrackList.Rows[i].Cells["OrderInList"].Value);
                    if (trackIdInPlaylist == idInPlaylist)
                    {
                        dgvTrackList.Rows[i].DefaultCellStyle.BackColor = Color.LightSeaGreen;
                        break;
                    }
                }
            }
            else
            {
                if(rowIndex > -1)
                {
                    dgvTrackList.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightSeaGreen;
                }
            }
        }
        //PAUSE TRACK
        private void PauseTrack()
        {
            if (mediaPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                lblCurrentTrack.Text = lblCurrentTrack.Text.Replace("Playing: ", "Paused: ");

                currentPlayPosition = mediaPlayer.Ctlcontrols.currentPosition;
                mediaPlayer.Ctlcontrols.pause();
            }
        }
        //NEXT/PREVIOUS TRACK
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
        private void PrevTrack()
        {
            this.StepTrack(true);
        }
        private void NextTrack()
        {
            this.StepTrack(false);
        }
        private void StepTrack(bool backward)
        {
            if (trackIdInPlaylist == -1)
            {
                if (dgvTrackList.Rows.Count > 0)
                {
                    if (dgvTrackList.SelectedRows.Count > 0)
                    {
                        trackIdInPlaylist = Convert.ToInt32(dgvTrackList.SelectedRows[0].Cells["OrderInList"].Value);
                        lastTrackIndex = dgvTrackList.Rows.IndexOf(dgvTrackList.SelectedRows[0]);
                    }
                }
            }
            else
            {
                if (dgvTrackList.Rows.Count > 0)
                {
                    if (dgvTrackList.SelectedRows.Count > 0)
                    {
                        if (dgvTrackList.Rows.Cast<DataGridViewRow>().ToList().Exists(x => Convert.ToInt32(x.Cells["OrderInList"].Value) == trackIdInPlaylist))
                        {
                            lastTrackIndex = dgvTrackList.Rows.Cast<DataGridViewRow>().Where(x => Convert.ToInt32(x.Cells["OrderInList"].Value) == trackIdInPlaylist).First().Index;
                        }
                    }
                    else
                    {
                        if (dgvTrackList.Rows.Cast<DataGridViewRow>().ToList().Exists(x => Convert.ToInt32(x.Cells["OrderInList"].Value) == trackIdInPlaylist))
                        {
                            lastTrackIndex = dgvTrackList.Rows.Cast<DataGridViewRow>().Where(x => Convert.ToInt32(x.Cells["OrderInList"].Value) == trackIdInPlaylist).First().Index;
                        }
                    }
                }
            }

            if (!backward) 
            {
                if (lastTrackIndex < dgvTrackList.Rows.Count - 1)
                {
                    lastTrackIndex = lastTrackIndex + 1;
                }
            }
            else 
            { 
                if (lastTrackIndex > 0) 
                { 
                    lastTrackIndex = lastTrackIndex - 1; 
                } 
            }

            currentTrackIndex = lastTrackIndex;
            this.StopTrack();
            this.PlayTrack();
        }
        //STOP TRACK
        private void StopTrack()
        {
            currentPlayPosition = 0;
            trackIdInPlaylist = -1;

            ClearCurrentTrackColor();

            lblCurrentTrack.Text = "Playing: -";
            lblTrackStart.Text = "";
            lblTrackEnd.Text = "";
            pBar.Value = 0;
            mediaPlayer.Ctlcontrols.stop();
            mediaPlayer.Ctlcontrols.currentPosition = 0;
        }
        private void ClearCurrentTrackColor()
        {
            for (int i = 0; i < dgvTrackList.Rows.Count; i++)
            {
                if (dgvTrackList.Rows[i].DefaultCellStyle.BackColor == Color.LightSeaGreen)
                {
                    dgvTrackList.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    break;
                }
            }
        }
        //RANDOM TRACK
        private void RandomTrack()
        {
            if (dgvTrackList.Rows.Count > 0)
            {
                Random rand = new Random();
                currentTrackIndex = rand.Next(0, dgvTrackList.Rows.Count - 1);
                this.StopTrack();
                this.PlayTrack();
            }
        }
        //VOLUME
        private void trackVolume_Scroll(object sender, EventArgs e)
        {
            lblVolume.Text = trackVolume.Value.ToString() + "%";
            mediaPlayer.settings.volume = trackVolume.Value;
            ListEventArgs args = new ListEventArgs();
            args.IntegerField1 = trackVolume.Value;
            ChangeVolume?.Invoke(this, args);
        }
        //PROGRESS BAR
        private void pBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (mediaPlayer != null && mediaPlayer.Ctlcontrols != null && mediaPlayer.Ctlcontrols.currentItem != null)
            {
                mediaPlayer.Ctlcontrols.currentPosition = mediaPlayer.currentMedia.duration * e.X / pBar.Width;
            }
                
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (mediaPlayer != null && mediaPlayer.Ctlcontrols != null && mediaPlayer.Ctlcontrols.currentItem != null)
            {
                if (mediaPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
                {
                    pBar.Maximum = (int)mediaPlayer.Ctlcontrols.currentItem.duration;
                    pBar.Value = (int)mediaPlayer.Ctlcontrols.currentPosition;

                    if (mediaPlayer.Ctlcontrols.currentItem.duration > 0 && 
                        mediaPlayer.Ctlcontrols.currentPosition >= mediaPlayer.Ctlcontrols.currentItem.duration-0.3)
                    {
                        NextTrack();
                    }
                }
                try
                {
                    if (mediaPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying)
                    {
                        if(mediaPlayer.Ctlcontrols.currentPositionString.Length > 5)
                        {
                            lblTrackStart.Text = mediaPlayer.Ctlcontrols.currentPositionString;
                        }
                        else
                        {
                            lblTrackStart.Text = "00:" + mediaPlayer.Ctlcontrols.currentPositionString;
                        }
                        if (mediaPlayer.Ctlcontrols.currentItem.durationString.ToString().Length > 5)
                        {
                            lblTrackEnd.Text = mediaPlayer.Ctlcontrols.currentItem.durationString.ToString();
                        }
                        else
                        {
                            lblTrackEnd.Text = "00:" + mediaPlayer.Ctlcontrols.currentItem.durationString.ToString();
                        }
                    }
                    else
                    {
                        lblTrackStart.Text = "";
                        lblTrackEnd.Text = "";
                    }
                }catch{}
            }
        }
        //SELECTED TRACK COUNT AND LENGTH
        private void dgvTrackList_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvTrackList.SelectedRows != null && dgvTrackList.SelectedRows.Count > 0)
            {
                //kijelölt elemek számának megjelenítése
                if (dgvTrackList.SelectedRows.Count == 1)
                {
                    lblSelectedItemsCount.Text = dgvTrackList.SelectedRows.Count.ToString() + " item selected";
                }
                else
                {
                    lblSelectedItemsCount.Text = dgvTrackList.SelectedRows.Count.ToString() + " items selected";
                }

                String[] parts = null;

                int seconds = 0;

                for (int i = 0; i < dgvTrackList.SelectedRows.Count; i++)
                {

                    parts = dgvTrackList.SelectedRows[i].Cells["Length"].Value.ToString().Split(':');

                    if (parts.Length == 3)
                    {
                        seconds += Int32.Parse(parts[0]) * 60 * 60;
                        seconds += Int32.Parse(parts[1]) * 60;
                        seconds += Int32.Parse(parts[2]);
                    }
                    else if (parts.Length == 2)
                    {
                        seconds += Int32.Parse(parts[0]) * 60;
                        seconds += Int32.Parse(parts[1]);
                    }
                    else if (parts.Length == 1)
                    {
                        seconds += Int32.Parse(parts[0]);
                    }
                }

                //kijelölt elemek felvétel hossza
                TimeSpan t = TimeSpan.FromSeconds(seconds);
                String length = String.Empty;
                if (t.Hours > 0)
                {
                    length = string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);
                }
                else
                {
                    if (t.Minutes > 0)
                    {
                        length = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
                    }
                    else
                    {
                        length = string.Format("{0:D2}:{1:D2}", 0, t.Seconds);
                    }
                }
                lblSelectedItemsLength.Text = "Length: " + length.ToString();
            }
        }
        #endregion

        #region TRACKLIST
        //ORDER BY COLUMNS
        private void dgvTrackList_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ListEventArgs args = new ListEventArgs();
            args.ColumnIndex = e.ColumnIndex;
            OrderTableByColumn?.Invoke(this, args);

            this.SetCurrentTrackColor();
        }
        //KEY PRESS - SELECT AND DELETE
        /*
         * ha ctrl-t vagy shift-et nyomunk, bekapcsol egy flag
         * delet gombra a kijelölt elemek törlődnek
         */
        private void dgvTrackList_KeyDown(object sender, KeyEventArgs e)
        {
            if (dgvTrackList.Rows.Count > 0 && e.KeyCode == Keys.Delete)
            {
                ListEventArgs args = new ListEventArgs();
                args.Rows = dgvTrackList.Rows;
                DeleteTracks?.Invoke(this, args);

                if(dgvTrackList.Rows == null || dgvTrackList.Rows.Count == 0)
                {
                    trackIdInPlaylist = -1;
                    currentTrackIndex = -1;
                    lastTrackIndex = -1;
                }
            }
            if (dgvTrackList.Rows.Count > 0 && (e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.ShiftKey))
            {
                controlKey = true;
            }
        }
        /*
         * ha ctrl-t vagy shift-et engedünk el, kikapcsol egy flag
         */
        private void dgvTrackList_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.ShiftKey)
            {
                controlKey = false;
            }
        }

        //ROW SELECTION
        private bool mouseDown = false;
        private int currentRowIndex = -1;
        private bool controlKey = false;
        private bool dragEnabled = false;
        private bool dragOneRow = false;
        private int initialFirstDisplayedScrollingRowIndex = -1;
        /*
         * jobb klikk egy sorra, mouseDown flaget bekapcsoljuk
         * ha a sor nincs kijelölve, kijelöli és elmenti az indexet
         * ha a sor már ki van jelölve, nem nyomunk közben ctrl-t és shift-et és nem az összes sor van kijelölve, akkor aktiválódik a drag and drop
         * ha csak egy sort jelöltünk ki, akkor a drag and drop később, a mouse move-ban aktiválódik
         * ha több sor van, akkor a drag and drop azonnal aktiválódik
         */
        private void dgvTrackList_MouseDown(object sender, MouseEventArgs e) 
        {
           
            mouseDown = true;
            HitTestInfo hitTest = dgvTrackList.HitTest(e.X, e.Y);
            if (hitTest != null && hitTest.RowIndex > -1) 
            {
                currentRowIndex = hitTest.RowIndex;

                if (dgvTrackList.Rows[hitTest.RowIndex].Selected)
                {
                    if (!controlKey)
                    {
                        if(dgvTrackList.Rows.Count != dgvTrackList.SelectedRows.Count)
                        {
                            dragEnabled = true;
                            if (dgvTrackList.SelectedRows.Count > 1)
                            {
                                dragOneRow = false;
                                this.Drag();
                            }
                            else
                            {
                                dragOneRow = true;
                            }
                        }
                    }
                }
            }
        }
        //DRAG
        /*
         * aktiválja a drag flag-et
         * ha vannak kijelölt sorok, lementi és törli őket a táblából
         * törli a kijelölést
         * indítja a draganddrop-ot
         */
        int rowCount = -1;
        private void Drag()
        {
            initialFirstDisplayedScrollingRowIndex = dgvTrackList.FirstDisplayedScrollingRowIndex;
            rowCount = dgvTrackList.RowCount;

            SaveSelectedRows();
            RemoveSelectedRows();
            
            dgvTrackList.ClearSelection();
            dgvTrackList.Invalidate();

            BindingSource dataSource = selectedTrackListBindingSource.DataSource as BindingSource;
            DataTable targetTable = dataSource.DataSource as DataTable;
            dgvTrackList.DoDragDrop(targetTable, DragDropEffects.Move);

            dgvTrackList.Capture = true;
        }
        /*
         * a céltáblát törli
         * végigmegy a forrástábla elemein és ami ki van jelölve, hozzáadja egy másik táblához
         */
        private void SaveSelectedRows()
        {
            BindingSource dataSource = trackListBindingSource.DataSource as BindingSource;
            DataTable sourceTable = dataSource.DataSource as DataTable;
            BindingSource dataSource2 = selectedTrackListBindingSource.DataSource as BindingSource;
            DataTable targetTable = dataSource2.DataSource as DataTable;

            targetTable.Rows.Clear();

            if (dgvTrackList.SelectedRows != null && dgvTrackList.SelectedRows.Count > 0)
            {
                for (int i = 0; i < dgvTrackList.Rows.Count; i++)
                {
                    if (dgvTrackList.Rows[i].Selected)
                    {
                        DataRow dataRow = sourceTable.Rows[i];
                        targetTable.Rows.Add(dataRow.ItemArray);
                    }
                }
            }
        }
        /*
         * ha van kijelölt sor a táblában, végigmegy a tábla elemein visszafele és ami ki van jelölve, azt törli
         */
        private void RemoveSelectedRows() 
        {
            if (dgvTrackList.SelectedRows != null && dgvTrackList.SelectedRows.Count > 0)
            {
                for (int i = dgvTrackList.Rows.Count - 1; i >= 0; i--)
                {
                    if (dgvTrackList.Rows[i].Selected)
                    {
                        dgvTrackList.Rows.RemoveAt(i);
                    }
                }
            }
        }
        private int dropLocationRowIndex = -1;
        //DRAG OVER
        /*
         * beállítja a drag ikonját (filedrop vagy szám mozgatása
         * vezérli a scrollozást
         */
        private void dgvTrackList_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
            else
            {
                e.Effect = DragDropEffects.Move;
            }

            //scrollozás - 93 a tábla magassága!! (ezt át kell állítani, ha változik a méret)
            int mousepos = PointToClient(Cursor.Position).Y;

            if (mousepos > (93+dgvTrackList.Location.Y + (dgvTrackList.Height * 0.95)))
            {
                if (dgvTrackList.FirstDisplayedScrollingRowIndex < dgvTrackList.RowCount - 1)
                {
                    dgvTrackList.FirstDisplayedScrollingRowIndex = dgvTrackList.FirstDisplayedScrollingRowIndex + 1;
                }
            }
            if (mousepos < (93+dgvTrackList.Location.Y + (dgvTrackList.Height * 0.05)))
            {
                if (dgvTrackList.FirstDisplayedScrollingRowIndex > 0)
                {
                    dgvTrackList.FirstDisplayedScrollingRowIndex = dgvTrackList.FirstDisplayedScrollingRowIndex - 1;
                }
            }

            Point clientPoint = dgvTrackList.PointToClient(new Point(e.X, e.Y));
            dropLocationRowIndex = dgvTrackList.HitTest(clientPoint.X, clientPoint.Y).RowIndex;
            dgvTrackList.Invalidate();
        }

        System.Windows.Forms.DataGridView.HitTestInfo hti = null;
        System.Threading.Timer scrollTimer = null;

        //SCROLL
        private bool scrolling = false;
        /*
         * ha a drag be van kapcsolva és a bal klikk le van nyomva, egy kijelölt sor esetén itt aktiválódik a drag
         * az egér mozgatásra a tábla scrollozódik
         */
        private void dgvTrackList_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragEnabled && e.Button == MouseButtons.Left)
            {
                Point p = dgvTrackList.PointToClient(new Point(e.X, e.Y));
                int dragIndex = dgvTrackList.HitTest(p.X, p.Y).RowIndex;

                if (dragOneRow && currentRowIndex != dragIndex)
                {
                    this.Drag();
                }

                DataGridView.HitTestInfo newhti = dgvTrackList.HitTest(e.X, e.Y);
                if (hti != null && hti.RowIndex != newhti.RowIndex)
                {
                    System.Diagnostics.Debug.WriteLine("invalidating " + hti.RowIndex.ToString());
                    Invalidate();
                }
                hti = newhti;

                System.Diagnostics.Debug.WriteLine(string.Format("{0:000} {1}  ", hti.RowIndex, e.Location));

                Point clientPoint = this.PointToClient(e.Location);

                System.Diagnostics.Debug.WriteLine(e.Location + "  " + this.Bounds.Size);

                if (scrollTimer == null && ShouldScrollDown(e.Location))
                {
                    scrollTimer = new System.Threading.Timer(new System.Threading.TimerCallback(TimerScroll), 1, 0, 250);
                }
                if (scrollTimer == null && ShouldScrollUp(e.Location))
                {
                    scrollTimer = new System.Threading.Timer(new System.Threading.TimerCallback(TimerScroll), -1, 0, 250);
                }
            }
            else
            {
                dragEnabled = false;
            }
            if (!(ShouldScrollUp(e.Location) || ShouldScrollDown(e.Location)))
            {
                StopAutoScrolling();
                if (scrollTimer != null)
                {
                    scrollTimer.Dispose();
                    scrollTimer = null;
                }
            }
        }
        bool ShouldScrollUp(Point location)
        {
            return location.Y > dgvTrackList.ColumnHeadersHeight
                && location.Y < dgvTrackList.ColumnHeadersHeight + 15
                && location.X >= 0
                && location.X <= this.Bounds.Width;
        }
        bool ShouldScrollDown(Point location)
        {
            return location.Y > this.Bounds.Height - 15
                && location.Y < this.Bounds.Height
                && location.X >= 0
                && location.X <= this.Bounds.Width;
        }
        void StopAutoScrolling()
        {
            if (scrollTimer != null)
            {
                scrollTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                scrollTimer = null;
            }
        }
        void TimerScroll(object state)
        {
            SetScrollBar((int)state);
        }
        void SetScrollBar(int direction)
        {
            if (scrolling)
            {
                return;
            }
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<int>(SetScrollBar), new object[] { direction });
            }
            else
            {
                scrolling = true;

                if (0 < direction)
                {
                    if (dgvTrackList.FirstDisplayedScrollingRowIndex < dgvTrackList.Rows.Count - 1)
                    {
                        dgvTrackList.FirstDisplayedScrollingRowIndex++;
                    }
                }
                else
                {
                    if (dgvTrackList.FirstDisplayedScrollingRowIndex > 0)
                    {
                        dgvTrackList.FirstDisplayedScrollingRowIndex--;
                    }
                }

                scrolling = false;
            }

        }
        /*
         * rajzol egy vonalat a drag and drop közben, ami jelöli, hogy hova szúrjuk be az elemeket
         */
        private void dgvTrackList_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == dropLocationRowIndex - 1)
            {
                e.Paint(e.ClipBounds, DataGridViewPaintParts.All);
                var graphics = e.Graphics;
                var cellBounds = e.CellBounds;
                var pen = new Pen(Color.Black, 4);
                graphics.DrawLine(pen, cellBounds.Left, cellBounds.Bottom, cellBounds.Right, cellBounds.Bottom);
                e.Handled = true;
            }
        }
        //MOUSE UP
        private void dgvTrackList_MouseUp(object sender, MouseEventArgs e)
        {
            mouseDown = false;

            if(rowCount > -1 && rowCount != dgvTrackList.RowCount)
            {
                if (Control.MouseButtons != MouseButtons.Left)
                {
                    rowCount = -1;
                    this.Drop(currentRowIndex);
                    dgvTrackList.Capture = false;
                }
            }
        }
        //DROP
        private List<int> selectionIndicesAfterDragAndDrop = new List<int>();
        /*
         * külső fájlokat húzunk be vagy számokat drag-elünk és elengedjük az egér gombot
         */
        private void dgvTrackList_DragDrop(object sender, DragEventArgs e)
        {
            Point p = dgvTrackList.PointToClient(new Point(e.X, e.Y));
            int dragIndex = dgvTrackList.HitTest(p.X, p.Y).RowIndex;

            if (e.Effect == DragDropEffects.Link)
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                ListEventArgs args = new ListEventArgs();
                args.DragAndDropFiles = files;
                args.IntegerField1 = dragIndex;
                ScanFiles?.Invoke(this, args);
            } 
            else if (dragEnabled && e.Effect == DragDropEffects.Move)
            {
                this.Drop(dragIndex);
            }
        }
        private void Drop(int dragIndex)
        {
            this.InsertSelectedRows(dragIndex);

            currentRowIndex = -1;

            this.BeginInvoke(new Action(() =>
            {
                dgvTrackList.ClearSelection();

                if (selectionIndicesAfterDragAndDrop != null && selectionIndicesAfterDragAndDrop.Count > 0)
                {
                    foreach (int i in selectionIndicesAfterDragAndDrop)
                    {
                        dgvTrackList.Rows[i].Selected = true;
                    }
                    initialFirstDisplayedScrollingRowIndex = dgvTrackList.FirstDisplayedScrollingRowIndex;
                    dgvTrackList.FirstDisplayedScrollingRowIndex = initialFirstDisplayedScrollingRowIndex;
                }
                selectionIndicesAfterDragAndDrop = new List<int>();

                this.SetCurrentTrackColor();
            }));

            //változók resetelése
            dragEnabled = false;
            dragOneRow = false;
            mouseDown = false;

            //scroll kikapcsolása
            hti = null;
            this.StopAutoScrolling();
            this.Invalidate();

            ListEventArgs args = new ListEventArgs();
            args.Rows = dgvTrackList.Rows;
            this.TrackDragAndDrop?.Invoke(this, args);
        }
        /*
         * visszarakni a sorokat az eredeti helyükre, ha ki drop-oljuk a grid-en kívül
         */
        private void dgvTrackList_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            if (Control.MouseButtons != MouseButtons.Left)
            {
                if (!dgvTrackList.ClientRectangle.Contains(dgvTrackList.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y))))
                {
                    e.Action = DragAction.Cancel;
                    this.Drop(currentRowIndex);
                }
                dgvTrackList.Capture = false;
            }
        }
        /*
         * beszúrja az elmentett sorokat a megadott indexre
         */
        private void InsertSelectedRows(int insertIndex)
        {
            BindingSource dataSource = selectedTrackListBindingSource.DataSource as BindingSource;
            DataTable sourceTable = dataSource.DataSource as DataTable;

            BindingSource dataSource2 = trackListBindingSource.DataSource as BindingSource;
            DataTable targetTable = dataSource2.DataSource as DataTable;

            if (insertIndex == -1)
            {
                insertIndex = targetTable.Rows.Count;
            }

            for (int i = 0; i < sourceTable.Rows.Count; i++)
            {
                var desRow = targetTable.NewRow();
                var sourceRow = sourceTable.Rows[i];
                desRow.ItemArray = sourceRow.ItemArray.Clone() as object[];
                targetTable.Rows.InsertAt(desRow, insertIndex);
                selectionIndicesAfterDragAndDrop.Add(insertIndex+i);
            }

            dropLocationRowIndex = -1;
            sourceTable.Rows.Clear();
            trackListBindingSource.ResetBindings(false);
            dgvTrackList.Invalidate();
        }

        #endregion

        #region PLAYLIST LIST
        //CONTECT MENU
        private String selectedPlaylistName;
        private void dgvPlaylistList_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(Cursor.Position);
            }
        }
        //ADD PLAYLIST
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CreatePlaylist();
        }
        private void CreatePlaylistExt()
        {
            ListEventArgs args = new ListEventArgs();
            args.IntegerField1 = -1;
            this.ShowPlaylistEditorView?.Invoke(this, args);
        }
        //EDIT PLAYLIST
        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.RenamePlaylist();
        }
        private void RenamePlaylistExt()
        {
            if (dgvPlaylistList.SelectedRows.Count > 0)
            {
                ListEventArgs args = new ListEventArgs();
                args.IntegerField1 = dgvPlaylistList.SelectedRows[0].Index;
                this.ShowPlaylistEditorView?.Invoke(this, args);
            }
        }
        //LOAD PLAYLIST
        private void dgvPlaylistList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            LoadPlaylistExt();
        }

        public void LoadPlaylistExt()
        {
            ListEventArgs args = new ListEventArgs();
            args.IntegerField1 = dgvPlaylistList.SelectedRows[0].Index;
            LoadPlaylist?.Invoke(this, args);

            if(mediaPlayer.playState == WMPLib.WMPPlayState.wmppsPlaying || mediaPlayer.playState == WMPLib.WMPPlayState.wmppsPaused)
            {
                String path1 = mediaPlayer.URL;
                for(int i = 0; i<= dgvTrackList.Rows.Count - 1; i++)
                {
                    if (((string)dgvTrackList.Rows[i].Cells["Path"].Value).Equals(path1))
                    {
                        this.SetCurrentTrackColor(i);
                        break;
                    }
                }
            }

            trackIdInPlaylist = -1;
            currentTrackIndex = -1;
            lastTrackIndex = -1;
        }

        //DELETE PLAYLIST
        private void removeStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvPlaylistList.SelectedRows.Count > 0)
            {
                this.DeletePlaylistExt();
            }
        }
        //DELETE PLAYLIST BY PRESSING DEL
        private void dgvPlaylistList_KeyDown(object sender, KeyEventArgs e)
        {
            if (dgvPlaylistList.Rows.Count > 0 && dgvPlaylistList.SelectedRows.Count > 0 && e.KeyCode == Keys.Delete)
            {
                this.DeletePlaylistExt();
            }
        }
        public void DeletePlaylistExt()
        {
            ListEventArgs args = new ListEventArgs();
            args.IntegerField1 = dgvPlaylistList.SelectedRows[0].Index;
            DeletePlaylist?.Invoke(this, args);

            if (dgvTrackList.Rows == null || dgvTrackList.Rows.Count == 0)
            {
                trackIdInPlaylist = -1;
                currentTrackIndex = -1;
                lastTrackIndex = -1;
            }
        }
        #endregion

        private void mediaPlayer_Enter(object sender, EventArgs e)
        {

        }

        private void lblTrackStart_Click(object sender, EventArgs e)
        {

        }
    }
}
