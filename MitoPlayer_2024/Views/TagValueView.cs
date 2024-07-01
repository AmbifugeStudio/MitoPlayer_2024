using MitoPlayer_2024.Helpers;
using MitoPlayer_2024.Models;
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
        public event EventHandler<ListEventArgs> EditTag;
        public event EventHandler<ListEventArgs> DeleteTag;
        public event EventHandler<ListEventArgs> CreateTagValue;
        public event EventHandler<ListEventArgs> EditTagValue;
        public event EventHandler<ListEventArgs> DeleteTagValue;
        public event EventHandler CloseWithOk;
        public event EventHandler CloseWithCancel;
        public event EventHandler<ListEventArgs> SetCurrentTagId;
        public event EventHandler<ListEventArgs> SetCurrentTagValueId;

        public TagValueView()
        {
            InitializeComponent();
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

        public void SetTagListBindingSource(BindingSource tagList)
        {
            this.tagListBindingSource = new BindingSource();
            this.tagListBindingSource.DataSource = tagList;
            this.dgvTagList.DataSource = this.tagListBindingSource.DataSource;
            this.dgvTagList.Columns["Id"].Visible = false;
        }
        public void SetTagValueListBindingSource(BindingSource tagValueList)
        {
            this.tagValueListBindingSource = new BindingSource();
            this.tagValueListBindingSource.DataSource = tagValueList;
            this.dgvTagValueList.DataSource = this.tagValueListBindingSource.DataSource;
            this.dgvTagValueList.Columns["Id"].Visible = false;
            this.dgvTagValueList.Columns["Color"].Visible = false;

            
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
                }
            }));
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
                this.EditTag?.Invoke(this, new ListEventArgs() { IntegerField1 = Convert.ToInt32(this.dgvTagList.SelectedRows[0].Index) });
            }
        }

        private void btnDeleteTag_Click(object sender, EventArgs e)
        {
            if (this.dgvTagList.SelectedRows.Count > 0)
            {
                this.DeleteTag?.Invoke(this, new ListEventArgs() { IntegerField1 = Convert.ToInt32(this.dgvTagList.SelectedRows[0].Index) });
            }
        }

        private void btnAddTagValue_Click(object sender, EventArgs e)
        {
            if (this.dgvTagList.SelectedRows.Count > 0)
                this.CreateTagValue?.Invoke(this, new ListEventArgs() { IntegerField1 = Convert.ToInt32(this.dgvTagList.SelectedRows[0].Index) });
        }

        private void btnEditTagValue_Click(object sender, EventArgs e)
        {
            if (this.dgvTagValueList.SelectedRows.Count > 0)
            {
                this.EditTagValue?.Invoke(this, new ListEventArgs() { IntegerField1 = Convert.ToInt32(this.dgvTagValueList.SelectedRows[0].Index) });
            }
        }

        private void btnDeleteTagValue_Click(object sender, EventArgs e)
        {
            if (this.dgvTagValueList.SelectedRows.Count > 0)
            {
                this.DeleteTagValue?.Invoke(this, new ListEventArgs() { IntegerField1 = Convert.ToInt32(this.dgvTagValueList.SelectedRows[0].Index) });
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

        private void dgvTagList_SelectionChanged(object sender, EventArgs e)
        {
            
        }

        private void dgvTagValueList_SelectionChanged(object sender, EventArgs e)
        {
           
        }
        private void dgvTagList_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (this.dgvTagList.SelectedRows.Count > 0)
            {
                this.SetCurrentTagId?.Invoke(this, new ListEventArgs() { IntegerField1 = Convert.ToInt32(this.dgvTagList.SelectedRows[0].Index) });
            }
        }
        private void dgvTagValueList_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (this.dgvTagValueList.SelectedRows.Count > 0)
            {
                this.SetCurrentTagValueId?.Invoke(this, new ListEventArgs() { IntegerField1 = Convert.ToInt32(this.dgvTagValueList.SelectedRows[0].Index) });
            }
        }

        private void dgvTagList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                if (this.dgvTagList.SelectedRows.Count > 0)
                {
                    this.DeleteTag?.Invoke(this, new ListEventArgs() { IntegerField1 = Convert.ToInt32(this.dgvTagList.SelectedRows[0].Index) });
                }
            }
            if(e.KeyCode == Keys.Enter)
            {
                if (this.dgvTagList.SelectedRows.Count > 0)
                {
                    this.EditTag?.Invoke(this, new ListEventArgs() { IntegerField1 = Convert.ToInt32(this.dgvTagList.SelectedRows[0].Index) });
                }
            }
        }

        private void dgvTagValueList_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Delete)
            {
                if (this.dgvTagValueList.SelectedRows.Count > 0)
                {
                    this.DeleteTagValue?.Invoke(this, new ListEventArgs() { IntegerField1 = Convert.ToInt32(this.dgvTagValueList.SelectedRows[0].Index) });
                }

            }
            if (e.KeyCode == Keys.Enter)
            {
                if (this.dgvTagValueList.SelectedRows.Count > 0)
                {
                    this.EditTagValue?.Invoke(this, new ListEventArgs() { IntegerField1 = Convert.ToInt32(this.dgvTagValueList.SelectedRows[0].Index) });
                }
            }
        }

       

        
    }
}
