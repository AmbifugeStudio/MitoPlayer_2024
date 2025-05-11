using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.IViews;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MitoPlayer_2024.Views
{
    public partial class SelectorView : Form, ISelectorView
    {
        private Form parentView { get; set; }

        //DATATABLES
        private BindingSource playlistListBindingSource { get; set; }
        private BindingSource trackListBindingSource { get; set; }
        private BindingSource selectorTrackListBindingSource { get; set; }
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
        //TRACKLIST
        public event EventHandler<Messenger> OrderByColumnEvent;
        public event EventHandler<Messenger> DeleteTracksEvent;
        public event EventHandler<Messenger> InternalDragAndDropIntoTracklistEvent;
        public event EventHandler<Messenger> InternalDragAndDropIntoPlaylistEvent;
        public event EventHandler<Messenger> ExternalDragAndDropIntoTracklistEvent;
        public event EventHandler<Messenger> ExternalDragAndDropIntoPlaylistEvent;
        public event EventHandler ShowColumnVisibilityEditorEvent;
        public event EventHandler<Messenger> MoveTracklistRowsEvent;
        //SELECTOR TRACKLIST
        public event EventHandler<Messenger> DeleteTracksFromSelectorEvent;
        //PLAYLIST
        public event EventHandler<Messenger> CreatePlaylist;
        public event EventHandler<Messenger> EditPlaylist;
        public event EventHandler<Messenger> LoadPlaylistEvent;
        public event EventHandler<Messenger> MovePlaylistEvent;
        public event EventHandler<Messenger> DeletePlaylistEvent;
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
        public event EventHandler SaveSelectorTrackListEvent;

        private bool isFilterEnabled = true;
        private bool isInitializing = true;

        private bool isPlaylistDragging = false;
        public SelectorView()
        {
            InitializeComponent();
            this.SetControlColors();

            this.playlistListBindingSource = new BindingSource();
            this.trackListBindingSource = new BindingSource();
            this.selectorTrackListBindingSource = new BindingSource();

            dgvTrackList.AllowDrop = true;
            dgvPlaylistList.AllowDrop = true;
            dgvSelectorTrackList.AllowDrop = true;

            playlistListClickTimer = new System.Windows.Forms.Timer();
            playlistListClickTimer.Interval = playlistListDoubleClickTime;
            playlistListClickTimer.Tick += PlaylistListClickTimer_Tick;

            tracklistClickTimer = new System.Windows.Forms.Timer();
            tracklistClickTimer.Interval = tracklistDoubleClickTime;
            tracklistClickTimer.Tick += TracklistClickTimer_Tick;
            autoScrollTimer = new System.Windows.Forms.Timer();
            autoScrollTimer.Interval = 100;
            autoScrollTimer.Tick += AutoScrollTimer_Tick;

            selectorClickTimer = new System.Windows.Forms.Timer();
            selectorClickTimer.Interval = selectorDoubleClickTime;
            selectorClickTimer.Tick += SelectorClickTimer_Tick;
            selectorAutoScrollTimer = new System.Windows.Forms.Timer();
            selectorAutoScrollTimer.Interval = 100;
            selectorAutoScrollTimer.Tick += SelectorAutoScrollTimer_Tick;

            isInitializing = true;
        }

        private void SetControlColors()
        {
            this.BackColor = CustomColor.BackColor;
            this.ForeColor = CustomColor.ForeColor;

            this.btnPlaylistListPanelToggle.BackColor = CustomColor.ButtonBackColor;
            this.btnPlaylistListPanelToggle.ForeColor = CustomColor.ForeColor;
            this.btnPlaylistListPanelToggle.FlatAppearance.BorderColor = CustomColor.ButtonBorderColor;

            this.dgvPlaylistList.BackgroundColor = CustomColor.ButtonBackColor;
            this.dgvPlaylistList.ColumnHeadersDefaultCellStyle.BackColor = CustomColor.ButtonBackColor;
            this.dgvPlaylistList.ColumnHeadersDefaultCellStyle.ForeColor = CustomColor.ForeColor;
            this.dgvPlaylistList.EnableHeadersVisualStyles = false;
            this.dgvPlaylistList.ColumnHeadersDefaultCellStyle.SelectionBackColor = CustomColor.ButtonBackColor;
            this.dgvPlaylistList.DefaultCellStyle.SelectionBackColor = CustomColor.GridSelectionColor;

            this.dgvTrackList.BackgroundColor = CustomColor.ButtonBackColor;
            this.dgvTrackList.ColumnHeadersDefaultCellStyle.BackColor = CustomColor.ButtonBackColor;
            this.dgvTrackList.ColumnHeadersDefaultCellStyle.ForeColor = CustomColor.ForeColor;
            this.dgvTrackList.EnableHeadersVisualStyles = false;
            this.dgvTrackList.ColumnHeadersDefaultCellStyle.SelectionBackColor = CustomColor.ButtonBackColor;
            this.dgvTrackList.DefaultCellStyle.SelectionBackColor = CustomColor.GridSelectionColor;

            this.dgvSelectorTrackList.BackgroundColor = CustomColor.ButtonBackColor;
            this.dgvSelectorTrackList.ColumnHeadersDefaultCellStyle.BackColor = CustomColor.ButtonBackColor;
            this.dgvSelectorTrackList.ColumnHeadersDefaultCellStyle.ForeColor = CustomColor.ForeColor;
            this.dgvSelectorTrackList.EnableHeadersVisualStyles = false;
            this.dgvSelectorTrackList.ColumnHeadersDefaultCellStyle.SelectionBackColor = CustomColor.ButtonBackColor;
            this.dgvSelectorTrackList.DefaultCellStyle.SelectionBackColor = CustomColor.GridSelectionColor;

            this.contextMenuStrip1.BackColor = CustomColor.BackColor;
            this.contextMenuStrip1.ForeColor = CustomColor.ForeColor;

            this.btnSave.BackColor = CustomColor.ButtonBackColor;
            this.btnSave.ForeColor = CustomColor.ForeColor;
            this.btnSave.FlatAppearance.BorderColor = CustomColor.ButtonBorderColor;

            this.lblMessage.ForeColor = CustomColor.ActiveButtonColor;;
        }

        #region SINGLETON

        public static SelectorView instance;
        public static SelectorView GetInstance(Form parentView)
        {
            if (instance == null || instance.IsDisposed)
            {
                instance = new SelectorView();
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
                        this.dgvPlaylistList.Rows[i].DefaultCellStyle.BackColor = CustomColor.GridPlayingColor;
                    }
                    else
                    {
                        if (i == 0 || i % 2 == 0)
                        {
                            this.dgvPlaylistList.Rows[i].DefaultCellStyle.BackColor = CustomColor.GridLineColor1;
                        }
                        else
                        {
                            this.dgvPlaylistList.Rows[i].DefaultCellStyle.BackColor = CustomColor.GridLineColor2;
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
                        ? CustomColor.GridPlayingColor
                        : (row.Index % 2 == 0 ? CustomColor.GridLineColor1 : CustomColor.GridLineColor2);
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = (currentTrackIdInPlaylist != -1 && trackIdInPlaylist == currentTrackIdInPlaylist)
                        ? CustomColor.GridPlayingColor
                        : (row.Index % 2 == 0 ? CustomColor.GridLineColor1 : CustomColor.GridLineColor2);

                        if (this.isShortTrackColouringEnabled)
                        {
                            if (this.dgvTrackList.Columns["Length"].Visible)
                            {
                                string cellValue = row.Cells["Length"].Value as string;
                                if (cellValue.Length < 6)
                                {
                                    cellValue = "00:" + cellValue;
                                    TimeSpan.TryParseExact(cellValue, @"hh\:mm\:ss", null, out songLength);
                                }

                                TimeSpan threshold = this.shortTrackColouringThreshold;

                                if (songLength < threshold)
                                {
                                    row.Cells["Length"].Style.ForeColor = CustomColor.RedWarningColor;
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
                                       ? CustomColor.GridPlayingColor
                                       : (row.Index % 2 == 0 ? CustomColor.GridLineColor1 : CustomColor.GridLineColor2);
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
                                ? CustomColor.GridPlayingColor
                                : (row.Index % 2 == 0 ? CustomColor.GridLineColor1 : CustomColor.GridLineColor2);
                            }
                        }
                    }
                }
            }
        }


        // SELECTOR TRACKLIST DATA BINDING
        public void InitializeSelectorTrackList(DataTableModel model)
        {
            if (model.BindingSource != null)
            {
                this.selectorTrackListBindingSource.DataSource = model.BindingSource;
                this.dgvSelectorTrackList.DataSource = this.selectorTrackListBindingSource.DataSource;
            }

            if (model.ColumnVisibilityArray != null && model.ColumnVisibilityArray.Length > 0)
            {
                for (int i = 0; i <= this.dgvTrackList.Columns.Count - 1; i++)
                {
                    this.dgvTrackList.Columns[i].Visible = model.ColumnVisibilityArray[i];
                }
            }

            this.selectorTrackListBindingSource.ResetBindings(false);

            this.UpdateSelectorTracklistColor(model.CurrentTrackIdInPlaylist);

        }
        private void dgvSelectorTrackList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            this.UpdateSelectorTracklistColor();
        }
        public void ReloadSelectorTrackList(DataTableModel model)
        {
            this.UpdateSelectorTracklistColor(model.CurrentTrackIdInPlaylist);
        }

        private bool isShortTrackColouringEnabledInSelectorTrackList { get; set; }
        private TimeSpan shortTrackColouringThresholdInSelectorTrackList { get; set; }

        internal void InitializeShortTrackColouringInSelectorTrackList(bool isColouringEnabled, TimeSpan shortTrackColouringThreshold)
        {
            this.isShortTrackColouringEnabledInSelectorTrackList = isColouringEnabled;
            this.shortTrackColouringThresholdInSelectorTrackList = shortTrackColouringThreshold;
        }

        TimeSpan songLengthInSelectorTrackList = new TimeSpan();
        TimeSpan thresholdInSelectorTrackList = new TimeSpan();
        public void UpdateSelectorTracklistColor(int currentTrackIdInPlaylist = -1)
        {
            bool isTagValueColoringEnabled = this.tagList != null && this.tagList.Count > 0 && this.tagValueDictionary != null && this.tagValueDictionary.Count > 0;

            if (this.dgvSelectorTrackList != null && this.dgvSelectorTrackList.Rows != null && this.dgvSelectorTrackList.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in this.dgvSelectorTrackList.Rows)
                {
                    int trackIdInPlaylist = Convert.ToInt32(row.Cells["TrackIdInPlaylist"].Value);
                    bool isMissing = Convert.ToBoolean(row.Cells["IsMissing"].Value);

                    if (isMissing)
                    {
                        row.DefaultCellStyle.ForeColor = Color.Salmon;
                        row.DefaultCellStyle.BackColor = (currentTrackIdInPlaylist != -1 && trackIdInPlaylist == currentTrackIdInPlaylist)
                        ? CustomColor.GridPlayingColor
                        : (row.Index % 2 == 0 ? CustomColor.GridLineColor1 : CustomColor.GridLineColor2);
                    }
                    else
                    {
                        row.DefaultCellStyle.BackColor = (currentTrackIdInPlaylist != -1 && trackIdInPlaylist == currentTrackIdInPlaylist)
                        ? CustomColor.GridPlayingColor
                        : (row.Index % 2 == 0 ? CustomColor.GridLineColor1 : CustomColor.GridLineColor2);

                        if (this.isShortTrackColouringEnabled)
                        {
                            if (this.dgvTrackList.Columns["Length"].Visible)
                            {
                                string cellValue = row.Cells["Length"].Value as string;
                                if (cellValue.Length < 6)
                                {
                                    cellValue = "00:" + cellValue;
                                    TimeSpan.TryParseExact(cellValue, @"hh\:mm\:ss", null, out songLength);
                                }

                                TimeSpan threshold = this.shortTrackColouringThreshold;

                                if (songLength < threshold)
                                {
                                    row.Cells["Length"].Style.ForeColor = CustomColor.RedWarningColor;
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
                                       ? CustomColor.GridPlayingColor
                                       : (row.Index % 2 == 0 ? CustomColor.GridLineColor1 : CustomColor.GridLineColor2);
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
                                ? CustomColor.GridPlayingColor
                                : (row.Index % 2 == 0 ? CustomColor.GridLineColor1 : CustomColor.GridLineColor2);
                            }
                        }
                    }
                }
            }
        }
        #endregion

        private bool controlKey = false;

        #region TRACKLIST - ORDER BY COLUMN HEADER
        private void dgvTrackList_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.OrderByColumnEvent?.Invoke(this, new Messenger() { StringField1 = dgvTrackList.Columns[e.ColumnIndex].Name });
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

        #region MEDIA PLAYER

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
            this.NextTrackEvent?.Invoke(this, new Messenger() { });
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

        public void UpdateAfterPlayTrackInSelectorTrackList(int currentTrackIndex, int currentTrackIdInPlaylist)
        {
            String artist = "Playing: ";
            String title = String.Empty;
            String path = String.Empty;

            if (this.dgvSelectorTrackList.Rows[currentTrackIndex].Cells["Title"].Value != null)
                artist += (string)this.dgvSelectorTrackList.Rows[currentTrackIndex].Cells["Artist"].Value;
            if (this.dgvSelectorTrackList.Rows[currentTrackIndex].Cells["Title"].Value != null)
                title = (string)this.dgvSelectorTrackList.Rows[currentTrackIndex].Cells["Title"].Value;
            if (this.dgvSelectorTrackList.Rows[currentTrackIndex].Cells["Path"].Value != null)
                path = (string)this.dgvSelectorTrackList.Rows[currentTrackIndex].Cells["Path"].Value;

            if (!String.IsNullOrEmpty(title))
            {
                artist += " - " + title;
            }

           ((MainView)this.parentView).UpdateAfterPlayTrackInSelectorTrackList(artist);

            this.UpdateSelectorTracklistColor(currentTrackIdInPlaylist);
        }
        public void UpdateAfterPlayTrackAfterPauseInSelectorTrackLis()
        {
            ((MainView)this.parentView).UpdateAfterPlayTrackAfterPauseInSelectorTrackList();
        }
        public void UpdateAfterStopTrackInSelectorTrackLis()
        {
            ((MainView)this.parentView).UpdateAfterStopTrackInSelectorTrackList();
            this.UpdateSelectorTracklistColor();
        }
        public void UpdateAfterPauseTrackInSelectorTrackLis()
        {
            ((MainView)this.parentView).UpdateAfterPauseTrackInSelectorTrackList();
        }
        public void SetCurrentTrackColorInSelectorTrackLis(int trackIdInPlaylist)
        {
            this.ClearCurrentTrackColorInSelectorTrackLis();

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
        private void ClearCurrentTrackColorInSelectorTrackLis()
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

            if (this.dgvPlaylistList.Rows != null && this.dgvPlaylistList.Rows.Count > 0)
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

            if (this.dgvTrackList.Rows.Count > 0)
            {
                lblTrackCount.Show();
                lblTrackSumLength.Show();
                if (trackCount == 1)
                {
                    lblTrackCount.Text = "1 track in [" + playlistName + "]";
                }
                else
                {
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

        }
        private void dgvTrackList_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.ShiftKey)
            {
                controlKey = false;
            }
        }
        #endregion
       
        #region SELECTEDTRACKLIST - CONTROL BUTTONS
        private void dgvSelectedTrackList_KeyDown(object sender, KeyEventArgs e)
        {
            //DELETE - Delete Track(s)
            if (this.dgvSelectorTrackList.Rows.Count > 0 && e.KeyCode == Keys.Delete)
            {
                this.DeleteTracksFromSelectorEvent?.Invoke(this, new Messenger() { Rows = this.dgvSelectorTrackList.Rows });
            }

            if (this.dgvSelectorTrackList.Rows.Count > 0 && (e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.ShiftKey))
            {
                this.controlKey = true;
            }

            //ENTER or SPACE - Play Track
            if (this.dgvSelectorTrackList.SelectedRows.Count > 0 && e.KeyCode == Keys.Space ||
                this.dgvSelectorTrackList.SelectedRows.Count > 0 && e.KeyCode == Keys.Enter)
            {
                this.CallStopTrackEvent();
                this.CallPlayTrackEvent();
            }

            //CTRL+S - Save playlist
            if (this.controlKey && e.KeyCode == Keys.S && !this.isFilterEnabled)
            {
                this.SaveSelectorTrackListEvent?.Invoke(this, new EventArgs());
            }

        }
        private void dgvSelectorTrackList_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.ShiftKey)
            {
                controlKey = false;
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

        #region TAG EDITOR

        public void InitializeTagComponent(List<Tag> tagList, List<List<TagValue>> tagValueListContainer, bool isTagEditorDisplayed, bool isOnlyPlayingRowModeEnabled, bool isFilterModeEnabled)
        {
            this.tagValueEditorPanel.Controls.Clear();

            if (isFilterModeEnabled)
            {
                this.lblFilter.Show();
                this.txtbFilter.Show();
                this.btnFilter.Show();
                this.btnFilterIsPressed = false;
                this.txtbFilter.Text = string.Empty;
                this.isFilterEnabled = true;
            }
            else
            {
                this.lblFilter.Hide();
                this.txtbFilter.Hide();
                this.btnFilter.Hide();
                this.btnFilterIsPressed = false;
                this.txtbFilter.Text = string.Empty;
                this.isFilterEnabled = false;
            }

            if (isFilterModeEnabled && isTagEditorDisplayed)
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
                gp.ForeColor = CustomColor.ForeColor;

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

                        btn.BackColor = CustomColor.ButtonBackColor;
                        btn.ForeColor = CustomColor.ForeColor;
                        btn.FlatAppearance.BorderColor = CustomColor.ButtonBorderColor;

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

                        btn.BackColor = CustomColor.ButtonBackColor;
                        btn.ForeColor = CustomColor.ForeColor;
                        btn.FlatAppearance.BorderColor = CustomColor.ButtonBorderColor;

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

                            btn.BackColor = CustomColor.ButtonBackColor;
                            btn.ForeColor = tagValueListContainer[i][j - 1].Color;
                            btn.FlatAppearance.BorderColor = CustomColor.ButtonBorderColor;

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

        }
        private int originalTagValueEditorPanelHeight;
        public void InitializeDisplayTagComponent(bool isTagComponentDisplayed)
        {
            this.isFilterEnabled = false;

            this.btnFilter.BackColor = CustomColor.ButtonBackColor;
            this.btnFilter.ForeColor = CustomColor.ForeColor;
            this.btnFilter.FlatAppearance.BorderColor = CustomColor.ButtonBorderColor;

            this.btnClearTagValueFilter.Hide();
            this.lblFilter.Hide();
            this.txtbFilter.Hide();
            this.btnFilter.Hide();
            this.txtbFilter.Text = string.Empty;
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

            this.btnFilter.BackColor = CustomColor.ButtonBackColor;
            this.btnFilter.ForeColor = CustomColor.ForeColor;
            this.btnFilter.FlatAppearance.BorderColor = CustomColor.ButtonBorderColor;

            this.btnSave.Enabled = false;

            this.btnClearTagValueFilter.Show();
            this.lblFilter.Show();
            this.txtbFilter.Show();
            this.btnFilter.Show();
            this.btnFilterIsPressed = false;
            this.txtbFilter.Text = string.Empty;

            this.SetFocusToDataGridView();

            this.EnableFilterModeEvent?.Invoke(this, new EventArgs());

            int rowIndex = 0;
            if (this.dgvTrackList != null && this.dgvTrackList.SelectedCells.Count > 0)
            {
                rowIndex = this.dgvTrackList.SelectedCells[0].RowIndex;
            }

            this.LoadCoversEvent?.Invoke(this, new Messenger() { IntegerField1 = rowIndex });
        }
        internal void SetFocusToDataGridView()
        {
            this.dgvTrackList.Focus();
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
                            ((TagValueButton)buttonOrTextBox).FlatAppearance.BorderColor = CustomColor.ButtonBorderColor;
                            ((TagValueButton)buttonOrTextBox).IsPressed = false;

                            if (((TagValueButton)buttonOrTextBox).TextBox != null)
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
                    this.btnFilter.FlatAppearance.BorderColor = CustomColor.ButtonBorderColor;
                    this.btnFilterIsPressed = false;
                }
            }

            this.isFilterEnabled = false;

            this.btnSave.Enabled = true;

            this.btnClearTagValueFilter.Hide();
            this.lblFilter.Hide();
            this.txtbFilter.Hide();
            this.btnFilter.Hide();
            this.btnFilterIsPressed = false;
            this.txtbFilter.Text = string.Empty;

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
                    btnFilter.FlatAppearance.BorderColor = CustomColor.ActiveButtonColor;
                }
                else
                {
                    if (String.IsNullOrEmpty(this.txtbFilter.Text))
                    {
                        btnFilterIsPressed = false;
                        btnFilter.FlatAppearance.BorderColor = CustomColor.ButtonBorderColor;
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
                btnFilter.FlatAppearance.BorderColor = CustomColor.ActiveButtonColor;
            }
            else
            {
                btnFilter.FlatAppearance.BorderColor = CustomColor.ButtonBorderColor;
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
            if (!this.isSaving)
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
                    button.FlatAppearance.BorderColor = CustomColor.ActiveButtonColor;
                }
                else
                {
                    button.FlatAppearance.BorderColor = CustomColor.ButtonBorderColor;
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
            if (e.KeyCode == Keys.Enter)
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
                        ((TagValueButton)btn).FlatAppearance.BorderColor = CustomColor.ButtonBorderColor;
                        ((TagValueButton)btn).IsPressed = false;
                    }
                }
                this.txtbFilter.Text = string.Empty;
                this.btnFilter.FlatAppearance.BorderColor = CustomColor.ButtonBorderColor;
                this.btnFilterIsPressed = false;
            }

            if (this.dgvTrackList.SelectedRows.Count > 0)
            {
                this.ClearTagValueEvent?.Invoke(this, new Messenger() { StringField1 = button.TagName, Rows = this.dgvTrackList.Rows });
            }

            this.SetFocusToDataGridView();
        }

        public void SetTagValueFilter(List<TagValueFilter> tagValueFilterList)
        {
            String tagValueAsFilter = String.Empty;

            if (tagValueFilterList != null && tagValueFilterList.Count > 0)
            {
                for (int i = 0; i < tagValueFilterList.Count; i++)
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
                            ((TagValueButton)button).FlatAppearance.BorderColor = CustomColor.ButtonBorderColor;
                            ((TagValueButton)button).IsPressed = false;
                        }
                    }
                    this.txtbFilter.Text = string.Empty;
                    this.btnFilter.FlatAppearance.BorderColor = CustomColor.ButtonBorderColor;
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

        public void ChangeSaveButtonColor(bool isTableChanged)
        {
            if (isTableChanged)
            {
                if (!this.isFilterEnabled)
                {
                    this.btnSave.BackColor = CustomColor.ActiveButtonColor;
                    this.btnSave.ForeColor = CustomColor.ButtonBackColor;
                }
            }
            else
            {
                this.btnSave.BackColor = CustomColor.ButtonBackColor;
                this.btnSave.ForeColor = CustomColor.ForeColor;
            }
        }

        #endregion


        private void btnSave_Click(object sender, EventArgs e)
        {
            this.SaveTrackListEvent?.Invoke(this, new EventArgs());
        }
        private void btnSaveSelector_Click(object sender, EventArgs e)
        {
            this.SaveSelectorTrackListEvent?.Invoke(this, new EventArgs());
        }



        #region TRACKLIST DRAG AND DROP

        private const string TrackListDataFormat = "TrackListData";
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
        private bool isCoverBrowserUpdateEnabled = true;
        private bool isDragAndDropInProgress = false;
        private System.Windows.Forms.Timer autoScrollTimer = new System.Windows.Forms.Timer();
        private const int FastScrollAreaHeight = 10; // Percentage for fast scrolling
        private const int SlowScrollAreaHeight = 20; // Percentage for slow scrolling
        private const int FastScrollSpeed = 3; // Number of rows to scroll for fast scrolling
        private const int SlowScrollSpeed = 1; // Number of rows to scroll for slow scrolling

        public void ToggleTracklistSelection(bool enabled)
        {
            isInitializing = !enabled;
        }
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
        // Event handler for mouse down events in the DataGridView
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

                using (Pen pen = new Pen(CustomColor.ActiveButtonColor, 2))
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
        #endregion

        #region PLAYLIST LIST DRAG AND DROP
        // Custom data format for drag-and-drop
        private const string PlaylistListDataFormat = "PlaylistListData";
        // Timer to handle double-click detection for playlist list
        private System.Windows.Forms.Timer playlistListClickTimer;
        // Time interval for detecting double-clicks
        private const int playlistListDoubleClickTime = 2000; // Adjust the delay as needed
                                                              // Flags to track dragging state for playlist list
        private bool isPlaylistMouseDown = false;
        // Point to store the starting position of a drag for playlist list
        private Point playlistDragStartPoint;
        // Index of the first selected row in the playlist list
        private int firstSelectedPlaylistRowIndex = -1;
        // Index to track the insertion line position for playlist list
        private int playlistInsertionLineIndex = -1;
        // Field to track the index of the row with the colored border
        private int highlightedPlaylistRowIndex = -1;

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
                using (Pen pen = new Pen(CustomColor.ActiveButtonColor, 2)) // Change to your desired color
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

                using (Pen pen = new Pen(CustomColor.ActiveButtonColor, 2))
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

        #endregion

        #region SELECTOR TRACKLIST DRAG AND DROP

        private const string SelectorTrackListDataFormat = "SelectorTrackListData";
        private System.Windows.Forms.Timer selectorClickTimer;
        private const int selectorDoubleClickTime = 2000;

        private bool selectorIsDragging = false;
        private bool selectorIsMouseDown = false;
        private Point selectorDragStartPoint;

        private int selectorFirstSelectedRowIndex = -1;
        private bool selectorCoverBrowserUpdateEnabled = true;
        private bool selectorDragAndDropInProgress = false;
        private System.Windows.Forms.Timer selectorAutoScrollTimer = new System.Windows.Forms.Timer();
        private const int SelectorFastScrollAreaHeight = 10;
        private const int SelectorSlowScrollAreaHeight = 20;
        private const int SelectorFastScrollSpeed = 3;
        private const int SelectorSlowScrollSpeed = 1;

        private int selectorInsertionLineIndex = -1;
        private int selectorLastMouseDownRowIndex = -1;

        public void ToggleSelectorTracklistSelection(bool enabled)
        {
            isInitializing = !enabled;
        }
        private void dgvSelectorTrackList_SelectionChanged(object sender, EventArgs e)
        {
            if (isInitializing || !selectorCoverBrowserUpdateEnabled || selectorDragAndDropInProgress)
                return;

            if (dgvSelectorTrackList.SelectedRows.Count > 0)
            {
                int selectedIndex = dgvSelectorTrackList.SelectedRows[dgvSelectorTrackList.SelectedRows.Count - 1].Index;
                selectorFirstSelectedRowIndex = selectedIndex;
                this.CallSetCurrentTrackEvent(selectedIndex);
            }

            if (dgvSelectorTrackList.SelectedRows.Count > 0)
            {
                this.lblSelectedItemsCount.Text = $"{dgvSelectorTrackList.SelectedRows.Count} item{(dgvSelectorTrackList.SelectedRows.Count > 1 ? "s" : "")} selected";

                int totalSeconds = 0;
                foreach (DataGridViewRow row in dgvSelectorTrackList.SelectedRows)
                {
                    string[] parts = row.Cells["Length"].Value.ToString().Split(':');
                    if (parts.Length == 3) totalSeconds += int.Parse(parts[0]) * 3600 + int.Parse(parts[1]) * 60 + int.Parse(parts[2]);
                    else if (parts.Length == 2) totalSeconds += int.Parse(parts[0]) * 60 + int.Parse(parts[1]);
                    else if (parts.Length == 1) totalSeconds += int.Parse(parts[0]);
                }

                TimeSpan totalTime = TimeSpan.FromSeconds(totalSeconds);
                string length = totalTime.Days > 0 ? $"{totalTime.Days:D2}.{totalTime.Hours:D2}:{totalTime.Minutes:D2}:{totalTime.Seconds:D2}" :
                                totalTime.Hours > 0 ? $"{totalTime.Hours:D2}:{totalTime.Minutes:D2}:{totalTime.Seconds:D2}" :
                                totalTime.Minutes > 0 ? $"{totalTime.Minutes:D2}:{totalTime.Seconds:D2}" :
                                $"00:{totalTime.Seconds:D2}";

                this.lblSelectedItemsLength.Text = $"Length: {length}";
            }
        }
        private void dgvSelectorTrackList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!selectorIsDragging)
            {
                var hitTestInfo = dgvSelectorTrackList.HitTest(e.X, e.Y);
                if (hitTestInfo.RowIndex >= 0)
                {
                    this.PlayTrackEvent?.Invoke(this, new Messenger() { });
                }
            }
        }
        private void dgvSelectorTrackList_MouseDown(object sender, MouseEventArgs e)
        {
            var hitTestInfo = dgvSelectorTrackList.HitTest(e.X, e.Y);
            if (hitTestInfo.RowIndex >= 0 && e.Button == MouseButtons.Left)
            {
                selectorLastMouseDownRowIndex = hitTestInfo.RowIndex;
                selectorCoverBrowserUpdateEnabled = false;

                bool isRowSelected = dgvSelectorTrackList.SelectedRows.Cast<DataGridViewRow>().Any(row => row.Index == hitTestInfo.RowIndex);

                if (ModifierKeys.HasFlag(Keys.Shift) || ModifierKeys.HasFlag(Keys.Control))
                    return;

                if (isRowSelected)
                {
                    selectorDragAndDropInProgress = true;
                    if (dgvSelectorTrackList.SelectedRows.Count > 1)
                    {
                        StartSelectorDragAndDrop(e.Location);
                    }
                    else
                    {
                        PrepareSelectorForDrag(e.Location, hitTestInfo.RowIndex);
                    }
                }
                else
                {
                    dgvSelectorTrackList.ClearSelection();
                    dgvSelectorTrackList.Rows[hitTestInfo.RowIndex].Selected = true;
                }
            }
        }
        private void StartSelectorDragAndDrop(Point location)
        {
            selectorIsMouseDown = true;
            selectorIsDragging = true;
            selectorDragStartPoint = location;
            dgvSelectorTrackList.DoDragDrop(new DataObject(SelectorTrackListDataFormat, dgvSelectorTrackList.SelectedRows.Cast<DataGridViewRow>().ToArray()), DragDropEffects.Move | DragDropEffects.Copy);
        }
        private void PrepareSelectorForDrag(Point location, int rowIndex)
        {
            selectorIsMouseDown = true;
            selectorIsDragging = false;
            selectorDragStartPoint = location;
            selectorFirstSelectedRowIndex = rowIndex;
            selectorClickTimer.Start();
        }
        private void dgvSelectorTrackList_MouseMove(object sender, MouseEventArgs e)
        {
            if (selectorIsMouseDown && !selectorIsDragging)
            {
                if (Math.Abs(e.X - selectorDragStartPoint.X) > SystemInformation.DragSize.Width ||
                    Math.Abs(e.Y - selectorDragStartPoint.Y) > SystemInformation.DragSize.Height)
                {
                    selectorIsDragging = true;
                    selectorClickTimer.Stop();
                    dgvSelectorTrackList.DoDragDrop(new DataObject(SelectorTrackListDataFormat, dgvSelectorTrackList.SelectedRows.Cast<DataGridViewRow>().ToArray()), DragDropEffects.Move | DragDropEffects.Copy);
                }
            }
        }
        private void dgvSelectorTrackList_MouseUp(object sender, MouseEventArgs e)
        {
            selectorIsMouseDown = false;
            if (!selectorIsDragging)
            {
                selectorClickTimer.Stop();
            }
            selectorIsDragging = false;
            selectorCoverBrowserUpdateEnabled = true;
            selectorDragAndDropInProgress = false;
            selectorInsertionLineIndex = -1;
            dgvSelectorTrackList.Invalidate();
        }
        private void SelectorClickTimer_Tick(object sender, EventArgs e)
        {
            selectorClickTimer.Stop();
            if (!selectorIsDragging)
            {
                var hitTestInfo = dgvSelectorTrackList.HitTest(selectorDragStartPoint.X, selectorDragStartPoint.Y);
                if (hitTestInfo.RowIndex >= 0)
                {
                    this.PlayTrackEvent?.Invoke(this, new Messenger() { });
                }
            }
        }
        private void dgvSelectorTrackList_DragOver(object sender, DragEventArgs e)
        {
            if (isFilterEnabled)
            {
                e.Effect = DragDropEffects.None;
                selectorInsertionLineIndex = -1;
            }
            else
            {
                Point clientPoint = dgvSelectorTrackList.PointToClient(new Point(e.X, e.Y));
                int rowIndex = dgvSelectorTrackList.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

                if (e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    e.Effect = DragDropEffects.Copy;
                    if (rowIndex < 0)
                    {
                        rowIndex = dgvSelectorTrackList.Rows.Count;
                    }
                    else
                    {
                        Rectangle rowBounds = dgvSelectorTrackList.GetRowDisplayRectangle(rowIndex, false);
                        int cursorY = clientPoint.Y - rowBounds.Top;

                        DrawSelectorInsertionLine(cursorY < rowBounds.Height / 2 ? rowIndex : rowIndex + 1);
                    }

                    if (rowIndex == dgvSelectorTrackList.Rows.Count - 1)
                    {
                        Rectangle lastRowBounds = dgvSelectorTrackList.GetRowDisplayRectangle(rowIndex, false);
                        if (clientPoint.Y > lastRowBounds.Bottom - (lastRowBounds.Height / 2))
                        {
                            DrawSelectorInsertionLine(dgvSelectorTrackList.Rows.Count);
                            e.Effect = DragDropEffects.Copy;
                        }
                    }
                    else if (rowIndex == dgvSelectorTrackList.Rows.Count)
                    {
                        DrawSelectorInsertionLine(rowIndex);
                        e.Effect = DragDropEffects.Copy;
                    }
                }
                else if (e.Data.GetDataPresent(SelectorTrackListDataFormat))
                {
                    if (rowIndex < 0 && rowIndex != -1)
                    {
                        e.Effect = DragDropEffects.None;
                        selectorInsertionLineIndex = -1;
                    }
                    else
                    {
                        if (rowIndex >= 0)
                        {
                            Rectangle rowBounds = dgvSelectorTrackList.GetRowDisplayRectangle(rowIndex, false);
                            int cursorY = clientPoint.Y - rowBounds.Top;

                            DrawSelectorInsertionLine(cursorY < rowBounds.Height / 2 ? rowIndex : rowIndex + 1);
                        }
                        else
                        {
                            DrawSelectorInsertionLine(dgvSelectorTrackList.Rows.Count);
                        }

                        e.Effect = DragDropEffects.Move;
                    }

                    if (rowIndex == dgvSelectorTrackList.Rows.Count - 1)
                    {
                        Rectangle lastRowBounds = dgvSelectorTrackList.GetRowDisplayRectangle(rowIndex, false);
                        if (clientPoint.Y > lastRowBounds.Bottom - (lastRowBounds.Height / 2))
                        {
                            DrawSelectorInsertionLine(dgvSelectorTrackList.Rows.Count);
                            e.Effect = DragDropEffects.Move;
                        }
                    }
                    else if (rowIndex == dgvSelectorTrackList.Rows.Count || rowIndex == -1)
                    {
                        DrawSelectorInsertionLine(dgvSelectorTrackList.Rows.Count);
                        e.Effect = DragDropEffects.Move;
                    }
                }
                else
                {
                    e.Effect = DragDropEffects.None;
                    selectorInsertionLineIndex = -1;
                }

                dgvSelectorTrackList.Invalidate();
            }
        }
        private void DrawSelectorInsertionLine(int rowIndex)
        {
            if (rowIndex >= 0 && rowIndex <= dgvSelectorTrackList.Rows.Count)
            {
                selectorInsertionLineIndex = rowIndex;
                dgvSelectorTrackList.Invalidate();
            }
        }
        private void dgvSelectorTrackList_DragDrop(object sender, DragEventArgs e)
        {
            if (isFilterEnabled) return;

            selectorAutoScrollTimer.Stop();

            Point clientPoint = dgvSelectorTrackList.PointToClient(new Point(e.X, e.Y));
            var hitTestInfo = dgvSelectorTrackList.HitTest(clientPoint.X, clientPoint.Y);
            int targetIndex = hitTestInfo.RowIndex >= 0 ? hitTestInfo.RowIndex : dgvSelectorTrackList.Rows.Count;

            if (targetIndex < 0 || targetIndex > dgvSelectorTrackList.Rows.Count)
            {
                targetIndex = dgvSelectorTrackList.Rows.Count;
            }

            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                this.ExternalDragAndDropIntoTracklistEvent?.Invoke(this, new Messenger()
                {
                    DragAndDropFilePathArray = files,
                    IntegerField1 = targetIndex
                });

                dgvSelectorTrackList.ClearSelection();
                foreach (string file in files)
                {
                    int addedRowIndex = FindSelectorRowIndexByFile(file);
                    if (addedRowIndex >= 0)
                    {
                        dgvSelectorTrackList.Rows[addedRowIndex].Selected = true;
                    }
                }
            }
            else if (dgvSelectorTrackList.SelectedRows.Count > 0)
            {
                List<int> selectedIndices = dgvSelectorTrackList.SelectedRows.Cast<DataGridViewRow>().OrderBy(r => r.Index).Select(r => r.Index).ToList();

                this.MoveTracklistRowsEvent?.Invoke(this, new Messenger()
                {
                    SelectedIndices = selectedIndices,
                    IntegerField1 = targetIndex
                });

                dgvSelectorTrackList.BeginInvoke(new Action(() =>
                {
                    dgvSelectorTrackList.ClearSelection();
                    int newIndex = targetIndex;

                    if (targetIndex > selectedIndices.Min())
                    {
                        newIndex -= selectedIndices.Count;
                    }

                    foreach (int index in selectedIndices)
                    {
                        if (newIndex < dgvSelectorTrackList.Rows.Count)
                        {
                            dgvSelectorTrackList.Rows[newIndex].Selected = true;
                            newIndex++;
                        }
                    }
                }));
            }

            selectorIsDragging = false;
            selectorIsMouseDown = false;
            selectorInsertionLineIndex = -1;
            dgvSelectorTrackList.Invalidate();
        }
        private int FindSelectorRowIndexByFile(string filePath)
        {
            foreach (DataGridViewRow row in dgvSelectorTrackList.Rows)
            {
                if (row.Cells["Path"].Value.ToString() == filePath)
                {
                    return row.Index;
                }
            }
            return -1;
        }
        private void dgvSelectorTrackList_Paint(object sender, PaintEventArgs e)
        {
            if (selectorInsertionLineIndex >= 0 && selectorInsertionLineIndex < dgvSelectorTrackList.Rows.Count)
            {
                int y = 0;
                if (dgvSelectorTrackList.Rows.Count == 0)
                {
                    y = dgvSelectorTrackList.ClientRectangle.Top;
                }
                else if (selectorInsertionLineIndex < dgvSelectorTrackList.Rows.Count)
                {
                    Rectangle rowRect = dgvSelectorTrackList.GetRowDisplayRectangle(selectorInsertionLineIndex, true);
                    y = rowRect.Top;
                }
                else
                {
                    Rectangle rowRect = dgvSelectorTrackList.GetRowDisplayRectangle(selectorInsertionLineIndex - 1, true);
                    y = rowRect.Bottom;
                }

                using (Pen pen = new Pen(CustomColor.ActiveButtonColor, 2))
                {
                    e.Graphics.DrawLine(pen, new Point(0, y), new Point(dgvSelectorTrackList.Width, y));
                }
            }
        }
        private void dgvSelectorTrackList_DragEnter(object sender, DragEventArgs e)
        {
            if (!isFilterEnabled)
            {
                selectorAutoScrollTimer.Start();
                selectorCoverBrowserUpdateEnabled = false;

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
        private void dgvSelectorTrackList_DragLeave(object sender, EventArgs e)
        {

        }
        private void StartSelectorFileDrop()
        {
            if (dgvSelectorTrackList.SelectedRows.Count == 1)
            {
                string trackPath = dgvSelectorTrackList.SelectedRows[0].Cells["Path"].Value.ToString();
                DataObject dataObject = new DataObject(DataFormats.FileDrop, new string[] { trackPath });
                DoDragDrop(dataObject, DragDropEffects.Copy);
            }
        }
        private void SelectorAutoScrollTimer_Tick(object sender, EventArgs e)
        {
            Point clientPoint = dgvSelectorTrackList.PointToClient(Cursor.Position);
            int fastScrollArea = dgvSelectorTrackList.Height * SelectorFastScrollAreaHeight / 100;
            int slowScrollArea = dgvSelectorTrackList.Height * SelectorSlowScrollAreaHeight / 100;

            if (clientPoint.Y < fastScrollArea)
            {
                if (dgvSelectorTrackList.FirstDisplayedScrollingRowIndex > 0)
                {
                    dgvSelectorTrackList.FirstDisplayedScrollingRowIndex = Math.Max(0, dgvSelectorTrackList.FirstDisplayedScrollingRowIndex - SelectorFastScrollSpeed);
                }
            }
            else if (clientPoint.Y < slowScrollArea)
            {
                if (dgvSelectorTrackList.FirstDisplayedScrollingRowIndex > 0)
                {
                    dgvSelectorTrackList.FirstDisplayedScrollingRowIndex = Math.Max(0, dgvSelectorTrackList.FirstDisplayedScrollingRowIndex - SelectorSlowScrollSpeed);
                }
            }
            else if (clientPoint.Y > dgvSelectorTrackList.Height - fastScrollArea)
            {
                if (dgvSelectorTrackList.FirstDisplayedScrollingRowIndex < dgvSelectorTrackList.RowCount - 1)
                {
                    dgvSelectorTrackList.FirstDisplayedScrollingRowIndex = Math.Min(dgvSelectorTrackList.RowCount - 1, dgvSelectorTrackList.FirstDisplayedScrollingRowIndex + SelectorFastScrollSpeed);
                }
            }
            else if (clientPoint.Y > dgvSelectorTrackList.Height - slowScrollArea)
            {
                if (dgvSelectorTrackList.FirstDisplayedScrollingRowIndex < dgvSelectorTrackList.RowCount - 1)
                {
                    dgvSelectorTrackList.FirstDisplayedScrollingRowIndex = Math.Min(dgvSelectorTrackList.RowCount - 1, dgvSelectorTrackList.FirstDisplayedScrollingRowIndex + SelectorSlowScrollSpeed);
                }
            }
        }

        #endregion

        private void PlaylistView_Shown(object sender, EventArgs e)
        {
            this.ToggleTracklistSelection(true);
        }
        private void SelectorPlaylistView_Shown(object sender, EventArgs e)
        {
            this.ToggleSelectorTracklistSelection(true);
        }

 
    }
}
