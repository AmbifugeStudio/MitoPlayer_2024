using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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
        public event EventHandler<ListEventArgs> TrackDragAndDropEvent;
        public event EventHandler<ListEventArgs> CopyTracksToPlaylistEvent;
        public event EventHandler ShowColumnVisibilityEditorEvent;

        //PLAYLIST
        public event EventHandler<ListEventArgs> ShowPlaylistEditorViewEvent;
        public event EventHandler<ListEventArgs> LoadPlaylistEvent;
        public event EventHandler<ListEventArgs> DeletePlaylistEvent;
        public event EventHandler<ListEventArgs> SetQuickListEvent;

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
            this.dgvPlaylistList.Columns["G"].AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
            this.dgvPlaylistList.Columns["G"].Width = 20;

        }
        public void SetTrackListBindingSource(BindingSource trackList, bool[] columnVisibility, int[] columnSortingId)
        {
            this.trackListBindingSource = new BindingSource();
            this.trackListBindingSource.DataSource = trackList;
            this.dgvTrackList.DataSource = this.trackListBindingSource.DataSource;
            for (int i = 0; i <= this.dgvTrackList.Columns.Count - 1; i++)
            {
                this.dgvTrackList.Columns[i].Visible = columnVisibility[i];
                this.dgvTrackList.Columns[i].DisplayIndex = columnSortingId[i];

            }

            int isMissingColumnIndex = this.dgvTrackList.Columns["IsMissing"].Index;
            for (int i = 0; i < this.dgvTrackList.Rows.Count; i++)
            {
                if ((bool)this.dgvTrackList.Rows[i].Cells[isMissingColumnIndex].Value)
                {
                    this.dgvTrackList.Rows[i].DefaultCellStyle.BackColor = Color.Salmon;
                }
            }
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
            if (this.dgvTrackList.Rows.Count > 0 && e.KeyCode == Keys.Delete)
            {
                this.DeleteTracksEvent?.Invoke(this, new ListEventArgs() { Rows = this.dgvTrackList.Rows });
            }

            if (this.dgvTrackList.Rows.Count > 0 && (e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.ShiftKey))
            {
                this.controlKey = true;
            }
                
            if (this.dgvPlaylistList.SelectedRows.Count > 0 && e.KeyCode == Keys.Space ||
                this.dgvPlaylistList.SelectedRows.Count > 0 && e.KeyCode == Keys.Enter)
            {
                if(this.dgvTrackList.SelectedRows.Count > 0)
                {
                    int rowIndex = this.dgvTrackList.Rows.IndexOf(this.dgvTrackList.SelectedRows[0]);

                    this.CallSetCurrentTrackEvent(rowIndex);
                    this.CallStopTrackEvent();
                    this.CallPlayTrackEvent();
                }
            }

            if(this.dgvPlaylistList.SelectedRows.Count > 0 && e.KeyCode == Keys.D1)
            {
                this.CopyTracksToPlaylistEvent?.Invoke(this, new ListEventArgs() { SelectedRows = this.dgvTrackList.SelectedRows, IntegerField1 = 1 });
            }
            if (this.dgvPlaylistList.SelectedRows.Count > 0 && e.KeyCode == Keys.D2)
            {
                this.CopyTracksToPlaylistEvent?.Invoke(this, new ListEventArgs() { SelectedRows = this.dgvTrackList.SelectedRows, IntegerField1 = 2 });
            }
            if (this.dgvPlaylistList.SelectedRows.Count > 0 && e.KeyCode == Keys.D3)
            {
                this.CopyTracksToPlaylistEvent?.Invoke(this, new ListEventArgs() { SelectedRows = this.dgvTrackList.SelectedRows, IntegerField1 = 3 });
            }
            if (this.dgvPlaylistList.SelectedRows.Count > 0 && e.KeyCode == Keys.D4)
            {
                this.CopyTracksToPlaylistEvent?.Invoke(this, new ListEventArgs() { SelectedRows = this.dgvTrackList.SelectedRows, IntegerField1 = 4 });
            }
            if (this.dgvPlaylistList.Rows.Count > 0 && e.KeyCode == Keys.R)
            {
                this.CallRandomTrackEvent();
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

        #region TRACKLIST - DRAG AND DROP
        private bool mouseDown = false;
        private int currentRowIndex = -1;

        private bool dragEnabled = false;
        private bool dragOneRow = false;
        private int initialFirstDisplayedScrollingRowIndex = -1;

        private int rowCount = -1;
        private int dropLocationRowIndex = -1;

        private System.Windows.Forms.DataGridView.HitTestInfo hti = null;
        private System.Threading.Timer scrollTimer = null;
        private bool scrolling = false;

        private List<int> selectionIndicesAfterDragAndDrop = new List<int>();
        /*
         * jobb klikk egy sorra, mouseDown flaget bekapcsoljuk
         * ha a sor nincs kijelölve, kijelöli és elmenti az indexet
         * ha a sor már ki van jelölve, nem nyomunk közben ctrl-t és shift-et és nem az összes sor van kijelölve, akkor aktiválódik a drag and drop
         * ha csak egy sort jelöltünk ki, akkor a drag and drop később, a mouse move-ban aktiválódik
         * ha több sor van, akkor a drag and drop azonnal aktiválódik
         */
        private void dgvTrackList_MouseDown(object sender, MouseEventArgs e) 
        {
            this.mouseDown = true;
            HitTestInfo hitTest = this.dgvTrackList.HitTest(e.X, e.Y);
            if (hitTest != null && hitTest.RowIndex > -1) 
            {
                this.currentRowIndex = hitTest.RowIndex;

                if (this.dgvTrackList.Rows[hitTest.RowIndex].Selected)
                {
                    if (!this.controlKey)
                    {
                        if(this.dgvTrackList.Rows.Count != this.dgvTrackList.SelectedRows.Count)
                        {
                            this.dragEnabled = true;
                            if (this.dgvTrackList.SelectedRows.Count > 1)
                            {
                                this.dragOneRow = false;
                                this.Drag();
                            }
                            else
                            {
                                this.dragOneRow = true;
                            }
                        }
                    }
                }
            }
        }
        /*
         * aktiválja a drag flag-et
         * ha vannak kijelölt sorok, lementi és törli őket a táblából
         * törli a kijelölést
         * indítja a draganddrop-ot
         */
        private void Drag()
        {
            this.initialFirstDisplayedScrollingRowIndex = this.dgvTrackList.FirstDisplayedScrollingRowIndex;
            this.rowCount = this.dgvTrackList.RowCount;

            this.SaveSelectedRows();
            this.RemoveSelectedRows();

            this.dgvTrackList.ClearSelection();
            this.dgvTrackList.Invalidate();

            BindingSource dataSource = this.selectedTrackListBindingSource.DataSource as BindingSource;
            DataTable targetTable = dataSource.DataSource as DataTable;
            
            this.dgvTrackList.DoDragDrop(targetTable, DragDropEffects.Move);
            this.dgvTrackList.Capture = true;
        }
        /*
         * a céltáblát törli
         * végigmegy a forrástábla elemein és ami ki van jelölve, hozzáadja egy másik táblához
         */
        private void SaveSelectedRows()
        {
            BindingSource dataSource = this.trackListBindingSource.DataSource as BindingSource;
            DataTable sourceTable = dataSource.DataSource as DataTable;
            BindingSource dataSource2 = this.selectedTrackListBindingSource.DataSource as BindingSource;
            DataTable targetTable = dataSource2.DataSource as DataTable;

            targetTable.Rows.Clear();

            if (this.dgvTrackList.SelectedRows != null && this.dgvTrackList.SelectedRows.Count > 0)
            {
                for (int i = 0; i < this.dgvTrackList.Rows.Count; i++)
                {
                    if (this.dgvTrackList.Rows[i].Selected)
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
            if (this.dgvTrackList.SelectedRows != null && this.dgvTrackList.SelectedRows.Count > 0)
            {
                for (int i = this.dgvTrackList.Rows.Count - 1; i >= 0; i--)
                {
                    if (this.dgvTrackList.Rows[i].Selected)
                    {
                        this.dgvTrackList.Rows.RemoveAt(i);
                    }
                }
            }
        }
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

            if (mousepos > (93 + this.dgvTrackList.Location.Y + (this.dgvTrackList.Height * 0.95)))
            {
                if (this.dgvTrackList.FirstDisplayedScrollingRowIndex < this.dgvTrackList.RowCount - 1)
                {
                    this.dgvTrackList.FirstDisplayedScrollingRowIndex = this.dgvTrackList.FirstDisplayedScrollingRowIndex + 1;
                }
            }
            if (mousepos < (93 + this.dgvTrackList.Location.Y + (this.dgvTrackList.Height * 0.05)))
            {
                if (this.dgvTrackList.FirstDisplayedScrollingRowIndex > 0)
                {
                    this.dgvTrackList.FirstDisplayedScrollingRowIndex = this.dgvTrackList.FirstDisplayedScrollingRowIndex - 1;
                }
            }

            Point clientPoint = this.dgvTrackList.PointToClient(new Point(e.X, e.Y));
            this.dropLocationRowIndex = this.dgvTrackList.HitTest(clientPoint.X, clientPoint.Y).RowIndex;
            this.dgvTrackList.Invalidate();
        }
        /*
         * ha a drag be van kapcsolva és a bal klikk le van nyomva, egy kijelölt sor esetén itt aktiválódik a drag
         * az egér mozgatásra a tábla scrollozódik
         */
        private void dgvTrackList_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.dragEnabled && e.Button == MouseButtons.Left)
            {
                Point p = this.dgvTrackList.PointToClient(new Point(e.X, e.Y));
                int dragIndex = this.dgvTrackList.HitTest(p.X, p.Y).RowIndex;

                if (this.dragOneRow && this.currentRowIndex != dragIndex)
                {
                    this.Drag();
                }

                DataGridView.HitTestInfo newhti = this.dgvTrackList.HitTest(e.X, e.Y);
                if (this.hti != null && this.hti.RowIndex != newhti.RowIndex)
                {
                    System.Diagnostics.Debug.WriteLine("invalidating " + this.hti.RowIndex.ToString());
                    this.Invalidate();
                }
                this.hti = newhti;

                System.Diagnostics.Debug.WriteLine(string.Format("{0:000} {1}  ", this.hti.RowIndex, e.Location));

                Point clientPoint = this.PointToClient(e.Location);

                System.Diagnostics.Debug.WriteLine(e.Location + "  " + this.Bounds.Size);

                if (this.scrollTimer == null && this.ShouldScrollDown(e.Location))
                {
                    this.scrollTimer = new System.Threading.Timer(new System.Threading.TimerCallback(this.TimerScroll), 1, 0, 250);
                }
                if (this.scrollTimer == null && this.ShouldScrollUp(e.Location))
                {
                    this.scrollTimer = new System.Threading.Timer(new System.Threading.TimerCallback(this.TimerScroll), -1, 0, 250);
                }
            }
            else
            {
                this.dragEnabled = false;
            }
            if (!(this.ShouldScrollUp(e.Location) || this.ShouldScrollDown(e.Location)))
            {
                this.StopAutoScrolling();
                if (this.scrollTimer != null)
                {
                    this.scrollTimer.Dispose();
                    this.scrollTimer = null;
                }
            }
        }
        bool ShouldScrollUp(Point location)
        {
            return location.Y > this.dgvTrackList.ColumnHeadersHeight
                && location.Y < this.dgvTrackList.ColumnHeadersHeight + 15
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
            if (this.scrollTimer != null)
            {
                this.scrollTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                this.scrollTimer = null;
            }
        }
        void TimerScroll(object state)
        {
            this.SetScrollBar((int)state);
        }
        void SetScrollBar(int direction)
        {
            if (this.scrolling)
            {
                return;
            }
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<int>(this.SetScrollBar), new object[] { direction });
            }
            else
            {
                this.scrolling = true;

                if (0 < direction)
                {
                    if (this.dgvTrackList.FirstDisplayedScrollingRowIndex < this.dgvTrackList.Rows.Count - 1)
                    {
                        this.dgvTrackList.FirstDisplayedScrollingRowIndex++;
                    }
                }
                else
                {
                    if (this.dgvTrackList.FirstDisplayedScrollingRowIndex > 0)
                    {
                        this.dgvTrackList.FirstDisplayedScrollingRowIndex--;
                    }
                }

                this.scrolling = false;
            }
        }
        /*
         * rajzol egy vonalat a drag and drop közben, ami jelöli, hogy hova szúrjuk be az elemeket
         */
        private void dgvTrackList_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex == this.dropLocationRowIndex - 1)
            {
                e.Paint(e.ClipBounds, DataGridViewPaintParts.All);
                var graphics = e.Graphics;
                var cellBounds = e.CellBounds;
                var pen = new Pen(Color.Black, 4);
                graphics.DrawLine(pen, cellBounds.Left, cellBounds.Bottom, cellBounds.Right, cellBounds.Bottom);
                e.Handled = true;
            }
        }
        private void dgvTrackList_MouseUp(object sender, MouseEventArgs e)
        {
            this.mouseDown = false;

            if(this.rowCount > -1 && this.rowCount != this.dgvTrackList.RowCount)
            {
                if (Control.MouseButtons != MouseButtons.Left)
                {
                    this.rowCount = -1;
                    this.Drop(this.currentRowIndex);
                    this.dgvTrackList.Capture = false;
                }
            }
        }
        private void dgvTrackList_DragDrop(object sender, DragEventArgs e)
        {
            Point p = this.dgvTrackList.PointToClient(new Point(e.X, e.Y));
            int dragIndex = this.dgvTrackList.HitTest(p.X, p.Y).RowIndex;

            if (e.Effect == DragDropEffects.Link)
            {
                var files = (string[])e.Data.GetData(DataFormats.FileDrop);
                ((MainView)instance.parentView).OpenFilesFromDragAndDrop(files, dragIndex);
            }
            else if (this.dragEnabled && e.Effect == DragDropEffects.Move)
            {
                this.Drop(dragIndex);
            }
        }
        private void Drop(int dragIndex)
        {
            this.InsertSelectedRows(dragIndex);

            this.currentRowIndex = -1;

            this.BeginInvoke(new Action(() =>
            {
                this.dgvTrackList.ClearSelection();

                if (this.selectionIndicesAfterDragAndDrop != null && this.selectionIndicesAfterDragAndDrop.Count > 0)
                {
                    foreach (int i in this.selectionIndicesAfterDragAndDrop)
                    {
                        this.dgvTrackList.Rows[i].Selected = true;
                    }
                    this.initialFirstDisplayedScrollingRowIndex = this.dgvTrackList.FirstDisplayedScrollingRowIndex;
                    this.dgvTrackList.FirstDisplayedScrollingRowIndex = this.initialFirstDisplayedScrollingRowIndex;
                }
                this.selectionIndicesAfterDragAndDrop = new List<int>();

                this.CallSetCurrentTrackColorEvent();
            }));

            //változók resetelése
            this.dragEnabled = false;
            this.dragOneRow = false;
            this.mouseDown = false;

            //scroll kikapcsolása
            this.hti = null;

            this.StopAutoScrolling();
            this.Invalidate();

            this.TrackDragAndDropEvent?.Invoke(this, new ListEventArgs() { Rows = this.dgvTrackList.Rows });
        }
        /*
         * visszarakni a sorokat az eredeti helyükre, ha ki drop-oljuk a grid-en kívül
         */
        private void dgvTrackList_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {
            if (Control.MouseButtons != MouseButtons.Left)
            {
                if (!this.dgvTrackList.ClientRectangle.Contains(this.dgvTrackList.PointToClient(new Point(Control.MousePosition.X, Control.MousePosition.Y))))
                {
                    e.Action = DragAction.Cancel;
                    this.Drop(this.currentRowIndex);
                }
                this.dgvTrackList.Capture = false;
            }
        }
        /*
         * beszúrja az elmentett sorokat a megadott indexre
         */
        private void InsertSelectedRows(int insertIndex)
        {
            BindingSource dataSource = this.selectedTrackListBindingSource.DataSource as BindingSource;
            DataTable sourceTable = dataSource.DataSource as DataTable;

            BindingSource dataSource2 = this.trackListBindingSource.DataSource as BindingSource;
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
                this.selectionIndicesAfterDragAndDrop.Add(insertIndex+i);
            }

            this.dropLocationRowIndex = -1;
            sourceTable.Rows.Clear();
            this.trackListBindingSource.ResetBindings(false);
            this.dgvTrackList.Invalidate();
        }
        #endregion

        #region MEDIA PLAYER
        /*
         * a jelenleg kijelölt szám indexe
         */
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
        public void UpdateAfterPlayTrack(int currentTrackIndex)
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
            this.CallSetCurrentTrackColorEvent(currentTrackIndex);
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
            List<Button> buttonList = new List<Button> { this.btnTag1, this.btnTag2, this.btnTag3, 
                                                        this.btnTag4, this.btnTag5, this.btnTag6,
                                                        this.btnTag7,this.btnTag8,this.btnTag9 };
            for (int i = 0; i <= buttonList.Count - 1; i++)
            {
                buttonList[i].Hide();
            }
            for (int i = 0; i <= tagList.Count - 1; i++)
            {
                buttonList[i].Text = tagList[i].Name;
                buttonList[i].Show();
            }
        }
        public void InitializeTagValueEditor(List<TagValue> tagValueList)
        {
            List<Button> buttonList = new List<Button> { this.btnTagValue1, this.btnTagValue2, this.btnTagValue3,
                                                        this.btnTagValue4, this.btnTagValue5, this.btnTagValue6,
                                                        this.btnTagValue7,this.btnTagValue8,this.btnTagValue9,
                                                        this.btnTagValue10, this.btnTagValue11, this.btnTagValue12,
                                                        this.btnTagValue13, this.btnTagValue14, this.btnTagValue15,
                                                         this.btnTagValue16, this.btnTagValue17, this.btnTagValue18,
                                                         this.btnTagValue19, this.btnTagValue20, this.btnTagValue21,
                                                         this.btnTagValue22, this.btnTagValue23, this.btnTagValue24 };
            for (int i = 0; i <= buttonList.Count - 1; i++)
            {
                buttonList[i].Hide();
            }
            if(tagValueList != null && tagValueList.Count > 0)
            {
                for (int i = 0; i <= tagValueList.Count - 1; i++)
                {
                    buttonList[i].Text = tagValueList[i].Name;
                    buttonList[i].Show();
                }
            }
            
        }
        public void CallDisplayTagEditor(bool isTagEditorDisplayed)
        {
            if (!isTagEditorDisplayed)
            {
                this.btnDisplayTagEditor.Text = "<";
                this.groupBoxTag.Hide();
                this.groupBoxTagValue.Hide();
                this.dgvTrackList.Width = this.dgvTrackList.Width + 260;
            }
            else
            {
                this.btnDisplayTagEditor.Text = ">";
                this.groupBoxTag.Show();
                this.groupBoxTagValue.Show();
                this.dgvTrackList.Width = this.dgvTrackList.Width - 260;
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

        #region PLAYLIST LIST
        private void dgvPlaylistList_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                contextMenuStrip1.Show(Cursor.Position);
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
            this.ShowPlaylistEditorViewEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = -1 });
        }
        public void CallRenamePlaylistEvent()
        {
            if (this.dgvPlaylistList.SelectedRows.Count > 0)
                this.ShowPlaylistEditorViewEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = dgvPlaylistList.SelectedRows[0].Index });
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


        #endregion

        private void btnColumnVisibility_Click(object sender, EventArgs e)
        {
            this.ShowColumnVisibilityEditorEvent?.Invoke(this, new EventArgs());
        }

        private void btnDisplayTagEditor_Click(object sender, EventArgs e)
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
    }
}
