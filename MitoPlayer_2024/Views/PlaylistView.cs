using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Properties;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MitoPlayer_2024.Views
{
    public partial class PlaylistView : Form, IPlaylistView
    {
        private Form parentView { get; set; }

        private System.Windows.Forms.Panel pnlProgress;
        private System.Windows.Forms.ProgressBar progressBar;

        //DATATABLES
        private BindingSource playlistListBindingSource { get; set; }
        private BindingSource trackListBindingSource { get; set; }

        //PLAYER
        public event EventHandler<Messenger> SetCurrentTrackEvent;
        public event EventHandler<Messenger> PlayTrackEvent;
        public event EventHandler PauseTrackEvent;
        public event EventHandler StopTrackEvent;
        public event EventHandler CopyCurrentPlayingTrackToDefaultPlaylistEvent;
        public event EventHandler<Messenger> PrevTrackEvent;
        public event EventHandler<Messenger> NextTrackEvent;
        public event EventHandler RandomTrackEvent;
        public event EventHandler JumpBackwardEvent;
        public event EventHandler JumpForwardEvent;
        public event EventHandler<Messenger> ChangeVolumeEvent;
        public event EventHandler<Messenger> ChangeProgressEvent;
       // public event EventHandler GetMediaPlayerProgressStatusEvent;

        //TRACKLIST
        public event EventHandler<Messenger> OrderByColumnEvent;
        public event EventHandler<Messenger> DeleteTracksEvent;
        public event EventHandler<Messenger> InternalDragAndDropIntoTracklistEvent;
        public event EventHandler<Messenger> InternalDragAndDropIntoPlaylistEvent;
        public event EventHandler<Messenger> ExternalDragAndDropIntoTracklistEvent;
        public event EventHandler<Messenger> ExternalDragAndDropIntoPlaylistEvent;
        //public event EventHandler<ListEventArgs> ChangeTracklistColorEvent;
        public event EventHandler ShowColumnVisibilityEditorEvent;
        public event EventHandler ScanKeyAndBpmEvent;

        public event EventHandler<Messenger> MoveTracklistRowsEvent;

        //PLAYLIST
        public event EventHandler<Messenger> CreatePlaylist;
        public event EventHandler<Messenger> EditPlaylist;
        public event EventHandler<Messenger> LoadPlaylistEvent;
        public event EventHandler<Messenger> MovePlaylistEvent;
        public event EventHandler<Messenger> DeletePlaylistEvent;
        public event EventHandler<Messenger> SetQuickListEvent;
        public event EventHandler<Messenger> ExportToM3UEvent;
        public event EventHandler<Messenger> ExportToTXTEvent;
        public event EventHandler<Messenger> ExportToDirectoryEvent;
        public event EventHandler<Messenger> MovePlaylistRowEvent;
        public event EventHandler DisplayPlaylistListEvent;

        //TAG EDITOR
        public event EventHandler DisplayTagEditorEvent;
        public event EventHandler<Messenger> SelectTagEvent;
        public event EventHandler<Messenger> SetTagValueEvent;
        public event EventHandler<Messenger> ClearTagValueEvent;

        public event EventHandler<Messenger> ChangeFilterModeEnabled;
        public event EventHandler EnableFilterModeEvent;
        public event EventHandler EnableSetterModeEvent;
        public event EventHandler<Messenger> LoadCoversEvent;
        public event EventHandler<Messenger> ChangeOnlyPlayingRowModeEnabled;
        public event EventHandler<Messenger> ChangeFilterParametersEvent;
        public event EventHandler RemoveTagValueFilter;

        public event EventHandler SaveTrackListEvent;

        public event EventHandler TrainKeyDetectorEvent;
        
        public event EventHandler<Messenger> DetectKeyEvent;
        public event EventHandler<Messenger> AddToKeyDetectorEvent;
        
        public event EventHandler CreateModelEvent;

        public event EventHandler OpenModelTrainerEvent;

        public event EventHandler LiveStreamAnimationEvent;
        public event EventHandler LiveStreamAnimationSettingEvent;
        public event EventHandler DisplayCoverImageComponentEvent;

        private int trackListLeftOffset = 190;
        private int trackListRightOffset = 285;
        private int trackListTopOffset = 58;
        private int tagValueEditorPanelBottomOffset = 25;
        private int coverImageSize = 60;
        private int mainCoverImageSize = 75;

        private bool isFilterEnabled = false;
        private bool isInitializing = true;
        public PlaylistView()
        {

            this.InitializeComponent();
            this.SetControlColors();

            this.playlistListBindingSource = new BindingSource();
            this.trackListBindingSource = new BindingSource();

            dgvTrackList.AllowDrop = true;
            dgvPlaylistList.AllowDrop = true;

            tracklistClickTimer = new System.Windows.Forms.Timer();
            tracklistClickTimer.Interval = tracklistDoubleClickTime;
            tracklistClickTimer.Tick += TracklistClickTimer_Tick;

            playlistListClickTimer = new System.Windows.Forms.Timer();
            playlistListClickTimer.Interval = playlistListDoubleClickTime;
            playlistListClickTimer.Tick += PlaylistListClickTimer_Tick;
            
            coverBrowserClickTimer = new System.Windows.Forms.Timer();
            coverBrowserClickTimer.Interval = coverBrowserDoubleClickTime;
            coverBrowserClickTimer.Tick += CoverBrowserClickTimer_Tick;
            
            autoScrollTimer.Interval = 100; // Adjust the interval as needed
            autoScrollTimer.Tick += AutoScrollTimer_Tick;

            isInitializing = true;

            //InitializeBackgroundWorker();
        }
       
        //Dark Color Theme
        Color BackgroundColor = System.Drawing.ColorTranslator.FromHtml("#363639");
        Color FontColor = System.Drawing.ColorTranslator.FromHtml("#c6c6c6");
        Color ButtonColor = System.Drawing.ColorTranslator.FromHtml("#292a2d");
        Color ButtonBorderColor = System.Drawing.ColorTranslator.FromHtml("#1b1b1b");
        Color GridLineColor1 = System.Drawing.ColorTranslator.FromHtml("#131315");
        Color GridLineColor2 = System.Drawing.ColorTranslator.FromHtml("#212224");
        Color GridPlayingColor = System.Drawing.ColorTranslator.FromHtml("#4d4d4d");
        Color GridSelectionColor = System.Drawing.ColorTranslator.FromHtml("#626262");
        Color ActiveButtonColor = System.Drawing.ColorTranslator.FromHtml("#FFBF80");
        Color RedWarningColor = System.Drawing.ColorTranslator.FromHtml("#ED6E7D");
        
        private void SetControlColors()
        {
            this.BackColor = this.BackgroundColor;
            this.ForeColor = this.FontColor;

            //this.groupBoxPlaylist.ForeColor = this.FontColor;
          /*  this.groupBox4.ForeColor = this.FontColor;
            this.groupBox3.ForeColor = this.FontColor;*/

          /*  this.btnNewPlaylist.BackColor = this.ButtonColor;
            this.btnNewPlaylist.ForeColor = this.FontColor;
            this.btnNewPlaylist.FlatAppearance.BorderColor = this.ButtonBorderColor;

            this.btnLoadPlaylist.BackColor = this.ButtonColor;
            this.btnLoadPlaylist.ForeColor = this.FontColor;
            this.btnLoadPlaylist.FlatAppearance.BorderColor = this.ButtonBorderColor;

            this.btnRenamePlaylist.BackColor = this.ButtonColor;
            this.btnRenamePlaylist.ForeColor = this.FontColor;
            this.btnRenamePlaylist.FlatAppearance.BorderColor = this.ButtonBorderColor;

            this.btnDeletePlaylist.BackColor = this.ButtonColor;
            this.btnDeletePlaylist.ForeColor = this.FontColor;
            this.btnDeletePlaylist.FlatAppearance.BorderColor = this.ButtonBorderColor;*/

            this.btnPlaylistListPanelToggle.BackColor = this.ButtonColor;
            this.btnPlaylistListPanelToggle.ForeColor = this.FontColor;
            this.btnPlaylistListPanelToggle.FlatAppearance.BorderColor = this.ButtonBorderColor;

            this.btnDisplayTagComponentToggle.BackColor = this.ButtonColor;
            this.btnDisplayTagComponentToggle.ForeColor = this.FontColor;
            this.btnDisplayTagComponentToggle.FlatAppearance.BorderColor = this.ButtonBorderColor;

            this.btnDisplayCoverImageToggle.BackColor = this.ButtonColor;
            this.btnDisplayCoverImageToggle.ForeColor = this.FontColor;
            this.btnDisplayCoverImageToggle.FlatAppearance.BorderColor = this.ButtonBorderColor;

            this.btnColumnVisibilityWithTagEditor.BackColor = this.ButtonColor;
            this.btnColumnVisibilityWithTagEditor.ForeColor = this.FontColor;
            this.btnColumnVisibilityWithTagEditor.FlatAppearance.BorderColor = this.ButtonBorderColor;

            this.dgvPlaylistList.BackgroundColor = this.ButtonColor;
            this.dgvPlaylistList.ColumnHeadersDefaultCellStyle.BackColor = this.ButtonColor;
            this.dgvPlaylistList.ColumnHeadersDefaultCellStyle.ForeColor = this.FontColor;
            this.dgvPlaylistList.EnableHeadersVisualStyles = false;
            this.dgvPlaylistList.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.ButtonColor;
            this.dgvPlaylistList.DefaultCellStyle.SelectionBackColor = this.GridSelectionColor;

            this.dgvTrackList.BackgroundColor = this.ButtonColor;
            this.dgvTrackList.ColumnHeadersDefaultCellStyle.BackColor = this.ButtonColor;
            this.dgvTrackList.ColumnHeadersDefaultCellStyle.ForeColor = this.FontColor;
            this.dgvTrackList.EnableHeadersVisualStyles = false;
            this.dgvTrackList.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.ButtonColor;
            this.dgvTrackList.DefaultCellStyle.SelectionBackColor = this.GridSelectionColor;

            this.btnScanKeyAndBpm.BackColor = this.ButtonColor;
            this.btnScanKeyAndBpm.ForeColor = this.FontColor;
            this.btnScanKeyAndBpm.FlatAppearance.BorderColor = this.ButtonBorderColor;

            this.contextMenuStrip1.BackColor = this.BackColor;
            this.contextMenuStrip1.ForeColor = this.FontColor;

            this.btnSave.BackColor = this.ButtonColor;
            this.btnSave.ForeColor = this.FontColor;
            this.btnSave.FlatAppearance.BorderColor = this.ButtonBorderColor;

            this.lblMessage.ForeColor = this.ActiveButtonColor;

            this.btnGenerateTrainingSet.BackColor = this.ButtonColor;
            this.btnGenerateTrainingSet.ForeColor = this.FontColor;
            this.btnGenerateTrainingSet.FlatAppearance.BorderColor = this.ButtonBorderColor;

        }

        #region SINGLETON

        public static PlaylistView instance;
        public static PlaylistView GetInstance(Form parentView)
        {
            if (instance == null || instance.IsDisposed)
            {
                instance = new PlaylistView();
                instance.parentView = parentView;
                instance.MdiParent = parentView;
                instance.FormBorderStyle = FormBorderStyle.None;
                instance.Dock = DockStyle.Fill;
                instance.WindowState = FormWindowState.Normal;
            }
            else
            {
                instance.BringToFront();
            }
            return instance;
        }
        #endregion

        #region TABLE BINDINGS AND INIT

        //TAGS AND TAGVALUES
        private List<Tag> tagList { get; set; }
        private Dictionary<String, Dictionary<String, Color>> tagValueDictionary { get; set; }
        public void InitializeTagsAndTagValues(List<Tag> tagList, Dictionary<String, Dictionary<String, Color>> tagValueDictionary)
        {
            this.tagList = tagList;
            this.tagValueDictionary = tagValueDictionary;
        }

        //PLAYLIST DATA BINDING
        private int currentPlaylistId { get; set; }
        public void InitializePlaylistList(DataTableModel model)
        {
            if (model.BindingSource != null)
            {
                this.playlistListBindingSource.DataSource = model.BindingSource;
                this.dgvPlaylistList.DataSource = this.playlistListBindingSource.DataSource;
            }

            if (model.ColumnVisibilityArray != null && model.ColumnVisibilityArray.Length > 0)
            {
                for (int i = 0; i <= this.dgvPlaylistList.Columns.Count - 1; i++)
                {
                    this.dgvPlaylistList.Columns[i].Visible = model.ColumnVisibilityArray[i];
                    this.dgvPlaylistList.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }
            }

            if (model.CurrentPlaylistId != -1)
            {
                this.currentPlaylistId = model.CurrentPlaylistId;
                this.lblActualPlaylistName.Text = "[" + model.CurrentPlaylistName + "]";
            }

            this.playlistListBindingSource.ResetBindings(false);

            this.UpdatePlaylistListColor(model.CurrentPlaylistId);
        }
        private void dgvPlaylistList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            this.UpdatePlaylistListColor(this.currentPlaylistId);
        }
        public void ReloadPlaylistList(DataTableModel model)
        {
            this.UpdatePlaylistListColor(model.CurrentPlaylistId);
        }
        public void UpdatePlaylistListColor(int currentPlaylistId = -1)
        {
            int playlistId = -1;
            if (this.dgvPlaylistList != null && this.dgvPlaylistList.Rows != null && this.dgvPlaylistList.Rows.Count > 0)
            {
                for (int i = 0; i < this.dgvPlaylistList.Rows.Count; i++)
                {
                    playlistId = (int)this.dgvPlaylistList.Rows[i].Cells["Id"].Value;
                    if (currentPlaylistId != -1 && playlistId == currentPlaylistId)
                    {
                        this.dgvPlaylistList.Rows[i].DefaultCellStyle.BackColor = this.GridPlayingColor;
                    }
                    else
                    {
                        if (i == 0 || i % 2 == 0)
                        {
                            this.dgvPlaylistList.Rows[i].DefaultCellStyle.BackColor = this.GridLineColor1;
                        }
                        else
                        {
                            this.dgvPlaylistList.Rows[i].DefaultCellStyle.BackColor = this.GridLineColor2;
                        }
                    }
                }
            }
        }

        // TRACKLIST DATA BINDING
        public void InitializeTrackList(DataTableModel model)
        {
            if (model.BindingSource != null)
            {
                this.trackListBindingSource.DataSource = model.BindingSource;
                this.dgvTrackList.DataSource = this.trackListBindingSource.DataSource;
            }

            if (model.ColumnVisibilityArray != null && model.ColumnVisibilityArray.Length > 0)
            {
                for (int i = 0; i <= this.dgvTrackList.Columns.Count - 1; i++)
                {
                    this.dgvTrackList.Columns[i].Visible = model.ColumnVisibilityArray[i];
                }
            }
               
          /*  if (model.ColumnDisplayIndexArray != null && model.ColumnDisplayIndexArray.Length > 0)
            {
                for (int i = 0; i <= this.dgvTrackList.Columns.Count - 1; i++)
                {
                    this.dgvTrackList.Columns[i].DisplayIndex = model.ColumnDisplayIndexArray[i];
                }
            }*/

            this.trackListBindingSource.ResetBindings(false);

            this.UpdateTracklistColor(model.CurrentTrackIdInPlaylist);

        }
        private void dgvTrackList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            this.UpdateTracklistColor();
        }
        public void ReloadTrackList(DataTableModel model)
        {
            this.UpdateTracklistColor(model.CurrentTrackIdInPlaylist);
        }
         
        private bool isShortTrackColouringEnabled { get; set; }
        private TimeSpan shortTrackColouringThreshold { get; set; }

        internal void InitializeShortTrackColouring(bool isColouringEnabled, TimeSpan shortTrackColouringThreshold)
        {
            this.isShortTrackColouringEnabled = isColouringEnabled;
            this.shortTrackColouringThreshold = shortTrackColouringThreshold;
        }

        TimeSpan songLength = new TimeSpan();
        TimeSpan threshold = new TimeSpan();
        public void UpdateTracklistColor(int currentTrackIdInPlaylist = -1)
        {
            bool isTagValueColoringEnabled = this.tagList != null && this.tagList.Count > 0 && this.tagValueDictionary != null && this.tagValueDictionary.Count > 0;

            if (this.dgvTrackList != null && this.dgvTrackList.Rows != null && this.dgvTrackList.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in this.dgvTrackList.Rows)
                {
                    int trackIdInPlaylist = Convert.ToInt32(row.Cells["TrackIdInPlaylist"].Value);
                    bool isMissing = Convert.ToBoolean(row.Cells["IsMissing"].Value);

                    if (isMissing)
                    {
                        row.DefaultCellStyle.ForeColor = Color.Salmon;
                        row.DefaultCellStyle.BackColor = (currentTrackIdInPlaylist != -1 && trackIdInPlaylist == currentTrackIdInPlaylist)
                        ? this.GridPlayingColor
                        : (row.Index % 2 == 0 ? this.GridLineColor1 : this.GridLineColor2);
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = (currentTrackIdInPlaylist != -1 && trackIdInPlaylist == currentTrackIdInPlaylist)
                        ? this.GridPlayingColor
                        : (row.Index % 2 == 0 ? this.GridLineColor1 : this.GridLineColor2);

                        if (this.isShortTrackColouringEnabled)
                        {
                            if (this.dgvTrackList.Columns["Length"].Visible)
                            {
                                string cellValue = row.Cells["Length"].Value as string;
                                if(cellValue.Length < 6)
                                {
                                    cellValue = "00:" + cellValue;
                                    TimeSpan.TryParseExact(cellValue, @"hh\:mm\:ss", null, out songLength);
                                }

                                TimeSpan threshold = this.shortTrackColouringThreshold;

                                if (songLength < threshold)
                                {
                                    row.Cells["Length"].Style.ForeColor = this.RedWarningColor;
                                }
                            }
                        }
                        
                    }

                    if (isTagValueColoringEnabled)
                    {
                        foreach (var tag in this.tagList)
                        {
                            string tagName = tag.Name;
                            string tagValueName = row.Cells[tagName].Value?.ToString() ?? string.Empty;

                            if (tag.HasMultipleValues)
                            {
                                tagValueName = tagName;
                            }

                            if (!string.IsNullOrEmpty(tagValueName))
                            {
                                if (!tag.HasMultipleValues)
                                {
                                    if (tag.TextColoring)
                                    {
                                        row.Cells[tagName].Style.ForeColor = this.tagValueDictionary[tagName][tagValueName];
                                        row.Cells[tagName].Style.BackColor = (currentTrackIdInPlaylist != -1 && trackIdInPlaylist == currentTrackIdInPlaylist)
                                       ? this.GridPlayingColor
                                       : (row.Index % 2 == 0 ? this.GridLineColor1 : this.GridLineColor2);
                                    }
                                    else
                                    {
                                        Color color = this.tagValueDictionary[tagName][tagValueName];
                                        row.Cells[tagName].Style.BackColor = color;
                                        row.Cells[tagName].Style.ForeColor = (color.R < 100 && color.G < 100) ||
                                        (color.R < 100 && color.B < 100) ||
                                        (color.B < 100 && color.G < 100)
                                        ? Color.White : Color.Black;
                                    }
                                }
                                   
                            }
                            else
                            {
                                row.Cells[tagName].Style.BackColor = (currentTrackIdInPlaylist != -1 && trackIdInPlaylist == currentTrackIdInPlaylist)
                                ? this.GridPlayingColor
                                : (row.Index % 2 == 0 ? this.GridLineColor1 : this.GridLineColor2);
                            }
                        }
                    }
                }
            }
        }
        #endregion
        #region TRACKLIST - ROW SELECTION
        ///az aktuális tracklist elemeinek kijelölésekor megjelenik, hogy hány darab szám lett kijelölve és azoknak mennyi az össz játékideje

        private bool controlKey = false;
       
        #endregion

        #region TRACKLIST - ORDER BY COLUMN HEADER
        private void dgvTrackList_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.OrderByColumnEvent?.Invoke(this, new Messenger() { StringField1 = dgvTrackList.Columns[e.ColumnIndex].Name });
        }
        #endregion

        #region TRACKLIST - CONTROL BUTTONS
        private void dgvTrackList_KeyDown(object sender, KeyEventArgs e)
        {
            //DELETE - Delete Track(s)
            if (this.dgvTrackList.Rows.Count > 0 && e.KeyCode == Keys.Delete)
            {
                this.DeleteTracksEvent?.Invoke(this, new Messenger() { Rows = this.dgvTrackList.Rows });
            }

            if (this.dgvTrackList.Rows.Count > 0 && (e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.ShiftKey))
            {
                this.controlKey = true;
            }
            
            //ENTER or SPACE - Play Track
            if (this.dgvTrackList.SelectedRows.Count > 0 && e.KeyCode == Keys.Space ||
                this.dgvTrackList.SelectedRows.Count > 0 && e.KeyCode == Keys.Enter)
            {
                this.CallStopTrackEvent();
                this.CallPlayTrackEvent();
            }

            //CTRL+S - SAve playlist
            if (this.controlKey && e.KeyCode == Keys.S && !this.isFilterEnabled)
            {
                this.SaveTrackListEvent?.Invoke(this, new EventArgs());
            }

            //R - Play random track
            if (this.dgvPlaylistList.Rows.Count > 0 && e.KeyCode == Keys.R)
            {
                this.CallRandomTrackEvent();
            }

            //V - Play previous track
            if (this.dgvPlaylistList.Rows.Count > 0 && e.KeyCode == Keys.V)
            {
                this.CallPrevTrackEvent();
            }
            //B - Play next track
            if (this.dgvPlaylistList.Rows.Count > 0 && e.KeyCode == Keys.B)
            {
                this.CallNextTrackEvent();
            }

            //LEFT/RIGHT - Step +-5 sec in track
            if (this.dgvPlaylistList.Rows.Count > 0 && e.KeyCode == Keys.Left)
            {
                this.CallJumpBackwardEvent();
            }
            if (this.dgvPlaylistList.Rows.Count > 0 && e.KeyCode == Keys.Right)
            {
                this.CallJumpFourwardEvent();
            }
            if (this.dgvPlaylistList.Rows.Count > 0 && e.KeyCode == Keys.P)
            {
                this.CallPauseTrackEvent();
            }
            if (this.dgvPlaylistList.Rows.Count > 0 && e.KeyCode == Keys.S)
            {
                this.CallStopTrackEvent();
            }
            if (this.dgvPlaylistList.Rows.Count > 0 && e.KeyCode == Keys.D0)
            {
                this.CopyCurrentPlayingTrackToDefaultPlaylistEvent?.Invoke(this, EventArgs.Empty);
            }


        }
        private void dgvTrackList_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.ShiftKey)
            {
                controlKey = false;
            }   
        }

        /* private BackgroundWorker backgroundWorker;
         private void InitializeBackgroundWorker()
         {
             backgroundWorker = new BackgroundWorker();
             backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
             backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
         }
         private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
         {
             if (e.Argument is Action action)
             {
                 action();
             }
         }
         private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
         {
             TurnOffProgressBar();
         }*/

        private  void btnScanBpm_Click(object sender, EventArgs e)
        {
            this.ScanKeyAndBpmEvent?.Invoke(this, EventArgs.Empty);
        }
        #endregion



        #region TRACKLIST - DRAG AND DROP





        #endregion

        #region MEDIA PLAYER
        private void dgvTrackList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //this.CallSetCurrentTrackEvent((int)e.RowIndex);
        }
        


        //EVENT CALLINGS
        public void CallSetCurrentTrackEvent(int rowIndex = -1)
        {
            this.SetCurrentTrackEvent?.Invoke(this, new Messenger() { IntegerField1 = rowIndex });
        }
        public void CallPlayTrackEvent()
        {
            this.PlayTrackEvent?.Invoke(this, new Messenger() { });
        }
        public void CallPauseTrackEvent()
        {
            this.PauseTrackEvent?.Invoke(this, EventArgs.Empty);
        }
        public void CallStopTrackEvent()
        {
            this.StopTrackEvent?.Invoke(this, EventArgs.Empty);
        }
        public void CallPrevTrackEvent()
        {
            this.PrevTrackEvent?.Invoke(this, new Messenger() { });
        }
        public void CallNextTrackEvent()
        {
            this.NextTrackEvent?.Invoke(this, new Messenger() {  });
        }
        public void CallRandomTrackEvent()
        {
            this.RandomTrackEvent?.Invoke(this, EventArgs.Empty);
        }
        public void CallJumpBackwardEvent()
        {
            this.JumpBackwardEvent?.Invoke(this, EventArgs.Empty);
        }
        public void CallJumpFourwardEvent()
        {
            this.JumpForwardEvent?.Invoke(this, EventArgs.Empty);
        }


        //UPDATE PLAYLIST VIEW
        public void UpdateAfterPlayTrack(int currentTrackIndex, int currentTrackIdInPlaylist)
        {
            String artist = "Playing: ";
            String title = String.Empty;
            String path = String.Empty;

            if (dgvTrackList.Rows[currentTrackIndex].Cells["Title"].Value != null)
                artist += (string)dgvTrackList.Rows[currentTrackIndex].Cells["Artist"].Value;
            if (dgvTrackList.Rows[currentTrackIndex].Cells["Title"].Value != null)
                title = (string)dgvTrackList.Rows[currentTrackIndex].Cells["Title"].Value;
            if (dgvTrackList.Rows[currentTrackIndex].Cells["Path"].Value != null)
                path = (string)dgvTrackList.Rows[currentTrackIndex].Cells["Path"].Value;

            if (!String.IsNullOrEmpty(title))
            {
                artist += " - " + title;
            }

            ((MainView)this.parentView).UpdateAfterPlayTrack(artist);
            ((MainView)this.parentView).InitializeSoundWavesAndPlot(path);
            this.UpdateTracklistColor(currentTrackIdInPlaylist);
        }
        public void UpdateAfterPlayTrackAfterPause()
        {
            ((MainView)this.parentView).UpdateAfterPlayTrackAfterPause();
        }
        public void UpdateAfterStopTrack()
        {
            ((MainView)this.parentView).UpdateAfterStopTrack();
            this.UpdateTracklistColor();
        }
        public void UpdateAfterPauseTrack()
        {
            ((MainView)this.parentView).UpdateAfterPauseTrack();
        }

        public void SetCurrentTrackColor(int trackIdInPlaylist)
        {
            this.ClearCurrentTrackColor();

            if (trackIdInPlaylist != -1)
            {
                int idInPlaylist = -1;
                for (int i = 0; i <= this.dgvTrackList.Rows.Count - 1; i++)
                {
                    idInPlaylist = Convert.ToInt32(this.dgvTrackList.Rows[i].Cells["TrackIdInPlaylist"].Value);
                    if (trackIdInPlaylist == idInPlaylist)
                    {
                        this.dgvTrackList.Rows[i].DefaultCellStyle.BackColor = Color.LightSeaGreen;
                        break;
                    }
                }
            }
        }
        private void ClearCurrentTrackColor()
        {
            for (int i = 0; i < this.dgvTrackList.Rows.Count; i++)
            {
                if (this.dgvTrackList.Rows[i].DefaultCellStyle.BackColor == Color.LightSeaGreen)
                {
                    this.dgvTrackList.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    break;
                }
            }
        }
        public void UpdateTrackCountAndLength(int currentPlaylistId)
        {
            int trackCount = 0;
            String playlistName = "";
            String trackSumLenght = "";

            if(this.dgvPlaylistList.Rows != null && this.dgvPlaylistList.Rows.Count > 0)
            {
                playlistName = this.dgvPlaylistList.Rows
                    .Cast<DataGridViewRow>()
                    .Where(x => x.Cells["Id"].Value.ToString().Equals(currentPlaylistId.ToString()))
                    .First()
                    .Cells["Name"].Value.ToString();
            }

            if (this.dgvTrackList.Rows != null && this.dgvTrackList.Rows.Count > 0)
            {
                //ROW COUNT
                trackCount = this.dgvTrackList.Rows.Count;

                String[] parts = null;
                int seconds = 0;

                for (int i = 0; i < this.dgvTrackList.Rows.Count; i++)
                {
                    parts = this.dgvTrackList.Rows[i].Cells["Length"].Value.ToString().Split(':');

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

                //SUM OF TIME
                TimeSpan t = TimeSpan.FromSeconds(seconds);
                String length = String.Empty;
                if (t.Days > 0)
                {
                    length = string.Format("{0:D2}.{1:D2}:{2:D2}:{3:D2}", t.Days, t.Hours, t.Minutes, t.Seconds);
                }
                else if (t.Hours > 0)
                {
                    length = string.Format("{0:D2}:{1:D2}:{2:D2}", t.Hours, t.Minutes, t.Seconds);
                }
                else if (t.Minutes > 0)
                {
                    length = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);
                }
                else
                {
                    length = string.Format("{0:D2}:{1:D2}", 0, t.Seconds);
                }
                trackSumLenght = length.ToString();
            }

            if(this.dgvTrackList.Rows.Count > 0)
            {
                lblTrackCount.Show();
                lblTrackSumLength.Show();
                if(trackCount == 1)
                {
                    lblTrackCount.Text = "1 track in [" + playlistName + "]";
                }
                else {
                    lblTrackCount.Text = trackCount + " tracks in [" + playlistName + "]"; 
                }
               
                lblTrackSumLength.Text = "Length: " + trackSumLenght;
            }
            else
            {
                lblTrackCount.Hide();
                lblTrackSumLength.Hide();
            }

            
            
        }

        //alapesetben ha enabled, akkor csak a gomb label-t kell beállítani
        //ha disabled, akkor a gomb labelt kell állítani, eltüntetni a playlistlist panelt és 
        //csökkenteni a tracklist bal oldalát, növelni a tracklist méretét
        public void InitializeDisplayPlaylistList(bool isPlaylistListDisplayed)
        {
            if (isPlaylistListDisplayed)
            {
                this.btnPlaylistListPanelToggle.Image = Resources.Arrow_Left_20_20;
            }
            else
            {
                this.btnPlaylistListPanelToggle.Image = Resources.Arrow_Right_20_20;
                this.pnlPlaylistList.Hide();

                if (this.dgvTrackList.Left > 9)
                {
                    this.dgvTrackList.Left -= this.trackListLeftOffset;
                    this.dgvTrackList.Width += this.trackListLeftOffset;
                }
                
            }
        }
        public void UpdateDisplayPlaylistList(bool isPlaylistListDisplayed)
        {
            if (isPlaylistListDisplayed)
            {
                this.btnPlaylistListPanelToggle.Image = Resources.Arrow_Left_20_20;
                this.pnlPlaylistList.Show();

                this.dgvTrackList.Left += this.trackListLeftOffset;
                this.dgvTrackList.Width -= this.trackListLeftOffset;
            }
            else
            {
                this.btnPlaylistListPanelToggle.Image = Resources.Arrow_Right_20_20;
                this.pnlPlaylistList.Hide();

                this.dgvTrackList.Left -= this.trackListLeftOffset;
                this.dgvTrackList.Width += this.trackListLeftOffset;
            }
        }

        //UPDATE MAINVIEW VIEW
        public void SetVolume(int volume)
        {
            ((MainView)this.parentView).SetVolume(volume);
        }
        public void SetMuted(bool isMuted)
        {
            ((MainView)this.parentView).SetMuted(isMuted);
        }
       
        public void SetKeyAndBpmAnalization(bool showButton)
        {
            if (showButton)
            {
                this.btnScanKeyAndBpm.Show();
            }
            else
            {
                this.btnScanKeyAndBpm.Hide();
            }
        }
        #endregion

        #region PLAYLIST LIST - CONTROL BUTTONS

        private void dgvPlaylistList_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(Cursor.Position);
            }
            else
            {
                if (!isPlaylistDragging)
                {
                    this.CallLoadPlaylistEvent();
                }
            }
        }

        private void dgvPlaylistList_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.dgvPlaylistList.SelectedRows.Count > 0 && e.KeyCode == Keys.Delete)
            {
                this.CallDeletePlaylistEvent();
            }
            //ENTER or SPACE - Play Track
            if (this.dgvPlaylistList.SelectedRows.Count > 0 && e.KeyCode == Keys.Space ||
                this.dgvPlaylistList.SelectedRows.Count > 0 && e.KeyCode == Keys.Enter)
            {
                this.CallLoadPlaylistEvent();
            }
        }
        private void menuStripCreatePlaylist_Click(object sender, EventArgs e)
        {
            this.CallCreatePlaylistEvent();
        }
        private void menuStripLoadPlaylist_Click(object sender, EventArgs e)
        {
            this.CallLoadPlaylistEvent();
        }
        private void menuStripRenamePlaylist_Click(object sender, EventArgs e)
        {
            this.CallRenamePlaylistEvent();
        }
        private void menuStripDeletePlaylist_Click(object sender, EventArgs e)
        {
            this.CallDeletePlaylistEvent();
        }
        private void menuStripSetQuickListGroup1_Click(object sender, EventArgs e)
        {
            this.CallSetQuickListEvent(1);
        }
        private void menuStripSetQuickListGroup2_Click(object sender, EventArgs e)
        {
            this.CallSetQuickListEvent(2);
        }
        private void menuStripSetQuickListGroup3_Click(object sender, EventArgs e)
        {
            this.CallSetQuickListEvent(3);
        }
        private void menuStripSetQuickListGroup4_Click(object sender, EventArgs e)
        {
            this.CallSetQuickListEvent(4);
        }
        private void menuStripExportToM3uToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CallExportToM3UEvent();
        }
        private void menuStripExportToTxtToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CallExportToTXTEvent();
        }
        private void exportToDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.CallExportToDirectoryEvent();
        }

        //EVENT CALLINGS
        public void CallCreatePlaylistEvent()
        {
            this.CreatePlaylist?.Invoke(this, new Messenger() { IntegerField1 = -1 });
        }
        public void CallRenamePlaylistEvent()
        {
            if (this.dgvPlaylistList.SelectedRows.Count > 0)
                this.EditPlaylist?.Invoke(this, new Messenger() { IntegerField1 = dgvPlaylistList.SelectedRows[0].Index });
        }
        public void CallLoadPlaylistEvent()
        {
            if (!this.isFilterEnabled)
            {
                if (this.dgvPlaylistList.SelectedRows.Count > 0)
                {
                    this.LoadPlaylistEvent?.Invoke(this, new Messenger() { IntegerField1 = this.dgvPlaylistList.SelectedRows[0].Index });
                }
            }
            else
            {
                this.EnableSetterMode();

                if (this.dgvPlaylistList.SelectedRows.Count > 0)
                {
                    this.LoadPlaylistEvent?.Invoke(this, new Messenger() { IntegerField1 = this.dgvPlaylistList.SelectedRows[0].Index });
                }
            }

            int rowIndex = 0;
            if (this.dgvTrackList != null && this.dgvTrackList.SelectedCells.Count > 0)
            {
                rowIndex = this.dgvTrackList.SelectedCells[0].RowIndex;
            }
            this.LoadCoversEvent?.Invoke(this, new Messenger() { IntegerField1 = rowIndex });

        }
        public void DisplayLog(String message, decimal timeInSec)
        {
            LabelTimer.DisplayLabel(this.components, this.lblMessage, message, timeInSec);
        }
        public void CallDeletePlaylistEvent()
        {
            if (this.dgvPlaylistList.SelectedRows.Count > 0)
                this.DeletePlaylistEvent?.Invoke(this, new Messenger() { IntegerField1 = dgvPlaylistList.SelectedRows[0].Index });
        }
        public void CallSetQuickListEvent(int group)
        {
            if (this.dgvPlaylistList.SelectedRows.Count > 0)
                this.SetQuickListEvent?.Invoke(this, new Messenger() { IntegerField1 = dgvPlaylistList.Rows.IndexOf(dgvPlaylistList.SelectedRows[0]), IntegerField2 = group });
        }
        public void CallExportToM3UEvent()
        {
            if (this.dgvPlaylistList.SelectedRows.Count > 0)
            {
                this.ExportToM3UEvent?.Invoke(this, new Messenger() { IntegerField1 = this.dgvPlaylistList.SelectedRows[0].Index });
            }
        }
        public void CallExportToTXTEvent()
        {
            if (this.dgvPlaylistList.SelectedRows.Count > 0)
            {
                this.ExportToTXTEvent?.Invoke(this, new Messenger() { IntegerField1 = this.dgvPlaylistList.SelectedRows[0].Index });
            }
        }
        public void CallExportToDirectoryEvent()
        {
            if (this.dgvPlaylistList.SelectedRows.Count > 0)
            {
                this.ExportToDirectoryEvent?.Invoke(this, new Messenger() { IntegerField1 = this.dgvPlaylistList.SelectedRows[0].Index });
            }
        }
        #endregion

        #region PLAYLIST - DRAG AND DROP


  
        private void btnDisplayPlaylistList_Click(object sender, EventArgs e)
        {
            this.DisplayPlaylistListEvent?.Invoke(this, new EventArgs());
        }

        #endregion

        #region TRACKLIST - COLUMN VISIBILITY
        private void btnColumnVisibility_Click(object sender, EventArgs e)
        {
            this.ShowColumnVisibilityEditorEvent?.Invoke(this, new EventArgs());
        }
        private void btnColumnVisibility2_Click(object sender, EventArgs e)
        {
            this.ShowColumnVisibilityEditorEvent?.Invoke(this, new EventArgs());
        }
        #endregion

        #region TAG EDITOR

        public void InitializeTagComponent(List<Tag> tagList, List<List<TagValue>> tagValueListContainer, bool isTagEditorDisplayed, bool isOnlyPlayingRowModeEnabled, bool isFilterModeEnabled)
        {
            this.tagValueEditorPanel.Controls.Clear();

            if (isOnlyPlayingRowModeEnabled)
            {
                this.chbOnlyPlayingRowModeEnabled.Checked = true;
            }
            else
            {
                this.chbOnlyPlayingRowModeEnabled.Checked = false;
            }

            if (isFilterModeEnabled)
            {
                this.chbOnlyPlayingRowModeEnabled.Hide();
                this.lblFilter.Show();
                this.txtbFilter.Show();
                this.btnFilter.Show();
                this.btnFilterIsPressed = false;
                this.txtbFilter.Text = string.Empty;
                this.isFilterEnabled = true;
            }
            else
            {
                this.chbOnlyPlayingRowModeEnabled.Show();
                this.lblFilter.Hide();
                this.txtbFilter.Hide();
                this.btnFilter.Hide();
                this.btnFilterIsPressed = false;
                this.txtbFilter.Text = string.Empty;
                this.isFilterEnabled = false;
            }

            if(isFilterModeEnabled && isTagEditorDisplayed)
            {
                this.btnClearTagValueFilter.Show();
            }
            else
            {
                this.btnClearTagValueFilter.Hide();
            }

            int sumHeight = 0;
            int height = 0;
            for (int i = 0; i < tagList.Count; i++)
            {
                GroupBox gp = new GroupBox();
                gp.Text = tagList[i].Name;
                gp.ForeColor = this.FontColor;

                if (tagValueListContainer != null && tagValueListContainer.Count > 0
                        && tagValueListContainer[i] != null && tagValueListContainer[i].Count > 0)
                {
                    decimal heightFactor = (1 + (decimal)tagValueListContainer[i].Count) / (decimal)3;
                    heightFactor = Math.Ceiling(heightFactor);
                    height = 20 + (int)(heightFactor * 30);
                    gp.Size = new Size(255, height);

                    if (i == 0)
                    {
                        gp.Location = new Point(0, 0);
                    }
                    else
                    {
                        gp.Location = new Point(0, sumHeight);
                    }
                    sumHeight += height + 10;

                    if (tagList[i].HasMultipleValues)
                    {
                        TextBox txtBox = new TextBox();
                        txtBox.Size = new Size(156, 20);
                        txtBox.Location = new Point(5, 20);

                        TagValueButton btn = new TagValueButton();
                        btn.Text = "Set";
                        btn.Size = new Size(75, 23);
                        btn.Location = new Point(165, 19);

                        btn.FlatAppearance.BorderSize = 1;
                        btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        btn.UseVisualStyleBackColor = false;

                        btn.BackColor = this.ButtonColor;
                        btn.ForeColor = this.FontColor;
                        btn.FlatAppearance.BorderColor = this.ButtonBorderColor;

                        btn.TagName = gp.Text;
                        btn.TagValueName = btn.Text;
                        btn.TagValueId = -1;

                        btn.TextBox = txtBox;

                        txtBox.KeyDown += new KeyEventHandler(
                             (sender, e) => this.txtbSetTagValue_KeyDown(sender, e, btn));
                      /*  btn.Click += new System.EventHandler(
                             (sender, e) => this.btnSetTagValue_Click(sender, e));*/
                        btn.MouseDown += new MouseEventHandler(
                               (sender, e) => this.btnSetTagValue_Click(sender, e));

                        gp.Controls.Add(txtBox);
                        gp.Controls.Add(btn);
                    }
                    else
                    {
                        int buttonLengthX = 75;
                        int buttonLengthY = 23;
                        int buttonsIntervalX = 5;
                        int buttonsIntervalY = 20;

                        TagValueButton btn = new TagValueButton();
                        btn.Text = "-";
                        btn.Size = new Size(buttonLengthX, buttonLengthY);

                        btn.TagName = gp.Text;

                       /* btn.Click += new System.EventHandler(
                                (sender, e) => this.btnClearTagValue_Click(sender, e));*/
                        btn.Location = new Point(buttonsIntervalX, buttonsIntervalY);

                        btn.MouseDown += new MouseEventHandler(
                                (sender, e) => this.btnClearTagValue_Click(sender, e));

                        btn.FlatAppearance.BorderSize = 1;
                        btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                        btn.UseVisualStyleBackColor = false;

                        btn.BackColor = this.ButtonColor;
                        btn.ForeColor = this.FontColor;
                        btn.FlatAppearance.BorderColor = this.ButtonBorderColor;

                        gp.Controls.Add(btn);

                        for (int j = 1; j <= tagValueListContainer[i].Count; j++)
                        {

                            btn = new TagValueButton();
                            btn.Text = tagValueListContainer[i][j - 1].Name;
                            btn.Size = new Size(buttonLengthX, buttonLengthY);

                            btn.TagName = gp.Text;
                            btn.TagValueName = btn.Text;
                            btn.TagValueId = tagValueListContainer[i][j - 1].Id;

                           /* btn.Click += new System.EventHandler(
                                    (sender, e) => this.btnSetTagValue_Click(sender, e));*/
                            btn.MouseDown += new MouseEventHandler(
                               (sender, e) => this.btnSetTagValue_Click(sender, e));

                            btn.FlatAppearance.BorderSize = 1;
                            btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                            btn.UseVisualStyleBackColor = false;

                            btn.BackColor = this.ButtonColor;
                            btn.ForeColor = tagValueListContainer[i][j - 1].Color;
                            btn.FlatAppearance.BorderColor = this.ButtonBorderColor;

                            if (j == 1 || j == 2)
                            {
                                btn.Location = new Point(buttonsIntervalX + (j * (buttonLengthX + buttonsIntervalX)), buttonsIntervalY);
                                gp.Controls.Add(btn);
                            }
                            else if (j > 2 && j % 3 == 0)
                            {
                                buttonsIntervalY = buttonLengthY + buttonsIntervalY + 5;
                                btn.Location = new Point(buttonsIntervalX, buttonsIntervalY);
                                gp.Controls.Add(btn);
                            }
                            else if (j > 2 && (j % 3 == 1 || j % 3 == 2))
                            {
                                btn.Location = new Point(buttonsIntervalX + ((j % 3) * (buttonLengthX + buttonsIntervalX)), buttonsIntervalY);
                                gp.Controls.Add(btn);
                            }

                        }

                    }
                }

                this.tagValueEditorPanel.Controls.Add(gp);
            }

            if (isTagEditorDisplayed)
            {
                this.dgvTrackList.Width = this.dgvTrackList.Width + this.trackListRightOffset;
            }
        }
        private int originalTagValueEditorPanelHeight;
        public void InitializeDisplayTagComponent(bool isTagComponentDisplayed)
        {
            if (isTagComponentDisplayed)
            {
                this.btnDisplayTagComponentToggle.Image = Resources.Arrow_Right_20_20;
                this.dgvTrackList.Width = this.dgvTrackList.Width - this.trackListRightOffset;
            }
            else
            {
                this.btnDisplayTagComponentToggle.Image = Resources.Arrow_Left_20_20;
                this.pnlTagComponent.Hide();

                if (this.dgvTrackList.Width < 883)
                {
                    this.dgvTrackList.Width = this.dgvTrackList.Width + this.trackListRightOffset;
                }
            }

            this.isFilterEnabled = false;

            this.btnFilterModeToggle.BackColor = this.ButtonColor;
            this.btnFilterModeToggle.ForeColor = this.FontColor;
            this.btnFilterModeToggle.FlatAppearance.BorderColor = this.ButtonBorderColor;

            this.btnSetterModeToggle.BackColor = this.ActiveButtonColor;
            this.btnSetterModeToggle.ForeColor = this.ButtonColor;
            this.btnSetterModeToggle.FlatAppearance.BorderColor = this.ButtonBorderColor;

            this.btnFilter.BackColor = this.ButtonColor;
            this.btnFilter.ForeColor = this.FontColor;
            this.btnFilter.FlatAppearance.BorderColor = this.ButtonBorderColor;

            this.btnClearTagValueFilter.Hide();
            this.lblFilter.Hide();
            this.txtbFilter.Hide();
            this.btnFilter.Hide();
            this.txtbFilter.Text = string.Empty;

            this.chbOnlyPlayingRowModeEnabled.Show();
        }

        public void UpdateDisplayTagComponent(bool isTagComponentDisplayed)
        {
            if (isTagComponentDisplayed)
            {
                this.btnDisplayTagComponentToggle.Image = Resources.Arrow_Right_20_20;
                this.pnlTagComponent.Show();
                this.dgvTrackList.Width = this.dgvTrackList.Width - this.trackListRightOffset;
            }
            else
            {
                this.btnDisplayTagComponentToggle.Image = Resources.Arrow_Left_20_20;
                this.pnlTagComponent.Hide();
                this.dgvTrackList.Width = this.dgvTrackList.Width + this.trackListRightOffset;
            }
        }
        private void btnFilterModeToggle_Click(object sender, EventArgs e)
        {
            foreach (Control groupBox in this.tagValueEditorPanel.Controls)
            {
                if (groupBox.GetType() == typeof(GroupBox))
                {
                    foreach (Control buttonOrTextBox in groupBox.Controls)
                    {
                        if (buttonOrTextBox.GetType() == typeof(TagValueButton))
                        {
                            if (((TagValueButton)buttonOrTextBox).TextBox != null)
                            {
                                ((TagValueButton)buttonOrTextBox).Text = "Filter";
                            }
                        }
                    }
                }
            }

            this.isFilterEnabled = true;

            this.btnFilterModeToggle.BackColor = this.ActiveButtonColor;
            this.btnFilterModeToggle.ForeColor = this.ButtonColor;
            this.btnFilterModeToggle.FlatAppearance.BorderColor = this.ButtonBorderColor;

            this.btnSetterModeToggle.BackColor = this.ButtonColor;
            this.btnSetterModeToggle.ForeColor = this.FontColor;
            this.btnSetterModeToggle.FlatAppearance.BorderColor = this.ButtonBorderColor;

            this.btnFilter.BackColor = this.ButtonColor;
            this.btnFilter.ForeColor = this.FontColor;
            this.btnFilter.FlatAppearance.BorderColor = this.ButtonBorderColor;

            this.btnSave.Enabled = false;

            this.btnClearTagValueFilter.Show();
            this.lblFilter.Show();
            this.txtbFilter.Show();
            this.btnFilter.Show();
            this.btnFilterIsPressed = false;
            this.txtbFilter.Text = string.Empty;

            this.chbOnlyPlayingRowModeEnabled.Hide();

            this.SetFocusToDataGridView();

            this.EnableFilterModeEvent?.Invoke(this, new EventArgs());

            int rowIndex = 0;
            if (this.dgvTrackList != null && this.dgvTrackList.SelectedCells.Count > 0)
            {
                rowIndex = this.dgvTrackList.SelectedCells[0].RowIndex;
            }

            this.LoadCoversEvent?.Invoke(this, new Messenger() { IntegerField1 = rowIndex });
        }
        private void btnSetterModeToggle_Click(object sender, EventArgs e)
        {
            this.EnableSetterMode();
        }
        private void EnableSetterMode()
        {

            this.RemoveTagValueFilter?.Invoke(this, new EventArgs());

            foreach (Control groupBox in this.tagValueEditorPanel.Controls)
            {
                if (groupBox.GetType() == typeof(GroupBox))
                {
                    foreach (Control buttonOrTextBox in groupBox.Controls)
                    {
                        if (buttonOrTextBox.GetType() == typeof(TagValueButton))
                        {
                            ((TagValueButton)buttonOrTextBox).FlatAppearance.BorderColor = this.ButtonBorderColor;
                            ((TagValueButton)buttonOrTextBox).IsPressed = false;

                            if(((TagValueButton)buttonOrTextBox).TextBox != null)
                            {
                                ((TagValueButton)buttonOrTextBox).Text = "Set";
                            }
                        }
                        else if (buttonOrTextBox.GetType() == typeof(TextBox))
                        {
                            ((TextBox)buttonOrTextBox).Text = String.Empty;
                        }
                    }
                    this.txtbFilter.Text = string.Empty;
                    this.btnFilter.FlatAppearance.BorderColor = this.ButtonBorderColor;
                    this.btnFilterIsPressed = false;
                }
            }

            this.isFilterEnabled = false;

            this.btnFilterModeToggle.BackColor = this.ButtonColor;
            this.btnFilterModeToggle.ForeColor = this.FontColor;

            this.btnSetterModeToggle.BackColor = this.ActiveButtonColor;
            this.btnSetterModeToggle.ForeColor = this.ButtonColor;

            this.btnSave.Enabled = true;

            this.btnClearTagValueFilter.Hide();
            this.lblFilter.Hide();
            this.txtbFilter.Hide();
            this.btnFilter.Hide();
            this.btnFilterIsPressed = false;
            this.txtbFilter.Text = string.Empty;

            this.chbOnlyPlayingRowModeEnabled.Show();

            this.SetFocusToDataGridView();

            this.EnableSetterModeEvent?.Invoke(this, new EventArgs());

            int rowIndex = 0;
            if (this.dgvTrackList != null && this.dgvTrackList.SelectedCells.Count > 0)
            {
                rowIndex = this.dgvTrackList.SelectedCells[0].RowIndex;
            }


            this.LoadCoversEvent?.Invoke(this, new Messenger() { IntegerField1 = rowIndex });
        }
        private void btnDisplayTagEditor_Click(object sender, EventArgs e)
        {
            this.DisplayTagEditorEvent?.Invoke(this, new EventArgs());
            this.chbOnlyPlayingRowModeEnabled.Show();

            if (this.isFilterEnabled)
            {
                this.btnClearTagValueFilter.Show();
            }

            this.SetFocusToDataGridView();
        }

        private bool btnFilterIsPressed = false;
        private void btnFilter_Click(object sender, EventArgs e)
        {
            this.ChangeFilterParameters();
        }
        private void txtbFilter_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!btnFilterIsPressed)
                {
                    btnFilterIsPressed = true;
                    btnFilter.FlatAppearance.BorderColor = this.ActiveButtonColor;
                }
                else
                {
                    if (String.IsNullOrEmpty(this.txtbFilter.Text))
                    {
                        btnFilterIsPressed = false;
                        btnFilter.FlatAppearance.BorderColor = this.ButtonBorderColor;
                        txtbFilter.Text = String.Empty;
                    }
                }

                this.ChangeFilterParametersEvent?.Invoke(this, new Messenger() { StringField1 = this.txtbFilter.Text });
            }
        }
        private void ChangeFilterParameters()
        {
            btnFilterIsPressed = !btnFilterIsPressed;
            if (btnFilterIsPressed)
            {
                btnFilter.FlatAppearance.BorderColor = this.ActiveButtonColor;
            }
            else
            {
                btnFilter.FlatAppearance.BorderColor = this.ButtonBorderColor;
                txtbFilter.Text = String.Empty;
            }

            this.ChangeFilterParametersEvent?.Invoke(this, new Messenger() { StringField1 = this.txtbFilter.Text });


        }
        private void btnSetTagValue_Click(object sender, EventArgs e)
        {
            this.SetTagValue(sender, e, null);
        }

        private bool isSaving = false;
        public void ChangeSaveStatus(bool isSaving)
        {
            this.isSaving = isSaving;
        }

        private void SetTagValue(object sender, EventArgs e, Button btn)
        {
            if(!this.isSaving)
            {
                TagValueButton button = (sender as TagValueButton);

                if (btn != null)
                    button = (TagValueButton)btn;

                String tagName = button.TagName;
                String tagValueName = button.TagValueName;
                int tagValueId = button.TagValueId;
                String tagValueValue = !String.IsNullOrEmpty(button.TextBox?.Text) ? button.TextBox?.Text : "-1";

                if (!this.isFilterEnabled)
                {
                    if (button.TextBox != null)
                    {
                        button.TextBox.Text = String.Empty;
                    }
                }
                else
                {
                    if (button.TextBox != null && button.IsPressed)
                    {
                        button.TextBox.Text = String.Empty;
                    }

                    button.IsPressed = !button.IsPressed;
                }

                if (button.IsPressed)
                {
                    button.FlatAppearance.BorderColor = this.ActiveButtonColor;
                }
                else
                {
                    button.FlatAppearance.BorderColor = this.ButtonBorderColor;
                }

                this.SetTagValueEvent?.Invoke(this, new Messenger()
                {
                    StringField1 = tagName,
                    StringField2 = tagValueName,
                    StringField3 = tagValueValue,
                    IntegerField1 = tagValueId,
                    Rows = this.dgvTrackList.Rows
                });
                
            }
            this.SetFocusToDataGridView();
        }

        private void txtbSetTagValue_KeyDown(object sender, KeyEventArgs e, Button btn)
        {
            if(e.KeyCode == Keys.Enter)
            {
                this.SetTagValue(sender, e, btn);
            }
        }
        private void btnClearTagValue_Click(object sender, MouseEventArgs e)
        {
            TagValueButton button = (sender as TagValueButton);

            if (this.isFilterEnabled)
            {
                foreach (Control btn in ((GroupBox)button.Parent).Controls)
                {
                    if (btn.GetType() == typeof(TagValueButton))
                    {
                        ((TagValueButton)btn).FlatAppearance.BorderColor = this.ButtonBorderColor;
                        ((TagValueButton)btn).IsPressed = false;
                    }
                }
                this.txtbFilter.Text = string.Empty;
                this.btnFilter.FlatAppearance.BorderColor = this.ButtonBorderColor;
                this.btnFilterIsPressed = false;
            }

            if (this.chbOnlyPlayingRowModeEnabled.Checked)
            {
                this.ClearTagValueEvent?.Invoke(this, new Messenger() { StringField1 = button.TagName, Rows = this.dgvTrackList.Rows });
            }
            else
            {
                if (this.dgvTrackList.SelectedRows.Count > 0)
                {
                    this.ClearTagValueEvent?.Invoke(this, new Messenger() { StringField1 = button.TagName, Rows = this.dgvTrackList.Rows });
                }
            }

            this.SetFocusToDataGridView();
        }
        private void chbOnlyPlayingRowModeEnabled_CheckedChanged(object sender, EventArgs e)
        {
            this.SetFocusToDataGridView();
            this.ChangeOnlyPlayingRowModeEnabled?.Invoke(this, new Messenger() { BooleanField1 = this.chbOnlyPlayingRowModeEnabled.Checked });
        }
        public void SetTagValueFilter(List<TagValueFilter> tagValueFilterList)
        {
            String tagValueAsFilter = String.Empty;

            if (tagValueFilterList != null && tagValueFilterList.Count > 0)
            {
                for(int i = 0; i < tagValueFilterList.Count; i++)
                {
                    if (String.IsNullOrEmpty(tagValueAsFilter))
                    {
                        if (tagValueFilterList[i].TagValueValue == "-1")
                        {
                            tagValueAsFilter = "[" + tagValueFilterList[i].TagName + ": " + tagValueFilterList[i].TagValueName + "]";
                        }
                        else
                        {
                            tagValueAsFilter = "[" + tagValueFilterList[i].TagName + ": " + tagValueFilterList[i].TagValueValue + "]";
                        }
                    }
                    else
                    {
                        if (tagValueFilterList[i].TagValueValue == "-1")
                        {
                            tagValueAsFilter = tagValueAsFilter + "  [" + tagValueFilterList[i].TagName + ": " + tagValueFilterList[i].TagValueName + "]";
                        }
                        else
                        {
                            tagValueAsFilter = tagValueAsFilter + "  [" + tagValueFilterList[i].TagName + ": " + tagValueFilterList[i].TagValueValue + "]";
                        }
                    }
                }
            }
            this.ChangeFilterParametersEvent?.Invoke(this, new Messenger() { StringField1 = this.txtbFilter.Text });

            int rowIndex = 0;
            if (this.dgvTrackList != null && this.dgvTrackList.SelectedCells.Count > 0)
            {
                rowIndex = this.dgvTrackList.SelectedCells[0].RowIndex;
            }
            this.LoadCoversEvent?.Invoke(this, new Messenger() { IntegerField1 = rowIndex });
        }



        private void btnClearTagValueFilter_Click(object sender, EventArgs e)
        {
            foreach (Control groupBox in this.tagValueEditorPanel.Controls)
            {
                if (groupBox.GetType() == typeof(GroupBox))
                {
                    foreach (Control button in groupBox.Controls)
                    {
                        if (button.GetType() == typeof(TagValueButton))
                        {
                            ((TagValueButton)button).FlatAppearance.BorderColor = this.ButtonBorderColor;
                            ((TagValueButton)button).IsPressed = false;
                        }
                    }
                    this.txtbFilter.Text = string.Empty;
                    this.btnFilter.FlatAppearance.BorderColor = this.ButtonBorderColor;
                    this.btnFilterIsPressed = false;
                }
            }

            this.RemoveTagValueFilter?.Invoke(this, new EventArgs());

            int rowIndex = 0;
            if (this.dgvTrackList != null && this.dgvTrackList.SelectedCells.Count > 0)
            {
                rowIndex = this.dgvTrackList.SelectedCells[0].RowIndex;
            }
            this.LoadCoversEvent?.Invoke(this, new Messenger() { IntegerField1 = rowIndex });
        }


        private void tagValueEditorPanel_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //R - Play random track
            if (this.dgvPlaylistList.Rows.Count > 0 && e.KeyCode == Keys.R)
            {
                this.CallRandomTrackEvent();
            }

            //B - Play next track
            if (this.dgvPlaylistList.Rows.Count > 0 && e.KeyCode == Keys.B)
            {
                this.CallNextTrackEvent();
            }
        }

        public void ChangeSaveButtonColor(bool isTableChanged)
        {
            if (isTableChanged)
            {
                if (!this.isFilterEnabled)
                {
                    this.btnSave.BackColor = this.ActiveButtonColor;
                    this.btnSave.ForeColor = this.ButtonColor;
                }
            }
            else
            {
                this.btnSave.BackColor = this.ButtonColor;
                this.btnSave.ForeColor = this.FontColor;
            }
        }

        #endregion

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.SaveTrackListEvent?.Invoke(this, new EventArgs());
        }


       




        // Timer to handle double-click detection
        private System.Windows.Forms.Timer tracklistClickTimer;
        // Time interval for detecting double-clicks
        private const int tracklistDoubleClickTime = 2000; // Adjust the delay as needed
                                                 // Flags to track dragging state
        private bool isDragging = false;
        private bool isMouseDown = false;
        // Point to store the starting position of a drag
        private Point dragStartPoint;

        // Index of the first selected row
        private int firstSelectedRowIndex = -1;

        public void ToggleTracklistSelection(bool enabled)
        {
            isInitializing = !enabled;
        }

        private bool isCoverBrowserUpdateEnabled = true;
        private bool isDragAndDropInProgress = false;
        // Event handler for selection changes in the DataGridView
        private void dgvTrackList_SelectionChanged(object sender, EventArgs e)
        {
            if (isInitializing || !isCoverBrowserUpdateEnabled || isDragAndDropInProgress)
            {

            }
            else
            {
                if (dgvTrackList.SelectedRows.Count > 0)
                {
                    int selectedIndex = dgvTrackList.SelectedRows[dgvTrackList.SelectedRows.Count - 1].Index;
                    firstSelectedRowIndex = selectedIndex;
                    this.CallSetCurrentTrackEvent(selectedIndex);
                }
            }

            if (dgvTrackList.SelectedRows.Count > 0)
            {
                this.lblSelectedItemsCount.Text = $"{this.dgvTrackList.SelectedRows.Count} item{(this.dgvTrackList.SelectedRows.Count > 1 ? "s" : "")} selected";

                int totalSeconds = 0;

                foreach (DataGridViewRow row in this.dgvTrackList.SelectedRows)
                {
                    string[] parts = row.Cells["Length"].Value.ToString().Split(':');

                    if (parts.Length == 3)
                    {
                        totalSeconds += int.Parse(parts[0]) * 3600;
                        totalSeconds += int.Parse(parts[1]) * 60;
                        totalSeconds += int.Parse(parts[2]);
                    }
                    else if (parts.Length == 2)
                    {
                        totalSeconds += int.Parse(parts[0]) * 60;
                        totalSeconds += int.Parse(parts[1]);
                    }
                    else if (parts.Length == 1)
                    {
                        totalSeconds += int.Parse(parts[0]);
                    }
                }

                // Calculate total time
                TimeSpan totalTime = TimeSpan.FromSeconds(totalSeconds);
                string length;

                if (totalTime.Days > 0)
                {
                    length = $"{totalTime.Days:D2}.{totalTime.Hours:D2}:{totalTime.Minutes:D2}:{totalTime.Seconds:D2}";
                }
                else if (totalTime.Hours > 0)
                {
                    length = $"{totalTime.Hours:D2}:{totalTime.Minutes:D2}:{totalTime.Seconds:D2}";
                }
                else if (totalTime.Minutes > 0)
                {
                    length = $"{totalTime.Minutes:D2}:{totalTime.Seconds:D2}";
                }
                else
                {
                    length = $"{0:D2}:{totalTime.Seconds:D2}";
                }

                this.lblSelectedItemsLength.Text = $"Length: {length}";
            }
        }
       

        // Event handler for double-clicks in the DataGridView
        private void dgvTrackList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!isDragging)
            {
                var hitTestInfo = dgvTrackList.HitTest(e.X, e.Y);
                if (hitTestInfo.RowIndex >= 0)
                {
                    this.PlayTrackEvent?.Invoke(this, new Messenger() { });
                }
            }
        }

        private void dgvTrackList_MouseDown(object sender, MouseEventArgs e)
        {
            var hitTestInfo = dgvTrackList.HitTest(e.X, e.Y);
            if (hitTestInfo.RowIndex >= 0 && e.Button == MouseButtons.Left)
            {
                lastMouseDownRowIndex = hitTestInfo.RowIndex; // Capture the index
                isCoverBrowserUpdateEnabled = false; // Disable cover browser update

                // Check if the clicked row is already selected
                bool isRowSelected = dgvTrackList.SelectedRows.Cast<DataGridViewRow>().Any(row => row.Index == hitTestInfo.RowIndex);

                if (ModifierKeys.HasFlag(Keys.Shift) || ModifierKeys.HasFlag(Keys.Control))
                {
                    // Handle multi-row selection with Shift or Ctrl key
                    return;
                }

                if (isRowSelected)
                {
                    // Only start drag-and-drop if the clicked row is already selected
                    if (dgvTrackList.SelectedRows.Count > 1)
                    {
                        // Multiple rows are selected, start drag-and-drop immediately
                        isDragAndDropInProgress = true; // Set the flag
                        StartDragAndDrop(e.Location);
                    }
                    else
                    {
                        // Single row is already selected, start drag-and-drop
                        isDragAndDropInProgress = true; // Set the flag
                        PrepareForDrag(e.Location, hitTestInfo.RowIndex);
                    }
                }
                else
                {
                    // Row is not selected, select it but do not start drag-and-drop
                    dgvTrackList.ClearSelection();
                    dgvTrackList.Rows[hitTestInfo.RowIndex].Selected = true;
                }
            }
        }
        // Method to start drag-and-drop
        private void StartDragAndDrop(Point location)
        {
            isMouseDown = true;
            isDragging = true;
            dragStartPoint = location;
            dgvTrackList.DoDragDrop(new DataObject(TrackListDataFormat, dgvTrackList.SelectedRows.Cast<DataGridViewRow>().ToArray()), DragDropEffects.Move | DragDropEffects.Copy);
        }
        // Method to prepare for drag-and-drop
        private void PrepareForDrag(Point location, int rowIndex)
        {
            isMouseDown = true;
            isDragging = false;
            dragStartPoint = location;
            firstSelectedRowIndex = rowIndex;
            tracklistClickTimer.Start();
        }
        // Event handler for mouse move events in the DataGridView
        private void dgvTrackList_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseDown && !isDragging)
            {
                if (Math.Abs(e.X - dragStartPoint.X) > SystemInformation.DragSize.Width ||
                Math.Abs(e.Y - dragStartPoint.Y) > SystemInformation.DragSize.Height)
                {
                    isDragging = true;
                    tracklistClickTimer.Stop();
                    dgvTrackList.DoDragDrop(new DataObject(TrackListDataFormat, dgvTrackList.SelectedRows.Cast<DataGridViewRow>().ToArray()), DragDropEffects.Move | DragDropEffects.Copy);
                }
            }
        }
        private int lastMouseDownRowIndex = -1;
        // Event handler for mouse up events in the DataGridView
        private void dgvTrackList_MouseUp(object sender, MouseEventArgs e)
        {
            isMouseDown = false;
            if (!isDragging)
            {
                tracklistClickTimer.Stop();
            }
            isDragging = false;
            isCoverBrowserUpdateEnabled = true; // Enable cover browser update
            isDragAndDropInProgress = false; // Clear the flag

            if (lastMouseDownRowIndex >= 0)
            {
                UpdateCoverBrowser(lastMouseDownRowIndex); // Update the cover browser with the captured index
            }
            // Reset insertion line
            insertionLineIndex = -1;
            dgvTrackList.Invalidate(); // Refresh the DataGridView display
        }

        // Timer tick event handler to handle single-click actions
        private void TracklistClickTimer_Tick(object sender, EventArgs e)
        {
            tracklistClickTimer.Stop();
            if (!isDragging)
            {
                var hitTestInfo = dgvTrackList.HitTest(dragStartPoint.X, dragStartPoint.Y);
                if (hitTestInfo.RowIndex >= 0)
                {
                    this.PlayTrackEvent?.Invoke(this, new Messenger() { });
                }
            }
        }

        // Index to track the insertion line position
        private int insertionLineIndex = -1;

        // Event handler for drag over events in the DataGridView
        private void dgvTrackList_DragOver(object sender, DragEventArgs e)
        {

            if (isFilterEnabled)
            {
                e.Effect = DragDropEffects.None;
                insertionLineIndex = -1; // Clear the insertion line
            }
            else
            {
                Point clientPoint = dgvTrackList.PointToClient(new Point(e.X, e.Y));
                int rowIndex = dgvTrackList.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    e.Effect = DragDropEffects.Copy;
                    if (rowIndex < 0) // Prevent dropping above the header
                    {
                        rowIndex = dgvTrackList.Rows.Count; // Allow dropping at the end 
                    }
                    else
                    {
                        Rectangle rowBounds = dgvTrackList.GetRowDisplayRectangle(rowIndex, false);
                        int rowHeight = rowBounds.Height;
                        int cursorY = clientPoint.Y - rowBounds.Top;

                        if (cursorY < rowHeight / 2)
                        {
                            DrawTrackListInsertionLine(rowIndex);
                        }
                        else
                        {
                            DrawTrackListInsertionLine(rowIndex + 1);
                        }
                    }
                    // Check if cursor is below the last row
                    if (rowIndex == dgvTrackList.Rows.Count - 1)
                    {
                        Rectangle lastRowBounds = dgvTrackList.GetRowDisplayRectangle(rowIndex, false);
                        if (clientPoint.Y > lastRowBounds.Bottom - (lastRowBounds.Height / 2))
                        {
                            DrawTrackListInsertionLine(dgvTrackList.Rows.Count);
                            e.Effect = DragDropEffects.Copy; // Allow dropping at the end
                        }
                    }
                    else if (rowIndex == dgvTrackList.Rows.Count)
                    {
                        DrawTrackListInsertionLine(rowIndex);
                        e.Effect = DragDropEffects.Copy; // Allow dropping at the end
                    }
                    // Handle drag over from external source (files)
                    /* if (rowIndex < 0) // Prevent dropping above the header
                     {
                         e.Effect = DragDropEffects.None;
                         insertionLineIndex = -1; // Clear the insertion line
                     }
                     else
                     {
                         e.Effect = DragDropEffects.Copy;

                         Rectangle rowBounds = dgvTrackList.GetRowDisplayRectangle(rowIndex, false);
                         int rowHeight = rowBounds.Height;
                         int cursorY = clientPoint.Y - rowBounds.Top;

                         if (cursorY < rowHeight / 2)
                         {
                             DrawTrackListInsertionLine(rowIndex);
                         }
                         else
                         {
                             DrawTrackListInsertionLine(rowIndex + 1);
                         }
                     }*/
                }
                else if (e.Data.GetDataPresent(TrackListDataFormat))
                {
                    // Handle drag over within track list
                    if (rowIndex < 0 && rowIndex != -1) // Prevent dropping above the header
                    {
                        e.Effect = DragDropEffects.None;
                        insertionLineIndex = -1; // Clear the insertion line
                    }
                    else
                    {
                       /* e.Effect = DragDropEffects.Move;

                        Rectangle rowBounds = dgvTrackList.GetRowDisplayRectangle(rowIndex, false);
                        int rowHeight = rowBounds.Height;
                        int cursorY = clientPoint.Y - rowBounds.Top;

                        if (cursorY < rowHeight / 2)
                        {
                            DrawTrackListInsertionLine(rowIndex);
                        }
                        else
                        {
                            DrawTrackListInsertionLine(rowIndex + 1);
                        }*/
                        if (rowIndex >= 0)
                        {
                            Rectangle rowBounds = dgvTrackList.GetRowDisplayRectangle(rowIndex, false);
                            int rowHeight = rowBounds.Height;
                            int cursorY = clientPoint.Y - rowBounds.Top;

                            if (cursorY < rowHeight / 2)
                            {
                                DrawTrackListInsertionLine(rowIndex);
                            }
                            else
                            {
                                DrawTrackListInsertionLine(rowIndex + 1);
                            }
                        }
                        else
                        {
                            DrawTrackListInsertionLine(dgvTrackList.Rows.Count);
                        }

                        e.Effect = DragDropEffects.Move; // Set the effect to Move
                    }
                    // Check if cursor is below the last row
                    if (rowIndex == dgvTrackList.Rows.Count - 1)
                    {
                        Rectangle lastRowBounds = dgvTrackList.GetRowDisplayRectangle(rowIndex, false);
                        if (clientPoint.Y > lastRowBounds.Bottom - (lastRowBounds.Height / 2))
                        {
                            DrawTrackListInsertionLine(dgvTrackList.Rows.Count);
                            e.Effect = DragDropEffects.Move; // Allow dropping at the end
                        }
                    }
                    else if (rowIndex == dgvTrackList.Rows.Count)
                    {
                        DrawTrackListInsertionLine(rowIndex);
                        e.Effect = DragDropEffects.Move; // Allow dropping at the end
                    }
                    else if (rowIndex == -1)
                    {
                        DrawTrackListInsertionLine(dgvTrackList.Rows.Count);
                        e.Effect = DragDropEffects.Move; // Allow dropping at the end
                    }
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                    insertionLineIndex = -1; // Clear the insertion line
                }

                dgvTrackList.Invalidate(); // Update the line
            }

        }

        // Method to draw the insertion line in the DataGridView
        private void DrawTrackListInsertionLine(int rowIndex)
        {
            if (rowIndex >= 0 && rowIndex <= dgvTrackList.Rows.Count) // Ensure the insertion line is not drawn above the header
            {
                insertionLineIndex = rowIndex;
                dgvTrackList.Invalidate();
            }
        }


        // Event handler for drag drop events in the DataGridView
        private void dgvTrackList_DragDrop(object sender, DragEventArgs e)
        {
            if (isFilterEnabled)
            {
                return;
            }

            autoScrollTimer.Stop();

            Point clientPoint = dgvTrackList.PointToClient(new Point(e.X, e.Y));
            var hitTestInfo = dgvTrackList.HitTest(clientPoint.X, clientPoint.Y);
            int targetIndex = hitTestInfo.RowIndex >= 0 ? hitTestInfo.RowIndex : dgvTrackList.Rows.Count;

            if (targetIndex < 0 || targetIndex > dgvTrackList.Rows.Count) // Allow dropping at the end
            {
                targetIndex = dgvTrackList.Rows.Count;
            }

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Handle files dropped from a directory
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);

                // Call your function to process the files
                this.ExternalDragAndDropIntoTracklistEvent?.Invoke(this, new Messenger() { DragAndDropFilePathArray = files, IntegerField1 = targetIndex });

                // Select the newly added files
                dgvTrackList.ClearSelection();
                foreach (string file in files)
                {
                    int addedRowIndex = FindRowIndexByFile(file); // Implement this method to find the row index of the added file
                    if (addedRowIndex >= 0)
                    {
                        dgvTrackList.Rows[addedRowIndex].Selected = true;
                    }
                }

                // Clear the insertion line
                insertionLineIndex = -1;
                dgvTrackList.Invalidate(); // Clear the line
            }
            else if (dgvTrackList.SelectedRows.Count > 0)
            {
                // Handle reordering of existing rows
                List<int> selectedIndices = dgvTrackList.SelectedRows.Cast<DataGridViewRow>().OrderBy(r => r.Index).Select(r => r.Index).ToList();

                this.MoveTracklistRowsEvent?.Invoke(this, new Messenger() { SelectedIndices = selectedIndices, IntegerField1 = targetIndex });

                // Re-select the moved rows using BeginInvoke to ensure it runs after the DataGridView has rendered
                dgvTrackList.BeginInvoke(new Action(() =>
                {
                    dgvTrackList.ClearSelection();
                    int newIndex = targetIndex;

                    // Adjust the target index if necessary
                    if (targetIndex > selectedIndices.Min())
                    {
                        newIndex -= selectedIndices.Count;
                    }

                    foreach (int index in selectedIndices)
                    {
                        if (newIndex < dgvTrackList.Rows.Count)
                        {
                            dgvTrackList.Rows[newIndex].Selected = true;
                            newIndex++;
                        }
                    }
                }));

                // Clear the insertion line
                insertionLineIndex = -1;
                dgvTrackList.Invalidate(); // Clear the line
            }

            isCoverBrowserUpdateEnabled = true; // Enable cover browser update
            if (lastMouseDownRowIndex >= 0)
            {
                UpdateCoverBrowser(lastMouseDownRowIndex); // Update the cover browser with the captured index
            }

            // Reset state variables
            isDragging = false;
            isMouseDown = false;
            insertionLineIndex = -1;
            dgvTrackList.Invalidate(); // Refresh the DataGridView display
        }
        private int FindRowIndexByFile(string filePath)
        {
            foreach (DataGridViewRow row in dgvTrackList.Rows)
            {
                if (row.Cells["Path"].Value.ToString() == filePath) // Feltételezve, hogy van egy "FilePathColumn" oszlop
                {
                    return row.Index;
                }
            }
            return -1; // Ha nem található
        }
        private void UpdateCoverBrowser(int index)
        {
            if (isCoverBrowserUpdateEnabled)
            {
                // Call the method to update the cover browser
                this.SetCurrentTrackEvent?.Invoke(this, new Messenger() { IntegerField1 = index });
            }
        }

        // Event handler for paint events in the DataGridView
        private void dgvTrackList_Paint(object sender, PaintEventArgs e)
        {
            if (insertionLineIndex >= 0 && insertionLineIndex < dgvTrackList.Rows.Count)
            {
                int y = 0;
                if (dgvTrackList.Rows.Count == 0)
                {
                    // Handle the case when the track list is empty
                    y = dgvTrackList.ClientRectangle.Top;
                }
                else if (insertionLineIndex < dgvTrackList.Rows.Count)
                {
                    Rectangle rowRect = dgvTrackList.GetRowDisplayRectangle(insertionLineIndex, true);
                    y = rowRect.Top;
                }
                else
                {
                    Rectangle rowRect = dgvTrackList.GetRowDisplayRectangle(insertionLineIndex - 1, true);
                    y = rowRect.Bottom;
                }

                using (Pen pen = new Pen(this.ActiveButtonColor, 2))
                {
                    e.Graphics.DrawLine(pen, new Point(0, y), new Point(dgvTrackList.Width, y));
                }
            }
        }
        private void dgvTrackList_DragEnter(object sender, DragEventArgs e)
        {
            if (!isFilterEnabled)
            {
                autoScrollTimer.Start();
                isCoverBrowserUpdateEnabled = false; // Disable cover browser update

                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    e.Effect = DragDropEffects.Copy;
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                }
            }

        }
        private void dgvTrackList_DragLeave(object sender, EventArgs e)
        {
            autoScrollTimer.Stop();
            insertionLineIndex = -1;
            dgvTrackList.Invalidate(); // Clear the line

            if (isDragging)
            {
                // Handle the drag leave event to initiate a file drop
                StartFileDrop();
            }
        }
        private void StartFileDrop()
        {
            if (dgvTrackList.SelectedRows.Count == 1)
            {
                DataGridViewRow selectedRow = dgvTrackList.SelectedRows[0];
                string trackPath = dgvTrackList.SelectedRows[0].Cells["Path"].Value.ToString();

                // Initiate the file drop operation
                DataObject dataObject = new DataObject(DataFormats.FileDrop, new string[] { trackPath });
                DragDropEffects effect = DoDragDrop(dataObject, DragDropEffects.Copy);
            }
        }

        private System.Windows.Forms.Timer autoScrollTimer = new System.Windows.Forms.Timer();
        private const int FastScrollAreaHeight = 10; // Percentage for fast scrolling
        private const int SlowScrollAreaHeight = 20; // Percentage for slow scrolling
        private const int FastScrollSpeed = 3; // Number of rows to scroll for fast scrolling
        private const int SlowScrollSpeed = 1; // Number of rows to scroll for slow scrolling

        // Timer Tick event to handle auto-scrolling
        private void AutoScrollTimer_Tick(object sender, EventArgs e)
        {
            Point clientPoint = dgvTrackList.PointToClient(Cursor.Position);
            int fastScrollArea = dgvTrackList.Height * FastScrollAreaHeight / 100;
            int slowScrollArea = dgvTrackList.Height * SlowScrollAreaHeight / 100;

            if (clientPoint.Y < fastScrollArea)
            {
                // Fast scroll up
                if (dgvTrackList.FirstDisplayedScrollingRowIndex > 0)
                {
                    dgvTrackList.FirstDisplayedScrollingRowIndex = Math.Max(0, dgvTrackList.FirstDisplayedScrollingRowIndex - FastScrollSpeed);
                }
            }
            else if (clientPoint.Y < slowScrollArea)
            {
                // Slow scroll up
                if (dgvTrackList.FirstDisplayedScrollingRowIndex > 0)
                {
                    dgvTrackList.FirstDisplayedScrollingRowIndex = Math.Max(0, dgvTrackList.FirstDisplayedScrollingRowIndex - SlowScrollSpeed);
                }
            }
            else if (clientPoint.Y > dgvTrackList.Height - fastScrollArea)
            {
                // Fast scroll down
                if (dgvTrackList.FirstDisplayedScrollingRowIndex < dgvTrackList.RowCount - 1)
                {
                    dgvTrackList.FirstDisplayedScrollingRowIndex = Math.Min(dgvTrackList.RowCount - 1, dgvTrackList.FirstDisplayedScrollingRowIndex + FastScrollSpeed);
                }
            }
            else if (clientPoint.Y > dgvTrackList.Height - slowScrollArea)
            {
                // Slow scroll down
                if (dgvTrackList.FirstDisplayedScrollingRowIndex < dgvTrackList.RowCount - 1)
                {
                    dgvTrackList.FirstDisplayedScrollingRowIndex = Math.Min(dgvTrackList.RowCount - 1, dgvTrackList.FirstDisplayedScrollingRowIndex + SlowScrollSpeed);
                }
            }
        }



        // Custom data format for drag-and-drop
        private const string TrackListDataFormat = "TrackListData";
        private const string PlaylistListDataFormat = "PlaylistListData";
        // Timer to handle double-click detection for playlist list
        private System.Windows.Forms.Timer playlistListClickTimer;
        // Time interval for detecting double-clicks
        private const int playlistListDoubleClickTime = 2000; // Adjust the delay as needed
                                                         // Flags to track dragging state for playlist list
        private bool isPlaylistDragging = false;
        private bool isPlaylistMouseDown = false;
        // Point to store the starting position of a drag for playlist list
        private Point playlistDragStartPoint;

        // Index of the first selected row in the playlist list
        private int firstSelectedPlaylistRowIndex = -1;
        // Index to track the insertion line position for playlist list
        private int playlistInsertionLineIndex = -1;

        // Event handler for selection changes in the playlist DataGridView
        private void dgvPlaylistList_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPlaylistList.SelectedRows.Count > 0)
            {
                int selectedIndex = dgvPlaylistList.SelectedRows[0].Index;
                firstSelectedPlaylistRowIndex = selectedIndex;
            }
        }
        // Event handler for double-clicks in the playlist DataGridView
        private void dgvPlaylistList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
           /* if (!isPlaylistDragging)
            {
               // this.LoadPlaylistEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = this.dgvPlaylistList.SelectedRows[0].Index });
                this.CallLoadPlaylistEvent();
            }*/
        }
        // Event handler for mouse down events in the playlist DataGridView
        private void dgvPlaylistList_MouseDown(object sender, MouseEventArgs e)
        {
            var hitTestInfo = dgvPlaylistList.HitTest(e.X, e.Y);
            if (hitTestInfo.RowIndex > 0 && e.Button == MouseButtons.Left) // Prevent dragging the first row
            {
                if (dgvPlaylistList.SelectedRows.Count == 1 && dgvPlaylistList.SelectedRows[0].Index == hitTestInfo.RowIndex)
                {
                    // Single row is already selected, start drag-and-drop
                    isPlaylistMouseDown = true;
                    isPlaylistDragging = false;
                    playlistDragStartPoint = e.Location;
                    firstSelectedPlaylistRowIndex = hitTestInfo.RowIndex;
                    playlistListClickTimer.Start();
                }
                else
                {
                    // Row is not selected, select it
                    dgvPlaylistList.ClearSelection();
                    dgvPlaylistList.Rows[hitTestInfo.RowIndex].Selected = true;
                    isPlaylistMouseDown = true;
                    isPlaylistDragging = false;
                    playlistDragStartPoint = e.Location;
                    firstSelectedPlaylistRowIndex = hitTestInfo.RowIndex;
                    playlistListClickTimer.Start();
                }
            }
            if (hitTestInfo.RowIndex >= 0 && e.Button == MouseButtons.Right)
            {
                dgvPlaylistList.ClearSelection();
                dgvPlaylistList.Rows[hitTestInfo.RowIndex].Selected = true;
            }
        }
        // Event handler for mouse move events in the playlist DataGridView
        private void dgvPlaylistList_MouseMove(object sender, MouseEventArgs e)
        {
            if (isPlaylistMouseDown && !isPlaylistDragging)
            {
                if (Math.Abs(e.X - playlistDragStartPoint.X) > SystemInformation.DragSize.Width ||
                Math.Abs(e.Y - playlistDragStartPoint.Y) > SystemInformation.DragSize.Height)
                {
                    isPlaylistDragging = true;
                    playlistListClickTimer.Stop();
                    dgvPlaylistList.DoDragDrop(new DataObject(PlaylistListDataFormat, dgvPlaylistList.SelectedRows[0]), DragDropEffects.Move);
                }
            }
        }
        // Event handler for mouse up events in the playlist DataGridView
        private void dgvPlaylistList_MouseUp(object sender, MouseEventArgs e)
        {
            isPlaylistMouseDown = false;
            if (!isPlaylistDragging)
            {
                playlistListClickTimer.Stop();
            }
            isPlaylistDragging = false;
        }
        // Timer tick event handler to handle single-click actions for playlist list
        private void PlaylistListClickTimer_Tick(object sender, EventArgs e)
        {
            playlistListClickTimer.Stop();
            if (!isPlaylistDragging)
            {
                this.CallLoadPlaylistEvent();
               // this.LoadPlaylistEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = this.dgvPlaylistList.SelectedRows[0].Index });
            }
        }
        // Field to track the index of the row with the colored border
        private int highlightedPlaylistRowIndex = -1;

        // Event handler for drag over events in the DataGridView
        private void dgvPlaylistList_DragOver(object sender, DragEventArgs e)
        {
            Point clientPoint = dgvPlaylistList.PointToClient(new Point(e.X, e.Y));
            int rowIndex = dgvPlaylistList.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Handle drag over from external source (files)
                if (rowIndex < 0) // Prevent dropping on the first row
                {
                    e.Effect = DragDropEffects.None;
                    highlightedPlaylistRowIndex = -1;
                }
                else
                {
                    e.Effect = DragDropEffects.Copy;
                    DrawHighlightedPlaylistRowBorder(rowIndex);
                }
                playlistInsertionLineIndex = -1; // Ensure insertion line is not drawn
            }
            else if (e.Data.GetDataPresent(TrackListDataFormat))
            {
                // Handle drag over from track list
                if (rowIndex < 0) // Prevent dropping on the first row
                {
                    e.Effect = DragDropEffects.None;
                    highlightedPlaylistRowIndex = -1;
                }
                else
                {
                    e.Effect = DragDropEffects.Move;
                    DrawHighlightedPlaylistRowBorder(rowIndex);
                }
                playlistInsertionLineIndex = -1; // Ensure insertion line is not drawn
            }
            else if (e.Data.GetDataPresent(PlaylistListDataFormat))
            {
                // Handle drag over within playlist list
                if (rowIndex <= 0) // Prevent dropping on the first row
                {
                    e.Effect = DragDropEffects.None;
                    playlistInsertionLineIndex = -1;
                }
                else
                {
                    e.Effect = DragDropEffects.Move;

                    Rectangle rowBounds = dgvPlaylistList.GetRowDisplayRectangle(rowIndex, false);
                    int rowHeight = rowBounds.Height;
                    int cursorY = clientPoint.Y - rowBounds.Top;

                    if (cursorY < rowHeight / 2)
                    {
                        DrawPlaylistInsertionLine(rowIndex);
                    }
                    else
                    {
                        DrawPlaylistInsertionLine(rowIndex + 1);
                    }
                }
                highlightedPlaylistRowIndex = -1; // Ensure border is not drawn
            }

            dgvPlaylistList.Invalidate(); // Update the line or border
        }
        // Method to draw the insertion line in the playlist list
        private void DrawPlaylistInsertionLine(int rowIndex)
        {
            playlistInsertionLineIndex = rowIndex;
            dgvPlaylistList.Invalidate();
        }
        private void dgvPlaylistList_DragDrop(object sender, DragEventArgs e)
        {
            Point clientPoint = dgvPlaylistList.PointToClient(new Point(e.X, e.Y));
            int rowIndex = dgvPlaylistList.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                // Handle files dropped from a directory
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            
                // Call your function to process the files
                this.ExternalDragAndDropIntoPlaylistEvent?.Invoke(this, new Messenger() { DragAndDropFilePathArray = files, IntegerField1 = rowIndex });


                // Clear the highlighted row
                highlightedPlaylistRowIndex = -1;
                dgvPlaylistList.Invalidate(); // Clear the border
            }
            else if (e.Data.GetDataPresent(TrackListDataFormat))
            {
                // Handle drop from track list
                if (rowIndex >= 0) // Prevent dropping on the first row
                {
                    this.InternalDragAndDropIntoPlaylistEvent?.Invoke(this, new Messenger() { SelectedRows = this.dgvTrackList.SelectedRows, IntegerField1 = rowIndex });
                }
                playlistInsertionLineIndex = -1; // Clear the insertion line
            }
            else if (e.Data.GetDataPresent(PlaylistListDataFormat))
            {
                // Handle drop within playlist list
                if (rowIndex > 0) // Prevent dropping on the first row
                {
                    this.MovePlaylistRowEvent?.Invoke(this, new Messenger() { IntegerField1 = firstSelectedPlaylistRowIndex, IntegerField2 = rowIndex });
                }
                highlightedPlaylistRowIndex = -1; // Clear the border
            }

            // Re-select the moved rows using BeginInvoke to ensure it runs after the DataGridView has rendered
            dgvPlaylistList.BeginInvoke(new Action(() =>
            {
                dgvPlaylistList.ClearSelection();
                dgvPlaylistList.Rows[rowIndex].Selected = true;
            }));

            // Clear the insertion line and highlighted row
            playlistInsertionLineIndex = -1;
            highlightedPlaylistRowIndex = -1;
            dgvPlaylistList.Invalidate(); // Clear the line and border
        }
        // Event handler for drag leave events in the DataGridView
        private void dgvPlaylistList_DragLeave(object sender, EventArgs e)
        {
            highlightedPlaylistRowIndex = -1;
            dgvPlaylistList.Invalidate(); // Clear the border
        }
        // Method to draw the highlighted row border in the playlist list
        private void DrawHighlightedPlaylistRowBorder(int rowIndex)
        {
            highlightedPlaylistRowIndex = rowIndex;
            dgvPlaylistList.Invalidate();
        }
        // Event handler for painting the playlist DataGridView
        private void dgvPlaylistList_Paint(object sender, PaintEventArgs e)
        {
            if (highlightedPlaylistRowIndex >= 0 && highlightedPlaylistRowIndex < dgvPlaylistList.Rows.Count)
            {
                Rectangle rowRect = dgvPlaylistList.GetRowDisplayRectangle(highlightedPlaylistRowIndex, true);
                using (Pen pen = new Pen(this.ActiveButtonColor, 2)) // Change to your desired color
                {
                    e.Graphics.DrawRectangle(pen, rowRect);
                }
            }

            if (playlistInsertionLineIndex >= 0 && playlistInsertionLineIndex <= dgvPlaylistList.Rows.Count)
            {
                int y = 0;
                if (playlistInsertionLineIndex < dgvPlaylistList.Rows.Count)
                {
                    Rectangle rowRect = dgvPlaylistList.GetRowDisplayRectangle(playlistInsertionLineIndex, true);
                    y = rowRect.Top;
                }
                else
                {
                    Rectangle rowRect = dgvPlaylistList.GetRowDisplayRectangle(playlistInsertionLineIndex - 1, true);
                    y = rowRect.Bottom;
                }

                using (Pen pen = new Pen(this.ActiveButtonColor, 2))
                {
                    e.Graphics.DrawLine(pen, new Point(0, y), new Point(dgvPlaylistList.Width, y));
                }
            }
        }
        private void dgvPlaylistList_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }


        public void InitializeCoverImageComponent(bool isCoverImageComponentDisplayed)
        {
            this.coverImageComponentEnabled = isCoverImageComponentDisplayed;

            if (this.coverImageComponentEnabled)
            {
                this.btnDisplayCoverImageToggle.Image = Resources.Arrow_Up_20_20;
            }
            else
            {
                this.btnDisplayCoverImageToggle.Image = Resources.Arrow_Down_20_20;
                this.grbCoverImageComponent.Hide();
                this.coverBrowserClickTimer.Stop();
                if (this.dgvTrackList.Top > 43)
                {
                    this.dgvTrackList.Top -= this.trackListTopOffset;
                    this.dgvTrackList.Height += this.trackListTopOffset;

                }
            }
        }
        private bool coverImageComponentEnabled { get; set; }

        public void UpdateDisplayCoverImageComponent(bool isCoverImageComponentDisplayed)
        {
            this.coverImageComponentEnabled = isCoverImageComponentDisplayed;

            if (this.coverImageComponentEnabled)
            {
                this.btnDisplayCoverImageToggle.Image = Resources.Arrow_Up_20_20;
                this.grbCoverImageComponent.Show();
                this.coverBrowserClickTimer.Start();

                int rowIndex = 0;
                if (this.dgvTrackList != null && this.dgvTrackList.SelectedCells.Count > 0)
                {
                    rowIndex = this.dgvTrackList.SelectedCells[0].RowIndex;
                }
                this.LoadCoversEvent?.Invoke(this, new Messenger() { IntegerField1 = rowIndex });
                this.dgvTrackList.Top = this.dgvTrackList.Top + this.trackListTopOffset;
                this.dgvTrackList.Height -= this.trackListTopOffset;
            }
            else
            {
                this.btnDisplayCoverImageToggle.Image = Resources.Arrow_Down_20_20;
                this.grbCoverImageComponent.Hide();
                this.coverBrowserClickTimer.Stop();
                this.dgvTrackList.Top = this.dgvTrackList.Top - this.trackListTopOffset;
                this.dgvTrackList.Height += this.trackListTopOffset;
            }
        }

        private bool isWaiting = false;
        private System.Windows.Forms.Timer coverBrowserClickTimer;
        private const int coverBrowserDoubleClickTime = 200;
        private PictureBoxExtension currentPictureBox;

        public void UpdateCoverList(ConcurrentQueue<ImageExtension> coverQueue)
        {
            if (this.coverImageComponentEnabled)
            {
                this.grbCoverImageComponent.Controls.Clear();

                List<ImageExtension> coverList = coverQueue.ToList();

                int imageXOffset = 10;
                int imageYOffset = 10;
                int imageSpacing = 5;
                int xPos = 10;
                int yPos = 10;

                int imageCount = coverList.Count;
                int imageWidth = coverImageSize;
                int imageHeight = coverImageSize;

                int flowLayoutWidth = imageXOffset + ((imageCount - 1) * (imageWidth + imageSpacing) + 10 + mainCoverImageSize);
                int flowLayoutHeight = imageYOffset + imageHeight + imageSpacing;
                int centerOffset = (this.grbCoverImageComponent.Width - flowLayoutWidth) / 2;

                FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel
                {
                    FlowDirection = FlowDirection.LeftToRight,
                    Anchor = AnchorStyles.Top,
                    Size = new Size(flowLayoutWidth, flowLayoutHeight),
                    Location = new Point(centerOffset, 10),
                };

                while (coverQueue.TryDequeue(out ImageExtension cover))
                {
                    int actualImageSize = cover.IsMainCover ? mainCoverImageSize : coverImageSize;

                    PictureBoxExtension pictureBox = new PictureBoxExtension
                    {
                        Image = cover.Image,
                        FilePath = cover.FilePath,
                        TrackIdInPlaylist = cover.TrackIdInPlaylist,
                        SizeMode = PictureBoxSizeMode.StretchImage,
                        Size = new Size(actualImageSize, actualImageSize),
                        Location = new Point(xPos, yPos),
                    };

                    pictureBox.MouseDown += new MouseEventHandler(pcbCover_MouseDown);
                    pictureBox.MouseUp += new MouseEventHandler(pcbCover_MouseUp);
                    pictureBox.MouseDoubleClick += new MouseEventHandler(pcbCover_MouseDoubleClick);

                    flowLayoutPanel.Controls.Add(pictureBox);
                    xPos += pictureBox.Width + imageSpacing;
                }
                this.grbCoverImageComponent.Controls.Add(flowLayoutPanel);
            }
        }
        private void CoverBrowserClickTimer_Tick(object sender, EventArgs e)
        {
            if (isWaiting && currentPictureBox != null)
            {
                isWaiting = false;
                coverBrowserClickTimer.Stop();

                string filePath = currentPictureBox.FilePath;
                DataObject dataObject = new DataObject(DataFormats.FileDrop, new string[] { filePath });
                currentPictureBox.DoDragDrop(dataObject, DragDropEffects.Copy);
            }
        }
        private void pcbCover_MouseDown(object sender, MouseEventArgs e)
        {
            isWaiting = true;
            currentPictureBox = sender as PictureBoxExtension;
            coverBrowserClickTimer.Start();
        }
        private void pcbCover_MouseUp(object sender, MouseEventArgs e)
        {
            if (isWaiting)
            {
                isWaiting = false;
                coverBrowserClickTimer.Stop();
            }
        }
        private void pcbCover_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            int trackIdInPlaylist = ((PictureBoxExtension)sender).TrackIdInPlaylist;

            dgvTrackList.ClearSelection();

            foreach (DataGridViewRow row in dgvTrackList.Rows)
            {
                if (Convert.ToInt32(row.Cells["TrackIdInPlaylist"].Value) == trackIdInPlaylist)
                {
                    row.Selected = true;

                    DataGridViewCell firstVisibleCell = null;
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        if (cell.Visible)
                        {
                            firstVisibleCell = cell;
                            break;
                        }
                    }

                    if (firstVisibleCell != null)
                    {
                        dgvTrackList.CurrentCell = firstVisibleCell;
                    }
                    dgvTrackList.FirstDisplayedScrollingRowIndex = row.Index;
                    break;
                }
            }
        }
        private void btnDisplayCoverImageToggle_Click(object sender, EventArgs e)
        {
            this.DisplayCoverImageComponentEvent.Invoke(this, new EventArgs());
        }



        private void btnTrainKeyDetector_Click(object sender, EventArgs e)
        {

        }

        private void btnDetectKey_Click(object sender, EventArgs e)
        {
            if (this.dgvTrackList.SelectedRows.Count > 0)
            {
                this.DetectKeyEvent?.Invoke(this, new Messenger() { Rows = this.dgvTrackList.Rows });
            }
        }

        private void btnAddTrackToModel_Click(object sender, EventArgs e)
        {
            if (this.dgvTrackList.SelectedRows.Count > 0)
            {
                this.AddToKeyDetectorEvent?.Invoke(this, new Messenger() { Rows = this.dgvTrackList.Rows });
            }
        }

        public void UpdateTrackCountInModel(int count)
        {

        }
        public void UpdateLog(String message)
        {
           
            this.lblMessage.Text = message;
        }

        private void btnCreateModel_Click(object sender, EventArgs e)
        {
            this.CreateModelEvent?.Invoke(this, new EventArgs());
        }

        private void btnTrainModel_Click(object sender, EventArgs e)
        {
            this.TrainKeyDetectorEvent?.Invoke(this, new EventArgs());
        }

        private void PlaylistView_Shown(object sender, EventArgs e)
        {
            this.ToggleTracklistSelection(true);
        }



        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (this.dgvPlaylistList.SelectedRows.Count > 0)
            {
                
            }
        }

        private void btnGenerateTrainingSet_Click(object sender, EventArgs e)
        {
            this.OpenModelTrainerEvent?.Invoke(this, new EventArgs());
        }

        private void btnDetectKey_Click_1(object sender, EventArgs e)
        {
            if (this.dgvTrackList.SelectedRows.Count > 0)
            {
                this.DetectKeyEvent?.Invoke(this, new Messenger() { Rows = this.dgvTrackList.Rows });
            }
        }

        private void btnLiveStreamAnimation_Click(object sender, EventArgs e)
        {
            this.LiveStreamAnimationEvent?.Invoke(this, new EventArgs());
        }

        private void btnLiveStreamAnimationSetting_Click(object sender, EventArgs e)
        {
            this.LiveStreamAnimationSettingEvent?.Invoke(this, new EventArgs());
        }

        internal void SetFocusToDataGridView()
        {
            this.dgvTrackList.Focus();
        }

        private void dgvPlaylistList_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }
    }
}
