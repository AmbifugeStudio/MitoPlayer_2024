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
using System.Runtime.CompilerServices;
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

        //TRACKLIST
        public event EventHandler<ListEventArgs> OrderByColumnEvent;
        public event EventHandler<ListEventArgs> DeleteTracksEvent;

        //PLAYLIST
        public event EventHandler<ListEventArgs> ShowPlaylistEditorViewEvent;
        public event EventHandler<ListEventArgs> LoadPlaylistEvent;
        public event EventHandler<ListEventArgs> DeletePlaylistEvent;
        public event EventHandler<ListEventArgs> SetQuickListEvent;

        /*
        
        public event EventHandler<ListEventArgs> PlayTrack;
        public event EventHandler<ListEventArgs> PauseTrack;
        public event EventHandler StopTrack;
        public event EventHandler<ListEventArgs> PrevTrack;
        public event EventHandler<ListEventArgs> NextTrack;*/

       // 




        //public event EventHandler<ListEventArgs> AddTracksToTrackListEvent;


        
        //public List<Playlist> PlaylistList { get; set; }
        //public List<Track> TrackList { get; set; }
        //private int lastTrackIndex { get; set; }
        //private double currentPlayPosition { get; set; }
        /*
       





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
        */

        
        
        public PlaylistView()
        {
            InitializeComponent();
        }

        #region SINGLETON

        private static PlaylistView instance;
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
            
        }
        public void SetTrackListBindingSource(BindingSource trackList, bool[] columnVisibility)
        {
            this.trackListBindingSource = new BindingSource();
            this.trackListBindingSource.DataSource = trackList;
            this.dgvTrackList.DataSource = this.trackListBindingSource.DataSource;
            for (int i = 0; i <= this.dgvTrackList.Columns.Count - 1; i++)
            {
                this.dgvTrackList.Columns[i].Visible = columnVisibility[i];
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
        public void SetVolume(int volume)
        {
            trackVolume.Value = volume;
            lblVolume.Text = volume.ToString() + "%";
        }
        #endregion

        #region MEDIA PLAYER


        //CELL CLICK
        // private int currentTrackIndex = -1;
        // private int trackIdInPlaylist = -1;

        #endregion

        #region TRACKLIST

        private bool controlKey = false;
        //SELECT TRACKS BY LEFT CLICK - SET SELECTED ROW COOUNT AND TIME
        private void dgvTrackList_SelectionChanged(object sender, EventArgs e)
        {
            this.ChangeSelectedRowCountAndTime();
        }
        //CLICK ON COLUMNS
        private void dgvTrackList_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            this.CallOrderByColumnEvent(dgvTrackList.Columns[e.ColumnIndex].Name);
        }
        //PRESS DEL
        //SELECT WITH CTRL OR SHIFT
        private void dgvTrackList_KeyDown(object sender, KeyEventArgs e)
        {
            if (this.dgvTrackList.Rows.Count > 0 && e.KeyCode == Keys.Delete)
                this.CallDeleteTracksEvent(this.dgvTrackList.Rows);
            if (this.dgvTrackList.Rows.Count > 0 && (e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.ShiftKey))
                this.controlKey = true;
        }
        private void dgvTrackList_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.ControlKey || e.KeyCode == Keys.ShiftKey)
                controlKey = false;
        }



        //DROP FILES OR ROWS
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

                this.SetCurrentTrackColor();
            }));

            //változók resetelése
            this.dragEnabled = false;
            this.dragOneRow = false;
            this.mouseDown = false;

            //scroll kikapcsolása
            this.hti = null;

            this.StopAutoScrolling();
            this.Invalidate();

            ListEventArgs args = new ListEventArgs();
            args.Rows = dgvTrackList.Rows;
            this.TrackDragAndDrop?.Invoke(this, args);
        }

        private void ChangeSelectedRowCountAndTime()
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
        private void CallOrderByColumnEvent(String columnName)
        {
            this.OrderByColumnEvent?.Invoke(this, new ListEventArgs() { StringField1 = columnName });
            this.SetCurrentTrackColor();
        }
        private void CallDeleteTracksEvent(DataGridViewRowCollection rows)
        {
            this.DeleteTracksEvent?.Invoke(this, new ListEventArgs() { Rows = rows });
        }



        

        //ROW SELECTION
        private bool mouseDown = false;
        private int currentRowIndex = -1;
        
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





        #region MEDIA PLAYER
        /*
         * a jelenleg kijelölt szám indexe
         */
        //ROW CLICK - SET CURRENT TRACK
        private void dgvTrackList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            this.CallSetCurrentTrackEvent((int)e.RowIndex);
        }
        //ROW DOUBLE CLICK - PLAY TRACK
        /*
         * szám lejátszása
         */
        private void dgvTrackList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            this.CallStopTrackEvent();
            this.CallPlayTrackEvent();
        }
        //CHANGE VOLUME
        private void trackVolume_Scroll(object sender, EventArgs e)
        {
            this.CallChangeVolumeEvent();
        }
        //CLICK ON PROGRESS BAR
        private void pBar_MouseDown(object sender, MouseEventArgs e)
        {
            this.CallChangeProgressEvent(e.X, pBar.Width);
        }
        //TIMER
        private void timer1_Tick(object sender, EventArgs e)
        {
            this.CallGetMediaPlayerStatusEvent();
        }

        //EVENT CALLINGS
        public void CallSetCurrentTrackEvent(int rowIndex)
        {
            this.SetCurrentTrackEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = rowIndex });
        }
        public void CallPlayTrackEvent()
        {
            if (this.dgvTrackList.SelectedRows.Count > 0)
                this.PlayTrackEvent?.Invoke(this, new ListEventArgs() { 
                    IntegerField1 = Convert.ToInt32(dgvTrackList.SelectedRows[0].Cells["OrderInList"].Value),
                    IntegerField2 = dgvTrackList.Rows.IndexOf(dgvTrackList.SelectedRows[0])
                });
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
            int selectedRowIndex = -1;
            if(dgvPlaylistList.SelectedRows.Count > 0)
                selectedRowIndex = dgvPlaylistList.SelectedRows[0].Index;

            if (this.dgvTrackList.SelectedRows.Count > 0)
                this.PrevTrackEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = selectedRowIndex });
        }
        public void CallNextTrackEvent()
        {
            int selectedRowIndex = -1;
            if (dgvPlaylistList.SelectedRows.Count > 0)
                selectedRowIndex = dgvPlaylistList.SelectedRows[0].Index;

            if (this.dgvTrackList.SelectedRows.Count > 0)
                this.NextTrackEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = selectedRowIndex });
        }
        public void CallRandomTrackEvent()
        {
            if (this.dgvTrackList.SelectedRows.Count > 1)
                this.RandomTrackEvent?.Invoke(this, EventArgs.Empty);
        }
        public void CallChangeVolumeEvent()
        {
            this.ChangeVolumeEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = this.trackVolume.Value });
        }
        public void CallChangeProgressEvent(int position, int length)
        {
            this.ChangeProgressEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = position, IntegerField2 = length });
        } 
        public void CallGetMediaPlayerStatusEvent()
        {
            this.GetMediaPlayerProgressStatusEvent?.Invoke(this, EventArgs.Empty);
        }

        //UPDATE VIEW
        public void UpdateAfterPlayTrack(int currentTrackIndex)
        {
            String title = "Playing: " + (string)dgvTrackList.Rows[currentTrackIndex].Cells["Artist"].Value;
            if (!String.IsNullOrEmpty((string)dgvTrackList.Rows[currentTrackIndex].Cells["Title"].Value))
            {
                title += " - " + (string)dgvTrackList.Rows[currentTrackIndex].Cells["Title"].Value;
            }
            lblCurrentTrack.Text = title;

            this.SetCurrentTrackColor(currentTrackIndex);
        }
        public void UpdateAfterPlayTrackAfterPause()
        {
            lblCurrentTrack.Text = lblCurrentTrack.Text.Replace("Paused: ", "Playing: ");
        }
        public void UpdateAfterStopTrack()
        {
            this.lblCurrentTrack.Text = "Playing: -";
            this.lblTrackStart.Text = "";
            this.lblTrackEnd.Text = "";
            this.pBar.Value = 0;

            this.ClearCurrentTrackColor();
        }
        public void UpdateAfterPauseTrack()
        {
            this.lblCurrentTrack.Text = this.lblCurrentTrack.Text.Replace("Playing: ", "Paused: ");
        }
        public void UpdateAfterChangeVolume()
        {
            this.lblVolume.Text = this.trackVolume.Value.ToString() + "%";
        }
        public void UpdateMediaPlayerProgressStatus(double duration, String durationString, double currentPosition, String currentPositionString)
        {
            this.pBar.Maximum = (int)duration;
            this.pBar.Value = (int)currentPosition;
            this.lblTrackEnd.Text = durationString;
            this.lblTrackStart.Text = currentPositionString;
        }
        public void SetCurrentTrackColor(int rowIndex = -1)
        {
            for (int i = 0; i < this.dgvTrackList.Rows.Count; i++)
            {
                if (this.dgvTrackList.Rows[i].DefaultCellStyle.BackColor == Color.LightSeaGreen)
                {
                    this.dgvTrackList.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    break;
                }
            }
            if (this.trackIdInPlaylist != -1)
            {
                int idInPlaylist = -1;
                for (int i = 0; i < this.dgvTrackList.Rows.Count; i++)
                {
                    idInPlaylist = Convert.ToInt32(this.dgvTrackList.Rows[i].Cells["OrderInList"].Value);
                    if (this.trackIdInPlaylist == idInPlaylist)
                    {
                        this.dgvTrackList.Rows[i].DefaultCellStyle.BackColor = Color.LightSeaGreen;
                        break;
                    }
                }
            }
            else
            {
                if (rowIndex > -1)
                {
                    this.dgvTrackList.Rows[rowIndex].DefaultCellStyle.BackColor = Color.LightSeaGreen;
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
        #endregion






        #region PLAYLIST LIST
        //VIEW CONTROLS
        //RIGHT CLICK - CONTEXT MENU
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
        //PLAYLIST BUTTONS
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
        //DOUBLE CLICK - LOAD PLAYLIST
        private void dgvPlaylistList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            this.CallLoadPlaylistEvent();
        }
        //PRESSING DEL - DELETE PLAYLIST
        private void dgvPlaylistList_KeyDown(object sender, KeyEventArgs e)
        {
            if (dgvPlaylistList.SelectedRows.Count > 0 && e.KeyCode == Keys.Delete)
            {
                this.CallDeletePlaylistEvent();
            }
        }
        //QUICKLIST BUTTONS
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
                DeletePlaylistEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = dgvPlaylistList.SelectedRows[0].Index });
        }
        public void CallSetQuickListEvent(int group)
        {
            if (this.dgvPlaylistList.SelectedRows.Count > 0)
                SetQuickListEvent?.Invoke(this, new ListEventArgs() { IntegerField1 = dgvPlaylistList.SelectedRows[0].Index, IntegerField2 = group });
        }
        #endregion

        
    }
}
