using FlacLibSharp.Exceptions;
using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Models;
using NAudio.SoundFont;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

using static System.Windows.Forms.DataGridView;

namespace MitoPlayer_2024.Views
{
    public partial class PlaylistView : Form, IPlaylistView
    {
        private Form parentView { get; set; }

        //DATATABLES
        private BindingSource playlistListBindingSource { get; set; }
        private BindingSource trackListBindingSource { get; set; }

        private Tag currentTagForColors { get; set; }
        private Dictionary<String, Color> currentTagValueColorDic = new Dictionary<String, Color>();

        //PLAYER
        public event EventHandler<ListEventArgs> SetCurrentTrackEvent;
        public event EventHandler<ListEventArgs> PlayTrackEvent;
        public event EventHandler PauseTrackEvent;
        public event EventHandler StopTrackEvent;
        public event EventHandler<ListEventArgs> PrevTrackEvent;
        public event EventHandler<ListEventArgs> NextTrackEvent;
        public event EventHandler RandomTrackEvent;
        public event EventHandler<ListEventArgs> ChangeVolumeEvent;
        public event EventHandler<ListEventArgs> ChangeProgressEvent;
        public event EventHandler GetMediaPlayerProgressStatusEvent;

        //TRACKLIST
        public event EventHandler<ListEventArgs> OrderByColumnEvent;
        public event EventHandler<ListEventArgs> DeleteTracksEvent;
        public event EventHandler<ListEventArgs> InternalDragAndDropIntoTracklistEvent;
        public event EventHandler<ListEventArgs> InternalDragAndDropIntoPlaylistEvent;
        public event EventHandler<ListEventArgs> ExternalDragAndDropIntoTracklistEvent;
        public event EventHandler<ListEventArgs> ExternalDragAndDropIntoPlaylistEvent;
        //public event EventHandler<ListEventArgs> ChangeTracklistColorEvent;
        public event EventHandler ShowColumnVisibilityEditorEvent;
        public event EventHandler ScanKeyAndBpmEvent;

        //PLAYLIST
        public event EventHandler<ListEventArgs> CreatePlaylist;
        public event EventHandler<ListEventArgs> EditPlaylist;
        public event EventHandler<ListEventArgs> LoadPlaylistEvent;
        public event EventHandler<ListEventArgs> MovePlaylistEvent;
        public event EventHandler<ListEventArgs> DeletePlaylistEvent;
        public event EventHandler<ListEventArgs> SetQuickListEvent;
        public event EventHandler<ListEventArgs> ExportToM3UEvent;
        public event EventHandler<ListEventArgs> ExportToTXTEvent;
        public event EventHandler<ListEventArgs> ExportToDirectoryEvent;
        public event EventHandler DisplayPlaylistListEvent;

        //TAG EDITOR
        public event EventHandler DisplayTagEditorEvent;
        public event EventHandler<ListEventArgs> SelectTagEvent;
        public event EventHandler<ListEventArgs> SetTagValueEvent;
        public event EventHandler<ListEventArgs> ClearTagValueEvent;

        public event EventHandler<ListEventArgs> ChangeFilterModeEnabled;
        public event EventHandler<ListEventArgs> ChangeOnlyPlayingRowModeEnabled;
        public event EventHandler<ListEventArgs> ChangeFilterText;
        public event EventHandler RemoveTagValueFilter;

        public event EventHandler SaveTrackListEvent;

        private int trackListLeftOffset = 190;
        private int trackListRightOffset = 285;
        private int tagValueEditorPanelBottomOffset = 85;

        public PlaylistView()
        {
            this.InitializeComponent();
            this.SetControlColors();

            this.playlistListBindingSource = new BindingSource();
            this.trackListBindingSource = new BindingSource();
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
        private void SetControlColors()
        {
            this.BackColor = this.BackgroundColor;
            this.ForeColor = this.FontColor;

            this.groupBoxPlaylist.ForeColor = this.FontColor;
            this.groupBox4.ForeColor = this.FontColor;
            this.groupBox3.ForeColor = this.FontColor;

            this.btnNewPlaylist.BackColor = this.ButtonColor;
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
            this.btnDeletePlaylist.FlatAppearance.BorderColor = this.ButtonBorderColor;

            this.btnDisplayPlaylistList.BackColor = this.ButtonColor;
            this.btnDisplayPlaylistList.ForeColor = this.FontColor;
            this.btnDisplayPlaylistList.FlatAppearance.BorderColor = this.ButtonBorderColor;
            this.btnHidePlaylistList.BackColor = this.ButtonColor;
            this.btnHidePlaylistList.ForeColor = this.FontColor;
            this.btnHidePlaylistList.FlatAppearance.BorderColor = this.ButtonBorderColor;
            this.btnDisplayTagEditor.BackColor = this.ButtonColor;
            this.btnDisplayTagEditor.ForeColor = this.FontColor;
            this.btnDisplayTagEditor.FlatAppearance.BorderColor = this.ButtonBorderColor;
            this.btnHideTagEditor.BackColor = this.ButtonColor;
            this.btnHideTagEditor.ForeColor = this.FontColor;
            this.btnHideTagEditor.FlatAppearance.BorderColor = this.ButtonBorderColor;

            this.btnColumnVisibilityWithTagEditor.BackColor = this.ButtonColor;
            this.btnColumnVisibilityWithTagEditor.ForeColor = this.FontColor;
            this.btnColumnVisibilityWithTagEditor.FlatAppearance.BorderColor = this.ButtonBorderColor;
            this.btnColumnVisibility.BackColor = this.ButtonColor;
            this.btnColumnVisibility.ForeColor = this.FontColor;
            this.btnColumnVisibility.FlatAppearance.BorderColor = this.ButtonBorderColor;

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

        //PLAYLIST DATA BINDING
        private bool[] PlaylistColumnVisibility { get; set; }
        private int CurrentPlaylistId { get; set; }
        public void InitializePlaylistListBindingSource(BindingSource playlistList, bool[] columnVisibility, int currentPlaylistId)
        {
            this.PlaylistColumnVisibility = columnVisibility;
            this.playlistListBindingSource.DataSource = playlistList;
            this.CurrentPlaylistId = currentPlaylistId;
            this.dgvPlaylistList.DataSource = this.playlistListBindingSource.DataSource;
        }

        //playlist lista oszlopainak láthatóságának beállítása és rendezhetőségének letiltása
        //a group oszlop átméretezhetőségének letiltása és méretének összébb vétele
        //a cellák bordereinek kikapcsolása
        private void dgvPlaylistList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            for (int i = 0; i <= this.dgvPlaylistList.Columns.Count - 1; i++)
            {
                this.dgvPlaylistList.Columns[i].Visible = this.PlaylistColumnVisibility[i];
                this.dgvPlaylistList.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            this.dgvPlaylistList.Columns["G"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            this.dgvPlaylistList.Columns["G"].Width = 20;
            this.dgvPlaylistList.CellBorderStyle = DataGridViewCellBorderStyle.None;
            this.SetPlaylistListColors();
        }
        public void ReloadPlaylistListBindingSource(bool[] columnVisibility, int currentPlaylistId)
        {
            this.PlaylistColumnVisibility = columnVisibility;
            this.CurrentPlaylistId = currentPlaylistId;
            this.SetPlaylistListColors();
        }

        //ha van eleme a playlist listának, végigmegyünk az elemeken, megkeressük az aktuális playlist-et
        //kiszinezzük a sorokat párossával, az aktuális listát más színnel
        public void SetPlaylistListColors()
        {
            int playlistId = -1;
            if (this.dgvPlaylistList != null && this.dgvPlaylistList.Rows != null && this.dgvPlaylistList.Rows.Count > 0)
            {
                for (int i = 0; i < this.dgvPlaylistList.Rows.Count; i++)
                {
                    playlistId = (int)this.dgvPlaylistList.Rows[i].Cells["Id"].Value;
                    if (this.CurrentPlaylistId != -1 && playlistId == this.CurrentPlaylistId)
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
        //TRACKLIST DATA BINDING
        private bool[] TracklistColumnVisibility { get; set; }
        private int[] TracklistDisplayIndex { get; set; }
        private List<Tag> tagList { get; set; }
        private List<List<TagValue>> tagValueListList { get; set; }
        public void SetTagsAndTagValues(List<Tag> tagList, List<List<TagValue>> tagValueListList)
        {
            this.tagList = tagList;
            this.tagValueListList = tagValueListList;
        }
        private bool isInitializeComplete { get; set; }

        /// az inicializálás során beállítjuk az oszlopok láthatóságát és sorrendjét
        /// az inicializálást követi a datasource átadása
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
               
            if (model.ColumnDisplayIndexArray != null && model.ColumnDisplayIndexArray.Length > 0)
            {
                for (int i = 0; i <= this.dgvTrackList.Columns.Count - 1; i++)
                {
                    this.dgvTrackList.Columns[i].DisplayIndex = model.ColumnDisplayIndexArray[i];
                }
            }

            this.CurrentTrackIdInPlaylist = model.CurrentTrackIdInPlaylist;

            this.trackListBindingSource.ResetBindings(false);

            this.UpdateTracklistColor(this.CurrentTrackIdInPlaylist);

        }
        private int CurrentTrackIdInPlaylist { get; set; }
        private void dgvTrackList_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            
        }

        public void InitializeTrackListRows(DataTableModel model)
        {
            if (model.BindingSource != null)
                this.trackListBindingSource.DataSource = model.BindingSource;

            this.dgvTrackList.DataSource = this.trackListBindingSource.DataSource;

            if (model.ColumnVisibilityArray != null && model.ColumnVisibilityArray.Length > 0)
            {
                for (int i = 0; i <= this.dgvTrackList.Columns.Count - 1; i++)
                {
                    this.dgvTrackList.Columns[i].Visible = model.ColumnVisibilityArray[i];
                }
            }

            if (model.ColumnDisplayIndexArray != null && model.ColumnDisplayIndexArray.Length > 0)
            {
                for (int i = 0; i <= this.dgvTrackList.Columns.Count - 1; i++)
                {
                    this.dgvTrackList.Columns[i].DisplayIndex = model.ColumnDisplayIndexArray[i];
                }
            }
        }

        /// ennek mindig csak egyszer, a felület inicializálásakor kellene lefutnia, utána sose
        private void dgvTrackList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            ;
            /* if (!this.isInitializeComplete)
             {
                 this.isInitializeComplete = true;

             }*/
            this.UpdateTracklistColor();
        }

        

        /// a reload-nál 
        public void ReloadTrackListBindingSource(DataTableModel model)
        {
            // this.TracklistColumnVisibility = columnVisibility;
            // this.TracklistDisplayIndex = displayIndex;

          /*  if (model.BindingSource != null)
                this.trackListBindingSource.DataSource = model.BindingSource;

            this.dgvTrackList.DataSource = this.trackListBindingSource.DataSource;*/

            this.UpdateTracklistColor(model.CurrentTrackIdInPlaylist);
        }
        public void FilterTracklist(BindingSource trackList, bool[] columnVisibility, int[] displayIndex, int currentTrackIdInPlaylist)
        {
            this.TracklistColumnVisibility = columnVisibility;
            this.TracklistDisplayIndex = displayIndex;
            this.trackListBindingSource.DataSource = trackList;
            this.dgvTrackList.DataSource = this.trackListBindingSource.DataSource;

            this.UpdateTracklistColor(currentTrackIdInPlaylist);
        }


        //ha van eleme a tracklist-ben, végigmegyünk az elemeken, megkeressük az aktuális track-et
        //kiszinezzük a sorokat párossával, az aktuális számot más színnel
        //ha a szám hiányzik, pirossal
        public void UpdateTracklistColor(int currentTrackIdInPlaylist = -1)
        {
            bool isMissing = false;
            int trackIdInPlaylist = -1;
            if (this.dgvTrackList != null && this.dgvTrackList.Rows != null && this.dgvTrackList.Rows.Count > 0)
            {
                for (int i = 0; i < this.dgvTrackList.Rows.Count; i++)
                {
                    trackIdInPlaylist = (int)this.dgvTrackList.Rows[i].Cells["TrackIdInPlaylist"].Value;
                    isMissing = (bool)this.dgvTrackList.Rows[i].Cells["IsMissing"].Value;
                    if (isMissing)
                    {
                        this.dgvTrackList.Rows[i].DefaultCellStyle.ForeColor = Color.Salmon;
                    }
                    else
                    {
                        if (currentTrackIdInPlaylist != -1 && trackIdInPlaylist == currentTrackIdInPlaylist)
                        {
                            this.dgvTrackList.Rows[i].DefaultCellStyle.BackColor = this.GridPlayingColor;
                        }
                        else
                        {
                            if (i == 0 || i % 2 == 0)
                            {
                                this.dgvTrackList.Rows[i].DefaultCellStyle.BackColor = this.GridLineColor1;
                            }
                            else
                            {
                                this.dgvTrackList.Rows[i].DefaultCellStyle.BackColor = this.GridLineColor2;
                            }
                        }
                    }
                }

                if (this.tagList != null && this.tagList.Count > 0 &&
                    this.tagValueListList != null && this.tagValueListList.Count > 0)
                {
                    for (int j = 0; j < this.dgvTrackList.Columns.Count; j++)
                    {
                        Tag tag = this.tagList.Find(x => x.Name == this.dgvTrackList.Columns[j].Name.ToString());
                        if(tag != null)
                        {
                            int tagIndex = this.tagList.IndexOf(tag);
                            List<TagValue> tagValueList = this.tagValueListList[tagIndex];
                            TagValue tagValue = null;
                            if (tagValueList != null && tagValueList.Count > 0)
                            {
                                for (int i = 0; i < this.dgvTrackList.Rows.Count; i++)
                                {
                                    tagValue = tagValueList.Find(x => x.Name == this.dgvTrackList.Rows[i].Cells[j].Value.ToString());
                                    if (tagValue != null)
                                    {

                                        if (tag.TextColoring)
                                        {
                                            this.dgvTrackList.Rows[i].Cells[j].Style.ForeColor = tagValue.Color;
                                        }
                                        else
                                        {
                                            this.dgvTrackList.Rows[i].Cells[j].Style.BackColor = tagValue.Color;
                                            if ((tagValue.Color.R < 100 && tagValue.Color.G < 100) || (tagValue.Color.R < 100 && tagValue.Color.B < 100) || (tagValue.Color.B < 100 && tagValue.Color.G < 100))
                                            {
                                                this.dgvTrackList.Rows[i].Cells[j].Style.ForeColor = Color.White;
                                            }
                                            else
                                            {
                                                this.dgvTrackList.Rows[i].Cells[j].Style.ForeColor = Color.Black;
                                            }
                                        }
                                    }
                                }
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
        private void dgvTrackList_SelectionChanged(object sender, EventArgs e)
        {

            if (this.dgvTrackList.SelectedRows != null && this.dgvTrackList.SelectedRows.Count > 0)
            {
                ///ROW COUNT
                if (this.dgvTrackList.SelectedRows.Count == 1)
                    this.lblSelectedItemsCount.Text = this.dgvTrackList.SelectedRows.Count.ToString() + " item selected";
                else
                    this.lblSelectedItemsCount.Text = this.dgvTrackList.SelectedRows.Count.ToString() + " items selected";

                String[] parts = null;
                int seconds = 0;

                for (int i = 0; i < this.dgvTrackList.SelectedRows.Count; i++)
                {
                    parts = this.dgvTrackList.SelectedRows[i].Cells["Length"].Value.ToString().Split(':');

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

                ///SUM OF TIME
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
                this.lblSelectedItemsLength.Text = "Length: " + length.ToString();
            }
        }
        #endregion

        #region TRACKLIST - ORDER BY COLUMN HEADER
        private void dgvTrackList_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.OrderByColumnEvent?.Invoke(this, new ListEventArgs() { StringField1 = dgvTrackList.Columns[e.ColumnIndex].Name });
        }
        #endregion

        #region TRACKLIST - CONTROL BUTTONS
        private void dgvTrackList_KeyDown(object sender, KeyEventArgs e)
        {
            //DELETE - Delete Track(s)
            if (this.dgvTrackList.Rows.Count > 0 && e.KeyCode == Keys.Delete)
            {
                this.DeleteTracksEvent?.Invoke(this, new ListEventArgs() { Rows = this.dgvTrackList.Rows });
            }

            if (this.dgvTrackList.Rows.Count > 0 && (e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.ShiftKey))
            {
                this.controlKey = true;
            }
            
            //ENTER or SPACE - Play Track
            if (this.dgvTrackList.SelectedRows.Count > 0 && e.KeyCode == Keys.Space ||
                this.dgvTrackList.SelectedRows.Count > 0 && e.KeyCode == Keys.Enter)
            {
                int rowIndex = this.dgvTrackList.Rows.IndexOf(this.dgvTrackList.SelectedRows[0]);

                this.CallSetCurrentTrackEvent(rowIndex);
                this.CallStopTrackEvent();
                this.CallPlayTrackEvent();
            }

            if (this.controlKey && e.KeyCode == Keys.S)
            {
                this.SaveTrackListEvent?.Invoke(this, new EventArgs());
            }

            /* if (rdbPlaylist.Checked)
             {
                 //(1),(2),(3) or (4) - Set the group of the selected playlist 
                 if (this.dgvPlaylistList.SelectedRows.Count > 0 && e.KeyCode == Keys.D1)
                 {
                     this.InternalDragAndDropIntoPlaylistEvent?.Invoke(this, new ListEventArgs() { SelectedRows = this.dgvTrackList.SelectedRows, IntegerField1 = this.GetPlaylistIndex(1) });
                 }
                 if (this.dgvPlaylistList.SelectedRows.Count > 0 && e.KeyCode == Keys.D2)
                 {
                     this.InternalDragAndDropIntoPlaylistEvent?.Invoke(this, new ListEventArgs() { SelectedRows = this.dgvTrackList.SelectedRows, IntegerField1 = this.GetPlaylistIndex(2) });
                 }
                 if (this.dgvPlaylistList.SelectedRows.Count > 0 && e.KeyCode == Keys.D3)
                 {
                     this.InternalDragAndDropIntoPlaylistEvent?.Invoke(this, new ListEventArgs() { SelectedRows = this.dgvTrackList.SelectedRows, IntegerField1 = this.GetPlaylistIndex(2) });
                 }
                 if (this.dgvPlaylistList.SelectedRows.Count > 0 && e.KeyCode == Keys.D4)
                 {
                     this.InternalDragAndDropIntoPlaylistEvent?.Invoke(this, new ListEventArgs() { SelectedRows = this.dgvTrackList.SelectedRows, IntegerField1 = this.GetPlaylistIndex(3) });
                 }
             }*/
            /* else if (rdbTagValue.Checked){
                 if (this.dgvPlaylistList.SelectedRows.Count > 0)
                 {
                     if (this.dgvPlaylistList.SelectedRows.Count > 0 && e.KeyCode == Keys.D1)
                     {
                         if(tgvHotkeyIndex1 > -1)
                             this.SetTagValueEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = tgvHotkeyIndex1, Rows = this.dgvTrackList.Rows });
                     }
                     if (this.dgvPlaylistList.SelectedRows.Count > 0 && e.KeyCode == Keys.D2)
                     {
                         if (tgvHotkeyIndex2 > -1)
                             this.SetTagValueEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = tgvHotkeyIndex2, Rows = this.dgvTrackList.Rows });
                     }
                     if (this.dgvPlaylistList.SelectedRows.Count > 0 && e.KeyCode == Keys.D3)
                     {
                         if (tgvHotkeyIndex3 > -1)
                             this.SetTagValueEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = tgvHotkeyIndex3, Rows = this.dgvTrackList.Rows });
                     }
                     if (this.dgvPlaylistList.SelectedRows.Count > 0 && e.KeyCode == Keys.D4)
                     {
                         if (tgvHotkeyIndex4 > -1)
                             this.SetTagValueEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = tgvHotkeyIndex4, Rows = this.dgvTrackList.Rows });
                     }
                 }

             }*/


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
        private int GetPlaylistIndex(int hotKeyNumber)
        {
            int result = -1;
            for(int i = 0; i <= dgvPlaylistList.Rows.Count - 1; i++)
            {
                if (Convert.ToInt32(dgvPlaylistList.Rows[i].Cells["G"].Value) == hotKeyNumber)
                {
                    result = i;
                    break;
                }
            }
            return result;
        }
        private void dgvTrackList_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.ShiftKey)
            {
                controlKey = false;
            }   
        }
        private void btnScanBpm_Click(object sender, EventArgs e)
        {
            this.ScanKeyAndBpmEvent?.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region TRACKLIST - DRAG AND DROP

        private int currentRowIndex = -1;
        private bool dragMultipleRow = false;
        private bool dragOneRow = false;
        private bool prepareToDragOneRow = false;
        private void dgvTrackList_MouseDown(object sender, MouseEventArgs e) 
        {
            HitTestInfo hitTest = this.dgvTrackList.HitTest(e.X, e.Y);
            if (hitTest != null && hitTest.RowIndex > -1)
            {
                this.currentRowIndex = hitTest.RowIndex;

                if (this.dgvTrackList.Rows[hitTest.RowIndex].Selected)
                {
                    if (!this.controlKey)
                    {
                        if (this.dgvTrackList.Rows.Count != this.dgvTrackList.SelectedRows.Count)
                        {
                            
                            if (this.dgvTrackList.SelectedRows.Count > 1)
                            {
                                this.dragMultipleRow = true;
                                this.Drag();
                            }
                            else
                            {
                                this.prepareToDragOneRow = true;
                            } 
                        }
                    }
                }
            }
        }
        List<String> dragAndDropPathList = new List<String>();
        List<int> dragAndDropTrackIdInPlaylistList = new List<int>();
        private void Drag()
        {
            if (this.dgvTrackList.SelectedRows != null && this.dgvTrackList.SelectedRows.Count > 0)
            {
                dragAndDropPathList = new List<String>();
                for (int i = 0; i < this.dgvTrackList.Rows.Count; i++)
                {
                    if (this.dgvTrackList.Rows[i].Selected)
                    {
                        dragAndDropTrackIdInPlaylistList.Add(Convert.ToInt32(this.dgvTrackList.Rows[i].Cells["TrackIdInPlaylist"].Value));
                        dragAndDropPathList.Add(this.dgvTrackList.Rows[i].Cells["Path"].Value.ToString());
                    }
                }
            }
            this.dgvTrackList.DoDragDrop(dragAndDropTrackIdInPlaylistList, DragDropEffects.Copy);
        }
        private int trackListDropLocationRowIndex = -1;
        private void dgvTrackList_DragOver(object sender, DragEventArgs e)
        {
            if (!this.dragPlaylist)
            {
                if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    e.Effect = DragDropEffects.Copy;
                }
                else
                {
                    this.dragMultipleRow = true;
                    e.Effect = DragDropEffects.Link;
                }

                int mousepos = PointToClient(Cursor.Position).Y;
                int bottomThreshold = Convert.ToInt32(this.dgvTrackList.Bottom + 20 - (this.dgvTrackList.Height * 0.2));
                int bottomThreshold2 = Convert.ToInt32(this.dgvTrackList.Bottom + 20 - (this.dgvTrackList.Height * 0.1));
                int topThreshold = Convert.ToInt32(40 + this.dgvTrackList.Top + 20 + (this.dgvTrackList.Height * 0.2));
                int topThreshold2 = Convert.ToInt32(40 + this.dgvTrackList.Top + 20 + (this.dgvTrackList.Height * 0.1));
                if (mousepos > bottomThreshold && mousepos <= bottomThreshold2)
                {
                    if (this.dgvTrackList.FirstDisplayedScrollingRowIndex < this.dgvTrackList.RowCount - 2)
                    {
                        this.dgvTrackList.FirstDisplayedScrollingRowIndex = this.dgvTrackList.FirstDisplayedScrollingRowIndex + 1;
                    }
                }
                else if (mousepos > bottomThreshold2)
                {
                    if (this.dgvTrackList.FirstDisplayedScrollingRowIndex < this.dgvTrackList.RowCount - 2)
                    {
                        this.dgvTrackList.FirstDisplayedScrollingRowIndex = this.dgvTrackList.FirstDisplayedScrollingRowIndex + 2;
                    }
                }
                else if (mousepos < topThreshold && mousepos >= topThreshold2)
                {
                    if (this.dgvTrackList.FirstDisplayedScrollingRowIndex > 1)
                    {
                        this.dgvTrackList.FirstDisplayedScrollingRowIndex = this.dgvTrackList.FirstDisplayedScrollingRowIndex - 1;
                    }
                }
                else if (mousepos < topThreshold2)
                {
                    if (this.dgvTrackList.FirstDisplayedScrollingRowIndex > 1)
                    {
                        this.dgvTrackList.FirstDisplayedScrollingRowIndex = this.dgvTrackList.FirstDisplayedScrollingRowIndex - 2;
                    }
                }

                Point clientPoint = this.dgvTrackList.PointToClient(new Point(e.X, e.Y));
                this.trackListDropLocationRowIndex = this.dgvTrackList.HitTest(clientPoint.X, clientPoint.Y).RowIndex;
                this.dgvTrackList.Invalidate();
            }
        }
        private void dgvTrackList_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.prepareToDragOneRow && e.Button == MouseButtons.Left)
            {
                Point p = this.dgvTrackList.PointToClient(new Point(e.X, e.Y));
                int dragIndex = this.dgvTrackList.HitTest(p.X, p.Y).RowIndex;

                if (this.currentRowIndex != dragIndex)
                {
                    this.dragOneRow = true;
                    this.Drag();
                }
            }
        }
        Pen pen2 = new Pen(Color.LightSeaGreen, 4);
        private void dgvTrackList_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if((this.dragMultipleRow || this.dragOneRow) && e.RowIndex == this.trackListDropLocationRowIndex - 1)
            {
                e.Graphics.DrawLine(pen2, e.CellBounds.Left, e.CellBounds.Bottom, e.CellBounds.Right, e.CellBounds.Bottom);
                e.Handled = true;
            }
        }
        List<int> oldListTrackIdInPlaylistList = new List<int>();
        private void dgvTrackList_DragDrop(object sender, DragEventArgs e)
        {
            Point p = this.dgvTrackList.PointToClient(new Point(e.X, e.Y));
            int dragIndex = this.dgvTrackList.HitTest(p.X, p.Y).RowIndex;

            oldListTrackIdInPlaylistList = new List<int>();
            for (int i = this.dgvTrackList.Rows.Count - 1; i >= 0; i--)
            {
                oldListTrackIdInPlaylistList.Add(Convert.ToInt32(this.dgvTrackList.Rows[i].Cells["TrackIdInPlaylist"].Value));
            }

            if (e.Effect == DragDropEffects.Link)
            {
                string[] filePathArray = (string[])e.Data.GetData(DataFormats.FileDrop);
                this.ExternalDragAndDropIntoTracklistEvent?.Invoke(this, new ListEventArgs() { DragAndDropFilePathArray = filePathArray, IntegerField1 = dragIndex });
            }
            else if (e.Effect == DragDropEffects.Copy)
            {
                this.InternalDragAndDropIntoTracklistEvent?.Invoke(this, new ListEventArgs() { SelectedRows = this.dgvTrackList.SelectedRows, IntegerField1 = dragIndex });
                this.DeleteTracksEvent?.Invoke(this, new ListEventArgs() { Rows = this.dgvTrackList.Rows });
            }

            this.SetSelectionAfterDragAndDrop(oldListTrackIdInPlaylistList);

            this.dragMultipleRow = false;
            this.prepareToDragOneRow = false;
            this.dragOneRow = false;
            this.currentRowIndex = -1;
        }
        public void SetSelectionAfterDragAndDrop(List<int> oldListTrackIdInPlaylistList)
        {
            this.oldListTrackIdInPlaylistList = oldListTrackIdInPlaylistList;

            this.BeginInvoke(new Action(() =>
            {
                this.dgvTrackList.ClearSelection();
                foreach (DataGridViewRow row in this.dgvTrackList.Rows)
                {
                    if (!oldListTrackIdInPlaylistList.Contains(Convert.ToInt32(row.Cells["TrackIdInPlaylist"].Value)))
                    {
                        row.Selected = true;
                    }
                    else
                    {
                        row.Selected = false;
                    }
                }
                oldListTrackIdInPlaylistList = new List<int>();
            }));
        }
        private void dgvTrackList_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            if (!this.dragPlaylist)
            {
                if (Control.MouseButtons != MouseButtons.Left)
                {
                    Point cursorPoint1 = this.dgvTrackList.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                    Point cursorPoint2 = this.dgvPlaylistList.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                    if (!this.dgvTrackList.ClientRectangle.Contains(cursorPoint1) &&
                        !this.dgvPlaylistList.ClientRectangle.Contains(cursorPoint2))
                    {
                        if (this.dragMultipleRow)
                            this.dragMultipleRow = false;
                        if (this.prepareToDragOneRow)
                            this.prepareToDragOneRow = false;
                        if (this.dragOneRow)
                            this.dragOneRow = false;
                        e.Action = DragAction.Cancel;
                    }
                }
            }
            
        }

        #endregion

        #region MEDIA PLAYER
        private void dgvTrackList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.CallSetCurrentTrackEvent((int)e.RowIndex);
        }
        private void dgvTrackList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex > -1)
            {
                this.CallPlayTrackEvent();
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.GetMediaPlayerProgressStatusEvent?.Invoke(this, EventArgs.Empty);
        }
       
        //EVENT CALLINGS
        public void CallSetCurrentTrackEvent(int rowIndex = -1)
        {
            this.SetCurrentTrackEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = rowIndex });
        }
        public void CallPlayTrackEvent()
        {
            this.PlayTrackEvent?.Invoke(this, new ListEventArgs() { });
        }
        public void CallPauseTrackEvent()
        {
            this.timer1.Stop();
            this.PauseTrackEvent?.Invoke(this, EventArgs.Empty);
        }
        public void CallStopTrackEvent()
        {
            this.timer1.Stop();
            this.StopTrackEvent?.Invoke(this, EventArgs.Empty);
        }
        public void CallPrevTrackEvent()
        {
            this.PrevTrackEvent?.Invoke(this, new ListEventArgs() { });
        }
        public void CallNextTrackEvent()
        {
            this.NextTrackEvent?.Invoke(this, new ListEventArgs() {  });
        }
        public void CallRandomTrackEvent()
        {
            this.RandomTrackEvent?.Invoke(this, EventArgs.Empty);
        }
        /*public void CallSetCurrentTrackColorEvent(int rowIndex = -1)
        {
            this.SetCurrentTrackColorEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = rowIndex });
        }*/

        //UPDATE PLAYLIST VIEW
        public void UpdateAfterPlayTrack(int currentTrackIndex, int currentTrackIdInPlaylist)
        {
            this.timer1.Start();
            String artist = "Playing: " + (string)dgvTrackList.Rows[currentTrackIndex].Cells["Artist"].Value;
            String title = "";
            if (dgvTrackList.Rows[currentTrackIndex].Cells["Title"] != null)
                title = dgvTrackList.Rows[currentTrackIndex].Cells["Title"].Value.ToString();

            if (!String.IsNullOrEmpty(title))
            {
                artist += " - " + title;
            }
            this.lblCurrentTrack.Text = artist;
            //this.CallSetCurrentTrackColorEvent(currentTrackIndex);
            this.UpdateTracklistColor(currentTrackIdInPlaylist);
        }
        public void UpdateAfterPlayTrackAfterPause()
        {
            this.timer1.Start();
            this.lblCurrentTrack.Text = this.lblCurrentTrack.Text.Replace("Paused: ", "Playing: ");
        }
        public void UpdateAfterStopTrack()
        {
            this.lblCurrentTrack.Text = "Playing: -";
            this.UpdateTracklistColor();
        }
        public void UpdateAfterPauseTrack()
        {
            this.lblCurrentTrack.Text = this.lblCurrentTrack.Text.Replace("Playing: ", "Paused: ");
        }
        public void UpdateAfterCopyTracksToPlaylist(int count, String playlistName)
        {
            if(count == 1)
            {
                LabelTimer.DisplayLabel(this.components, this.lblMessage, count + " track copied to [" + playlistName + "]");
            }
            else
            {
                LabelTimer.DisplayLabel(this.components, this.lblMessage, count + " tracks copied to [" + playlistName + "]");
            }
           
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
        public void ResetPlaylistList(bool isPlaylistListDisplayed)
        {
            if (isPlaylistListDisplayed)
            {
                this.dgvTrackList.Left -= this.trackListLeftOffset;
                this.dgvTrackList.Width += this.trackListLeftOffset;
            }
        }
        public void CallDisplayPlaylistList(bool isPlaylistListDisplayed)
        {
            if (!isPlaylistListDisplayed)
            {
                this.btnHidePlaylistList.Text = ">";
                this.btnDisplayPlaylistList.Text = ">";
                this.btnHidePlaylistList.Hide();
                this.btnDisplayPlaylistList.Show();

                this.dgvPlaylistList.Hide();
                this.groupBoxPlaylist.Hide();

                if(this.dgvTrackList.Left > 9)
                {
                    this.dgvTrackList.Left -= this.trackListLeftOffset;
                    this.dgvTrackList.Width += this.trackListLeftOffset;
                }
            }
            else
            {
                this.btnHidePlaylistList.Text = "<";
                this.btnDisplayPlaylistList.Text = "<";
                this.btnHidePlaylistList.Show();
                this.btnDisplayPlaylistList.Hide();

                this.dgvPlaylistList.Show();
                this.groupBoxPlaylist.Show();
                this.dgvTrackList.Left += this.trackListLeftOffset;
                this.dgvTrackList.Width -= this.trackListLeftOffset;
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
        public void UpdateMediaPlayerProgressStatus(double duration, String durationString, double currentPosition, String currentPositionString)
        {
            ((MainView)this.parentView).UpdateMediaPlayerProgressStatus(duration, durationString, currentPosition, currentPositionString);
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
        }
        private void dgvPlaylistList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            this.CallLoadPlaylistEvent();
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
        private void btnNewPlaylist_Click(object sender, EventArgs e)
        {
            this.CallCreatePlaylistEvent();
        }
        private void btnLoadPlaylist_Click(object sender, EventArgs e)
        {
            this.CallLoadPlaylistEvent();
        }
        private void btnRenamePlaylist_Click(object sender, EventArgs e)
        {
            this.CallRenamePlaylistEvent();
        }
        private void btnDeletePlaylist_Click(object sender, EventArgs e)
        {
            this.CallDeletePlaylistEvent();
        }
        private void btnSetQuickListGroup1_Click(object sender, EventArgs e)
        {
            this.CallSetQuickListEvent(1);
        }
        private void btnSetQuickListGroup2_Click(object sender, EventArgs e)
        {
            this.CallSetQuickListEvent(2);
        }
        private void btnSetQuickListGroup3_Click(object sender, EventArgs e)
        {
            this.CallSetQuickListEvent(3);
        }
        private void btnSetQuickListGroup4_Click(object sender, EventArgs e)
        {
            this.CallSetQuickListEvent(4);
        }
        
        //EVENT CALLINGS
        public void CallCreatePlaylistEvent()
        {
            this.CreatePlaylist?.Invoke(this, new ListEventArgs() { IntegerField1 = -1 });
        }
        public void CallRenamePlaylistEvent()
        {
            if (this.dgvPlaylistList.SelectedRows.Count > 0)
                this.EditPlaylist?.Invoke(this, new ListEventArgs() { IntegerField1 = dgvPlaylistList.SelectedRows[0].Index });
        }
        public void CallLoadPlaylistEvent()
        {
            if (!this.chbFilterModeEnabled.Checked)
            {
                if (this.dgvPlaylistList.SelectedRows.Count > 0)
                {
                    this.LoadPlaylistEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = this.dgvPlaylistList.SelectedRows[0].Index });
                }
            }
            
        }
        public void CallDeletePlaylistEvent()
        {
            if (this.dgvPlaylistList.SelectedRows.Count > 0)
                this.DeletePlaylistEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = dgvPlaylistList.SelectedRows[0].Index });
        }
        public void CallSetQuickListEvent(int group)
        {
            if (this.dgvPlaylistList.SelectedRows.Count > 0)
                this.SetQuickListEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = dgvPlaylistList.Rows.IndexOf(dgvPlaylistList.SelectedRows[0]), IntegerField2 = group });
        }
        public void CallExportToM3UEvent()
        {
            if (this.dgvPlaylistList.SelectedRows.Count > 0)
                this.ExportToM3UEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = this.dgvPlaylistList.SelectedRows[0].Index });
        }
        public void CallExportToTXTEvent()
        {
            if (this.dgvPlaylistList.SelectedRows.Count > 0)
                this.ExportToTXTEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = this.dgvPlaylistList.SelectedRows[0].Index });
        }
        public void CallExportToDirectoryEvent()
        {
            if (this.dgvPlaylistList.SelectedRows.Count > 0)
                this.ExportToDirectoryEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = this.dgvPlaylistList.SelectedRows[0].Index });
        }
        /*
        public void SetCurrentPlaylistColor(int playlistId)
        {
            this.ClearCurrentPlaylistColor();

            if (playlistId != -1)
            {
                int idInList = -1;
                for (int i = 0; i <= this.dgvPlaylistList.Rows.Count - 1; i++)
                {
                    idInList = Convert.ToInt32(this.dgvPlaylistList.Rows[i].Cells["Id"].Value);
                    if (playlistId == idInList)
                    {
                        this.dgvPlaylistList.Rows[i].DefaultCellStyle.BackColor = Color.LightSeaGreen;
                        break;
                    }
                }
            }
        }*/
       /* private void ClearCurrentPlaylistColor()
        {
            for (int i = 0; i < this.dgvPlaylistList.Rows.Count; i++)
            {
                if (this.dgvPlaylistList.Rows[i].DefaultCellStyle.BackColor == Color.LightSeaGreen)
                {
                    this.dgvPlaylistList.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    break;
                }
            }
        }*/
        #endregion

        #region PLAYLIST - DRAG AND DROP

        private int currentPlaylistRowIndex = -1;
        private bool dragPlaylist = false;
        private bool prepareToPlaylistDrag = false;
        private void dgvPlaylistList_MouseDown(object sender, MouseEventArgs e)
        {
            HitTestInfo hitTest = this.dgvPlaylistList.HitTest(e.X, e.Y);
            if (hitTest != null && hitTest.RowIndex > -1)
            {
                this.currentPlaylistRowIndex = hitTest.RowIndex;

                if (this.dgvPlaylistList.Rows[hitTest.RowIndex].Selected)
                {
                    if (this.dgvPlaylistList.Rows.Count != this.dgvPlaylistList.SelectedRows.Count)
                    {
                        this.prepareToPlaylistDrag = true;
                    }
                }
            }
        }
        private void dgvPlaylistList_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.prepareToPlaylistDrag && e.Button == MouseButtons.Left)
            {
                Point p = this.dgvPlaylistList.PointToClient(new Point(e.X, e.Y));
                int dragIndex = this.dgvPlaylistList.HitTest(p.X, p.Y).RowIndex;

                if (this.currentPlaylistRowIndex != dragIndex)
                {
                    this.dragPlaylist = true;
                    this.DragPlaylist();
                }
            }
        }

        List<int> dragAndDropPlaylistIdList = new List<int>();
        private void DragPlaylist()
        {
            if (this.dgvPlaylistList.SelectedRows != null && this.dgvPlaylistList.SelectedRows.Count > 0)
            {
                dragAndDropPlaylistIdList = new List<int>();
                for (int i = 0; i < this.dgvPlaylistList.Rows.Count; i++)
                {
                    if (this.dgvPlaylistList.Rows[i].Selected)
                    {
                        dragAndDropPlaylistIdList.Add(Convert.ToInt32(this.dgvPlaylistList.Rows[i].Cells["Id"].Value));
                    }
                }
            }
            this.dgvPlaylistList.DoDragDrop(dragAndDropPlaylistIdList, DragDropEffects.Copy);
        }

        private int playlistDropLocationRowIndex = -1;
        private void dgvPlaylistList_DragOver(object sender, DragEventArgs e)
        {
            if (this.dragPlaylist)
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                if (!e.Data.GetDataPresent(DataFormats.FileDrop))
                {
                    e.Effect = DragDropEffects.Copy;
                }
                else
                {
                    e.Effect = DragDropEffects.Link;
                }
            }
           
            Point clientPoint = this.dgvPlaylistList.PointToClient(new Point(e.X, e.Y));
            this.playlistDropLocationRowIndex = this.dgvPlaylistList.HitTest(clientPoint.X, clientPoint.Y).RowIndex;
            this.dgvPlaylistList.Invalidate();
        }
        private void dgvPlaylistList_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (this.dragPlaylist && e.RowIndex == this.playlistDropLocationRowIndex)
            {
                Rectangle newRect = new Rectangle(e.CellBounds.X + 1,
                  e.CellBounds.Y + 1, e.CellBounds.Width - 4,
                  e.CellBounds.Height - 4);

                using (Brush gridBrush = new SolidBrush(Color.LightSeaGreen), backColorBrush = new SolidBrush(e.CellStyle.BackColor))
                {
                    using (Pen gridLinePen = new Pen(gridBrush))
                    {
                        e.Graphics.DrawRectangle(Pens.LightSeaGreen, newRect);
                        e.Handled = true;
                    }
                }
            }
            else if ((this.dragMultipleRow || this.dragOneRow) && e.RowIndex == this.playlistDropLocationRowIndex)
            {
                Rectangle newRect = new Rectangle(e.CellBounds.X + 1,
                  e.CellBounds.Y + 1, e.CellBounds.Width - 4,
                  e.CellBounds.Height - 4);

                using (Brush gridBrush = new SolidBrush(Color.LightSeaGreen), backColorBrush = new SolidBrush(e.CellStyle.BackColor))
                {
                    using (Pen gridLinePen = new Pen(gridBrush))
                    {
                        e.Graphics.DrawRectangle(Pens.LightSeaGreen, newRect);
                        e.Handled = true;
                    }
                }
            }
        }
        private void dgvPlaylistList_DragDrop(object sender, DragEventArgs e)
        {
            Point p = this.dgvPlaylistList.PointToClient(new Point(e.X, e.Y));
            int dragIndex = this.dgvPlaylistList.HitTest(p.X, p.Y).RowIndex;

           
            if (this.dragPlaylist)
            {
                if (e.Effect == DragDropEffects.Copy)
                {
                    this.MovePlaylistEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = this.dgvPlaylistList.SelectedRows[0].Index, IntegerField2 = dragIndex });
                }

                this.dragPlaylist = false;
                this.prepareToPlaylistDrag = false;
                this.currentPlaylistRowIndex = -1;

                this.SetSelectionAfterPlaylistDragAndDrop(dragIndex);
            }
            else
            {
                oldListTrackIdInPlaylistList = new List<int>();
                for (int i = this.dgvTrackList.Rows.Count - 1; i >= 0; i--)
                {
                    oldListTrackIdInPlaylistList.Add(Convert.ToInt32(this.dgvTrackList.Rows[i].Cells["TrackIdInPlaylist"].Value));
                }

                if (e.Effect == DragDropEffects.Link)
                {
                    string[] filePathArray = (string[])e.Data.GetData(DataFormats.FileDrop);
                    this.ExternalDragAndDropIntoPlaylistEvent?.Invoke(this, new ListEventArgs() { DragAndDropFilePathArray = filePathArray, IntegerField1 = dragIndex });
                }
                else if (e.Effect == DragDropEffects.Copy)
                {
                    this.InternalDragAndDropIntoPlaylistEvent?.Invoke(this, new ListEventArgs() { SelectedRows = this.dgvTrackList.SelectedRows, IntegerField1 = dragIndex });
                }

                this.dragMultipleRow = false;
                this.prepareToDragOneRow = false;
                this.dragOneRow = false;
                this.currentRowIndex = -1;

                this.dgvPlaylistList.Rows[dragIndex].Selected = true;
                this.SetSelectionAfterDragAndDrop(oldListTrackIdInPlaylistList);
            }
        }
        public void SetSelectionAfterPlaylistDragAndDrop(int dragIndex)
        {
            this.BeginInvoke(new Action(() =>
            {
                this.dgvPlaylistList.ClearSelection();
                this.dgvPlaylistList.Rows[dragIndex].Selected = true;
            }));
        }
        private void dgvPlaylistList_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            if (this.dragPlaylist)
            {
                if (Control.MouseButtons != MouseButtons.Left)
                {
                    Point cursorPoint = this.dgvPlaylistList.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y));
                    if (!this.dgvPlaylistList.ClientRectangle.Contains(cursorPoint))
                    {
                        if (this.dragPlaylist)
                            this.dragPlaylist = false;
                        if(this.prepareToPlaylistDrag)
                            this.prepareToPlaylistDrag = false;
                        e.Action = DragAction.Cancel;
                    }
                }
            }
            
        }
        private void btnDisplayPlaylistList_Click(object sender, EventArgs e)
        {
            this.DisplayPlaylistListEvent?.Invoke(this, new EventArgs());
        }
        private void btnDisplayPlaylistList2_Click(object sender, EventArgs e)
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

        public void InitializeTagValueEditor(List<Tag> tagList, List<List<TagValue>> tagValueListContainer, bool isTagEditorDisplayed, bool isOnlyPlayingRowModeEnabled, bool isFilterModeEnabled)
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
                this.txtbFilter.Text = string.Empty;
                this.chbFilterModeEnabled.Checked = true;
            }
            else
            {
                this.chbOnlyPlayingRowModeEnabled.Show();
                this.lblFilter.Hide();
                this.txtbFilter.Hide();
                this.txtbFilter.Text = string.Empty;
                this.chbFilterModeEnabled.Checked = false;
            }

            if(isFilterModeEnabled && isTagEditorDisplayed)
            {
                this.rtxtbTagValueEditorParams.Show();
                this.btnClearTagValueFilter.Show();
            }
            else
            {
                this.rtxtbTagValueEditorParams.Hide();
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
                        btn.Text = "Save";
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
                        btn.TextBox = txtBox;

                        txtBox.KeyDown += new KeyEventHandler(
                             (sender, e) => this.txtbSetTagValue_KeyDown(sender, e, btn));
                        btn.Click += new System.EventHandler(
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

                        btn.Click += new System.EventHandler(
                                (sender, e) => this.btnClearTagValue_Click(sender, e));
                        btn.Location = new Point(buttonsIntervalX, buttonsIntervalY);

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

                            btn.Click += new System.EventHandler(
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
        public void CallDisplayTagEditor(bool isTagEditorDisplayed)
        {
            ///kikapcsolt állapotban a panelt elrejtjük, textboxot és gombot elrejtjük, grid szélessége nő
            ///bekapcsolt állapotban a panel látszik, ha textbox van, akkor az látzik, ellenben a gombok, grid szélessége csökken
            if (!isTagEditorDisplayed)
            {
                this.btnDisplayTagEditor.Text = "<";
                this.btnHideTagEditor.Text = "<";
                this.btnDisplayTagEditor.Show();
                this.btnHideTagEditor.Hide();
                this.btnColumnVisibilityWithTagEditor.Show();
                this.btnColumnVisibility.Hide();

                this.chbOnlyPlayingRowModeEnabled.Hide();

                this.tagValueEditorPanel.Hide();

                if(this.dgvTrackList.Width < 1089)
                    this.dgvTrackList.Width = this.dgvTrackList.Width + this.trackListRightOffset;
            }
            else
            {
                this.btnDisplayTagEditor.Text = ">";
                this.btnHideTagEditor.Text = ">";
                this.btnDisplayTagEditor.Hide();
                this.btnHideTagEditor.Show();
                this.btnColumnVisibilityWithTagEditor.Hide();
                this.btnColumnVisibility.Show();

                if (!this.chbFilterModeEnabled.Checked)
                {
                    this.chbOnlyPlayingRowModeEnabled.Show();
                }


                this.tagValueEditorPanel.Show();

                this.dgvTrackList.Width = this.dgvTrackList.Width - this.trackListRightOffset;
            }
        }
        private void btnDisplayTagEditor_Click(object sender, EventArgs e)
        {
            this.DisplayTagEditorEvent?.Invoke(this, new EventArgs());
            this.chbOnlyPlayingRowModeEnabled.Show();

            if (this.chbFilterModeEnabled.Checked)
            {
                this.rtxtbTagValueEditorParams.Show();
                this.btnClearTagValueFilter.Show();
            }

            this.dgvTrackList.Focus();
        }
        private void btnDisplayTagEditor2_Click(object sender, EventArgs e)
        {
            this.DisplayTagEditorEvent?.Invoke(this, new EventArgs());
            this.chbOnlyPlayingRowModeEnabled.Hide();

            this.rtxtbTagValueEditorParams.Hide();
            this.btnClearTagValueFilter.Hide();

            this.dgvTrackList.Focus();
        }
        private void btnSetTagValue_Click(object sender, EventArgs e)
        {
            TagValueButton button = (sender as TagValueButton);

            if (this.dgvPlaylistList.SelectedRows.Count > 0)
            {
                String tagValueValue = String.Empty;
                if(button.TextBox != null)
                {
                    tagValueValue = button.TextBox.Text;

                    if(!this.chbFilterModeEnabled.Checked)
                        button.TextBox.Text = String.Empty;
                }
                this.SetTagValueEvent?.Invoke(this, new ListEventArgs() { StringField1 = button.TagName, StringField2 = button.TagValueName, StringField3 = tagValueValue,  Rows = this.dgvTrackList.Rows });
            }
            this.dgvTrackList.Focus();
        }
        private void txtbSetTagValue_KeyDown(object sender, KeyEventArgs e, Button btn)
        {
            if(e.KeyCode == Keys.Enter)
            {
                TagValueButton button = (btn as TagValueButton);

                if (this.chbOnlyPlayingRowModeEnabled.Checked)
                {
                    String tagValueValue = String.Empty;
                    if (button.TextBox != null)
                    {
                        tagValueValue = button.TextBox.Text;
                        button.TextBox.Text = String.Empty;
                    }
                    this.SetTagValueEvent?.Invoke(this, new ListEventArgs() { StringField1 = button.TagName, StringField2 = button.TagValueName, StringField3 = tagValueValue, Rows = this.dgvTrackList.Rows });
                }
                else
                {
                    if (this.dgvPlaylistList.SelectedRows.Count > 0)
                    {
                        String tagValueValue = String.Empty;
                        if (button.TextBox != null)
                        {
                            tagValueValue = button.TextBox.Text;
                            button.TextBox.Text = String.Empty;
                        }
                        this.SetTagValueEvent?.Invoke(this, new ListEventArgs() { StringField1 = button.TagName, StringField2 = button.TagValueName, StringField3 = tagValueValue, Rows = this.dgvTrackList.Rows });
                    }
                }

                this.dgvTrackList.Focus();
            }
        }
        private void btnClearTagValue_Click(object sender, EventArgs e)
        {
            TagValueButton button = (sender as TagValueButton);

            if (this.chbOnlyPlayingRowModeEnabled.Checked)
            {
                this.ClearTagValueEvent?.Invoke(this, new ListEventArgs() { StringField1 = button.TagName, Rows = this.dgvTrackList.Rows });
            }
            else
            {
                if (this.dgvPlaylistList.SelectedRows.Count > 0)
                {
                    this.ClearTagValueEvent?.Invoke(this, new ListEventArgs() { StringField1 = button.TagName, Rows = this.dgvTrackList.Rows });
                }
            }

            this.dgvTrackList.Focus();
        }
        private void chbOnlyPlayingRowModeEnabled_CheckedChanged(object sender, EventArgs e)
        {
            this.dgvTrackList.Focus();
            this.ChangeOnlyPlayingRowModeEnabled?.Invoke(this, new ListEventArgs() { BooleanField1 = this.chbOnlyPlayingRowModeEnabled.Checked });
        }
        private void chbFilterModeEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chbFilterModeEnabled.Checked)
            {
                this.rtxtbTagValueEditorParams.Show();
                this.btnClearTagValueFilter.Show();

                this.lblFilter.Show();
                this.txtbFilter.Show();
                this.txtbFilter.Text = string.Empty;

                if (this.tagValueEditorPanel.Height != 449)
                {
                    this.tagValueEditorPanel.Size = new Size(this.tagValueEditorPanel.Width, this.tagValueEditorPanel.Height - this.tagValueEditorPanelBottomOffset);
                }

                this.chbOnlyPlayingRowModeEnabled.Hide();
            }
            else
            {
                this.rtxtbTagValueEditorParams.Hide();
                this.btnClearTagValueFilter.Hide();

                this.lblFilter.Hide();
                this.txtbFilter.Hide();
                this.txtbFilter.Text = string.Empty;

                this.tagValueEditorPanel.Size = new Size(this.tagValueEditorPanel.Width, this.tagValueEditorPanel.Height + this.tagValueEditorPanelBottomOffset);

                this.chbOnlyPlayingRowModeEnabled.Show();
            }
            this.dgvTrackList.Focus();
            this.ChangeFilterModeEnabled?.Invoke(this, new ListEventArgs() { BooleanField1 = this.chbFilterModeEnabled.Checked });
        }
        private void txtbFilter_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.ChangeFilterText?.Invoke(this, new ListEventArgs() { StringField1 = this.txtbFilter.Text });
            }
        }
        public void SetTagValueFilter(Dictionary<String, String> tagValueFilterDic)
        {
            String tagValueAsFilter = String.Empty;
            if (tagValueFilterDic != null && tagValueFilterDic.Count > 0)
            {
                foreach (KeyValuePair<String, String> filter in tagValueFilterDic)
                {
                    if (String.IsNullOrEmpty(tagValueAsFilter))
                    {
                        tagValueAsFilter = "[" + filter.Key + ": " + filter.Value + "]";
                    }
                    else
                    {
                        tagValueAsFilter = tagValueAsFilter + "  [" + filter.Key + ": " + filter.Value + "]";
                    }
                }
            }
            this.rtxtbTagValueEditorParams.Text = tagValueAsFilter;

            this.ChangeFilterText?.Invoke(this, new ListEventArgs() { StringField1 = this.txtbFilter.Text });
        }
        private void btnClearTagValueFilter_Click(object sender, EventArgs e)
        {
            this.RemoveTagValueFilter?.Invoke(this, new EventArgs());
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
                this.btnSave.BackColor = this.ActiveButtonColor;
                this.btnSave.ForeColor = this.ButtonColor;
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
    }
}
