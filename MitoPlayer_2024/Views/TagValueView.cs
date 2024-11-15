using MitoPlayer_2024.Dao;
using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Models;
using MitoPlayer_2024.Presenters;
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

        public TagValueView()
        {
            this.InitializeComponent();
            this.SetControlColors();

            this.tagListBindingSource = new BindingSource();
            this.tagValueListBindingSource = new BindingSource();
            this.currentTag = null;
            
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


        /*
        public void SetTagValueListBindingSource(BindingSource tagValueList,bool hasMultipleValues = true)
        {
            this.tagValueListBindingSource = new BindingSource();
            this.tagValueListBindingSource.DataSource = tagValueList;
            this.dgvTagValueList.DataSource = this.tagValueListBindingSource.DataSource;
           

            this.BeginInvoke(new Action(() =>
            {
                for (int i = 0; i < this.dgvTagValueList.Rows.Count; i++)
                {
                    Color bgColor = HexToColor(this.dgvTagValueList.Rows[i].Cells["Color"].Value.ToString());
                    this.dgvTagValueList.Rows[i].DefaultCellStyle.BackColor = bgColor;

                    if ((bgColor.R < 100 && bgColor.G < 100) || (bgColor.R < 100 && bgColor.B < 100) || (bgColor.B < 100 && bgColor.G < 100))
                    {
                        this.dgvTagValueList.Rows[i].DefaultCellStyle.ForeColor = Color.White;
                    }
                    else
                    {
                        this.dgvTagValueList.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
            }));
        }
        
        */

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


        private void dgvTagList_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (this.dgvTagList.SelectedRows.Count > 0)
            {
                this.SetCurrentTagId?.Invoke(this, new Messenger() { IntegerField1 = Convert.ToInt32(this.dgvTagList.SelectedRows[0].Index) });
            }
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

        private void dgvTagList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dgvTagList.SelectedRows.Count > 0)
            {
                this.EditTag?.Invoke(this, new Messenger() { IntegerField1 = Convert.ToInt32(this.dgvTagList.SelectedRows[0].Index) });
            }
        }

        private void dgvTagValueList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.dgvTagValueList.SelectedRows.Count > 0)
            {
                this.EditTagValue?.Invoke(this, new Messenger() { IntegerField1 = Convert.ToInt32(this.dgvTagValueList.SelectedRows[0].Index) });
            }
        }

       

        
    }
}
