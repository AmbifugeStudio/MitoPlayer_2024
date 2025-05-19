using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace MitoPlayer_2024.Views
{
    public partial class TagValueView : Form, ITagValueView
    {
        private Form parentView { get; set; }
        private BindingSource tagListBindingSource { get; set; }
        private BindingSource tagValueListBindingSource { get; set; }

        public event EventHandler CreateTag;
        public event EventHandler<Messenger> EditTag;
        public event EventHandler<Messenger> DeleteTag;
        public event EventHandler<Messenger> CreateTagValue;
        public event EventHandler<Messenger> EditTagValue;
        public event EventHandler<Messenger> DeleteTagValue;
        public event EventHandler CloseWithOk;
        public event EventHandler CloseWithCancel;
        public event EventHandler<Messenger> SetCurrentTagId;
        public event EventHandler<Messenger> SetCurrentTagValueId;
        public event EventHandler OpenTagValueImportViewEvent;
        public event EventHandler<Messenger> MoveTagListRowEvent;

        public TagValueView()
        {
            this.InitializeComponent();
            this.SetControlColors();

            this.tagListBindingSource = new BindingSource();
            this.tagValueListBindingSource = new BindingSource();
            this.currentTag = null;

            tagListClickTimer = new System.Windows.Forms.Timer();
            tagListClickTimer.Interval = tagListDoubleClickTime;
            tagListClickTimer.Tick += TagListClickTimer_Tick;

            this.CenterToScreen();
        }

        #region SINGLETON

        public static TagValueView instance;
        public static TagValueView GetInstance(Form parentView)
        {
            if (instance == null || instance.IsDisposed)
            {
                instance = new TagValueView();
                instance.parentView = parentView;
                instance.MdiParent = parentView;
                instance.FormBorderStyle = FormBorderStyle.None;
                instance.Dock = DockStyle.Fill;
                instance.WindowState = FormWindowState.Normal;
            }
            else
            {
             /*   if (instance.WindowState == FormWindowState.Minimized)
                    instance.WindowState = FormWindowState.Normal;*/
                instance.BringToFront();
            }
            return instance;
        }
        #endregion

        //Dark Color Theme
        Color BackgroundColor = System.Drawing.ColorTranslator.FromHtml("#363639");
        Color FontColor = System.Drawing.ColorTranslator.FromHtml("#c6c6c6");
        Color ButtonColor = System.Drawing.ColorTranslator.FromHtml("#292a2d");
        Color ButtonBorderColor = System.Drawing.ColorTranslator.FromHtml("#1b1b1b");
        Color GridHeaderColor = System.Drawing.ColorTranslator.FromHtml("#36373a");
        Color GridLineColor1 = System.Drawing.ColorTranslator.FromHtml("#131315");
        Color GridLineColor2 = System.Drawing.ColorTranslator.FromHtml("#212224");
        Color WhiteColor = System.Drawing.ColorTranslator.FromHtml("#FFFFFF");
        Color GridPlayingColor = System.Drawing.ColorTranslator.FromHtml("#4d4d4d");
        Color GridSelectionColor = System.Drawing.ColorTranslator.FromHtml("#626262");
        Color ActiveColor = System.Drawing.ColorTranslator.FromHtml("#FFBF80");
        private void SetControlColors()
        {
            this.BackColor = this.BackgroundColor;
            this.ForeColor = this.FontColor;

            this.grbTags.ForeColor = this.FontColor;
            this.grbTagValues.ForeColor = this.FontColor;

            this.btnAddTag.BackColor = this.BackColor;
            this.btnAddTag.ForeColor = this.FontColor;
            this.btnAddTag.FlatAppearance.BorderColor = this.ButtonBorderColor;
            this.btnEditTag.BackColor = this.BackColor;
            this.btnEditTag.ForeColor = this.FontColor;
            this.btnEditTag.FlatAppearance.BorderColor = this.ButtonBorderColor;
            this.btnDeleteTag.BackColor = this.BackColor;
            this.btnDeleteTag.ForeColor = this.FontColor;
            this.btnDeleteTag.FlatAppearance.BorderColor = this.ButtonBorderColor;

            this.btnAddTagValue.BackColor = this.BackColor;
            this.btnAddTagValue.ForeColor = this.FontColor;
            this.btnAddTagValue.FlatAppearance.BorderColor = this.ButtonBorderColor;
            this.btnEditTagValue.BackColor = this.BackColor;
            this.btnEditTagValue.ForeColor = this.FontColor;
            this.btnEditTagValue.FlatAppearance.BorderColor = this.ButtonBorderColor;
            this.btnDeleteTagValue.BackColor = this.BackColor;
            this.btnDeleteTagValue.ForeColor = this.FontColor;
            this.btnDeleteTagValue.FlatAppearance.BorderColor = this.ButtonBorderColor;

            this.btnImport.BackColor = this.BackColor;
            this.btnImport.ForeColor = this.FontColor;
            this.btnImport.FlatAppearance.BorderColor = this.ButtonBorderColor;

            this.dgvTagList.BackgroundColor = this.ButtonColor;
            this.dgvTagList.ColumnHeadersDefaultCellStyle.BackColor = this.ButtonColor;
            this.dgvTagList.ColumnHeadersDefaultCellStyle.ForeColor = this.FontColor;
            this.dgvTagList.EnableHeadersVisualStyles = false;
            this.dgvTagList.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.ButtonColor;
            this.dgvTagList.DefaultCellStyle.SelectionBackColor = this.GridSelectionColor;

            this.dgvTagValueList.BackgroundColor = this.ButtonColor;
            this.dgvTagValueList.ColumnHeadersDefaultCellStyle.BackColor = this.ButtonColor;
            this.dgvTagValueList.ColumnHeadersDefaultCellStyle.ForeColor = this.FontColor;
            this.dgvTagValueList.EnableHeadersVisualStyles = false;
            this.dgvTagValueList.ColumnHeadersDefaultCellStyle.SelectionBackColor = this.ButtonColor;
            this.dgvTagValueList.DefaultCellStyle.SelectionBackColor = this.GridSelectionColor;

        }

        private Tag currentTag { get; set; }

        public void InitializeTagListBindingSource(BindingSource tagList)
        {
            this.tagListBindingSource.DataSource = tagList;
            this.dgvTagList.DataSource = this.tagListBindingSource.DataSource;
        }
        private void dgvTagList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            this.dgvTagList.Columns["Id"].Visible = false;
            this.dgvTagList.Columns["OrderInList"].Visible = false;
            this.dgvTagList.Columns["IsIntegrated"].Visible = false;

            foreach (DataGridViewColumn column in dgvTagList.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            this.SetTagListColors();
        }
        public void ReloadTagListBindingSource(Tag currentTag)
        {
            this.currentTag = currentTag;
            this.SetTagListColors();
        }
        public void SetTagListColors()
        {
            if (this.dgvTagList != null && this.dgvTagList.Rows != null && this.dgvTagList.Rows.Count > 0)
            {
                for (int i = 0; i < this.dgvTagList.Rows.Count; i++)
                {
                    if (i == 0 || i % 2 == 0)
                    {
                        this.dgvTagList.Rows[i].DefaultCellStyle.BackColor = this.GridLineColor1;
                    }
                    else
                    {
                        this.dgvTagList.Rows[i].DefaultCellStyle.BackColor = this.GridLineColor2;
                    }
                }
            }
        }

        public void InitializeTagValueListBindingSource(BindingSource tagValueList)
        {
            this.tagValueListBindingSource.DataSource = tagValueList;
            this.dgvTagValueList.DataSource = this.tagValueListBindingSource.DataSource;
        }
        private void dgvTagValueList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            this.dgvTagValueList.Columns["Id"].Visible = false;

            foreach (DataGridViewColumn column in dgvTagValueList.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            if(this.dgvTagValueList.Rows.Count > 0)
            {
                if (this.currentTag != null)
                {
                    if (this.currentTag.HasMultipleValues)
                    {
                        this.btnAddTagValue.Enabled = false;
                        this.btnDeleteTagValue.Enabled = false;
                    }
                    else
                    {
                        this.btnAddTagValue.Enabled = true;
                        this.btnDeleteTagValue.Enabled = true;
                    }
                }

                this.SetTagValueListColors();
            }

            
        }
        public void ReloadTagValueListBindingSource()
        {
            this.SetTagValueListColors();
        }
        public void SetTagValueListColors()
        {
            if (this.dgvTagValueList != null && this.dgvTagValueList.Rows != null && this.dgvTagValueList.Rows.Count > 0)
            {
                for (int i = 0; i < this.dgvTagValueList.Rows.Count; i++)
                {
                    if (i == 0 || i % 2 == 0)
                    {
                        this.dgvTagValueList.Rows[i].DefaultCellStyle.BackColor = this.GridLineColor1;
                    }
                    else
                    {
                        this.dgvTagValueList.Rows[i].DefaultCellStyle.BackColor = this.GridLineColor2;
                    }
                }

                if(this.currentTag != null)
                {
                    for (int i = 0; i < this.dgvTagValueList.Rows.Count; i++)
                    {
                        Color color = HexToColor(this.dgvTagValueList.Rows[i].Cells["Color"].Value.ToString());
                        if (this.currentTag.TextColoring)
                        {
                            this.dgvTagValueList.Rows[i].DefaultCellStyle.ForeColor = color;
                        }
                        else
                        {
                            this.dgvTagValueList.Rows[i].DefaultCellStyle.BackColor = color;
                            if ((color.R < 100 && color.G < 100) || (color.R < 100 && color.B < 100) || (color.B < 100 && color.G < 100))
                            {
                                this.dgvTagValueList.Rows[i].DefaultCellStyle.ForeColor = Color.White;
                            }
                            else
                            {
                                this.dgvTagValueList.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                            }
                        }
                        
                    }
                }
                
            }
        }
        private Color HexToColor(string hexValue)
        {
            return System.Drawing.ColorTranslator.FromHtml(hexValue);
        }


        private void btnAddTag_Click(object sender, EventArgs e)
        {
            this.CreateTag?.Invoke(this, EventArgs.Empty);
        }

        private void btnEditTag_Click(object sender, EventArgs e)
        {
            if(this.dgvTagList.SelectedRows.Count > 0)
            {
                this.EditTag?.Invoke(this, new Messenger() { IntegerField1 = Convert.ToInt32(this.dgvTagList.SelectedRows[0].Index) });
            }
        }

        private void btnDeleteTag_Click(object sender, EventArgs e)
        {
            if (this.dgvTagList.SelectedRows.Count > 0)
            {
                this.DeleteTag?.Invoke(this, new Messenger() { IntegerField1 = Convert.ToInt32(this.dgvTagList.SelectedRows[0].Index) });
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            this.OpenTagValueImportViewEvent?.Invoke(this, EventArgs.Empty);
        }

        private void btnAddTagValue_Click(object sender, EventArgs e)
        {
            if (this.dgvTagList.SelectedRows.Count > 0)
                this.CreateTagValue?.Invoke(this, new Messenger() { IntegerField1 = Convert.ToInt32(this.dgvTagList.SelectedRows[0].Index) });
        }

        private void btnEditTagValue_Click(object sender, EventArgs e)
        {
            if (this.dgvTagValueList.SelectedRows.Count > 0)
            {
                this.EditTagValue?.Invoke(this, new Messenger() { IntegerField1 = Convert.ToInt32(this.dgvTagValueList.SelectedRows[0].Index) });
            }
        }

        private void btnDeleteTagValue_Click(object sender, EventArgs e)
        {
            if (this.dgvTagValueList.SelectedRows.Count > 0)
            {
                this.DeleteTagValue?.Invoke(this, new Messenger() { IntegerField1 = Convert.ToInt32(this.dgvTagValueList.SelectedRows[0].Index) });
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.CloseWithOk?.Invoke(this, EventArgs.Empty);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.CloseWithCancel?.Invoke(this, EventArgs.Empty);
        }


       
        private void dgvTagValueList_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (this.dgvTagValueList.SelectedRows.Count > 0)
            {
                this.SetCurrentTagValueId?.Invoke(this, new Messenger() { IntegerField1 = Convert.ToInt32(this.dgvTagValueList.SelectedRows[0].Index) });
            }
        }

        private void dgvTagList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (this.dgvTagList.SelectedRows.Count > 0)
                {
                    this.DeleteTag?.Invoke(this, new Messenger() { IntegerField1 = Convert.ToInt32(this.dgvTagList.SelectedRows[0].Index) });
                }
            }
            if(e.KeyCode == Keys.Enter)
            {
                if (this.dgvTagList.SelectedRows.Count > 0)
                {
                    this.EditTag?.Invoke(this, new Messenger() { IntegerField1 = Convert.ToInt32(this.dgvTagList.SelectedRows[0].Index) });
                }
            }
        }

        private void dgvTagValueList_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Delete)
            {
                if (this.dgvTagValueList.SelectedRows.Count > 0)
                {
                    this.DeleteTagValue?.Invoke(this, new Messenger() { IntegerField1 = Convert.ToInt32(this.dgvTagValueList.SelectedRows[0].Index) });
                }

            }
            if (e.KeyCode == Keys.Enter)
            {
                if (this.dgvTagValueList.SelectedRows.Count > 0)
                {
                    this.EditTagValue?.Invoke(this, new Messenger() { IntegerField1 = Convert.ToInt32(this.dgvTagValueList.SelectedRows[0].Index) });
                }
            }
        }
        private void dgvTagValueList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dgvTagValueList.SelectedRows.Count > 0)
            {
                this.EditTagValue?.Invoke(this, new Messenger() { IntegerField1 = Convert.ToInt32(this.dgvTagValueList.SelectedRows[0].Index) });
            }
        }


        private int firstSelectedTagRowIndex = -1;
        private bool isTagDragging = false;
        private bool isTagListMouseDown = false;
        private Point tagListDragStartPoint;
        private System.Windows.Forms.Timer tagListClickTimer;
        private const int tagListDoubleClickTime = 2000;
        private const string TagListDataFormat = "TagListData";
        private int tagListInsertionLineIndex = -1;
        
        private void dgvTagList_SelectionChanged(object sender, EventArgs e)
        {
            if (!isInitializing)
            {
                if (dgvTagList.SelectedRows.Count > 0)
                {
                    int selectedIndex = dgvTagList.SelectedRows[0].Index;
                    firstSelectedTagRowIndex = selectedIndex;

                    if (!isTagDragging)
                        this.SetCurrentTagId?.Invoke(this, new Messenger() { IntegerField1 = Convert.ToInt32(this.dgvTagList.SelectedRows[0].Index) });
                }
            }
            
        }
        private bool isInitializing = true;
        public void ToggleTagListSelection(bool enabled)
        {
            isInitializing = !enabled;
        }
        private void dgvTagList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (!isTagDragging)
            {
                this.CallEditTagEvent();
            }
        }
        private void dgvTagList_MouseDown(object sender, MouseEventArgs e)
        {
            var hitTestInfo = dgvTagList.HitTest(e.X, e.Y);
            if(hitTestInfo.RowIndex != -1)
            {
                if (e.Button == MouseButtons.Left) // Prevent dragging the first row
                {
                    if (dgvTagList.SelectedRows.Count == 1 && dgvTagList.SelectedRows[0].Index == hitTestInfo.RowIndex)
                    {
                        // Single row is already selected, start drag-and-drop
                        isTagListMouseDown = true;
                        isTagDragging = false;
                        tagListDragStartPoint = e.Location;
                        firstSelectedTagRowIndex = hitTestInfo.RowIndex;
                        tagListClickTimer.Start();
                    }
                    else
                    {
                        // Row is not selected, select it
                        dgvTagList.ClearSelection();
                        dgvTagList.Rows[hitTestInfo.RowIndex].Selected = true;
                        isTagListMouseDown = true;
                        isTagDragging = false;
                        tagListDragStartPoint = e.Location;
                        firstSelectedTagRowIndex = hitTestInfo.RowIndex;
                        tagListClickTimer.Start();
                    }
                }
            }
            
        }
        private void dgvTagList_MouseMove(object sender, MouseEventArgs e)
        {
            if (isTagListMouseDown && !isTagDragging)
            {
                if (Math.Abs(e.X - tagListDragStartPoint.X) > SystemInformation.DragSize.Width ||
                Math.Abs(e.Y - tagListDragStartPoint.Y) > SystemInformation.DragSize.Height)
                {
                    isTagDragging = true;
                    tagListClickTimer.Stop();
                    dgvTagList.DoDragDrop(new DataObject(TagListDataFormat, dgvTagList.SelectedRows[0]), DragDropEffects.Move);
                }
            }
        }
        private void dgvTagList_MouseUp(object sender, MouseEventArgs e)
        {
            isTagListMouseDown = false;
            if (!isTagDragging)
            {
                tagListClickTimer.Stop();
            }
            isTagDragging = false;
        }
        private void TagListClickTimer_Tick(object sender, EventArgs e)
        {
            tagListClickTimer.Stop();
            if (!isTagDragging)
            {
                this.CallEditTagEvent();
            }
        }
        public void CallEditTagEvent()
        {
            if (this.dgvTagList.SelectedRows.Count > 0)
            {
                this.EditTag?.Invoke(this, new Messenger() { IntegerField1 = Convert.ToInt32(this.dgvTagList.SelectedRows[0].Index) });
            }
        }
        private int highlightedTagRowIndex = -1;
        private void dgvTagList_DragOver(object sender, DragEventArgs e)
        {
            Point clientPoint = dgvTagList.PointToClient(new Point(e.X, e.Y));
            int rowIndex = dgvTagList.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            if (e.Data.GetDataPresent(TagListDataFormat))
            {
                // Handle drag over within playlist list
                if (rowIndex <= 0) // Prevent dropping on the first row
                {
                    e.Effect = DragDropEffects.None;
                    tagListInsertionLineIndex = -1;
                }
                else
                {
                    e.Effect = DragDropEffects.Move;

                    Rectangle rowBounds = dgvTagList.GetRowDisplayRectangle(rowIndex, false);
                    int rowHeight = rowBounds.Height;
                    int cursorY = clientPoint.Y - rowBounds.Top;

                    if (cursorY < rowHeight / 2)
                    {
                        DrawTagListInsertionLine(rowIndex);
                    }
                    else
                    {
                        DrawTagListInsertionLine(rowIndex + 1);
                    }
                }
                highlightedTagRowIndex = -1; // Ensure border is not drawn
            }

            dgvTagList.Invalidate(); // Update the line or border
        }
        private void DrawTagListInsertionLine(int rowIndex)
        {
            tagListInsertionLineIndex = rowIndex;
            dgvTagList.Invalidate();
        }
        private void dgvTagList_DragDrop(object sender, DragEventArgs e)
        {
            Point clientPoint = dgvTagList.PointToClient(new Point(e.X, e.Y));
            int rowIndex = dgvTagList.HitTest(clientPoint.X, clientPoint.Y).RowIndex;

            if (e.Data.GetDataPresent(TagListDataFormat))
            {
                // Handle drop within playlist list
                if (rowIndex > 0) // Prevent dropping on the first row
                {
                    this.MoveTagListRowEvent?.Invoke(this, new Messenger() { IntegerField1 = firstSelectedTagRowIndex, IntegerField2 = rowIndex });
                }
                highlightedTagRowIndex = -1; // Clear the border
            }

            // Re-select the moved rows using BeginInvoke to ensure it runs after the DataGridView has rendered
            dgvTagList.BeginInvoke(new Action(() =>
            {
                dgvTagList.ClearSelection();
                dgvTagList.Rows[rowIndex].Selected = true;
            }));

            // Clear the insertion line and highlighted row
            tagListInsertionLineIndex = -1;
            highlightedTagRowIndex = -1;
            dgvTagList.Invalidate(); // Clear the line and border
        }
        private void dgvTagList_DragLeave(object sender, EventArgs e)
        {
            highlightedTagRowIndex = -1;
            dgvTagList.Invalidate(); // Clear the border
        }
        private void DrawHighlightedTagListRowBorder(int rowIndex)
        {
            highlightedTagRowIndex = rowIndex;
            dgvTagList.Invalidate();
        }
        private void dgvTagList_Paint(object sender, PaintEventArgs e)
        {
            if (highlightedTagRowIndex >= 0 && highlightedTagRowIndex < dgvTagList.Rows.Count)
            {
                Rectangle rowRect = dgvTagList.GetRowDisplayRectangle(highlightedTagRowIndex, true);
                using (Pen pen = new Pen(this.ActiveColor, 2)) // Change to your desired color
                {
                    e.Graphics.DrawRectangle(pen, rowRect);
                }
            }

            if (tagListInsertionLineIndex >= 0 && tagListInsertionLineIndex <= dgvTagList.Rows.Count)
            {
                int y = 0;
                if (tagListInsertionLineIndex < dgvTagList.Rows.Count)
                {
                    Rectangle rowRect = dgvTagList.GetRowDisplayRectangle(tagListInsertionLineIndex, true);
                    y = rowRect.Top;
                }
                else
                {
                    Rectangle rowRect = dgvTagList.GetRowDisplayRectangle(tagListInsertionLineIndex - 1, true);
                    y = rowRect.Bottom;
                }

                using (Pen pen = new Pen(this.ActiveColor, 2))
                {
                    e.Graphics.DrawLine(pen, new Point(0, y), new Point(dgvTagList.Width, y));
                }
            }
        }
        private void dgvTagList_DragEnter(object sender, DragEventArgs e)
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

        private void TagValueView_Shown(object sender, EventArgs e)
        {
            isInitializing = false;
        }
    }
}
