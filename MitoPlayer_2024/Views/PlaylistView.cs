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
using System.Windows.Forms;

using static System.Windows.Forms.DataGridView;

namespace MitoPlayer_2024.Views
{
    public partial class PlaylistView : Form, IPlaylistView
    {
        private Form parentView { get; set; }

        //DATATABLES
        private BindingSource playlistListBindingSource { get; set; }
        private BindingSource selectedTrackListBindingSource { get; set; }
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
        public event EventHandler<ListEventArgs> SetCurrentTrackColorEvent;

        //TRACKLIST
        public event EventHandler<ListEventArgs> OrderByColumnEvent;
        public event EventHandler<ListEventArgs> DeleteTracksEvent;
        public event EventHandler<ListEventArgs> InternalDragAndDropIntoTracklistEvent;
        public event EventHandler<ListEventArgs> InternalDragAndDropIntoPlaylistEvent;
        public event EventHandler<ListEventArgs> ExternalDragAndDropIntoTracklistEvent;
        public event EventHandler<ListEventArgs> ExternalDragAndDropIntoPlaylistEvent;
        public event EventHandler<ListEventArgs> ChangeTracklistColorEvent;
        public event EventHandler ShowColumnVisibilityEditorEvent;
        public event EventHandler ScanBpmEvent;

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
        

        public PlaylistView()
        {
            InitializeComponent();
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

        //CALLS FROM PLAYLIST PRESENTER
        public void SetPlaylistListBindingSource(BindingSource playlistList, bool[] columnVisibility, int currentPlaylistId)
        {
            this.playlistListBindingSource = new BindingSource();
            this.playlistListBindingSource.DataSource = playlistList;
            this.dgvPlaylistList.DataSource = this.playlistListBindingSource.DataSource;
            for (int i = 0; i <= this.dgvPlaylistList.Columns.Count - 1; i++)
            {
                this.dgvPlaylistList.Columns[i].Visible = columnVisibility[i];
            }
            
            if (this.dgvPlaylistList.Rows.Count > 0)
            {
                for (int i = 0; i < this.dgvPlaylistList.Rows.Count; i++)
                {
                    if (Convert.ToInt32(this.dgvPlaylistList.Rows[i].Cells["Id"].Value) == currentPlaylistId)
                    {
                        this.dgvPlaylistList.Rows[i].Selected = true;
                        this.dgvPlaylistList.CurrentCell = this.dgvPlaylistList.Rows[i].Cells["Name"];
                        break;
                    }
                }
            }
            foreach (DataGridViewColumn column in this.dgvPlaylistList.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            this.dgvPlaylistList.Columns["G"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            this.dgvPlaylistList.Columns["G"].Width = 20;

            this.dgvPlaylistList.CellBorderStyle = DataGridViewCellBorderStyle.None;

        }
        public void SetTrackListBindingSource(BindingSource trackList, bool[] columnVisibility, int[] displayIndex, int currentTrackIdInPlaylist)
        {
            this.trackListBindingSource = new BindingSource();
            this.trackListBindingSource.DataSource = trackList;
            this.dgvTrackList.DataSource = this.trackListBindingSource.DataSource;
            for (int i = 0; i <= this.dgvTrackList.Columns.Count - 1; i++)
            {
                this.dgvTrackList.Columns[i].Visible = columnVisibility[i];
                this.dgvTrackList.Columns[i].DisplayIndex = displayIndex[i];
            }

            this.SetColorAndSelection(currentTrackIdInPlaylist);
        }

        public void SetColorAndSelection(int currentTrackIdInPlaylist = -1)
        {
            bool isMissing = false;
            int trackIdInPlaylist = -1;

            int columnIndex = 0;

            for (int i = 0; i < this.dgvTrackList.Rows.Count; i++)
            {
                for (int j = 0; j < this.dgvTrackList.Columns.Count; j++)
                {
                    this.dgvTrackList.Rows[i].Cells[j].Style.BackColor = Color.White;
                    this.dgvTrackList.Rows[i].Cells[j].Style.ForeColor = Color.Black;
                }
            }

            if (this.currentTagForColors != null)
            {
                for (int j = 0; j < this.dgvTrackList.Columns.Count; j++)
                {
                    if (this.dgvTrackList.Columns[j].Name == this.currentTagForColors.Name)
                    {
                        columnIndex = j;
                        break;
                    }
                }
            }

            for (int i = 0; i < this.dgvTrackList.Rows.Count; i++)
            {
                isMissing = (bool)this.dgvTrackList.Rows[i].Cells["IsMissing"].Value;
                trackIdInPlaylist = (int)this.dgvTrackList.Rows[i].Cells["TrackIdInPlaylist"].Value;
                if (isMissing)
                {
                    this.dgvTrackList.Rows[i].DefaultCellStyle.BackColor = Color.Salmon;
                }
                else if(currentTrackIdInPlaylist != -1 && trackIdInPlaylist == currentTrackIdInPlaylist) 
                {
                    for (int j = 0; j < this.dgvTrackList.Columns.Count; j++)
                    {
                        this.dgvTrackList.Rows[i].Cells[j].Style.BackColor = Color.LightSeaGreen;
                    }
                }
                else
                {
                    if (this.currentTagForColors != null)
                    {
                        String actualTagValueName = this.dgvTrackList.Rows[i].Cells[this.currentTagForColors.Name].Value.ToString();
                        if (!String.IsNullOrEmpty(actualTagValueName))
                        {
                            Color color = this.currentTagValueColorDic[actualTagValueName];
                            if (color != null)
                            {
                                if (this.currentTagForColors.CellOnly)
                                {
                                    for (int j = 0; j < this.dgvTrackList.Columns.Count; j++)
                                    {
                                        if(columnIndex == j)
                                        {
                                            //this.dgvTrackList.Rows[i].Cells[j].Style.BackColor = color;
                                           // this.dgvTrackList.Rows[i].Cells[j].Style.BackColor = Color.Black;
                                            this.dgvTrackList.Rows[i].Cells[j].Style.BackColor = color;

                                            if ((color.R < 100 && color.G < 100) || (color.R < 100 && color.B < 100) || (color.B < 100 && color.G < 100))
                                            {
                                                this.dgvTrackList.Rows[i].Cells[j].Style.ForeColor = Color.White;
                                            }
                                            else
                                            {
                                                this.dgvTrackList.Rows[i].Cells[j].Style.ForeColor = Color.Black;
                                            }
                                        }
                                        else
                                        {
                                           // this.dgvTrackList.Rows[i].Cells[j].Style.BackColor = Color.White;
                                           this.dgvTrackList.Rows[i].Cells[j].Style.BackColor = Color.White;
                                           this.dgvTrackList.Rows[i].Cells[j].Style.ForeColor = Color.Black;

                                        }
                                        
                                    }

                                }
                                else
                                {
                                    for (int j = 0; j < this.dgvTrackList.Columns.Count; j++)
                                    {
                                        this.dgvTrackList.Rows[i].Cells[j].Style.BackColor = color;
                                       /* if ((color.R < 100 && color.G < 100) || (color.R < 100 && color.B < 100) || (color.B < 100 && color.G < 100))
                                        {
                                            this.dgvTrackList.Rows[i].Cells[j].Style.ForeColor = Color.White;
                                        }
                                        else
                                        {
                                            this.dgvTrackList.Rows[i].Cells[j].Style.ForeColor = Color.Black;
                                        }*/
                                    }
                                }
                            }
                        }

                    }
                    else
                    {
                        for (int j = 0; j < this.dgvTrackList.Columns.Count; j++)
                        {
                            this.dgvTrackList.Rows[i].Cells[j].Style.BackColor = Color.White;
                            this.dgvTrackList.Rows[i].Cells[j].Style.ForeColor = Color.Black;
                        }
                    }

                }
            }
            this.dgvTrackList.CellBorderStyle = DataGridViewCellBorderStyle.None;
        }


        public void SetSelectedTrackListBindingSource(BindingSource selectedTrackList)
        {
            this.selectedTrackListBindingSource = new BindingSource();
            this.selectedTrackListBindingSource.DataSource = selectedTrackList;
        }

       
        #endregion

        #region TRACKLIST - ROW SELECTION

        private bool controlKey = false;
        private void dgvTrackList_SelectionChanged(object sender, EventArgs e)
        {
            if (this.dgvTrackList.SelectedRows != null && this.dgvTrackList.SelectedRows.Count > 0)
            {
                //ROW COUNT
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

            if (rdbPlaylist.Checked)
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
            }
            else if (rdbTagValue.Checked){
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
               
            }
            

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
            this.ScanBpmEvent?.Invoke(this, EventArgs.Empty);
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
        public void CallSetCurrentTrackColorEvent(int rowIndex = -1)
        {
            this.SetCurrentTrackColorEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = rowIndex });
        }

        //UPDATE PLAYLIST VIEW
        public void UpdateAfterPlayTrack(int currentTrackIndex, int currentTrackId)
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
            this.SetColorAndSelection(currentTrackId);
        }
        public void UpdateAfterPlayTrackAfterPause()
        {
            this.timer1.Start();
            this.lblCurrentTrack.Text = this.lblCurrentTrack.Text.Replace("Paused: ", "Playing: ");
        }
        public void UpdateAfterStopTrack()
        {
            this.lblCurrentTrack.Text = "Playing: -";
            this.ClearCurrentTrackColor();
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
        public void InitializeTagEditor(List<Tag> tagList)
        {
            //tag-eket mutató gombok jelennek meg először, 9db gomb
            //induláskor minden megjelenne, ezért az összeset elhide-oljuk
            //taglist-en végigmegyünk és az első x darab label-jét beállítjuk, illetve megjelenítjük (ahány tag, annyi gomb)
            List<Button> buttonList = new List<Button> { this.btnTag1, this.btnTag2, this.btnTag3, 
                                                        this.btnTag4, this.btnTag5, this.btnTag6,
                                                        this.btnTag7,this.btnTag8,this.btnTag9 };
            for (int i = 0; i <= buttonList.Count - 1; i++)
            {
                buttonList[i].Hide();
            }
            if (tagList != null && tagList.Count > 0)
            {
                for (int i = 0; i <= tagList.Count - 1; i++)
                {
                    buttonList[i].Text = tagList[i].Name;
                    buttonList[i].Show();
                }
            }
        }

        int tgvHotkeyIndex1 = -1;
        int tgvHotkeyIndex2 = -1;
        int tgvHotkeyIndex3 = -1;
        int tgvHotkeyIndex4 = -1;
        public void InitializeTagValueEditor(List<TagValue> tagValueList, bool isTagEditorDisplayed, bool inputTextBoxEnabled = false)
        {
            //van 24 tagvalue gomb, induláskor megjelennének, ezért elhideoljuk őket
            //a hotkey text-eket inicializáljuk
            //ha inputtextbox van, akkor egy textbox meg a gomb jelenik csak meg, ellenben végigmegyünk a gombokon és a tag-hez tartozó tagvalue-k neveit felrakjuk a gombokra, majd megjelenítjük őket, azaz megjelenik x db tagvalue gomb
            //hotkey indexeket is inicializáljuk
            List<Button> buttonList = new List<Button> { this.btnTagValue1, this.btnTagValue2, this.btnTagValue3,
                                                        this.btnTagValue4, this.btnTagValue5, this.btnTagValue6,
                                                        this.btnTagValue7,this.btnTagValue8,this.btnTagValue9,
                                                        this.btnTagValue10, this.btnTagValue11, this.btnTagValue12,
                                                        this.btnTagValue13, this.btnTagValue14,this.btnTagValue15,
                                                        this.btnTagValue16,this.btnTagValue17,this.btnTagValue18,
                                                        this.btnTagValue19,this.btnTagValue20,this.btnTagValue21,
                                                        this.btnTagValue22,this.btnTagValue23,this.btnTagValue24};
            
            for (int i = 0; i <= buttonList.Count - 1; i++)
            {
                buttonList[i].Hide();
            }

            if (tagValueList != null && tagValueList.Count > 0)
            {
                for (int i = 0; i <= tagValueList.Count - 1; i++)
                {
                    this.tgvHotkeyName1.Text = "(1) - ";
                    this.tgvHotkeyName2.Text = "(2) - ";
                    this.tgvHotkeyName3.Text = "(3) - ";
                    this.tgvHotkeyName4.Text = "(4) - ";
                    this.tgvHotkeyIndex1 = -1;
                    this.tgvHotkeyIndex2 = -1;
                    this.tgvHotkeyIndex3 = -1;
                    this.tgvHotkeyIndex4 = -1;
                }
            }

            if (!inputTextBoxEnabled)
            {
                if (tagValueList != null && tagValueList.Count > 0)
                {
                    for (int i = 0; i <= tagValueList.Count - 1; i++)
                    {
                        buttonList[i].Text = tagValueList[i].Name;
                        buttonList[i].BackColor = tagValueList[i].Color;

                        if ((tagValueList[i].Color.R < 100 && tagValueList[i].Color.G < 100) || (tagValueList[i].Color.R < 100 && tagValueList[i].Color.B < 100) || (tagValueList[i].Color.B < 100 && tagValueList[i].Color.G < 100))
                        {
                           // buttonList[i].ForeColor = Color.White;
                        }

                        buttonList[i].Show();

                        if(tagValueList[i].Hotkey == 1)
                        {
                            this.tgvHotkeyIndex1 = i;
                            this.tgvHotkeyName1.Text = "(1) " + tagValueList[i].Name;
                        }
                        else if (tagValueList[i].Hotkey == 2)
                        {
                            this.tgvHotkeyIndex2 = i;
                            this.tgvHotkeyName2.Text = "(2) " + tagValueList[i].Name;
                        }
                        else if (tagValueList[i].Hotkey == 3)
                        {
                            this.tgvHotkeyIndex3 = i;
                            this.tgvHotkeyName3.Text = "(3) " + tagValueList[i].Name;
                        }
                        else if (tagValueList[i].Hotkey == 4)
                        {
                            this.tgvHotkeyIndex4 = i;
                            this.tgvHotkeyName4.Text = "(4) " + tagValueList[i].Name;
                        }
                    }
                }
                this.txtBoxTagValueInput.Hide();
                this.btnSetTagValue.Hide();
            }
            else
            {
                this.txtBoxTagValueInput.Show();
                this.btnSetTagValue.Show();
            }

            if (isTagEditorDisplayed)
            {
                this.dgvTrackList.Width = this.dgvTrackList.Width + 260;
            }

        }
        
        
        public void InitializeCurrentTagValueColors(List<Tag> tagList, int currentTagId)
        {
            int selectedIndex = 0;

            this.cmbColor.Items.Clear();
            this.cmbColor.Items.Add("No Color");

            if (tagList != null && tagList.Count > 0)
            {
                int comboboxContentCount = 0;
                

                for (int i = 0; i <= tagList.Count - 1; i++)
                {
                    if (!tagList[i].HasMultipleValues)
                    {
                        this.cmbColor.Items.Add(tagList[i].Name);
                        comboboxContentCount++;
                        if (tagList[i].Id == currentTagId)
                        {
                            selectedIndex = comboboxContentCount;
                        }
                    }
                       
                }
            }

            this.currentTagForColors = null;
            this.currentTagValueColorDic = null;
            //this.cmbColor.SelectedItem = this.cmbColor.Items[selectedIndex];

           // this.ChangeTracklistColorEvent?.Invoke(this, new ListEventArgs() { StringField1 = (String)this.cmbColor.SelectedItem });

        }
        public void ChangeCurrentTagValueColors(Tag tag, Dictionary<String, Color> tagValueColors)
        {
            this.currentTagForColors = tag;
            this.currentTagValueColorDic = tagValueColors;
        }
        public void CallDisplayTagEditor(bool isTagEditorDisplayed, bool inputTextBoxEnabled = false)
        {
            //kikapcsolt állapotban a panelt elrejtjük, textboxot és gombot elrejtjük, grid szélessége nő
            //bekapcsolt állapotban a panel látszik, ha textbox van, akkor az látzik, ellenben a gombok, grid szélessége csökken
            if (!isTagEditorDisplayed)
            {
                this.btnDisplayTagEditor.Text = "<";
                this.btnDisplayTagEditor2.Text = "<";
                this.btnDisplayTagEditor.Show();
                this.btnDisplayTagEditor2.Hide();
                this.btnColumnVisibility.Show();
                this.btnColumnVisibility2.Hide();

                this.groupBoxTag.Hide();
                this.groupBoxTagValue.Hide();
                this.groupBoxTagValueHotkeys.Hide();
                this.txtBoxTagValueInput.Hide();
                this.btnSetTagValue.Hide();
                this.dgvTrackList.Width = this.dgvTrackList.Width + 260;
            }
            else
            {
                this.btnDisplayTagEditor.Text = ">";
                this.btnDisplayTagEditor2.Text = ">";
                this.btnDisplayTagEditor.Hide();
                this.btnDisplayTagEditor2.Show();
                this.btnColumnVisibility.Hide();
                this.btnColumnVisibility2.Show();

                this.groupBoxTag.Show();
                this.groupBoxTagValue.Show();
                this.groupBoxTagValueHotkeys.Show();

                if (!inputTextBoxEnabled)
                {
                    this.txtBoxTagValueInput.Hide();
                    this.btnSetTagValue.Hide();
                }
                else
                {
                    this.txtBoxTagValueInput.Show();
                    this.btnSetTagValue.Show();
                }
               
                this.dgvTrackList.Width = this.dgvTrackList.Width - 260;
            }
        }

        public void ResetPlaylistList(bool isPlaylistListDisplayed)
        {
            if (isPlaylistListDisplayed)
            {
                this.dgvTrackList.Left -= 190;
                this.dgvTrackList.Width += 190;
            }
        }
        public void CallDisplayPlaylistList(bool isPlaylistListDisplayed)
        {
            if (!isPlaylistListDisplayed)
            {
                this.btnDisplayPlaylistList.Text = ">";
                this.btnDisplayPlaylistList2.Text = ">";
                this.btnDisplayPlaylistList.Hide();
                this.btnDisplayPlaylistList2.Show();

                this.dgvPlaylistList.Hide();
                this.groupBoxPlaylist.Hide();
                this.dgvTrackList.Left -= 190;
                this.dgvTrackList.Width += 190;
            }
            else
            {
                this.btnDisplayPlaylistList.Text = "<";
                this.btnDisplayPlaylistList2.Text = "<";
                this.btnDisplayPlaylistList.Show();
                this.btnDisplayPlaylistList2.Hide();

                this.dgvPlaylistList.Show();
                this.groupBoxPlaylist.Show();
                this.dgvTrackList.Left += 190;
                this.dgvTrackList.Width -= 190;
            }
        }

        //UPDATE MAINVIEW VIEW
        public void SetVolume(int volume)
        {
            ((MainView)this.parentView).SetVolume(volume);
        }
        public void UpdateMediaPlayerProgressStatus(double duration, String durationString, double currentPosition, String currentPositionString)
        {
            ((MainView)this.parentView).UpdateMediaPlayerProgressStatus(duration, durationString, currentPosition, currentPositionString);
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
            if (this.dgvPlaylistList.SelectedRows.Count > 0)
                this.LoadPlaylistEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = this.dgvPlaylistList.SelectedRows[0].Index });
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
        }
        private void ClearCurrentPlaylistColor()
        {
            for (int i = 0; i < this.dgvPlaylistList.Rows.Count; i++)
            {
                if (this.dgvPlaylistList.Rows[i].DefaultCellStyle.BackColor == Color.LightSeaGreen)
                {
                    this.dgvPlaylistList.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    break;
                }
            }
        }
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

        #region TRACKLIST - COLUMNS
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

        private void btnDisplayTagEditor_Click(object sender, EventArgs e)
        {
            this.DisplayTagEditorEvent?.Invoke(this, new EventArgs());
        }
        private void btnDisplayTagEditor2_Click(object sender, EventArgs e)
        {
            this.DisplayTagEditorEvent?.Invoke(this, new EventArgs());
        }
        private void btnTag1_Click(object sender, EventArgs e)
        {
            this.SelectTagEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = 0 });
        }
        private void btnTag2_Click(object sender, EventArgs e)
        {
            this.SelectTagEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = 1 });
        }
        private void btnTag3_Click(object sender, EventArgs e)
        {
            this.SelectTagEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = 2 });
        }
        private void btnTag4_Click(object sender, EventArgs e)
        {
            this.SelectTagEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = 3 });
        }
        private void btnTag5_Click(object sender, EventArgs e)
        {
            this.SelectTagEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = 4 });
        }
        private void btnTag6_Click(object sender, EventArgs e)
        {
            this.SelectTagEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = 5 });
        }
        private void btnTag7_Click(object sender, EventArgs e)
        {
            this.SelectTagEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = 6 });
        }
        private void btnTag8_Click(object sender, EventArgs e)
        {
            this.SelectTagEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = 7 });
        }
        private void btnTag9_Click(object sender, EventArgs e)
        {
            this.SelectTagEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = 8 });
        }
        private void btnTagValue1_Click(object sender, EventArgs e)
        {
            if(this.dgvPlaylistList.SelectedRows.Count > 0)
            {
                this.SetTagValueEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = 0, Rows = this.dgvTrackList.Rows });
            }
        }
        private void btnTagValue2_Click(object sender, EventArgs e)
        {
            if (this.dgvPlaylistList.SelectedRows.Count > 0)
            {
                this.SetTagValueEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = 1, Rows = this.dgvTrackList.Rows });
            }
        }
        private void btnTagValue3_Click(object sender, EventArgs e)
        {
            if (this.dgvPlaylistList.SelectedRows.Count > 0)
            {
                this.SetTagValueEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = 2, Rows = this.dgvTrackList.Rows });
            }
        }
        private void btnTagValue4_Click(object sender, EventArgs e)
        {
            if (this.dgvPlaylistList.SelectedRows.Count > 0)
            {
                this.SetTagValueEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = 3, Rows = this.dgvTrackList.Rows });
            }
        }
        private void btnTagValue5_Click(object sender, EventArgs e)
        {
            if (this.dgvPlaylistList.SelectedRows.Count > 0)
            {
                this.SetTagValueEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = 4, Rows = this.dgvTrackList.Rows });
            }
        }
        private void btnTagValue6_Click(object sender, EventArgs e)
        {
            if (this.dgvPlaylistList.SelectedRows.Count > 0)
            {
                this.SetTagValueEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = 5, Rows = this.dgvTrackList.Rows });
            }
        }
        private void btnTagValue7_Click(object sender, EventArgs e)
        {
            if (this.dgvPlaylistList.SelectedRows.Count > 0)
            {
                this.SetTagValueEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = 6, Rows = this.dgvTrackList.Rows });
            }
        }
        private void btnTagValue8_Click(object sender, EventArgs e)
        {
            if (this.dgvPlaylistList.SelectedRows.Count > 0)
            {
                this.SetTagValueEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = 7, Rows = this.dgvTrackList.Rows });
            }
        }
        private void btnTagValue9_Click(object sender, EventArgs e)
        {
            if (this.dgvPlaylistList.SelectedRows.Count > 0)
            {
                this.SetTagValueEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = 8, Rows = this.dgvTrackList.Rows });
            }
        }
        private void btnTagValue10_Click(object sender, EventArgs e)
        {
            if (this.dgvPlaylistList.SelectedRows.Count > 0)
            {
                this.SetTagValueEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = 9, Rows = this.dgvTrackList.Rows });
            }
        }
        private void btnTagValue11_Click(object sender, EventArgs e)
        {
            if (this.dgvPlaylistList.SelectedRows.Count > 0)
            {
                this.SetTagValueEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = 10, Rows = this.dgvTrackList.Rows });
            }
        }
        private void btnTagValue12_Click(object sender, EventArgs e)
        {
            if (this.dgvPlaylistList.SelectedRows.Count > 0)
            {
                this.SetTagValueEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = 11, Rows = this.dgvTrackList.Rows });
            }
        }
        private void btnTagValue13_Click(object sender, EventArgs e)
        {
            if (this.dgvPlaylistList.SelectedRows.Count > 0)
            {
                this.SetTagValueEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = 12, Rows = this.dgvTrackList.Rows });
            }
        }
        private void btnTagValue14_Click(object sender, EventArgs e)
        {
            if (this.dgvPlaylistList.SelectedRows.Count > 0)
            {
                this.SetTagValueEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = 13, Rows = this.dgvTrackList.Rows });
            }
        }
        private void btnTagValue15_Click(object sender, EventArgs e)
        {
            if (this.dgvPlaylistList.SelectedRows.Count > 0)
            {
                this.SetTagValueEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = 14, Rows = this.dgvTrackList.Rows });
            }
        }
        private void btnTagValue16_Click(object sender, EventArgs e)
        {
            if (this.dgvPlaylistList.SelectedRows.Count > 0)
            {
                this.SetTagValueEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = 15, Rows = this.dgvTrackList.Rows });
            }
        }
        private void btnTagValue17_Click(object sender, EventArgs e)
        {
            if (this.dgvPlaylistList.SelectedRows.Count > 0)
            {
                this.SetTagValueEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = 16, Rows = this.dgvTrackList.Rows });
            }
        }
        private void btnTagValue18_Click(object sender, EventArgs e)
        {
            if (this.dgvPlaylistList.SelectedRows.Count > 0)
            {
                this.SetTagValueEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = 17, Rows = this.dgvTrackList.Rows });
            }
        }
        private void btnTagValue19_Click(object sender, EventArgs e)
        {
            if (this.dgvPlaylistList.SelectedRows.Count > 0)
            {
                this.SetTagValueEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = 18, Rows = this.dgvTrackList.Rows });
            }
        }
        private void btnTagValue20_Click(object sender, EventArgs e)
        {
            if (this.dgvPlaylistList.SelectedRows.Count > 0)
            {
                this.SetTagValueEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = 19, Rows = this.dgvTrackList.Rows });
            }
        }
        private void btnTagValue21_Click(object sender, EventArgs e)
        {
            if (this.dgvPlaylistList.SelectedRows.Count > 0)
            {
                this.SetTagValueEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = 20, Rows = this.dgvTrackList.Rows });
            }
        }
        private void btnTagValue22_Click(object sender, EventArgs e)
        {
            if (this.dgvPlaylistList.SelectedRows.Count > 0)
            {
                this.SetTagValueEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = 21, Rows = this.dgvTrackList.Rows });
            }
        }
        private void btnTagValue23_Click(object sender, EventArgs e)
        {
            if (this.dgvPlaylistList.SelectedRows.Count > 0)
            {
                this.SetTagValueEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = 22, Rows = this.dgvTrackList.Rows });
            }
        }
        private void btnTagValue24_Click(object sender, EventArgs e)
        {
            if (this.dgvPlaylistList.SelectedRows.Count > 0)
            {
                this.SetTagValueEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = 23, Rows = this.dgvTrackList.Rows });
            }
        }
        private void btnSetTagValue_Click(object sender, EventArgs e)
        {
            if (this.dgvPlaylistList.SelectedRows.Count > 0)
            {
                this.SetTagValueEvent?.Invoke(this, new ListEventArgs() { StringField1 = this.txtBoxTagValueInput.Text, Rows = this.dgvTrackList.Rows });
                this.txtBoxTagValueInput.Text = null;
            }
        }
        private void txtBoxTagValueInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (this.dgvPlaylistList.SelectedRows.Count > 0)
                {
                    this.SetTagValueEvent?.Invoke(this, new ListEventArgs() { StringField1 = this.txtBoxTagValueInput.Text, Rows = this.dgvTrackList.Rows });
                    this.txtBoxTagValueInput.Text = null;
                }
            }
        }

        #endregion



        private void cmbColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ChangeTracklistColorEvent?.Invoke(this, new ListEventArgs() { StringField1 = (String)this.cmbColor.SelectedItem });
        }

         private void groupBoxTag_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (this.dgvPlaylistList.Rows.Count > 0 && e.KeyCode == Keys.B)
            {
                this.CallNextTrackEvent();
            }
        }

        private void groupBoxTagValue_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (this.dgvPlaylistList.Rows.Count > 0 && e.KeyCode == Keys.B)
            {
                this.CallNextTrackEvent();
            }
        }


    }
}
